package utility

import (
	"context"
	"errors"
	"fmt"
	"image"
	"image/jpeg"
	"image/png"
	"io"
	"math/rand"
	"os"
	"os/exec"
	"path"
	"path/filepath"
	"runtime"
	"strings"
	"time"

	log "github.com/sirupsen/logrus"

	"github.com/disintegration/imaging"
	oa_runtime "github.com/go-openapi/runtime"
	"sixcube.cn/ad_center/enums"
)

// ReadImage 读取图片
func ReadImage(picturePath string) (image.Image, error) {
	fileExt := strings.ToLower(filepath.Ext(picturePath))
	if len(fileExt) > 0 {
		fileExt = fileExt[1:]
	}

	if !StringInSlice(fileExt, []string{"jpg", "png"}) {
		return nil, fmt.Errorf("not support picture format: %v", fileExt)
	}

	src, err := os.Open(picturePath)
	if err != nil {
		return nil, fmt.Errorf("pic:%v %v", picturePath, err.Error())
	}
	defer src.Close()

	var img image.Image
	img, err = ReadIOImage(fileExt, src)
	if err != nil {
		return nil, fmt.Errorf("pic:%v %v", picturePath, err.Error())
	}
	return img, nil
}

// ReadIOImage 从io读取图片
func ReadIOImage(ext string, r io.Reader) (image.Image, error) {
	var img image.Image
	var err error
	if ext == "jpg" {
		if img, err = jpeg.Decode(r); err != nil {
			return nil, fmt.Errorf("ReadIOImage error: %v", err.Error())
		}
	} else if ext == "png" {
		if img, err = png.Decode(r); err != nil {
			return nil, fmt.Errorf("ReadIOImage error: %v", err.Error())
		}
	} else {
		return nil, errors.New("错误的文件名后缀")
	}
	return img, nil
}

// ReadIOImageConfig 从io读取图片配置
func ReadIOImageConfig(ext string, r io.Reader) (image.Image, error) {
	var config image.Image
	var err error
	if ext == "jpg" || ext == "png" {
		if config, err = imaging.Decode(r); err != nil {
			return config, fmt.Errorf("ReadIOImage error: %v", err.Error())
		}
	} else {
		return config, errors.New("错误的文件名后缀")
	}
	return config, nil
}

// CutImage 裁剪图片
func CutImage(picturePath string, startX int, startY int, endX int, endY int) (image.Image, error) {
	var img image.Image
	var err error
	img, err = ReadImage(picturePath)
	if err != nil {
		return nil, err
	}

	// 裁剪
	var subImg image.Image
	subImg, err = SubImage(img, startX, startY, endX-startX, endY-startY)
	if err != nil {
		return nil, err
	}

	return subImg, nil
}

// RotateImage 旋转图片
func RotateImage(srcImage image.Image, rotate int32) (image.Image, error) {
	rotate = rotate % 4
	if rotate == 0 {
		return srcImage, nil
	} else if rotate == 1 {
		return imaging.Rotate90(srcImage), nil
	} else if rotate == 2 {
		return imaging.Rotate180(srcImage), nil
	} else if rotate == 3 {
		return imaging.Rotate270(srcImage), nil
	}
	return nil, errors.New("Unknown rotate angle")
}

// SaveImage 保存image到目标目录，文件名为MD5值
func SaveImage(src image.Image, outputDir string, compressMethod int) (string, int64, error) {
	if fileInfo, err := os.Stat(outputDir); err != nil {
		return "", 0, err
	} else if fileInfo != nil && !fileInfo.IsDir() {
		return "", 0, err
	}

	tmpFileName := path.Join(outputDir, fmt.Sprintf("tmp_%v_%v.jpg", int(time.Now().Unix()), rand.Int()))
	var err error

	defer func() {
		// 删除临时文件
		os.Remove(tmpFileName)
	}()

	if err = imaging.Save(src, tmpFileName, imaging.JPEGQuality(100)); err != nil {
		return "", 0, err
	}

	switch compressMethod {
	case enums.CompressLossLess:
		if err = CompressImageLossless(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	case enums.CompressMediumQuality:
		if err = CompressImageMediumQuality(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	case enums.CompressHighQuality:
		if err = CompressImageHighQuality(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	case enums.CompressVeryHighQuality:
		if err = CompressImageVeryHighQuality(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	default:
		break
	}

	var filename = ""
	var written int64 = 0
	filename, written, err = CreateMD5File(tmpFileName, outputDir, "jpg")
	if err != nil {
		return "", 0, err
	}

	return filename, written, nil
}

// SaveIOImage 保存图片io
func SaveIOImage(imageFile io.ReadCloser, outputDir string, compressMethod int) (string, int64, error) {
	if fileInfo, err := os.Stat(outputDir); err != nil {
		return "", 0, err
	} else if fileInfo != nil && !fileInfo.IsDir() {
		return "", 0, err
	}

	tmpFileName := path.Join(outputDir, fmt.Sprintf("tmp_%v_%v.jpg", int(time.Now().Unix()), rand.Int()))
	var tmpFile *os.File
	var err error

	closeFile := func() {
		if tmpFile != nil {
			tmpFile.Close()
			tmpFile = nil
		}
	}
	defer func() {
		closeFile()
		// 删除临时文件
		os.Remove(tmpFileName)
	}()

	var input *oa_runtime.File
	var ok bool

	if input, ok = imageFile.(*oa_runtime.File); !ok {
		return "", 0, errors.New("cannot cast uploaded file")
	}

	tmpFile, err = os.OpenFile(tmpFileName, os.O_WRONLY|os.O_CREATE, 0666)
	if err != nil {
		return "", 0, err
	}

	input.Data.Seek(0, 0)
	_, err = io.Copy(tmpFile, input.Data)
	if err != nil {
		return "", 0, err
	}
	tmpFile.Close()
	tmpFile = nil

	switch compressMethod {
	case enums.CompressLossLess:
		if err = CompressImageLossless(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	case enums.CompressMediumQuality:
		if err = CompressImageMediumQuality(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	case enums.CompressHighQuality:
		if err = CompressImageHighQuality(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	case enums.CompressVeryHighQuality:
		if err = CompressImageVeryHighQuality(tmpFileName, tmpFileName); err != nil {
			return "", 0, err
		}
		break
	default:
		break
	}

	var filename = ""
	var written int64 = 0
	filename, written, err = CreateMD5File(tmpFileName, outputDir, "jpg")
	if err != nil {
		return "", 0, err
	}

	return filename, written, nil
}

// CompressImageLossless 无损压缩
func CompressImageLossless(srcImg string, tgtImg string) error {
	if runtime.GOOS == "linux" || runtime.GOOS == "windows" {
		var cmd *exec.Cmd = nil
		// jpegtran -copy none -optimize -perfect -progressive -outfile tgtImg srcImg
		ctx, cancelFn := context.WithTimeout(context.Background(), time.Second*60)
		defer cancelFn()
		cmd = exec.CommandContext(ctx, "jpegtran", "-copy", "none", "-optimize", "-perfect", "-progressive", "-outfile", tgtImg, srcImg)
		cmd.Env = os.Environ()
		_, err := cmd.CombinedOutput()
		if err != nil {
			return err
		}
	}
	return nil
}

// CompressImageVeryHighQuality 超高质量压缩
func CompressImageVeryHighQuality(srcImg string, tgtImg string) error {
	if runtime.GOOS == "linux" || runtime.GOOS == "windows" {
		var cmd *exec.Cmd = nil
		var err error = nil

		// jpeg-recompress --strip --accurate -m ms-ssim --min 90 srcimage targetimage
		ctx, cancelFn := context.WithTimeout(context.Background(), time.Second*600)
		defer cancelFn()
		cmd = exec.CommandContext(ctx, "jpeg-recompress", "--strip", "--accurate", "-m", "ms-ssim", "--min", "90", srcImg, tgtImg)
		cmd.Env = os.Environ()
		_, err = cmd.CombinedOutput()
		if err != nil {
			return err
		}
	} else {
		log.WithFields(log.Fields{
			"os": runtime.GOOS,
		}).Warn("ignore compress img")
	}
	return nil
}

// CompressImageHighQuality 高质量压缩
func CompressImageHighQuality(srcImg string, tgtImg string) error {
	if runtime.GOOS == "linux" || runtime.GOOS == "windows" {
		var cmd *exec.Cmd = nil
		var err error = nil
		// jpeg-recompress --strip --accurate -m ms-ssim --min 80 srcimage targetimage
		ctx, cancelFn := context.WithTimeout(context.Background(), time.Second*600)
		defer cancelFn()
		cmd = exec.CommandContext(ctx, "jpeg-recompress", "--strip", "--accurate", "-m", "ms-ssim", "--min", "80", srcImg, tgtImg)
		cmd.Env = os.Environ()
		_, err = cmd.CombinedOutput()
		if err != nil {
			return err
		}
	} else {
		log.WithFields(log.Fields{
			"os": runtime.GOOS,
		}).Warn("ignore compress img")
	}
	return nil
}

// CompressImageMediumQuality 中等质量压缩
func CompressImageMediumQuality(srcImg string, tgtImg string) error {
	if runtime.GOOS == "linux" || runtime.GOOS == "windows" {
		var cmd *exec.Cmd = nil
		// jpeg-recompress  -m ms-ssim srcimage targetimage
		ctx, cancelFn := context.WithTimeout(context.Background(), time.Second*600)
		defer cancelFn()
		cmd = exec.CommandContext(ctx, "jpeg-recompress", "-m", "ms-ssim", srcImg, tgtImg)
		cmd.Env = os.Environ()
		_, err := cmd.CombinedOutput()
		if err != nil {
			return err
		}
	} else {
		log.WithFields(log.Fields{
			"os": runtime.GOOS,
		}).Warn("ignore compress img")
	}
	return nil
}

// SubImage 裁剪图片
func SubImage(src image.Image, x, y, w, h int) (image.Image, error) {
	var subImg image.Image

	if rgbImg, ok := src.(*image.YCbCr); ok {
		subImg = rgbImg.SubImage(image.Rect(x, y, x+w, y+h)).(*image.YCbCr) //图片裁剪x0 y0 x1 y1
	} else if rgbImg, ok := src.(*image.RGBA); ok {
		subImg = rgbImg.SubImage(image.Rect(x, y, x+w, y+h)).(*image.RGBA) //图片裁剪x0 y0 x1 y1
	} else if rgbImg, ok := src.(*image.NRGBA); ok {
		subImg = rgbImg.SubImage(image.Rect(x, y, x+w, y+h)).(*image.NRGBA) //图片裁剪x0 y0 x1 y1
	} else {
		return subImg, errors.New("SubImage: 图片裁剪失败")
	}

	return subImg, nil
}
