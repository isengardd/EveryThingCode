package utility

import (
	"bytes"
	"encoding/base64"
	"encoding/json"
	"fmt"
	"io"
	"mime/multipart"
	"net"
	"net/http"
	"strconv"
	"strings"
	"time"

	log "github.com/sirupsen/logrus"
	"sixcube.cn/ad_center/enums"
)

// GetIPAdress 获取请求的ip地址
func GetIPAdress(r *http.Request) string {
	for _, h := range []string{"X-Forwarded-For", "X-Real-Ip"} {
		addresses := strings.Split(r.Header.Get(h), ",")
		// march from right to left until we get a public address
		// that will be the address right before our proxy.
		for i := len(addresses) - 1; i >= 0; i-- {
			ip := strings.TrimSpace(addresses[i])
			// header can contain spaces too, strip those out.
			realIP := net.ParseIP(ip)
			if !realIP.IsGlobalUnicast() {
				// bad address, go to next
				continue
			}
			return ip
		}
	}
	return ""
}

///////// http相关 /////////

// HTTPStatusOK 验证http返回值是否是成功
func HTTPStatusOK(respCode int) bool {
	return respCode >= http.StatusOK && respCode < 300
}

// HTTPPostJSON 发送post请求
func HTTPPostJSON(url string, data interface{}) (resp *http.Response, err error) {
	var postBody io.Reader = nil
	switch data.(type) {
	case map[string]interface{}:
		var postData []byte
		postData, err = json.Marshal(data.(map[string]interface{}))
		if err != nil {
			return nil, err
		}
		postBody = strings.NewReader(string(postData))
	case io.Reader:
		postBody = data.(io.Reader)
	case nil:
		break
	default:
		return nil, fmt.Errorf("HTTPPostJSON unknown post data type")
	}

	if resp, err = http.Post(url, "application/json", postBody); err != nil {
		return nil, err
	}

	return resp, nil
}

// HTTPPostForm 发送post请求
func HTTPPostForm(url string, contentType string, data interface{}) (resp *http.Response, err error) {
	if contentType == "" {
		contentType = "multipart/form-data"
	}
	var postBody io.Reader = nil
	switch data.(type) {
	case map[string]interface{}:
		body := &bytes.Buffer{}
		writer := multipart.NewWriter(body)
		for key, value := range data.(map[string]interface{}) {
			writer.WriteField(key, fmt.Sprintf("%v", value))
		}
		writer.Close()
		postBody = body
	case io.Reader:
		postBody = data.(io.Reader)
	case nil:
		break
	default:
		return nil, fmt.Errorf("HTTPPostForm unknown post data type")
	}
	if resp, err = http.Post(url, contentType, postBody); err != nil {
		return nil, err
	}
	// defer resp.Body.Close()
	// if respBody, err = ioutil.ReadAll(resp.Body); err != nil {
	// 	return 0, nil, err
	// }

	return resp, nil
}

// HTTPGetInTime 发送get请求
func HTTPGetInTime(url string, timeout time.Duration) (resp *http.Response, err error) {
	c := &http.Client{
		Timeout: timeout,
	}
	resp, err = c.Get(url)
	if err != nil {
		return nil, err
	}
	return resp, nil
}

// HTTPGet 发送get请求
func HTTPGet(url string) (resp *http.Response, err error) {
	return HTTPGetInTime(url, 300*time.Second)
}

// HTTPDelete 发送delete请求
func HTTPDelete(url string, data interface{}) (resp *http.Response, err error) {
	var postBody io.Reader = nil
	switch data.(type) {
	case map[string]interface{}:
		var postData []byte
		postData, err = json.Marshal(data.(map[string]interface{}))
		if err != nil {
			return nil, err
		}
		postBody = strings.NewReader(string(postData))
	case nil:
		break
	default:
		return nil, fmt.Errorf("HTTPDelete unknown data type")
	}
	c := &http.Client{
		Timeout: 300 * time.Second,
	}
	// Create request
	req, err := http.NewRequest("DELETE", url, postBody)
	if err != nil {
		return nil, err
	}
	resp, err = c.Do(req)

	return resp, nil
}

///////// session相关 /////////

// ValidSession 验证session是否合法
// 返回:
// 		如果合法，interface{}储存用户的userName
//    如果不合法，interface{}返回错误代码
func ValidSession(session string) (bool, interface{}) {
	decStr, err := base64.StdEncoding.DecodeString(session)
	if err != nil {
		log.WithField("error", err.Error()).Debug("ValidSession Decode Failed")
		return false, enums.ErrorSessionError
	}

	strSplits := strings.Split(string(decStr), "|")
	if len(strSplits) == 3 {
		if expire, err := strconv.Atoi(strSplits[1]); err != nil {
			log.WithField("error", err.Error()).Debug("ValidSession Atoi Failed")
			return false, enums.ErrorSessionError
		} else {
			if time.Now().Unix() > int64(expire) {
				return false, enums.ErrorSessionExpire
			}
			if !Md5Check(fmt.Sprintf("%v|%v", strSplits[0], strSplits[1]), strSplits[2]) {
				return false, enums.ErrorSessionError
			}
			return true, strSplits[0]
		}
	}
	return false, enums.ErrorSessionError
}

// session有效时间
const (
	SessionExpire = int64(90 * 24 * time.Hour / time.Second)
)

// GenerateSession 生成session
func GenerateSession(userName string) string {
	// username|expiretime|md5check
	var originStr = fmt.Sprintf("%v|%v", userName, time.Now().Unix()+SessionExpire)
	originStr = fmt.Sprintf("%v|%v", originStr, Md5Encode(originStr))
	return base64.StdEncoding.EncodeToString([]byte(originStr))
}
