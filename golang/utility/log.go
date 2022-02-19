package utility

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"path"
	"strings"
	"sync"
	"time"

	"github.com/sirupsen/logrus"
	"sixcube.cn/ad_center/enums"
)

// InitLog 初始化日志设置
func InitLog() error {
	logrus.SetFormatter(&logrus.TextFormatter{
		DisableSorting:  false,
		FullTimestamp:   true,
		TimestampFormat: "06-01-02 15:04:05.000",
	})
	// log.SetOutput(os.Stdout) 默认输出到stderr,也可以指定文件
	if Debug() {
		logrus.SetLevel(logrus.DebugLevel)
	} else {
		logrus.SetLevel(logrus.InfoLevel)
	}

	var err error
	var output *os.File
	err = CheckAndCreateDir(enums.LogDirName)
	if err != nil {
		println(err.Error())
		return err
	}
	output, err = os.OpenFile(path.Join(enums.LogDirName, GetServerLogName()), os.O_WRONLY|os.O_APPEND|os.O_CREATE, 0666)
	if err != nil {
		println(err.Error())
		return err
	}
	output = os.Stdout
	log.SetOutput(output)
	logrus.SetOutput(output)
	DealLogDaily()
	log.SetFlags(log.LstdFlags | log.Lshortfile)
	return nil
}

func GetServerLogName() string {
	return fmt.Sprintf(enums.ServerLogName, time.Now().Format("2006-01-02"))
}

var singleLog sync.Once

func DealLogDaily() {
	singleLog.Do(func() {
		go func() {
			for {
				todayBase := GetTodayBase()
				nextDayBase := todayBase + 86400
				nowTime := time.Now().Unix()
				<-time.After(time.Duration(nextDayBase-nowTime) * time.Second)
				output, err := os.OpenFile(path.Join(enums.LogDirName, GetServerLogName()), os.O_WRONLY|os.O_APPEND|os.O_CREATE, 0666)
				if err != nil {
					log.Println(err.Error())
					continue
				}
				log.SetOutput(output)
				logrus.SetOutput(output)

				// 删除旧日志
				var logDirInfo []os.FileInfo
				logDirInfo, err = ioutil.ReadDir(enums.LogDirName)
				if err != nil {
					log.Println(err.Error())
					continue
				}

				for _, fi := range logDirInfo {
					if fi.IsDir() {
						continue
					} else {
						if strings.Contains(fi.Name(), enums.ServerLogNamePrefix) {
							logFileDateStr := fi.Name()[len(enums.ServerLogNamePrefix):]
							logFileDateStr = strings.Split(logFileDateStr, ".")[0]
							// log.Println(logFileDateStr)
							finalDate := GetTodayBase() - 86400*enums.LogSaveDay
							local2, err2 := time.LoadLocation("Local") //服务器设置的时区
							if err2 != nil {
								log.Println(err2)
								continue
							}
							logFileTime, err := time.ParseInLocation("2006-01-02 ", logFileDateStr, local2)
							if err != nil {
								log.Println(err.Error())
								continue
							}
							// log.Println(fmt.Sprintf("filetime=%v, finaltime=%v", logFileTime.Unix(), finalDate))
							if logFileTime.Unix() <= finalDate {
								if err := os.Remove(path.Join(enums.LogDirName, fi.Name())); err != nil {
									log.Println(err.Error())
									continue
								}
							}
						}
					}
				}
			}
		}()
	})
}
