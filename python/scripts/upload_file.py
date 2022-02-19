#!/usr/bin/python
# -*- coding: utf-8 -*-
"""
    1.本脚本默认filepath路径下要上传的文件，文件名是该文件的md5值。用于上传到阿里云平台时，进行文件校验
    2.采用增量更新的方式，旧的图片不会被删除，已上传的图片不会重复上传
    todo:
        1.可以参考ossutil，增加多线程提高并发
        2.ossutil目前没有验证md5的增量更新（存在同名文件时，本地和远端的md5不同才上传），所以才会写本脚本模仿google gsutil进行验证md5的增量更新
          如果后期ossutil增加了上传时的md5验证功能，则可以完全用ossutil替换本脚本
"""
import oss2
import os,sys
import hashlib
from optparse import OptionParser
import threading
import time

OSS_END_POINT = ''
OSS_BUCKET = '
OSS_ACCESS_ID = ''
OSS_ACCESS_KEY = ''

class UploadThread(threading.Thread):
    def __init__(self, bucket, forceUpdate):
        threading.Thread.__init__(self)
        self.bucket = bucket
        self.forceUpdate = forceUpdate
        self.fileList = []
        self.threadLock = threading.Lock()
        self.finished = False
    def run(self):
        while True:
            fileData = self.getFile()
            if self.finished and len(fileData) == 0:
                break

            if len(fileData) > 0:
                localPath = fileData[0]
                remotePath = fileData[1]
                fileName = fileData[2]
                picturePath = os.path.join(localPath, fileName)
                objectPath = os.path.join(remotePath, fileName)
                # 不在对应bucket中的文件才需要上传
                if not self.bucket.object_exists(objectPath) or self.forceUpdate > 0:
                    print fileName
                    # <yourLocalFile>由本地文件路径加文件名包括后缀组成，例如/users/local/myfile.txt
                    self.bucket.put_object_from_file(objectPath, picturePath)
                else:
                    if fileName.endswith('.jpg'):
                        continue
                    objMeta = self.bucket.get_object_meta(objectPath)
                    md5Remote = objMeta.headers['ETag']
                    md5Remote = md5Remote.upper().strip('"')
                    #print "{0} remote md5: {1}".format(file, md5Remote)
                    # 计算本地文件的hash值
                    md5Local = ""
                    with open(picturePath, 'r') as localf:
                        m = hashlib.md5()
                        m.update(localf.read())
                        md5Local = m.hexdigest()
                        md5Local = md5Local.upper()
                    #print "{0} local md5: {1}".format(file, md5Local)
                    if md5Local == "" or md5Local != md5Remote:
                        print "{0} md5 not equal, do upload".format(fileName)
                        self.bucket.put_object_from_file(objectPath, picturePath)

            time.sleep(0.001)
    def getFile(self):
        popFile = ()
        self.threadLock.acquire()
        if len(self.fileList) > 0:
            popFile = self.fileList.pop(-1)
        self.threadLock.release()
        return popFile
    def putFile(self, localPath, remotePath, file):
        self.threadLock.acquire()
        self.fileList.append((localPath, remotePath, file))
        self.threadLock.release()

class ThreadPool:
    def __init__(self, threadCount, bucket, forceUpdate):
        self.threadCount = threadCount
        self.index = 0
        self.threadList = [UploadThread(bucket, forceUpdate) for x in range(threadCount)]

    def putTask(self, localPath, remotePath, file):
        if self.threadCount == 0:
            return

        self.threadList[self.index].putFile(localPath, remotePath, file)
        self.index = (self.index+1)%self.threadCount

    def start(self):
        for t in self.threadList:
            t.start()

    def finish(self):
        for t in self.threadList:
            t.finished = True

    def wait(self):
        for t in self.threadList:
            t.join()

def CreateParser():
    """
    create command line parser
    """
    usage = "usage:python upload_file.py --l localpath --r remotepath --f forceupdate"

    parser = OptionParser(add_help_option=True, description = "",
            usage = usage)
    parser.add_option('--l', '--localpath', dest='localpath')
    # remote svr file path
    parser.add_option('--r', '--remotepath', dest='remotepath')
    parser.add_option('--f', '--forceupdate', dest='forceupdate', type=int)
    return parser

def UploadDir(threadPool, localDir, remoteDir):
    files= os.listdir(localDir)
    for file in files:
        picturePath = os.path.join(localDir, file)
        objectPath = os.path.join(remoteDir, file)
        if os.path.isfile(picturePath):
            threadPool.putTask(localDir, remoteDir, file)
        elif os.path.isdir(picturePath):
            UploadDir(threadPool, picturePath, objectPath)

if __name__ == "__main__":
    parser = CreateParser()
    (options, args) = parser.parse_args()
    if None == options.localpath:
        print "localpath is none"
        sys.exit(1)

    if not os.path.isdir(options.localpath):
        print "{0} is not dir".format(options.localpath)
        sys.exit(1)
    
    if None == options.remotepath:
        print "remotepath is none"
        sys.exit(1)

    if len(options.remotepath) <= 1:
        print "remotepath length error"
        sys.exit(1)

    # 阿里云主账号AccessKey拥有所有API的访问权限，风险很高。强烈建议您创建并使用RAM账号进行API访问或日常运维，请登录 https://ram.console.aliyun.com 创建RAM账号。
    auth = oss2.Auth(OSS_ACCESS_ID, OSS_ACCESS_KEY)
    # Endpoint以杭州为例，其它Region请按实际情况填写。
    bucket = oss2.Bucket(auth, OSS_END_POINT, OSS_BUCKET)

    startTime = time.mktime(time.localtime())
    threadPool = ThreadPool(60, bucket, options.forceupdate)
    threadPool.start()

    UploadDir(threadPool, options.localpath, options.remotepath)

    threadPool.finish()
    threadPool.wait()
    endTime = time.mktime(time.localtime())
    print "upload_file use time: {0}".format(endTime-startTime)