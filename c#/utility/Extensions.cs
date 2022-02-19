using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> input)
    {
        int i = 0;
        foreach (var t in input)
        {
            yield return (i++, t);
        }
    }

    public static void RemoveOne<T>(this IList<T> list, Predicate<T> match)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (match(list[i]))
            {
                list.RemoveAt(i);
                return;
            }
        }
    }

    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < list.Count - 1; i++)
        {
            int swapIndex = random.Next(i, list.Count);
            list.Swap(i, swapIndex);
        }
    }

    public delegate int weightFunc<in T>(T obj);
    public static int RollIndex<T>(this IList<T> list, weightFunc<T> weight)
    {
        int totalWeight = list.Sum((v) => weight(v));
        if (totalWeight <= 0)
        {
            return -1;
        }

        int randomWeight = UnityEngine.Random.Range(0, totalWeight);
        int curWeight = 0;
        for (int i = 0; i < list.Count; i++)
        {
            curWeight += weight(list[i]);
            if (randomWeight < curWeight)
            {
                return i;
            }
        }
        return -1;
    }

    // string 对换大小写
    public static string SwitchCase(this string value)
    {
        char[] newChars = new char[value.Length];
        foreach (var (i, ch) in value.ToCharArray().Enumerate())
        {
            if (Char.IsUpper(ch))
            {
                newChars[i] = Char.ToLower(ch);
            }
            else
            {
                newChars[i] = Char.ToUpper(ch);
            }
        }
        return new string(newChars);
    }

    // 截断字符串
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    public static bool NearlyEqual(this double value1, double value2, double epsilon = 0.0001)
    {
        if (value1 != value2)
        {
            return Math.Abs(value1 - value2) < epsilon;
        }

        return true;
    }

    public static bool NearlyEqual(this float value1, float value2, float epsilon = 0.0001f)
    {
        if (value1 != value2)
        {
            return Math.Abs(value1 - value2) < epsilon;
        }

        return true;
    }
}