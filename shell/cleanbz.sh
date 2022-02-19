#!/bin/sh
bzdir=/opt/tank/bz2
cd $bzdir

find . -type d -mtime +90 > $bzdir/2017.log
find . -type d -mtime +90 | xargs rm -rf

cd /opt/tank/data
find . -maxdepth 1 -mtime +10 | grep .dat | xargs rm -f

cd /opt/tank/log/daily
find . -maxdepth 1 -mtime +90 | grep .log | xargs rm -f

cd /opt/tank/script
echo '' > realtime.log

cd /opt/tank/log/daily/bad
find . -maxdepth 1 -mtime +90 | grep .bad | xargs rm -f

cd /opt/tank/log/realtime/bad
find . -maxdepth 1 -mtime +90 | grep .bad | xargs rm -f
