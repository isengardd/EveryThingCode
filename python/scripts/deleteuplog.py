#!/usr/bin/python
# -*- coding: utf-8 -*-

# -----------------------------------------------------------------------
# @file       deleteuplog.py
# @brief
#
# @author     
# @date       2018-05-02
# @copyright  
# -----------------------------------------------------------------------

import commands,time,os
from optparse import OptionParser

# 过期天数
EXPIRE_TIME_DAY = 10

def CreateParser():
    """
    create command line parser
    """

    usage = "usage:python deleteuplog.py -f"

    parser = OptionParser(add_help_option=True, description = "",
            usage = usage)

    parser.add_option('-f', '--filename', dest='filename')

    return parser

if __name__ == "__main__":
    # 通过sed获取指定行号
    parser = CreateParser()
    (options, args) = parser.parse_args()

    if options.filename != "" and os.path.isfile(options.filename):
        # 过滤出时间戳所在的行和内容
        # {{}}是format中{}的转义形式
        a, b = commands.getstatusoutput("sed -n '/^[0-9]\{{10,\}}/{{=;p}}' {0}".format(options.filename))

        if 0 == a:
            t = int(time.time()) - EXPIRE_TIME_DAY*86400
            listnumberandtime = b.split('\n')
            #逆序遍历
            for x in range(len(listnumberandtime)-1,-1,-2):
                if x >=1 and int(listnumberandtime[x]) < t:
                    # 清除过期的所有更新记录
                    endlinenumber= '$' if x == len(listnumberandtime)-1 else int(listnumberandtime[x+1])-1
                    commands.getstatusoutput("sed -i '1,{1}d' {0}".format(options.filename,endlinenumber))
                    break
    else:
        print "filename error：{0}".format(options.filename)


