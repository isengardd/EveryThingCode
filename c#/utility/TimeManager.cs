using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class TimerStruct
{
    public uint timerId;
    public float timerDelay;
    public Action callBack;
    public int repeatCount;
    public object target;
    public float curTime;

    public TimerStruct(uint _timerId, float _timerDelay, Action _callBack, int _repeatCount, object _target)
    {
        timerId = _timerId;
        timerDelay = _timerDelay;
        curTime = _timerDelay;
        callBack = _callBack;
        repeatCount = _repeatCount;
        target = _target;
    }
}

public static class TimeManager
{
    private static Dictionary<uint, TimerStruct> _dicTimer;

    private static float _playingTime = 0; //本次游戏时间

    // 远程时间同步配置
    private static uint INIT_INTV = 10; // 远程时间未初始化，每x秒请求初始化远程时间
    private static uint RESYNC_INTV = 120 * TimeTool.MINUTE_SECOND; // 初始化完成后，每x分钟重新同步一次
    private static int TIMEOUT_INTV = 8000; // 每次同步请求，设置连接超时(毫秒)
    public static bool useLocalTime = false; // 使用本地时间
    private static bool remoteSyncing = false; // 远程时间同步中
    private static uint heartBeatCount = 0;

    private static UInt64 remoteMilliSeconds = 0; //远端UTC时间（毫秒）
    private static float remoteOffsetTime = 0; //获取远端时间后，本地走过的时间(秒)

    //ntp服务器索引
    private static int ntpSvrIdx {
        get {
            return PlayerPrefs.GetInt(PrefsKey.REMOTE_NTP_SERVER_INDEX, 0);
        }
        set
        {
            if (value > TimeTool.NTP_SERVERS.Length)
            {
                PlayerPrefs.SetInt(PrefsKey.REMOTE_NTP_SERVER_INDEX, 0);
            }
            else
            {
                PlayerPrefs.SetInt(PrefsKey.REMOTE_NTP_SERVER_INDEX, value);
            }
        }
    }
    public static int ntpErrorCode;

    private static uint _timeId = 0;

    static TimeManager()    //泛型
    {
        _dicTimer = new Dictionary<uint, TimerStruct>();
    }

    public static void Init()
    {
        InitRemoteUtcTimeAsync();
        TimeManager.Add(1f, () => {

            if (IsRemoteTimeInit())
            {
                if (heartBeatCount >= RESYNC_INTV)
                {
                    heartBeatCount = 0;
                    InitRemoteUtcTimeAsync(true);
                }
            }
            else
            {
                if (heartBeatCount >= INIT_INTV)
                {
                    heartBeatCount = 0;
                    InitRemoteUtcTimeAsync();
                }
            }
            heartBeatCount++;
        }, -1);
    }

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="interval">定时器间隔时间</param>
    /// <param name="callBack">定时器回调</param>
    /// <param name="repeatCount">定时器重复次数，repeatCount小于等于0时会无限重复</param>
    /// <param name="target">目标对象，可以给每个定时器设置目标对象，调用Remove(object target)时会移除目标对象上的所有定时器</param>
    /// <returns></returns>
    public static uint Add(float interval, Action callBack, int repeatCount, object target = null)
    {
        _timeId++;
        _dicTimer.Add(_timeId, new TimerStruct(_timeId, interval, callBack, repeatCount, target));
        return _timeId;
    }

    // 延迟_timeDelay后回调，单位：秒
    public static uint Add(float delaySecond, Action callBack, bool isLoop = false, object target = null)
    {
        return Add(delaySecond, callBack, isLoop ? 0 : 1, target);
    }

    /// <summary>
    /// 在下一帧执行回调
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static uint CallLater(Action callback, object target = null)
    {
        return Add(0.001f, callback, 1, target);
    }

    /// <summary>
    /// 返回int型，可以根据剩余的次数立即执行未执行的回调函数
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="removeAll"></param>false时仅取消最后注册的（和延迟时间无关）
    /// <returns></returns>
    public static int Remove(Action callback, bool removeAll = true)
    {
        List<uint> lstRemoveTimerId = new List<uint>();
        foreach (KeyValuePair<uint, TimerStruct> kv in _dicTimer)
        {
            if (kv.Value.callBack.Equals(callback))
            {
                lstRemoveTimerId.Insert(0, kv.Key);
            }
        }
        foreach (uint timerId in lstRemoveTimerId)
        {
            Remove(timerId);
            if (!removeAll)
                break;
        }
        return lstRemoveTimerId.Count;
    }

    /// <summary>
    /// 移除计时器
    /// </summary>
    /// <param name="timerId"></param>
    /// <returns></returns>
    public static void Remove(uint timerId)
    {
        if (_dicTimer.TryGetValue(timerId, out TimerStruct timerStruct))
        {
            timerStruct.callBack = null;
            _dicTimer.Remove(timerId);
        }
    }

    /// <summary>
    /// 移除计时器
    /// </summary>
    /// <param name="target">目标对象</param>
    public static void Remove(object target)
    {
        if (target == null)
            return;
        List<uint> lstRemoveTimerId = new List<uint>();
        foreach (KeyValuePair<uint, TimerStruct> kv in _dicTimer)
        {
            if (kv.Value.target == target)
                lstRemoveTimerId.Insert(0, kv.Key);
        }
        foreach (uint timerId in lstRemoveTimerId)
            Remove(timerId);
    }

    public static void ClearInvoke()
    {
        _dicTimer.Clear();
    }

    public static void Update(float _deltaTime)
    {
        UpdateRemoteTime(_deltaTime);
        UpdateTimer(_deltaTime);
    }

    private static void UpdateRemoteTime(float _deltaTime)
    {
        if (IsRemoteTimeInit())
        {
            remoteOffsetTime += _deltaTime;
        }
    }

    private static void UpdateTimer(float _deltaTime)
    {
        if (_deltaTime == 0)
            return;
        float time = Time.time;
        _playingTime += _deltaTime;

        List<uint> lstRemoveTimerId = new List<uint>();
        List<TimerStruct> lstTimerStruct = new List<TimerStruct>();
        foreach (KeyValuePair<uint, TimerStruct> kv in _dicTimer)
        {
            kv.Value.curTime -= _deltaTime;
            if (kv.Value.curTime <= 0)
            {
                lstTimerStruct.Add(kv.Value);
                if (kv.Value.repeatCount > 0)
                {
                    kv.Value.repeatCount--;
                    if (kv.Value.repeatCount <= 0)
                    {
                        lstRemoveTimerId.Add(kv.Key);
                        continue;
                    }
                }
                kv.Value.curTime += kv.Value.timerDelay;
                if (kv.Value.curTime < 0)
                    kv.Value.curTime = 0;
            }
        }
        foreach (TimerStruct timerStruct in lstTimerStruct)
        {
            try
            {
                if (timerStruct.callBack != null)
                    timerStruct.callBack();
            }
            catch (Exception e)
            {
                Remove(timerStruct.timerId);
                Debug.LogError(e);
            }
        }
        foreach (uint timerId in lstRemoveTimerId)
            Remove(timerId);
    }

    public static float GetPlayingTime()
    {
        return _playingTime;
    }

    // 获取远端同步的utc时间(秒)
    public static uint GetRemoteUtcTimeStamp()
    {
        if (GameManager.inst.cheat && useLocalTime)
        {
            return TimeTool.GetUtcSecond();
        }
        else if (IsRemoteTimeInit())
        {
            return (uint)((int)(remoteMilliSeconds / 1000) + (int)remoteOffsetTime);
        }
        else
        {
            return 0;
        }
    }

    public static DateTime GetRemoteUtcTime()
    {
        uint remoteUtcTimeStamp = GetRemoteUtcTimeStamp();
        if (0 == remoteUtcTimeStamp)
            return new DateTime(0);
        return TimeTool.GetDatetimeFromUtcsecond(remoteUtcTimeStamp);
    }

    public static bool IsRemoteTimeInit()
    {
        return (GameManager.inst.cheat && useLocalTime) || remoteMilliSeconds > 0;
    }

    /// <summary>
    /// 默认都由主线程调用，(不改变调度线程的话)await回来的代码也是在主线程执行
    /// </summary>
    public async static void InitRemoteUtcTimeAsync(bool bForceInit = false)
    {
        if (remoteSyncing)
            return;

        if ((!bForceInit && IsRemoteTimeInit()) || Application.internetReachability == NetworkReachability.NotReachable)
            return;

        await _InitRemoteUtcTimeAsync(bForceInit);
    }

    // 异步初始化远端UTC时间
    private async static Task<bool> _InitRemoteUtcTimeAsync(bool bForceInit = false)
    {
        if (remoteSyncing)
            return false;

        if (GameManager.inst.cheat && useLocalTime)
            return false;

        if (!bForceInit && IsRemoteTimeInit())
            return false;

        if (ntpSvrIdx >= TimeTool.NTP_SERVERS.Length)
        {
            ntpSvrIdx = 0;
        }

        string ntpSvr = TimeTool.NTP_SERVERS[ntpSvrIdx];

        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        try
        {
            remoteSyncing = true;

            var ntpData = new byte[48];
            ntpData[0] = 0x1B;
            //网络链接
            // 这里启用异步线程，防止主线程堵塞
            // 另外由于unity api无法在非主线程运行，所以调用unity api的逻辑需要回到主线程执行
            await Task.Run(() => {
                var addresses = Dns.GetHostEntry(ntpSvr).AddressList;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                socket.ReceiveTimeout = TIMEOUT_INTV;
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            });
            remoteOffsetTime = 0;

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            // 这里是距离1900-01-01的时间戳，需要转成utc定义的距离1970-01-01的时间戳
            remoteMilliSeconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            remoteMilliSeconds = remoteMilliSeconds - TimeTool.MILLISEC_1900_1970;

            remoteSyncing = false;
            Debug.Log(string.Format("InitRemoteUtcTime svr {0} OK, utctime={1}", ntpSvr, remoteMilliSeconds));
            return true;
        }
        catch (SocketException e)
        {
            socket.Close();
            Debug.Log(string.Format("InitRemoteUtcTime svr {0} catch socket error: {1} {2}", ntpSvr, e.ErrorCode, e.Message));
            // 下次切换ntp服务器
            ntpSvrIdx += 1;
            remoteSyncing = false;
            return false;
        }
        catch (Exception e)
        {
            socket.Close();
            Debug.Log(string.Format("InitRemoteUtcTime svr {0} catch error: {1}", ntpSvr, e.Message));
            // 下次切换ntp服务器
            ntpSvrIdx += 1;
            remoteSyncing = false;
            return false;
        }
    }
}