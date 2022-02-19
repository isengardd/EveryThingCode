package utility

import (
	"bytes"
	"crypto/md5"
	"encoding/hex"
	"io/ioutil"
	"os"
	"reflect"
	"runtime"
	"strings"
	"sync"

	"golang.org/x/text/encoding/simplifiedchinese"
	"golang.org/x/text/transform"
	"sixcube.cn/ad_center/enums"
)

// Debug 是否是debug环境
func Debug() bool {
	return os.Getenv("AD_DEBUG") == "1"
}

// OS 获取操作系统名称
func OS() string {
	return runtime.GOOS
}

// EraseSyncMap 清空syncmap
func EraseSyncMap(m *sync.Map) {
	m.Range(func(key interface{}, value interface{}) bool {
		m.Delete(key)
		return true
	})
}

// reverseAny 反转数组
func ReverseAny(s interface{}) {
	n := reflect.ValueOf(s).Len()
	swap := reflect.Swapper(s)
	for i, j := 0, n-1; i < j; i, j = i+1, j-1 {
		swap(i, j)
	}
}

// Md5Check md5校验
func Md5Check(content, encrypted string) bool {
	return strings.EqualFold(Md5Encode(content), encrypted)
}

// Md5Encode md5加密
func Md5Encode(data string) string {
	h := md5.New()
	h.Write([]byte(data + enums.SessionMD5Key))
	return hex.EncodeToString(h.Sum(nil))
}

// StringInSlice 字符串是否在数组中
func StringInSlice(a string, list []string) bool {
	for _, b := range list {
		if b == a {
			return true
		}
	}
	return false
}

// TrimSpaceStringSlice
func TrimSpaceStringSlice(list []string) []string {
	for idx, value := range list {
		list[idx] = strings.TrimSpace(value)
	}
	return list
}

// IfThenElseInt 3元操作符
func IfThenElseInt(cond bool, a int, b int) int {
	if cond {
		return a
	} else {
		return b
	}
}

// IfThenElseString 3元操作符
func IfThenElseString(cond bool, a string, b string) string {
	if cond {
		return a
	} else {
		return b
	}
}

// GbkToUtf8 编码转换
func GbkToUtf8(s []byte) ([]byte, error) {
	reader := transform.NewReader(bytes.NewReader(s), simplifiedchinese.GBK.NewDecoder())
	d, e := ioutil.ReadAll(reader)
	if e != nil {
		return nil, e
	}
	return d, nil
}

// Utf8ToGbk 编码转换
func Utf8ToGbk(s []byte) ([]byte, error) {
	reader := transform.NewReader(bytes.NewReader(s), simplifiedchinese.GBK.NewEncoder())
	d, e := ioutil.ReadAll(reader)
	if e != nil {
		return nil, e
	}
	return d, nil
}
