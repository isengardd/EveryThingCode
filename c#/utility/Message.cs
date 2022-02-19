using System;
using System.Collections.Generic;

public static class Message
{
    private static Dictionary<Enum, Action<object>> _list = new Dictionary<Enum, Action<object>>();

    public static void Send(Enum type, object param = null)
    {
        if (!_list.ContainsKey(type))
            return;
        var callback = _list[type];
        callback(param);
    }

    public static void AddListener(Enum type, Action<object> listener)
    {
        if (_list.ContainsKey(type))
        {
            _list[type] += listener;
        }
        else
        {
            _list[type] = listener;
        }
    }

    public static void RemoveListener(Enum type, Action<object> listener)
    {
        if (!_list.ContainsKey(type))
            return;
        _list[type] -= listener;

        if (_list[type] == null)
            _list.Remove(type);
    }

}
