package utility

import (
	"crypto/md5"
	"fmt"
	"io"
	"io/ioutil"
	"os"
	"path"
	"strings"

	"sixcube.cn/ad_center/persists"
)

// RemoveAllInDir 删除path下的所有内容，不包括path本身
func RemoveAllInDir(dirPath string) error {
	dir, err := ioutil.ReadDir(dirPath)
	if err != nil {
		return err
	}

	for _, fi := range dir {
		if fi.IsDir() {
			if err := os.RemoveAll(path.Join(dirPath, fi.Name())); err != nil {
				return err
			}
		} else {
			if err := os.Remove(path.Join(dirPath, fi.Name())); err != nil {
				return err
			}
		}
	}
	return nil
}

// RemoveFilesInDir 删除path下的所有文件
func RemoveFilesInDir(dirPath string) error {
	dir, err := ioutil.ReadDir(dirPath)
	if err != nil {
		return err
	}

	for _, fi := range dir {
		if fi.IsDir() {
			continue
		} else {
			if err := os.Remove(path.Join(dirPath, fi.Name())); err != nil {
				return err
			}
		}
	}
	return nil
}

// CheckAndCreateDir 检查对应路径的文件夹是否存在，不存在则创建
func CheckAndCreateDir(dirPath string) error {
	if _, err := os.Stat(dirPath); os.IsNotExist(err) {
		err = os.MkdirAll(dirPath, 0755)
		if err != nil {
			return err
		}
	} else if err != nil {
		return err
	}
	return nil
}

// WriteFile 文件写数据
func WriteFile(fileName string, data []byte) error {
	output, err := os.OpenFile(fileName, os.O_WRONLY|os.O_TRUNC|os.O_CREATE, 0666)
	if err != nil {
		return err
	}
	defer output.Close()
	_, err = output.Write(data)
	if err != nil {
		return err
	}
	return nil
}

// CopyFile 拷贝文件
func CopyFile(srcName, dstName string) error {
	src, err := os.Open(srcName)
	if err != nil {
		return err
	}
	defer src.Close()
	dst, err := os.OpenFile(dstName, os.O_WRONLY|os.O_TRUNC|os.O_CREATE, 0666)
	if err != nil {
		return err
	}
	defer dst.Close()
	_, err = io.Copy(dst, src)
	if err != nil {
		return err
	}
	return nil
}

// RemoveFile 删除指定的文件
func RemoveFile(fileName string, dir string) error {
	if fileName == "" {
		return nil
	}
	if dir == "" {
		dir = persists.Config().LocalPathImage
	}
	if dir == "" {
		return fmt.Errorf("can't remove file in empty dir")
	}
	return os.Remove(path.Join(dir, fileName))
}

// IsExcel 判断是否是excel文件
func IsExcel(fileName string, filtTmp bool) bool {
	// 过滤excel的临时文件
	if filtTmp && strings.HasPrefix(fileName, "~$") {
		return false
	}
	excelTypeList := []string{".xlsx"}
	for _, excelType := range excelTypeList {
		if strings.HasSuffix(fileName, excelType) {
			return true
		}
	}
	return false
}

// FilePrefix 返回文件名的前半部分，如a.txt返回a
func FilePrefix(fileName string) string {
	return fileName[0 : len(fileName)-len(path.Ext(fileName))]
}

// CreateMD5File 创建md5文件
func CreateMD5File(srcFile string, outputDir string, ext string) (string, int64, error) {
	var output *os.File
	var err error
	output, err = os.OpenFile(srcFile, os.O_RDONLY, 0666)
	defer func() {
		if output != nil {
			output.Close()
			output = nil
		}
	}()

	if err != nil {
		return "", 0, fmt.Errorf("CreateMD5File OpenFile %v fail:%v", srcFile, err.Error())
	}
	output.Seek(0, 0)
	h := md5.New()
	var written int64 = 0
	if written, err = io.Copy(h, output); err != nil {
		return "", 0, fmt.Errorf("CreateMD5File  %v fail:%v", srcFile, err.Error())
	}
	md5Sum := h.Sum(nil)
	filename := fmt.Sprintf("%x.%v", md5Sum, ext)
	filePath := path.Join(outputDir, filename)
	err = CopyFile(srcFile, filePath)
	if err != nil {
		return "", 0, fmt.Errorf("CreateMD5File  %v fail:%v", srcFile, err.Error())
	}
	return filename, written, nil
}

// MD5File 返回文件的MD5值
func MD5File(filePath string) (string, error) {
	var output *os.File
	var err error
	output, err = os.OpenFile(filePath, os.O_RDONLY, 0666)
	defer func() {
		if output != nil {
			output.Close()
			output = nil
		}
	}()

	if err != nil {
		return "", fmt.Errorf("MD5File OpenFile %v fail:%v", filePath, err.Error())
	}
	output.Seek(0, 0)
	h := md5.New()
	if _, err = io.Copy(h, output); err != nil {
		return "", fmt.Errorf("MD5File  %v Copy fail:%v", filePath, err.Error())
	}
	md5Sum := h.Sum(nil)
	return fmt.Sprintf("%x", md5Sum), nil
}
