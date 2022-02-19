package utility

import (
	"log"
	"time"
)

// GetTodayBase 获取今日0点时间戳
func GetTodayBase() int64 {
	t := time.Now()
	tm1 := time.Date(t.Year(), t.Month(), t.Day(), 0, 0, 0, 0, t.Location())
	return tm1.Unix()
}

func GetNowTime() int32 {
	return int32(time.Now().Unix())
}

func GetNowTimeMS() int64 {
	return time.Now().UnixNano() / int64(time.Millisecond)
}

type TimeRecord struct {
	startTime int64
	endTime   int64
}

func (rd *TimeRecord) Start(msg string) {
	rd.startTime = GetNowTimeMS()
	rd.endTime = 0
}

// 记录结束时间戳，并输出操作耗时。 支持多次End调用
func (rd *TimeRecord) End(msg string) {
	rd.endTime = GetNowTimeMS()
	log.Printf("%v: %v ms\n", msg, rd.endTime-rd.startTime)
}
