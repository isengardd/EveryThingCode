#!/usr/bin/python
# -*- coding: utf-8 -*-

# -----------------------------------------------------------------------
# @file       refreshcdn4399_new.py
# @brief
#
# @author     
# @date       2018-12-23
# @copyright  
# -----------------------------------------------------------------------

import os,commands,time
from optparse import OptionParser
import json
import urlparse
import os.path
from collections import defaultdict

apisrv=""
username=""
apipass=""

url_prefix="http://rez.tankt.iwan4399.com"
push_url_parent=['/', '/rez']  #这些目录下（不包含子目录）的文件，以url推送的方式刷新(这些目录子文件数过多，怕不适合目录刷新)
MAX_DIR_COUNT=10        #辣鸡4399cdn平台，每次提交目录更新不能超过10个目录

def CreateParser():
    """
    create command line parser
    """

    usage = "usage:python refreshcdn4399_new.py -f"

    parser = OptionParser(add_help_option=True, description = "",
            usage = usage)

    parser.add_option('-f', '--filename', dest='filename')

    return parser

def DoRefreshCdn(urlList, dirList):
    pushData = {}
    if len(urlList) > 0:
        pushData['urls'] = urlList
        pushData['urlAction'] = 'expire'
    if len(dirList) > 0:
        pushData['dirs'] = dirList
        pushData['dirAction'] = 'expire'
    pushDataJson = json.dumps(pushData)
    cdnrefreshcmd = "curl -k -H \"X-Auth-User:{1}\" -H \"X-Auth-Password:{2}\" -H \"Host: ncdn-api.gz4399.com\" https://{0}/api/portal/v1/push -d \'{3}\'".format(apisrv, username, apipass, pushDataJson)
    a, b = commands.getstatusoutput(cdnrefreshcmd)
    print b + "\n"
    #print cdnrefreshcmd

def PackDirs(dirList):
    rootNode = DirNode()
    for dirUrl in dirList:
        urlParse = urlparse.urlparse(dirUrl).path
        dirParse = urlParse.split('/')
        rootNode.pushDir(dirParse[:-1])

    # newDirList = []
    # print rootNode.value
    # print rootNode.height
    # rootNode.dumpDir("", newDirList)
    # for dir in newDirList:
    #     print dir

    if rootNode.value > MAX_DIR_COUNT:
        for compressHeight in range(rootNode.height - 1, 0, -1):
            rootNode.compressValue(compressHeight, [rootNode.value - MAX_DIR_COUNT])
            #print "rootvalue={0}".format(rootNode.value)
            if rootNode.value <= MAX_DIR_COUNT:
                break

    newDirList = []
    rootNode.dumpDir("", newDirList)
    # for dir in newDirList:
    #     print dir
    return newDirList

class DirNode:
    def __init__(self):
        self.childs = []
        self.parent = None
        self.compressed = 0 #所有子目录都是压缩过的
        self.value = 1   #子目录的总个数
        self.name = ""
        self.height = 1

    def childChangeHeight(self, height):
        if self.height < height + 1:
            self.height = height + 1
            if self.parent != None:
                self.parent.childChangeHeight(self.height)
        elif self.height > height + 1:
            for child in self.childs:
                if child.height > height:
                    break
            else:
                self.height = height + 1
                if self.parent != None:
                    self.parent.childChangeHeight(self.height)
    def addValue(self, diffValue):
        self.value += diffValue
        if self.parent != None:
            self.parent.addValue(diffValue)

    def pushDir(self, dirNodeNames):
        if len(dirNodeNames) > 0:
            if dirNodeNames[0] != self.name:
                print "{0} != {1}\n".format(self.name, dirNodeNames[0])
                return
            if self.compressed == 1:
                return

            if len(dirNodeNames) > 1:
                for child in self.childs:
                    if child.name == dirNodeNames[1]:
                        child.pushDir(dirNodeNames[1:])
                        break
                else:
                    if len(self.childs) >= MAX_DIR_COUNT:
                        self.compress()
                        return
                    else:
                        childNode = DirNode()
                        childNode.name = dirNodeNames[1]
                        childNode.parent = self
                        self.childs.append(childNode)
                        if len(self.childs) > 1:
                            self.addValue(1)
                        else:
                            self.height += 1
                            if self.parent != None:
                                self.parent.childChangeHeight(self.height)
                        childNode.pushDir(dirNodeNames[1:])
                return
            else:
                self.compress()
                return
        else:
            print "error len(dir)=0\n"
            return
    def compress(self):
        diffValue = self.value - 1
        self.compressed = 1
        self.value = 1
        self.childs = []
        self.height = 1
        self.addValue(-diffValue)
        if self.parent != None:
            self.parent.childChangeHeight(self.height)
        return -diffValue

    def dumpDir(self, parentPath, resultList):
        if self.name != "":
            parentPath += "/" + self.name

        if len(self.childs) == 0:
            resultList.append(url_prefix + parentPath + "/")
        else:
            for child in self.childs:
                child.dumpDir(parentPath, resultList)

    def compressValue(self, height, value):
        if height <= 0 or value[0] <= 0:
            return
        #当子目录数大于MAX_DIR_COUNT时，递归压缩子目录; 需要压缩的子目录数是value
        if self.compressed or len(self.childs) == 0:
            return

        if height > 1:
            for child in self.childs:
                child.compressValue(height-1, value)
                if value[0] <= 0:
                    break
        elif height == 1:
            if len(self.childs) == 1:
                self.compressed = 1
            else:
                diffValue = self.compress()
                value[0] += diffValue

if __name__ == "__main__":
    parser = CreateParser()
    (options, args) = parser.parse_args()

    if options.filename != "" and os.path.isfile(options.filename):
        #读取文件内容
        file_object = open(options.filename,'rU')
        try:
            refreshCount = 0
            urlList = []
            dirList = []
            urlSet = set()
            dirMap = defaultdict(list)
            for line in file_object:
                refreshCount += 1
                line = line.rstrip('\n') #line带"\n"
                urlParse = urlparse.urlparse(line).path
                #print urlParse + '\n'
                dirName = os.path.dirname(urlParse)
                #print dirName + '\n'
                if dirName in push_url_parent:
                    urlSet.add(line)
                else:
                    dirMap[os.path.dirname(line) + '/'].append(line) #目录必须以 / 结尾
            else:
                if len(urlSet) > 0 or len(dirMap) > 0:
                    for v in urlSet:
                        urlList.append(v)
                    for k,v in dirMap.items():
                        if len(v) <= 5 or refreshCount <= 100:    #目录内小于5条数据的，或者总的更新数小于100条，还是走url更新
                            urlList.extend(v)
                        else:             #数据过多时，推荐走目录更新
                            dirList.append(k)
                    # 如果目录数大于MAX_DIR_COUNT，进一步压缩目录（4399推送接口一次只允许最多10个目录更新）
                    if len(dirList) > MAX_DIR_COUNT:
                        dirList = PackDirs(dirList)
                    #dirList.extend([k for k,v in dirMap.items() if len(v) > 5])
                    DoRefreshCdn(urlList, dirList)
        finally:
             file_object.close()
    else:
        print "filename error：{0}".format(options.filename)