package utility

import (
	"sync"
)

type GoPool struct {
	queue chan int
	wg    *sync.WaitGroup
}

func NewGoPool(size int) *GoPool {
	if size <= 0 {
		size = 1
	}
	return &GoPool{
		queue: make(chan int, size),
		wg:    &sync.WaitGroup{},
	}
}

func (p *GoPool) Add(delta int) {
	p.wg.Add(delta)
	for i := 0; i < delta; i++ {
		p.queue <- 1
	}
}

func (p *GoPool) Done() {
	<-p.queue
	p.wg.Done()
}

func (p *GoPool) Wait() {
	p.wg.Wait()
}
