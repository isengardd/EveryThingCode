using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

namespace CodeSafe
{
    // int 内存安全封装
    [JsonConverter(typeof(SafeIntConverter))]
    public class SafeInt : IComparable
    {
        private int fix;
        private int value;
        // GameGuardian这类内存修改器，提供异或指定偏移位后，进行搜索。 这里再加个random值，防止这类搜索生效
        private static int RANDOM_INT = UnityEngine.Random.Range(12345678, int.MaxValue);
        public static implicit operator int(SafeInt i) => i.value ^ i.fix ^ RANDOM_INT;
        public static implicit operator SafeInt(int i) => new SafeInt(i);

        public SafeInt(int value = 0)
        {
            fix = UnityEngine.Random.Range(9999999, int.MaxValue);
            this.value = value ^ fix ^ RANDOM_INT;
        }

        public void Dispose()
        {
            fix = 0;
            value = 0;
        }

        public override string ToString()
        {
            return ((int)this).ToString();
        }

        // 重载==,!=时，需要重载Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SafeInt))
                return false;

            return Equals((SafeInt)obj);
        }

        public bool Equals(SafeInt other)
        {
            return (int)this == (int)other;
        }

        public int CompareTo(object obj)
        {
            if (obj is SafeInt)
                return ((int)this).CompareTo((int)(SafeInt)obj);
            else if (obj is long)
                return ((int)this).CompareTo((int)(long)obj);
            else
                return ((int)this).CompareTo(obj);
        }

        // 重载==,!=时，需要重载GetHashCode
        public override int GetHashCode()
        {
            return ((int)this).GetHashCode();
        }

        // 重载操作符
        public static SafeInt operator +(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 + (int)i2);
        }

        public static SafeInt operator +(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 + (int)i2);
        }

        public static SafeInt operator +(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 + (int)i2);
        }

        public static SafeInt operator -(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 - (int)i2);
        }

        public static SafeInt operator -(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 - (int)i2);
        }

        public static SafeInt operator -(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 - (int)i2);
        }

        public static SafeInt operator *(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 * (int)i2);
        }

        public static SafeInt operator *(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 * (int)i2);
        }

        public static SafeInt operator *(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 * (int)i2);
        }

        public static SafeInt operator /(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 / (int)i2);
        }

        public static SafeInt operator /(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 / (int)i2);
        }

        public static SafeInt operator /(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 / (int)i2);
        }

        public static SafeInt operator %(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 % (int)i2);
        }

        public static SafeInt operator %(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 % (int)i2);
        }

        public static SafeInt operator %(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 % (int)i2);
        }

        public static SafeInt operator ^(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 ^ (int)i2);
        }

        public static SafeInt operator ^(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 ^ (int)i2);
        }

        public static SafeInt operator ^(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 ^ (int)i2);
        }

        public static SafeInt operator |(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 | (int)i2);
        }

        public static SafeInt operator |(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 | (int)i2);
        }

        public static SafeInt operator |(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 | (int)i2);
        }

        public static SafeInt operator &(SafeInt i1, SafeInt i2)
        {
            return new SafeInt((int)i1 & (int)i2);
        }

        public static SafeInt operator &(int i1, SafeInt i2)
        {
            return new SafeInt((int)i1 & (int)i2);
        }

        public static SafeInt operator &(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 & (int)i2);
        }

        public static bool operator ==(SafeInt i1, SafeInt i2)
        {
            return (int)i1 == (int)i2;
        }

        public static bool operator !=(SafeInt i1, SafeInt i2)
        {
            return (int)i1 != (int)i2;
        }

        public static bool operator ==(SafeInt i1, int i2)
        {
            return (int)i1 == (int)i2;
        }

        public static bool operator !=(SafeInt i1, int i2)
        {
            return (int)i1 != (int)i2;
        }

        public static bool operator ==(int i1, SafeInt i2)
        {
            return (int)i1 == (int)i2;
        }

        public static bool operator !=(int i1, SafeInt i2)
        {
            return (int)i1 != (int)i2;
        }

        public static SafeInt operator <<(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 << (int)i2);
        }

        public static SafeInt operator >>(SafeInt i1, int i2)
        {
            return new SafeInt((int)i1 >> (int)i2);
        }

        public static SafeInt operator ~(SafeInt i1)
        {
            return new SafeInt(~(int)i1);
        }

        private class TestJsonClass
        {
            public int p1;
            public SafeInt p2;
        };

        public static void Test()
        {
            bool testOK = true;
            bool R(bool success)
            {
                if (!success)
                    testOK = false;

                return success;
            }

            SafeInt si1 = new SafeInt(6);
            Debug.Log(string.Format("SafeInt test. si1={0}, {1}", si1, R(si1 == 6)));
            SafeInt si2 = new SafeInt(12);
            Debug.Log(string.Format("SafeInt test. si2={0}, {1}", si2, R(si2 == 12)));

            si1 += si2;
            Debug.Log(string.Format("SafeInt test. si1=si1 += si2={0}, {1}", si1, R(si1 == 18)));
            si1 *= si2;
            Debug.Log(string.Format("SafeInt test. si1=si1 *= si2={0}, {1}", si1, R(si1 == 18*12)));
            si1 -= si2;
            Debug.Log(string.Format("SafeInt test. si1=si1 -= si2={0}, {1}", si1, R(si1 == 18*12-12)));
            si1 /= si2;
            Debug.Log(string.Format("SafeInt test. si1=si1 /= si2={0}, {1}", si1, R(si1 == (18*12-12)/12)));

            si1 = si2;
            SafeInt si3 = si1 + si2;
            Debug.Log(string.Format("SafeInt test. si3=si1 + si2={0}, {1}", si3, R(si3 == 12 + 12)));
            si3 = si1 + 12;
            Debug.Log(string.Format("SafeInt test. si3=si1 + 12={0}, {1}", si3, R(si3 == 12 + 12)));
            si3 = 12 + si2;
            Debug.Log(string.Format("SafeInt test. si3=12+si2 ={0}, {1}", si3, R(si3 == 12 + 12)));
            si3 = si1 * si2;
            Debug.Log(string.Format("SafeInt test. si3=si1 * si2={0}, {1}", si3, R(si3 == 12 * 12)));
            si3 = si1 * 12;
            Debug.Log(string.Format("SafeInt test. si3=si1 * 12={0}, {1}", si3, R(si3 == 12 * 12)));
            si3 = 12 * si2;
            Debug.Log(string.Format("SafeInt test. si3=12 * si2={0}, {1}", si3, R(si3 == 12 * 12)));
            si3 = si1 - si2;
            Debug.Log(string.Format("SafeInt test. si3=si1 - si2={0}, {1}", si3, R(si3 == 0)));
            si3 = si1 - 12;
            Debug.Log(string.Format("SafeInt test. si3=si1 - 12={0}, {1}", si3, R(si3 == 0)));
            si3 = 12 - si2;
            Debug.Log(string.Format("SafeInt test. si3=12 - si2={0}, {1}", si3, R(si3 == 0)));
            si3 = 36 / si2;
            Debug.Log(string.Format("SafeInt test. si3=36/si2={0}, {1}", si3, R(si3 == 36 / 12)));
            si1 = 49;
            si3 = si1 / si2;
            Debug.Log(string.Format("SafeInt test. si3=49/si2={0}, {1}", si3, R(si3 == 49 / 12)));
            si3 = si1 / 12;
            Debug.Log(string.Format("SafeInt test. si3=si1/12={0}, {1}", si3, R(si3 == 49 / 12)));
            si3 = 49 / si2;
            Debug.Log(string.Format("SafeInt test. si3=49/si2={0}, {1}", si3, R(si3 == 49 / 12)));

            si1 = 1;
            si2 = 2;
            si3 = si1 & si2;
            Debug.Log(string.Format("SafeInt test. si3=si1 & si2={0}, {1}", si3, R(si3 == (1 & 2))));
            si3 = si1 & 2;
            Debug.Log(string.Format("SafeInt test. si3=si1 & 2={0}, {1}", si3, R(si3 == (1 & 2))));
            si3 = 1 & si2;
            Debug.Log(string.Format("SafeInt test. si3=1 & si2={0}, {1}", si3, R(si3 == (1 & 2))));
            si3 = si1 | si2;
            Debug.Log(string.Format("SafeInt test. si3=si1 | si2={0}, {1}", si3, R(si3 == (1 | 2))));
            si3 = si1 | 2;
            Debug.Log(string.Format("SafeInt test. si3=si1 | 2={0}, {1}", si3, R(si3 == (1 | 2))));
            si3 = 1 | si2;
            Debug.Log(string.Format("SafeInt test. si3=1 | si2={0}, {1}", si3, R(si3 == (1 | 2))));
            si3 = si1 ^ si2;
            Debug.Log(string.Format("SafeInt test. si3=si1 ^ si2={0}, {1}", si3, R(si3 == (1 ^ 2))));
            si3 = si1 ^ 2;
            Debug.Log(string.Format("SafeInt test. si3=si1 ^ 2={0}, {1}", si3, R(si3 == (1 ^ 2))));
            si3 = 1 ^ si2;
            Debug.Log(string.Format("SafeInt test. si3=1 ^ si2={0}, {1}", si3, R(si3 == (1 ^ 2))));
            si3 = si1 << si2;
            Debug.Log(string.Format("SafeInt test. si3=si1 << si2={0}, {1}", si3, R(si3 == (1 << 2))));
            si3 = si2 >> si1;
            Debug.Log(string.Format("SafeInt test. si3=si2 >> si1={0}, {1}", si3, R(si3 == (2 >> 1))));
            si3 = ~si1;
            Debug.Log(string.Format("SafeInt test. si3=~si1={0}, {1}", si3, R(si3 == (~1))));

            si1 = 23;
            si2 = 3456;
            si3 = si2 % si1;
            Debug.Log(string.Format("SafeInt test. si3=si2%si1={0}, {1}", si3, R(si3 == (3456%23))));
            si1 = si2;
            Debug.Log(string.Format("SafeInt test. si1 == si2 ={0}, {1}", si1 == si2, R((si1 == si2) == true)));
            si2 = 12;
            Debug.Log(string.Format("SafeInt test. si1 == si2 ={0}, {1}", si1 == si2, R((si1 == si2) == false)));

            si1 = -10;
            si2 = 100;
            int i = 3+si1 + 3;
            Debug.Log(string.Format("SafeInt test. si1 > si2 ={0}, {1}", si1 > si2, R((si1 > si2) == false)));
            Debug.Log(string.Format("SafeInt test. si1 >= si2 ={0}, {1}", si1 >= si2, R((si1 >= si2) == false)));
            Debug.Log(string.Format("SafeInt test. si1 < si2 ={0}, {1}", si1 < si2, R((si1 < si2) == true)));
            Debug.Log(string.Format("SafeInt test. si1 <= si2 ={0}, {1}", si1 <= si2, R((si1 <= si2) == true)));

            si1 = -5;
            si1++;
            Debug.Log(string.Format("SafeInt test. si1++ ={0}, {1}", -4, R(si1 == -4)));
            si1++;
            Debug.Log(string.Format("SafeInt test. si1++ ={0}, {1}", -3, R(si1 == -3)));
            si1--;
            Debug.Log(string.Format("SafeInt test. si1-- ={0}, {1}", -4, R(si1 == -4)));

            si1 = -5;
            ++si1;
            Debug.Log(string.Format("SafeInt test. ++si1 ={0}, {1}", -4, R(si1 == -4)));
            ++si1;
            Debug.Log(string.Format("SafeInt test. ++si1 ={0}, {1}", -3, R(si1 == -3)));
            --si1;
            Debug.Log(string.Format("SafeInt test. --si1 ={0}, {1}", -4, R(si1 == -4)));

            // 数据容器测试
            List<SafeInt> listSafeInts = new List<SafeInt>();
            listSafeInts.Add(1);
            listSafeInts.Add(1);
            listSafeInts.Add(new SafeInt(123));
            listSafeInts.Add(1 + 2);
            Debug.Log(string.Format("SafeInt test. list contains(1)={0}, {1}", listSafeInts.Contains(1), R(listSafeInts.Contains(1) == true)));
            Debug.Log(string.Format("SafeInt test. list contains(SafeInt(1))={0}, {1}", listSafeInts.Contains(new SafeInt(1)), R(listSafeInts.Contains(new SafeInt(1)) == true)));
            Debug.Log(string.Format("SafeInt test. list contains(SafeInt(1+2))={0}, {1}", listSafeInts.Contains(new SafeInt(3)), R(listSafeInts.Contains(new SafeInt(3)) == true)));

            Dictionary<int, int> dicSafeInts = new Dictionary<int, int>();
            dicSafeInts.Add(new SafeInt(5), new SafeInt(6));
            dicSafeInts.Add(new SafeInt(-3), new SafeInt(7));
            Debug.Log(string.Format("SafeInt test. dic ContainsKey (5)={0}, {1}", dicSafeInts.ContainsKey(5), R(dicSafeInts[new SafeInt(5)] == 6)));
            Debug.Log(string.Format("SafeInt test. dic ContainsKey (-3)={0}, {1}", dicSafeInts.ContainsKey(-3), R(dicSafeInts[new SafeInt(-3)] == 7)));

            Dictionary<SafeInt, SafeInt> dicSafeInts2 = new Dictionary<SafeInt, SafeInt>();
            dicSafeInts2.Add(8, 9);
            dicSafeInts2[new SafeInt(8)] = new SafeInt(12);
            Debug.Log(string.Format("SafeInt test. dic[8]={0}, {1}", dicSafeInts2[new SafeInt(8)], R(dicSafeInts2[new SafeInt(8)] == 12)));

            // json相关测试
            SafeInt siObj = new SafeInt(6);
            string jsonStr = JsonTool.ToJson(siObj);
            Debug.Log(string.Format("SafeInt test. ToJson={0}, {1}", jsonStr, R(jsonStr == "6")));
            SafeInt deSiObj = JsonTool.ToObject<SafeInt>(jsonStr);
            Debug.Log(string.Format("SafeInt test. ToObj={0}, {1}", deSiObj, R(deSiObj == 6)));

            TestJsonClass jsonClass = new TestJsonClass();
            jsonClass.p1 = 5;
            jsonClass.p2 = 111;
            string jsonClassStr = JsonTool.ToJson(jsonClass);
            Debug.Log(string.Format("SafeInt test. class ToJson={0}, {1}", jsonClassStr, R(jsonClassStr == "{\"p1\":5,\"p2\":111}")));
            TestJsonClass deJsonClassObj = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeInt test. class json ToObj={0}, {1}", deJsonClassObj, R(deJsonClassObj.p1 == 5 && deJsonClassObj.p2 == 111)));
            // 结果汇总
            Debug.Log(string.Format("Test result: {0}", testOK ? "pass" : "failed"));
        }
    }

    // SafeInt json转换接口
    public class SafeIntConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SafeInt safeInt = (SafeInt)value;

            writer.WriteValue((int)safeInt);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SafeInt safeInt = null;
            if (reader.Value is long)
            {
                safeInt = new SafeInt((int)(long)reader.Value);
            }
            else if (reader.Value is int)
            {
                safeInt = new SafeInt((int)reader.Value);
            }

            return safeInt;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SafeInt);
        }
    }

    // uint 内存安全封装
    [JsonConverter(typeof(SafeUIntConverter))]
    public class SafeUInt : IComparable
    {
        private uint fix;
        private uint value;
        private static uint RANDOM_UINT = (uint)UnityEngine.Random.Range(12345678, int.MaxValue);
        public static implicit operator uint(SafeUInt i) => i.value ^ i.fix ^ RANDOM_UINT;
        public static implicit operator SafeUInt(uint i) => new SafeUInt(i);

        public SafeUInt(uint value = 0)
        {
            fix = (uint)UnityEngine.Random.Range(9999999, int.MaxValue);
            this.value = value ^ fix ^ RANDOM_UINT;
        }

        public void Dispose()
        {
            fix = 0;
            value = 0;
        }

        public override string ToString()
        {
            return ((uint)this).ToString();
        }

        // 重载==,!=时，需要重载Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SafeUInt))
                return false;

            return Equals((SafeUInt)obj);
        }

        public bool Equals(SafeUInt other)
        {
            return (uint)this == (uint)other;
        }

        public int CompareTo(object obj)
        {
            if (obj is SafeUInt)
                return ((uint)this).CompareTo((uint)(SafeUInt)obj);
            else if (obj is long)
                return ((uint)this).CompareTo((uint)(long)obj);
            else
                return ((uint)this).CompareTo(obj);
        }

        // 重载==,!=时，需要重载GetHashCode
        public override int GetHashCode()
        {
            return ((uint)this).GetHashCode();
        }

        // 重载操作符
        public static SafeUInt operator +(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 + (uint)i2);
        }

        public static SafeUInt operator +(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 + (uint)i2);
        }

        public static SafeUInt operator +(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 + (uint)i2);
        }

        public static SafeUInt operator -(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 - (uint)i2);
        }

        public static SafeUInt operator -(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 - (uint)i2);
        }

        public static SafeUInt operator -(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 - (uint)i2);
        }

        public static SafeUInt operator *(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 * (uint)i2);
        }

        public static SafeUInt operator *(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 * (uint)i2);
        }

        public static SafeUInt operator *(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 * (uint)i2);
        }

        public static SafeUInt operator /(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 / (uint)i2);
        }

        public static SafeUInt operator /(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 / (uint)i2);
        }

        public static SafeUInt operator /(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 / (uint)i2);
        }

        public static SafeUInt operator %(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 % (uint)i2);
        }

        public static SafeUInt operator %(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 % (uint)i2);
        }

        public static SafeUInt operator %(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 % (uint)i2);
        }

        public static SafeUInt operator ^(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 ^ (uint)i2);
        }

        public static SafeUInt operator ^(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 ^ (uint)i2);
        }

        public static SafeUInt operator ^(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 ^ (uint)i2);
        }

        public static SafeUInt operator |(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 | (uint)i2);
        }

        public static SafeUInt operator |(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 | (uint)i2);
        }

        public static SafeUInt operator |(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 | (uint)i2);
        }

        public static SafeUInt operator &(SafeUInt i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 & (uint)i2);
        }

        public static SafeUInt operator &(uint i1, SafeUInt i2)
        {
            return new SafeUInt((uint)i1 & (uint)i2);
        }

        public static SafeUInt operator &(SafeUInt i1, uint i2)
        {
            return new SafeUInt((uint)i1 & (uint)i2);
        }

        public static bool operator ==(SafeUInt i1, SafeUInt i2)
        {
            return (uint)i1 == (uint)i2;
        }

        public static bool operator !=(SafeUInt i1, SafeUInt i2)
        {
            return (uint)i1 != (uint)i2;
        }

        public static bool operator ==(SafeUInt i1, uint i2)
        {
            return (uint)i1 == (uint)i2;
        }

        public static bool operator !=(SafeUInt i1, uint i2)
        {
            return (uint)i1 != (uint)i2;
        }

        public static bool operator ==(uint i1, SafeUInt i2)
        {
            return (uint)i1 == (uint)i2;
        }

        public static bool operator !=(uint i1, SafeUInt i2)
        {
            return (uint)i1 != (uint)i2;
        }

        public static SafeUInt operator <<(SafeUInt i1, int i2)
        {
            return new SafeUInt((uint)i1 << (int)i2);
        }

        public static SafeUInt operator >>(SafeUInt i1, int i2)
        {
            return new SafeUInt((uint)i1 >> (int)i2);
        }

        public static SafeUInt operator ~(SafeUInt i1)
        {
            return new SafeUInt(~(uint)i1);
        }

        private class TestJsonClass
        {
            public uint p1;
            public SafeUInt p2;
        };

        public static void Test()
        {
            bool testOK = true;
            bool R(bool success)
            {
                if (!success)
                    testOK = false;

                return success;
            }

            SafeUInt si1 = new SafeUInt(6);
            Debug.Log(string.Format("SafeUInt test. si1={0}, {1}", si1, R(si1 == (uint)6)));
            SafeUInt si2 = new SafeUInt(12);
            Debug.Log(string.Format("SafeUInt test. si2={0}, {1}", si2, R(si2 == (uint)12)));

            si1 += si2;
            Debug.Log(string.Format("SafeUInt test. si1=si1 += si2={0}, {1}", si1, R(si1 == (uint)18)));
            si1 *= si2;
            Debug.Log(string.Format("SafeUInt test. si1=si1 *= si2={0}, {1}", si1, R(si1 == (uint)18 * 12)));
            si1 -= si2;
            Debug.Log(string.Format("SafeUInt test. si1=si1 -= si2={0}, {1}", si1, R(si1 == (uint)18 * 12 - 12)));
            si1 /= si2;
            Debug.Log(string.Format("SafeUInt test. si1=si1 /= si2={0}, {1}", si1, R(si1 == (uint)(18 * 12 - 12) / 12)));

            si1 = si2;
            SafeUInt si3 = si1 + si2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 + si2={0}, {1}", si3, R(si3 == (uint)12 + 12)));
            si3 = si1 + (uint)12;
            Debug.Log(string.Format("SafeUInt test. si3=si1 + 12={0}, {1}", si3, R(si3 == (uint)12 + 12)));
            si3 = (uint)12 + si2;
            Debug.Log(string.Format("SafeUInt test. si3=12+si2 ={0}, {1}", si3, R(si3 == (uint)12 + 12)));
            si3 = si1 * si2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 * si2={0}, {1}", si3, R(si3 == (uint)12 * 12)));
            si3 = si1 * (uint)12;
            Debug.Log(string.Format("SafeUInt test. si3=si1 * 12={0}, {1}", si3, R(si3 == (uint)12 * 12)));
            si3 = (uint)12 * si2;
            Debug.Log(string.Format("SafeUInt test. si3=12 * si2={0}, {1}", si3, R(si3 == (uint)12 * 12)));
            si3 = si1 - si2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 - si2={0}, {1}", si3, R(si3 == (uint)0)));
            si3 = si1 - (uint)12;
            Debug.Log(string.Format("SafeUInt test. si3=si1 - 12={0}, {1}", si3, R(si3 == (uint)0)));
            si3 = (uint)12 - si2;
            Debug.Log(string.Format("SafeUInt test. si3=12 - si2={0}, {1}", si3, R(si3 == (uint)0)));
            si3 = 36 / (uint)si2;
            Debug.Log(string.Format("SafeUInt test. si3=36/si2={0}, {1}", si3, R(si3 == 36 / (uint)12)));
            si1 = 49;
            si3 = si1 / si2;
            Debug.Log(string.Format("SafeUInt test. si3=49/si2={0}, {1}", si3, R(si3 == (uint)49 / 12)));
            si3 = si1 / (uint)12;
            Debug.Log(string.Format("SafeUInt test. si3=si1/12={0}, {1}", si3, R(si3 == (uint)49 / 12)));
            si3 = (uint)49 / si2;
            Debug.Log(string.Format("SafeUInt test. si3=49/si2={0}, {1}", si3, R(si3 == (uint)49 / 12)));

            si1 = 1;
            si2 = 2;
            si3 = si1 & si2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 & si2={0}, {1}", si3, R(si3 == (uint)(1 & 2))));
            si3 = si1 & (uint)2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 & 2={0}, {1}", si3, R(si3 == (uint)(1 & 2))));
            si3 = (uint)1 & si2;
            Debug.Log(string.Format("SafeUInt test. si3=1 & si2={0}, {1}", si3, R(si3 == (uint)(1 & 2))));
            si3 = si1 | si2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 | si2={0}, {1}", si3, R(si3 == (uint)(1 | 2))));
            si3 = si1 | (uint)2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 | 2={0}, {1}", si3, R(si3 == (uint)(1 | 2))));
            si3 = (uint)1 | si2;
            Debug.Log(string.Format("SafeUInt test. si3=1 | si2={0}, {1}", si3, R(si3 == (uint)(1 | 2))));
            si3 = si1 ^ si2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 ^ si2={0}, {1}", si3, R(si3 == (uint)(1 ^ 2))));
            si3 = si1 ^ (uint)2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 ^ 2={0}, {1}", si3, R(si3 == (uint)(1 ^ 2))));
            si3 = (uint)1 ^ si2;
            Debug.Log(string.Format("SafeUInt test. si3=1 ^ si2={0}, {1}", si3, R(si3 == (uint)(1 ^ 2))));
            si3 = si1 << (int)(uint)si2;
            Debug.Log(string.Format("SafeUInt test. si3=si1 << si2={0}, {1}", si3, R(si3 == (uint)(1 << 2))));
            si3 = si2 >> (int)(uint)si1;
            Debug.Log(string.Format("SafeUInt test. si3=si2 >> si1={0}, {1}", si3, R(si3 == (uint)(2 >> 1))));
            si3 = ~si1;
            Debug.Log(string.Format("SafeUInt test. si3=~si1={0}, {1}", si3, R(si3 == (~(uint)1))));

            si1 = 23;
            si2 = 3456;
            si3 = si2 % si1;
            Debug.Log(string.Format("SafeUInt test. si3=si2%si1={0}, {1}", si3, R(si3 == (uint)(3456 % 23))));
            si1 = si2;
            Debug.Log(string.Format("SafeUInt test. si1 == si2 ={0}, {1}", si1 == si2, R((si1 == si2) == true)));
            si2 = 12;
            Debug.Log(string.Format("SafeUInt test. si1 == si2 ={0}, {1}", si1 == si2, R((si1 == si2) == false)));

            si1 = 10;
            si2 = 100;
            Debug.Log(string.Format("SafeUInt test. si1 > si2 ={0}, {1}", si1 > si2, R((si1 > si2) == false)));
            Debug.Log(string.Format("SafeUInt test. si1 >= si2 ={0}, {1}", si1 >= si2, R((si1 >= si2) == false)));
            Debug.Log(string.Format("SafeUInt test. si1 < si2 ={0}, {1}", si1 < si2, R((si1 < si2) == true)));
            Debug.Log(string.Format("SafeUInt test. si1 <= si2 ={0}, {1}", si1 <= si2, R((si1 <= si2) == true)));

            si1 = 5;
            si1++;
            Debug.Log(string.Format("SafeUInt test. si1++ ={0}, {1}", 6, R(si1 == (uint)6)));
            si1++;
            Debug.Log(string.Format("SafeUInt test. si1++ ={0}, {1}", 7, R(si1 == (uint)7)));
            si1--;
            Debug.Log(string.Format("SafeUInt test. si1-- ={0}, {1}", 6, R(si1 == (uint)6)));

            si1 = 5;
            ++si1;
            Debug.Log(string.Format("SafeUInt test. ++si1 ={0}, {1}", 6, R(si1 == (uint)6)));
            ++si1;
            Debug.Log(string.Format("SafeUInt test. ++si1 ={0}, {1}", 7, R(si1 == (uint)7)));
            --si1;
            Debug.Log(string.Format("SafeUInt test. --si1 ={0}, {1}", 6, R(si1 == (uint)6)));

            // 数据容器测试
            List<SafeUInt> listSafeUInts = new List<SafeUInt>();
            listSafeUInts.Add(1);
            listSafeUInts.Add(1);
            listSafeUInts.Add(new SafeUInt(123));
            listSafeUInts.Add(1 + 2);
            Debug.Log(string.Format("SafeUInt test. list contains(1)={0}, {1}", listSafeUInts.Contains(1), R(listSafeUInts.Contains(1) == true)));
            Debug.Log(string.Format("SafeUInt test. list contains(SafeUInt(1))={0}, {1}", listSafeUInts.Contains(new SafeUInt(1)), R(listSafeUInts.Contains(new SafeUInt(1)) == true)));
            Debug.Log(string.Format("SafeUInt test. list contains(SafeUInt(1+2))={0}, {1}", listSafeUInts.Contains(new SafeUInt(3)), R(listSafeUInts.Contains(new SafeUInt(3)) == true)));

            Dictionary<uint, uint> dicSafeUInts = new Dictionary<uint, uint>();
            dicSafeUInts.Add(new SafeUInt(5), new SafeUInt(6));
            dicSafeUInts.Add(new SafeUInt(3), new SafeUInt(7));
            Debug.Log(string.Format("SafeUInt test. dic ContainsKey (5)={0}, {1}", dicSafeUInts.ContainsKey(5), R(dicSafeUInts[new SafeUInt(5)] == 6)));
            Debug.Log(string.Format("SafeUInt test. dic ContainsKey (3)={0}, {1}", dicSafeUInts.ContainsKey(3), R(dicSafeUInts[new SafeUInt(3)] == 7)));

            Dictionary<SafeUInt, SafeUInt> dicSafeUInts2 = new Dictionary<SafeUInt, SafeUInt>();
            dicSafeUInts2.Add(8, 9);
            dicSafeUInts2[new SafeUInt(8)] = new SafeUInt(12);
            Debug.Log(string.Format("SafeUInt test. dic[8]={0}, {1}", dicSafeUInts2[new SafeUInt(8)], R(dicSafeUInts2[new SafeUInt(8)] == (uint)12)));

            // json相关测试
            SafeUInt siObj = new SafeUInt(6);
            string jsonStr = JsonTool.ToJson(siObj);
            Debug.Log(string.Format("SafeUInt test. ToJson={0}, {1}", jsonStr, R(jsonStr == "6")));
            SafeUInt deSiObj = JsonTool.ToObject<SafeUInt>(jsonStr);
            Debug.Log(string.Format("SafeUInt test. ToObj={0}, {1}", deSiObj, R(deSiObj == (uint)6)));

            TestJsonClass jsonClass = new TestJsonClass();
            jsonClass.p1 = 5;
            jsonClass.p2 = 111;
            string jsonClassStr = JsonTool.ToJson(jsonClass);
            Debug.Log(string.Format("SafeUInt test. class ToJson={0}, {1}", jsonClassStr, R(jsonClassStr == "{\"p1\":5,\"p2\":111}")));
            TestJsonClass deJsonClassObj = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeUInt test. class json ToObj={0}, {1}", deJsonClassObj, R(deJsonClassObj.p1 == 5 && deJsonClassObj.p2 == (uint)111)));
            // 结果汇总
            Debug.Log(string.Format("Test result: {0}", testOK ? "pass" : "failed"));
        }
    }

    // SafeUInt json转换接口
    public class SafeUIntConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SafeUInt safeUInt = (SafeUInt)value;

            writer.WriteValue((uint)safeUInt);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SafeUInt safeUInt = null;
            if (reader.Value is long)
            {
                safeUInt = new SafeUInt((uint)(long)reader.Value);
            }
            else if (reader.Value is uint)
            {
                safeUInt = new SafeUInt((uint)reader.Value);
            }

            return safeUInt;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SafeUInt);
        }
    }

    // long 内存安全封装
    [JsonConverter(typeof(SafeLongConverter))]
    public class SafeLong : IComparable
    {
        private long fix;
        private long value;
        private static int RANDOM_LONG = UnityEngine.Random.Range(12345678, int.MaxValue);
        public static implicit operator long(SafeLong i) => i.value ^ i.fix ^ RANDOM_LONG;
        public static implicit operator SafeLong(long i) => new SafeLong(i);

        public SafeLong(long value = 0)
        {
            fix = UnityEngine.Random.Range(9999999, int.MaxValue);
            fix = ((fix << 32) | fix);
            this.value = value ^ fix ^ RANDOM_LONG;
        }

        public void Dispose()
        {
            fix = 0;
            value = 0;
        }

        public override string ToString()
        {
            return ((long)this).ToString();
        }

        // 重载==,!=时，需要重载Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SafeLong))
                return false;

            return Equals((SafeLong)obj);
        }

        public bool Equals(SafeLong other)
        {
            return (long)this == (long)other;
        }

        public int CompareTo(object obj)
        {
            if (obj is SafeLong)
                return ((long)this).CompareTo((long)(SafeLong)obj);
            else if (obj is long)
                return ((long)this).CompareTo((long)obj);
            else
                return ((long)this).CompareTo(obj);
        }

        // 重载==,!=时，需要重载GetHashCode
        public override int GetHashCode()
        {
            return ((long)this).GetHashCode();
        }

        // 重载操作符
        public static SafeLong operator +(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 + (long)i2);
        }

        public static SafeLong operator +(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 + (long)i2);
        }

        public static SafeLong operator +(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 + (long)i2);
        }

        public static SafeLong operator -(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 - (long)i2);
        }

        public static SafeLong operator -(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 - (long)i2);
        }

        public static SafeLong operator -(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 - (long)i2);
        }

        public static SafeLong operator *(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 * (long)i2);
        }

        public static SafeLong operator *(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 * (long)i2);
        }

        public static SafeLong operator *(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 * (long)i2);
        }

        public static SafeLong operator /(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 / (long)i2);
        }

        public static SafeLong operator /(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 / (long)i2);
        }

        public static SafeLong operator /(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 / (long)i2);
        }

        public static SafeLong operator %(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 % (long)i2);
        }

        public static SafeLong operator %(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 % (long)i2);
        }

        public static SafeLong operator %(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 % (long)i2);
        }

        public static SafeLong operator ^(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 ^ (long)i2);
        }

        public static SafeLong operator ^(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 ^ (long)i2);
        }

        public static SafeLong operator ^(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 ^ (long)i2);
        }

        public static SafeLong operator |(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 | (long)i2);
        }

        public static SafeLong operator |(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 | (long)i2);
        }

        public static SafeLong operator |(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 | (long)i2);
        }

        public static SafeLong operator &(SafeLong i1, SafeLong i2)
        {
            return new SafeLong((long)i1 & (long)i2);
        }

        public static SafeLong operator &(long i1, SafeLong i2)
        {
            return new SafeLong((long)i1 & (long)i2);
        }

        public static SafeLong operator &(SafeLong i1, long i2)
        {
            return new SafeLong((long)i1 & (long)i2);
        }

        public static bool operator ==(SafeLong i1, SafeLong i2)
        {
            return (long)i1 == (long)i2;
        }

        public static bool operator !=(SafeLong i1, SafeLong i2)
        {
            return (long)i1 != (long)i2;
        }

        public static bool operator ==(SafeLong i1, long i2)
        {
            return (long)i1 == (long)i2;
        }

        public static bool operator !=(SafeLong i1, long i2)
        {
            return (long)i1 != (long)i2;
        }

        public static bool operator ==(long i1, SafeLong i2)
        {
            return (long)i1 == (long)i2;
        }

        public static bool operator !=(long i1, SafeLong i2)
        {
            return (long)i1 != (long)i2;
        }

        public static SafeLong operator <<(SafeLong i1, int i2)
        {
            return new SafeLong((long)i1 << (int)i2);
        }

        public static SafeLong operator >>(SafeLong i1, int i2)
        {
            return new SafeLong((long)i1 >> (int)i2);
        }

        public static SafeLong operator ~(SafeLong i1)
        {
            return new SafeLong(~(long)i1);
        }

        private class TestJsonClass
        {
            public long p1;
            public SafeLong p2;
        };

        public static void Test()
        {
            bool testOK = true;
            bool R(bool success)
            {
                if (!success)
                    testOK = false;

                return success;
            }

            SafeLong si1 = new SafeLong(6);
            Debug.Log(string.Format("SafeLong test. si1={0}, {1}", si1, R(si1 == 6L)));
            SafeLong si2 = new SafeLong(12);
            Debug.Log(string.Format("SafeLong test. si2={0}, {1}", si2, R(si2 == (long)12)));

            si1 += si2;
            Debug.Log(string.Format("SafeLong test. si1=si1 += si2={0}, {1}", si1, R(si1 == (long)18)));
            si1 *= si2;
            Debug.Log(string.Format("SafeLong test. si1=si1 *= si2={0}, {1}", si1, R(si1 == (long)18 * 12)));
            si1 -= si2;
            Debug.Log(string.Format("SafeLong test. si1=si1 -= si2={0}, {1}", si1, R(si1 == (long)18 * 12 - 12)));
            si1 /= si2;
            Debug.Log(string.Format("SafeLong test. si1=si1 /= si2={0}, {1}", si1, R(si1 == (long)(18 * 12 - 12) / 12)));

            si1 = si2;
            SafeLong si3 = si1 + si2;
            Debug.Log(string.Format("SafeLong test. si3=si1 + si2={0}, {1}", si3, R(si3 == (long)12 + 12)));
            si3 = si1 + (long)12;
            Debug.Log(string.Format("SafeLong test. si3=si1 + 12={0}, {1}", si3, R(si3 == (long)12 + 12)));
            si3 = (long)12 + si2;
            Debug.Log(string.Format("SafeLong test. si3=12+si2 ={0}, {1}", si3, R(si3 == (long)12 + 12)));
            si3 = si1 * si2;
            Debug.Log(string.Format("SafeLong test. si3=si1 * si2={0}, {1}", si3, R(si3 == (long)12 * 12)));
            si3 = si1 * (long)12;
            Debug.Log(string.Format("SafeLong test. si3=si1 * 12={0}, {1}", si3, R(si3 == (long)12 * 12)));
            si3 = (long)12 * si2;
            Debug.Log(string.Format("SafeLong test. si3=12 * si2={0}, {1}", si3, R(si3 == (long)12 * 12)));
            si3 = si1 - si2;
            Debug.Log(string.Format("SafeLong test. si3=si1 - si2={0}, {1}", si3, R(si3 == (long)0)));
            si3 = si1 - (long)12;
            Debug.Log(string.Format("SafeLong test. si3=si1 - 12={0}, {1}", si3, R(si3 == (long)0)));
            si3 = (long)12 - si2;
            Debug.Log(string.Format("SafeLong test. si3=12 - si2={0}, {1}", si3, R(si3 == (long)0)));
            si3 = 36 / (long)si2;
            Debug.Log(string.Format("SafeLong test. si3=36/si2={0}, {1}", si3, R(si3 == 36 / (long)12)));
            si1 = 49;
            si3 = si1 / si2;
            Debug.Log(string.Format("SafeLong test. si3=49/si2={0}, {1}", si3, R(si3 == (long)49 / 12)));
            si3 = si1 / (long)12;
            Debug.Log(string.Format("SafeLong test. si3=si1/12={0}, {1}", si3, R(si3 == (long)49 / 12)));
            si3 = (long)49 / si2;
            Debug.Log(string.Format("SafeLong test. si3=49/si2={0}, {1}", si3, R(si3 == (long)49 / 12)));

            si1 = 1;
            si2 = 2;
            si3 = si1 & si2;
            Debug.Log(string.Format("SafeLong test. si3=si1 & si2={0}, {1}", si3, R(si3 == (long)(1 & 2))));
            si3 = si1 & (long)2;
            Debug.Log(string.Format("SafeLong test. si3=si1 & 2={0}, {1}", si3, R(si3 == (long)(1 & 2))));
            si3 = (long)1 & si2;
            Debug.Log(string.Format("SafeLong test. si3=1 & si2={0}, {1}", si3, R(si3 == (long)(1 & 2))));
            si3 = si1 | si2;
            Debug.Log(string.Format("SafeLong test. si3=si1 | si2={0}, {1}", si3, R(si3 == (long)(1 | 2))));
            si3 = si1 | (long)2;
            Debug.Log(string.Format("SafeLong test. si3=si1 | 2={0}, {1}", si3, R(si3 == (long)(1 | 2))));
            si3 = (long)1 | si2;
            Debug.Log(string.Format("SafeLong test. si3=1 | si2={0}, {1}", si3, R(si3 == (long)(1 | 2))));
            si3 = si1 ^ si2;
            Debug.Log(string.Format("SafeLong test. si3=si1 ^ si2={0}, {1}", si3, R(si3 == (long)(1 ^ 2))));
            si3 = si1 ^ (long)2;
            Debug.Log(string.Format("SafeLong test. si3=si1 ^ 2={0}, {1}", si3, R(si3 == (long)(1 ^ 2))));
            si3 = (long)1 ^ si2;
            Debug.Log(string.Format("SafeLong test. si3=1 ^ si2={0}, {1}", si3, R(si3 == (long)(1 ^ 2))));
            si3 = si1 << (int)(long)si2;
            Debug.Log(string.Format("SafeLong test. si3=si1 << si2={0}, {1}", si3, R(si3 == (long)(1 << 2))));
            si3 = si2 >> (int)(long)si1;
            Debug.Log(string.Format("SafeLong test. si3=si2 >> si1={0}, {1}", si3, R(si3 == (long)(2 >> 1))));
            si3 = ~si1;
            Debug.Log(string.Format("SafeLong test. si3=~si1={0}, {1}", si3, R(si3 == (~(long)1))));

            si1 = 23;
            si2 = 3456;
            si3 = si2 % si1;
            Debug.Log(string.Format("SafeLong test. si3=si2%si1={0}, {1}", si3, R(si3 == (long)(3456 % 23))));
            si1 = si2;
            Debug.Log(string.Format("SafeLong test. si1 == si2 ={0}, {1}", si1 == si2, R((si1 == si2) == true)));
            si2 = 12;
            Debug.Log(string.Format("SafeLong test. si1 == si2 ={0}, {1}", si1 == si2, R((si1 == si2) == false)));

            si1 = 10;
            si2 = 100;
            Debug.Log(string.Format("SafeLong test. si1 > si2 ={0}, {1}", si1 > si2, R((si1 > si2) == false)));
            Debug.Log(string.Format("SafeLong test. si1 >= si2 ={0}, {1}", si1 >= si2, R((si1 >= si2) == false)));
            Debug.Log(string.Format("SafeLong test. si1 < si2 ={0}, {1}", si1 < si2, R((si1 < si2) == true)));
            Debug.Log(string.Format("SafeLong test. si1 <= si2 ={0}, {1}", si1 <= si2, R((si1 <= si2) == true)));

            si1 = -5;
            si1++;
            Debug.Log(string.Format("SafeLong test. si1++ ={0}, {1}", -4, R(si1 == (long)-4)));
            si1++;
            Debug.Log(string.Format("SafeLong test. si1++ ={0}, {1}", -3, R(si1 == (long)-3)));
            si1--;
            Debug.Log(string.Format("SafeLong test. si1-- ={0}, {1}", -4, R(si1 == (long)-4)));

            si1 = 5;
            ++si1;
            Debug.Log(string.Format("SafeLong test. ++si1 ={0}, {1}", 6, R(si1 == (long)6)));
            ++si1;
            Debug.Log(string.Format("SafeLong test. ++si1 ={0}, {1}", 7, R(si1 == (long)7)));
            --si1;
            Debug.Log(string.Format("SafeLong test. --si1 ={0}, {1}", 6, R(si1 == (long)6)));

            // 数据容器测试
            List<SafeLong> listSafeUInts = new List<SafeLong>();
            listSafeUInts.Add(1);
            listSafeUInts.Add(1);
            listSafeUInts.Add(new SafeLong(123));
            listSafeUInts.Add(1 + 2);
            Debug.Log(string.Format("SafeLong test. list contains(1)={0}, {1}", listSafeUInts.Contains(1), R(listSafeUInts.Contains(1) == true)));
            Debug.Log(string.Format("SafeLong test. list contains(SafeLong(1))={0}, {1}", listSafeUInts.Contains(new SafeLong(1)), R(listSafeUInts.Contains(new SafeLong(1)) == true)));
            Debug.Log(string.Format("SafeLong test. list contains(SafeLong(1+2))={0}, {1}", listSafeUInts.Contains(new SafeLong(3)), R(listSafeUInts.Contains(new SafeLong(3)) == true)));

            Dictionary<long, long> dicSafeUInts = new Dictionary<long, long>();
            dicSafeUInts.Add(new SafeLong(5), new SafeLong(6));
            dicSafeUInts.Add(new SafeLong(3), new SafeLong(7));
            Debug.Log(string.Format("SafeLong test. dic ContainsKey (5)={0}, {1}", dicSafeUInts.ContainsKey(5), R(dicSafeUInts[new SafeLong(5)] == 6)));
            Debug.Log(string.Format("SafeLong test. dic ContainsKey (3)={0}, {1}", dicSafeUInts.ContainsKey(3), R(dicSafeUInts[new SafeLong(3)] == 7)));

            Dictionary<SafeLong, SafeLong> dicSafeUInts2 = new Dictionary<SafeLong, SafeLong>();
            dicSafeUInts2.Add(8, 9);
            dicSafeUInts2[new SafeLong(8)] = new SafeLong(12);
            Debug.Log(string.Format("SafeLong test. dic[8]={0}, {1}", dicSafeUInts2[new SafeLong(8)], R(dicSafeUInts2[new SafeLong(8)] == (long)12)));

            // json相关测试
            SafeLong siObj = new SafeLong(6);
            string jsonStr = JsonTool.ToJson(siObj);
            Debug.Log(string.Format("SafeLong test. ToJson={0}, {1}", jsonStr, R(jsonStr == "6")));
            SafeLong deSiObj = JsonTool.ToObject<SafeLong>(jsonStr);
            Debug.Log(string.Format("SafeLong test. ToObj={0}, {1}", deSiObj, R(deSiObj == (long)6)));

            TestJsonClass jsonClass = new TestJsonClass();
            jsonClass.p1 = 5;
            jsonClass.p2 = 111;
            string jsonClassStr = JsonTool.ToJson(jsonClass);
            Debug.Log(string.Format("SafeLong test. class ToJson={0}, {1}", jsonClassStr, R(jsonClassStr == "{\"p1\":5,\"p2\":111}")));
            TestJsonClass deJsonClassObj = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeLong test. class json ToObj={0}, {1}", deJsonClassObj, R(deJsonClassObj.p1 == 5 && deJsonClassObj.p2 == (long)111)));
            // 结果汇总
            Debug.Log(string.Format("Test result: {0}", testOK ? "pass" : "failed"));
        }
    }

    // SafeLong json转换接口
    public class SafeLongConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SafeLong safeLong = (SafeLong)value;

            writer.WriteValue((long)safeLong);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SafeLong safeLong = null;
            if (reader.Value is long)
            {
                safeLong = new SafeLong((long)reader.Value);
            }

            return safeLong;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SafeLong);
        }
    }

    // float 内存安全封装
    [JsonConverter(typeof(SafeFloatConverter))]
    public class SafeFloat : IComparable
    {
        private float fix;
        private float value;
        private static int RANDOM_FLOAT = (int)UnityEngine.Random.Range(3, 10);
        public static implicit operator float(SafeFloat f) => f.value - f.fix + RANDOM_FLOAT;
        public static implicit operator SafeFloat(float f) => new SafeFloat(f);

        public SafeFloat(float value = 0)
        {
            fix = UnityEngine.Random.Range(321, +2000);
            this.value = value + fix - RANDOM_FLOAT;
        }

        public void Dispose()
        {
            fix = 0;
            value = 0;
        }

        // 重载==,!=时，需要重载Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SafeFloat))
                return false;

            return Equals((SafeFloat)obj);
        }

        public bool Equals(SafeFloat other)
        {
            return ((float)this).Equals((float)other);
        }

        public int CompareTo(object obj)
        {
            if (obj is SafeFloat)
                return ((float)this).CompareTo((float)(SafeFloat)obj);
            else if (obj is double)
                return ((float)this).CompareTo((float)(double)obj);
            else
                return ((float)this).CompareTo((float)obj);
        }

        // 重载==,!=时，需要重载GetHashCode
        public override int GetHashCode()
        {
            return ((float)this).GetHashCode();
        }

        public override string ToString()
        {
            return ((float)this).ToString();
        }

        public static SafeFloat operator +(SafeFloat f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 + (float)f2);
        }

        public static SafeFloat operator +(float f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 + (float)f2);
        }

        public static SafeFloat operator +(SafeFloat f1, float f2)
        {
            return new SafeFloat((float)f1 + (float)f2);
        }

        public static SafeFloat operator -(SafeFloat f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 - (float)f2);
        }

        public static SafeFloat operator -(float f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 - (float)f2);
        }

        public static SafeFloat operator -(SafeFloat f1, float f2)
        {
            return new SafeFloat((float)f1 - (float)f2);
        }

        public static SafeFloat operator *(SafeFloat f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 * (float)f2);
        }

        public static SafeFloat operator *(float f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 * (float)f2);
        }

        public static SafeFloat operator *(SafeFloat f1, float f2)
        {
            return new SafeFloat((float)f1 * (float)f2);
        }

        public static SafeFloat operator /(SafeFloat f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 / (float)f2);
        }

        public static SafeFloat operator /(float f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 / (float)f2);
        }

        public static SafeFloat operator /(SafeFloat f1, float f2)
        {
            return new SafeFloat((float)f1 / (float)f2);
        }

        public static SafeFloat operator %(SafeFloat f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 % (float)f2);
        }

        public static SafeFloat operator %(float f1, SafeFloat f2)
        {
            return new SafeFloat((float)f1 % (float)f2);
        }

        public static SafeFloat operator %(SafeFloat f1, float f2)
        {
            return new SafeFloat((float)f1 % (float)f2);
        }

        public static bool operator ==(SafeFloat f1, SafeFloat f2)
        {
            return (float)f1 == (float)f2;
        }

        public static bool operator !=(SafeFloat f1, SafeFloat f2)
        {
            return (float)f1 != (float)f2;
        }

        public static bool operator ==(SafeFloat f1, float f2)
        {
            return (float)f1 == (float)f2;
        }

        public static bool operator !=(SafeFloat f1, float f2)
        {
            return (float)f1 != (float)f2;
        }

        public static bool operator ==(float f1, SafeFloat f2)
        {
            return (float)f1 == (float)f2;
        }

        public static bool operator !=(float f1, SafeFloat f2)
        {
            return (float)f1 != (float)f2;
        }

        private class TestJsonClass
        {
            public float p1;
            public SafeFloat p2;
        };

        public static void Test()
        {
            bool testOK = true;
            bool R(bool success)
            {
                if (!success)
                    testOK = false;

                return success;
            }

            SafeFloat sf1 = 1.667f;
            SafeFloat sf2 = -2.8596f;
            Debug.Log(string.Format("SafeFloat test. sf1 = {0}, {1}", sf1, R(Mathf.Abs(sf1 - 1.667f) < 0.01f)));
            Debug.Log(string.Format("SafeFloat test. sf2 = {0}, {1}", sf2, R(Mathf.Abs(sf2 - (-2.8596f)) < 0.01f)));
            SafeFloat sf3 = sf1 + sf2;
            Debug.Log(string.Format("SafeFloat test. sf3 = sf1 + sf2 = {0}, {1}", sf3, R(Mathf.Abs(sf3 - (1.667f-2.8596f)) < 0.01f)));
            Debug.Log(string.Format("SafeFloat test. sf1 - sf2 = {0}, {1}", sf1 - sf2, R(Mathf.Abs(sf1 - sf2 - (1.667f+2.8596f)) < 0.01f)));
            Debug.Log(string.Format("SafeFloat test. sf1 * sf2 = {0}, {1}", sf1 * sf2, R(Mathf.Abs(sf1 * sf2 - (1.667f * -2.8596f)) < 0.01f)));
            Debug.Log(string.Format("SafeFloat test. sf1 / sf2 = {0}, {1}", sf1 / sf2, R(Mathf.Abs(sf1 / sf2 - (1.667f / -2.8596f)) < 0.01f)));
            Debug.Log(string.Format("SafeFloat test. sf1 == sf1 = {0}, {1}", sf1==sf1, R((sf1==sf1)==true)));
            Debug.Log(string.Format("SafeFloat test. sf1 == sf2 = {0}, {1}", sf1 == sf2, R((sf1 == sf2) == false)));
            Debug.Log(string.Format("SafeFloat test. sf1 != sf2 = {0}, {1}", sf1 != sf2, R((sf1 != sf2) == true)));
            Debug.Log(string.Format("SafeFloat test. sf1 != 7.7 = {0}, {1}", sf1 != 7.7f, R((sf1 != 7.7f) == true)));
            Debug.Log(string.Format("SafeFloat test. sf1 == 8.7 = {0}, {1}", sf1 == 8.7f, R((sf1 == 8.7f) == false)));
            Debug.Log(string.Format("SafeFloat test. sf1 % 2 = {0}, {1}", sf1 % 2f, R(Mathf.Abs((sf1 % 2f) - (1.667f % 2f)) < 0.01f)));

            // json串生成与解析
            SafeFloat siObj = new SafeFloat(6);
            string jsonStr = JsonTool.ToJson(siObj);
            Debug.Log(string.Format("SafeFloat test. ToJson={0}, {1}", jsonStr, R(jsonStr == "6.0")));
            SafeFloat deSfObj = JsonTool.ToObject<SafeFloat>(jsonStr);
            Debug.Log(string.Format("SafeFloat test. ToObj={0}, {1}", deSfObj, R(Mathf.Abs(deSfObj - 6f) < 0.01f)));

            siObj = new SafeFloat(7.6f);
            jsonStr = JsonTool.ToJson(siObj);
            deSfObj = JsonTool.ToObject<SafeFloat>(jsonStr);
            Debug.Log(string.Format("SafeFloat test. ToObj={0}, {1}", deSfObj, R(Mathf.Abs(deSfObj - 7.6f) < 0.01f)));

            // json类测试
            TestJsonClass jsonClass = new TestJsonClass();
            jsonClass.p1 = 5;
            jsonClass.p2 = 111;
            string jsonClassStr = JsonTool.ToJson(jsonClass);
            TestJsonClass deJsonClassObj = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeFloat test. class json ToObj1={0}, {1}", deJsonClassObj, R(deJsonClassObj.p1 == 5 && deJsonClassObj.p2 == (float)111)));

            jsonClass = new TestJsonClass();
            jsonClass.p1 = 5.5f;
            jsonClass.p2 = 111.15f;
            jsonClassStr = JsonTool.ToJson(jsonClass);
            deJsonClassObj = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeFloat test. class json ToObj2={0}, {1}", deJsonClassObj, R(Mathf.Abs(deJsonClassObj.p1-5.5f) < 0.01f && Mathf.Abs(deJsonClassObj.p2 - 111.15f) < 0.01f)));

            deJsonClassObj = JsonTool.ToObject<TestJsonClass>("{\"p1\":5,\"p2\":111}");
            Debug.Log(string.Format("SafeFloat test. class json ToObj3={0}, {1}", deJsonClassObj, R(Mathf.Abs(deJsonClassObj.p1 - 5f) < 0.01f && Mathf.Abs(deJsonClassObj.p2 - 111f) < 0.01f)));
            // 结果汇总
            Debug.Log(string.Format("Test result: {0}", testOK ? "pass" : "failed"));
        }
    }

    // SafeFloat json转换接口
    public class SafeFloatConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SafeFloat safeFloat = (SafeFloat)value;

            writer.WriteValue((float)safeFloat);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SafeFloat safeFloat = null;
            if (reader.Value is double)
            {
                safeFloat = new SafeFloat((float)(double)reader.Value);
            }
            else if (reader.Value is long)
            {
                safeFloat = new SafeFloat((float)(long)reader.Value);
            }
            else if (reader.Value is float)
            {
                safeFloat = new SafeFloat((float)reader.Value);
            }

            return safeFloat;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SafeFloat);
        }
    }

    // double 内存安全封装
    [JsonConverter(typeof(SafeDoubleConverter))]
    public class SafeDouble : IComparable
    { 
        private double fix;
        private double value;
        private static int RANDOM_DOUBLE = (int)UnityEngine.Random.Range(3, 10);
        public static implicit operator double(SafeDouble f) => f.value - f.fix - RANDOM_DOUBLE;
        public static implicit operator SafeDouble(double f) => new SafeDouble(f);

        public SafeDouble(double value = 0)
        {
            fix = UnityEngine.Random.Range(20210, +99999);
            this.value = value + fix + RANDOM_DOUBLE;
        }

        public void Dispose()
        {
            fix = 0;
            value = 0;
        }

        // 重载==,!=时，需要重载Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SafeDouble))
                return false;

            return Equals((SafeDouble)obj);
        }

        public bool Equals(SafeDouble other)
        {
            return ((double)this).Equals((double)other);
        }

        public int CompareTo(object obj)
        {
            if (obj is SafeDouble)
                return ((double)this).CompareTo((double)(SafeDouble)obj);
            else if (obj is double)
                return ((double)this).CompareTo((double)obj);
            else
                return ((double)this).CompareTo((double)obj);
        }

        // 重载==,!=时，需要重载GetHashCode
        public override int GetHashCode()
        {
            return ((double)this).GetHashCode();
        }

        public override string ToString()
        {
            return ((double)this).ToString();
        }

        public static SafeDouble operator +(SafeDouble f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 + (double)f2);
        }

        public static SafeDouble operator +(double f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 + (double)f2);
        }

        public static SafeDouble operator +(SafeDouble f1, double f2)
        {
            return new SafeDouble((double)f1 + (double)f2);
        }

        public static SafeDouble operator -(SafeDouble f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 - (double)f2);
        }

        public static SafeDouble operator -(double f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 - (double)f2);
        }

        public static SafeDouble operator -(SafeDouble f1, double f2)
        {
            return new SafeDouble((double)f1 - (double)f2);
        }

        public static SafeDouble operator *(SafeDouble f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 * (double)f2);
        }

        public static SafeDouble operator *(double f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 * (double)f2);
        }

        public static SafeDouble operator *(SafeDouble f1, double f2)
        {
            return new SafeDouble((double)f1 * (double)f2);
        }

        public static SafeDouble operator /(SafeDouble f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 / (double)f2);
        }

        public static SafeDouble operator /(double f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 / (double)f2);
        }

        public static SafeDouble operator /(SafeDouble f1, double f2)
        {
            return new SafeDouble((double)f1 / (double)f2);
        }

        public static SafeDouble operator %(SafeDouble f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 % (double)f2);
        }

        public static SafeDouble operator %(double f1, SafeDouble f2)
        {
            return new SafeDouble((double)f1 % (double)f2);
        }

        public static SafeDouble operator %(SafeDouble f1, double f2)
        {
            return new SafeDouble((double)f1 % (double)f2);
        }

        public static bool operator ==(SafeDouble f1, SafeDouble f2)
        {
            return (double)f1 == (double)f2;
        }

        public static bool operator !=(SafeDouble f1, SafeDouble f2)
        {
            return (double)f1 != (double)f2;
        }

        public static bool operator ==(SafeDouble f1, double f2)
        {
            return (double)f1 == (double)f2;
        }

        public static bool operator !=(SafeDouble f1, double f2)
        {
            return (double)f1 != (double)f2;
        }

        public static bool operator ==(double f1, SafeDouble f2)
        {
            return (double)f1 == (double)f2;
        }

        public static bool operator !=(double f1, SafeDouble f2)
        {
            return (double)f1 != (double)f2;
        }

        private class TestJsonClass
        {
            public double p1;
            public SafeDouble p2;
        };

        public static void Test()
        {
            bool testOK = true;
            bool R(bool success)
            {
                if (!success)
                    testOK = false;

                return success;
            }

            SafeDouble sf1 = 1.667f;
            SafeDouble sf2 = -2.8596f;
            Debug.Log(string.Format("SafeDouble test. sf1 = {0}, {1}", sf1, R(Math.Abs(sf1 - 1.667) < 0.01f)));
            Debug.Log(string.Format("SafeDouble test. sf2 = {0}, {1}", sf2, R(Math.Abs(sf2 - (-2.8596)) < 0.01f)));
            SafeDouble sf3 = sf1 + sf2;
            Debug.Log(string.Format("SafeDouble test. sf3 = sf1 + sf2 = {0}, {1}", sf3, R(Math.Abs(sf3 - (1.667 - 2.8596)) < 0.01f)));
            Debug.Log(string.Format("SafeDouble test. sf1 - sf2 = {0}, {1}", sf1 - sf2, R(Math.Abs(sf1 - sf2 - (1.667 + 2.8596)) < 0.01f)));
            Debug.Log(string.Format("SafeDouble test. sf1 * sf2 = {0}, {1}", sf1 * sf2, R(Math.Abs(sf1 * sf2 - (1.667 * -2.8596)) < 0.01f)));
            Debug.Log(string.Format("SafeDouble test. sf1 / sf2 = {0}, {1}", sf1 / sf2, R(Math.Abs(sf1 / sf2 - (1.667 / -2.8596)) < 0.01f)));
            Debug.Log(string.Format("SafeDouble test. sf1 == sf1 = {0}, {1}", sf1 == sf1, R((sf1 == sf1) == true)));
            Debug.Log(string.Format("SafeDouble test. sf1 == sf2 = {0}, {1}", sf1 == sf2, R((sf1 == sf2) == false)));
            Debug.Log(string.Format("SafeDouble test. sf1 != sf2 = {0}, {1}", sf1 != sf2, R((sf1 != sf2) == true)));
            Debug.Log(string.Format("SafeDouble test. sf1 != 7.7 = {0}, {1}", sf1 != 7.7, R((sf1 != 7.7) == true)));
            Debug.Log(string.Format("SafeDouble test. sf1 == 8.7 = {0}, {1}", sf1 == 8.7, R((sf1 == 8.7) == false)));
            Debug.Log(string.Format("SafeDouble test. sf1 % 2 = {0}, {1}", sf1 % 2d, R(Math.Abs((sf1 % 2d) - (1.667 % 2)) < 0.01f)));

            // json串生成与解析
            SafeDouble siObj = new SafeDouble(6);
            string jsonStr = JsonTool.ToJson(siObj);
            Debug.Log(string.Format("SafeDouble test. ToJson={0}, {1}", jsonStr, R(jsonStr == "6.0")));
            SafeDouble deSfObj = JsonTool.ToObject<SafeDouble>(jsonStr);
            Debug.Log(string.Format("SafeDouble test. ToObj={0}, {1}", deSfObj, R(Math.Abs(deSfObj - 6d) < 0.01f)));

            siObj = new SafeDouble(7.6f);
            jsonStr = JsonTool.ToJson(siObj);
            deSfObj = JsonTool.ToObject<SafeDouble>(jsonStr);
            Debug.Log(string.Format("SafeDouble test. ToObj={0}, {1}", deSfObj, R(Math.Abs(deSfObj - 7.6) < 0.01f)));

            // json类测试
            TestJsonClass jsonClass = new TestJsonClass();
            jsonClass.p1 = 5;
            jsonClass.p2 = 111;
            string jsonClassStr = JsonTool.ToJson(jsonClass);
            TestJsonClass deJsonClassObj = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeDouble test. class json ToObj1={0}, {1}", deJsonClassObj, R(deJsonClassObj.p1 == 5 && deJsonClassObj.p2 == (double)111)));

            jsonClass = new TestJsonClass();
            jsonClass.p1 = 5.5f;
            jsonClass.p2 = 111.15f;
            jsonClassStr = JsonTool.ToJson(jsonClass);
            deJsonClassObj = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeDouble test. class json ToObj2={0}, {1}", deJsonClassObj, R(Math.Abs(deJsonClassObj.p1 - 5.5f) < 0.01f && Math.Abs(deJsonClassObj.p2 - 111.15d) < 0.01f)));

            deJsonClassObj = JsonTool.ToObject<TestJsonClass>("{\"p1\":5,\"p2\":111}");
            Debug.Log(string.Format("SafeDouble test. class json ToObj3={0}, {1}", deJsonClassObj, R(Math.Abs(deJsonClassObj.p1 - 5f) < 0.01f && Math.Abs(deJsonClassObj.p2 - 111d) < 0.01f)));
            // 结果汇总
            Debug.Log(string.Format("Test result: {0}", testOK ? "pass" : "failed"));
        }
    }

    // SafeFloat json转换接口
    public class SafeDoubleConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SafeDouble safeDouble = (SafeDouble)value;

            writer.WriteValue((double)safeDouble);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SafeDouble safeDouble = null;
            if (reader.Value is double)
            {
                safeDouble = new SafeDouble((double)reader.Value);
            }
            else if (reader.Value is long)
            {
                safeDouble = new SafeDouble((double)(long)reader.Value);
            }

            return safeDouble;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SafeDouble);
        }
    }

    [JsonConverter(typeof(SafeStringConverter))]
    public class SafeString : IComparable
    {
        private byte fix1;
        private byte fix2;
        private byte[] value;
        public static implicit operator string(SafeString s)
        {
            if (s.value.Length == 0)
                return "";

            byte fixXOR = (byte)(s.fix1 ^ s.fix2);
            byte curFix = s.fix2;
            byte[] plainText = new byte[s.value.Length];
            for (int i = 0; i < s.value.Length; i++)
            {
                curFix = (byte)(curFix ^ fixXOR);
                plainText[i] = (byte)(s.value[i] ^ curFix);
            }
            return Encoding.UTF8.GetString(plainText);
        }
        public static implicit operator SafeString(string s) => new SafeString(s);

        public SafeString(string value = "")
        {
            this.value = Encoding.UTF8.GetBytes(value);
            fix1 = (byte)UnityEngine.Random.Range(1, 256);
            fix2 = (byte)UnityEngine.Random.Range(1, 256);
            byte fixXOR = (byte)(fix1 ^ fix2);
            byte curFix = fix2;
            for (int i = 0; i < this.value.Length; i++)
            {
                curFix = (byte)(curFix ^ fixXOR);
                this.value[i] = (byte)(this.value[i] ^ curFix);
            }
        }

        public void Dispose()
        {
            fix1 = 0;
            fix2 = 0;
            value = null;
        }

        // 重载==,!=时，需要重载Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SafeString))
                return false;

            return Equals((SafeString)obj);
        }

        public bool Equals(SafeString other)
        {
            return ((string)this).Equals((string)other);
        }

        public int CompareTo(object obj)
        {
            if (obj is SafeString)
                return ((string)this).CompareTo((string)(SafeString)obj);
            else
                return ((string)this).CompareTo((string)obj);
        }

        // 重载==,!=时，需要重载GetHashCode
        public override int GetHashCode()
        {
            return ((string)this).GetHashCode();
        }

        public override string ToString()
        {
            return (string)this;
        }

        public static bool operator ==(SafeString s1, SafeString s2)
        {
            return (string)s1 == (string)s2;
        }

        public static bool operator !=(SafeString s1, SafeString s2)
        {
            return (string)s1 != (string)s2;
        }

        public static bool operator ==(SafeString s1, string s2)
        {
            return (string)s1 == (string)s2;
        }

        public static bool operator !=(SafeString s1, string s2)
        {
            return (string)s1 != (string)s2;
        }

        public static bool operator ==(string s1, SafeString s2)
        {
            return (string)s1 == (string)s2;
        }

        public static bool operator !=(string s1, SafeString s2)
        {
            return (string)s1 != (string)s2;
        }

        private class TestJsonClass
        {
            public string p1;
            public SafeString p2;
        };

        public static void Test()
        {
            bool testOK = true;
            bool R(bool success)
            {
                if (!success)
                    testOK = false;

                return success;
            }

            SafeString safeString = "safeString";
            Debug.Log(string.Format("SafeString Test. string={0}, {1}", safeString, R(safeString == "safeString")));
            safeString += "666";
            Debug.Log(string.Format("SafeString Test. string={0}, {1}", safeString, R(safeString == "safeString666")));
            safeString = safeString + "b";
            Debug.Log(string.Format("SafeString Test. string={0}, {1}", safeString, R(safeString == "safeString666b")));
            Debug.Log(string.Format("SafeString Test. string={0}, {1}", safeString, R("safeString666b" == safeString)));
            SafeString safeString2 = "safeString666b";
            Debug.Log(string.Format("SafeString Test. string2={0}, {1}", safeString2, R(safeString2 == safeString)));
            // 容器测试
            safeString = "110";
            safeString2 = "120";
            Dictionary<string, string> dicString = new Dictionary<string, string>(){ { "110", "1234"},{ "130", "456"} };
            Debug.Log(string.Format("SafeString Test. dictionary containskKey {0}, {1}", safeString, R(dicString.ContainsKey(safeString))));
            Debug.Log(string.Format("SafeString Test. dictionary not containskKey {0}, {1}", safeString2, R(!dicString.ContainsKey(safeString2))));
            dicString.Add(safeString2, "789");
            Debug.Log(string.Format("SafeString Test. dictionary containskKey {0}, {1}", safeString2, R(dicString.ContainsKey(safeString2))));

            Dictionary<SafeString, SafeString> dicString2 = new Dictionary<SafeString, SafeString>() { { "110", "1234" }, { "130", "456" } };
            Debug.Log(string.Format("SafeString Test. dictionary2 containskKey {0}, {1}", safeString, R(dicString2.ContainsKey(safeString))));
            Debug.Log(string.Format("SafeString Test. dictionary2 not containskKey {0}, {1}", safeString2, R(!dicString2.ContainsKey(safeString2))));
            dicString2.Add(safeString2, "789");
            safeString2 = "140";
            Debug.Log(string.Format("SafeString Test. dictionary2 containskKey {0}, {1}", "120", R(dicString2.ContainsKey("120"))));
            // json测试
            safeString = "just dance";
            string delJson = JsonTool.ToJson(safeString);
            Debug.Log(string.Format("SafeString Test. json to string {0}, {1}", delJson, R(delJson == "\"just dance\"")));
            TestJsonClass jsonClass = new TestJsonClass();
            jsonClass.p1 = "json 1";
            jsonClass.p2 = "json 2";
            string jsonClassStr = JsonTool.ToJson(jsonClass);
            Debug.Log(string.Format("SafeString Test. json to string {0}, {1}", jsonClassStr, R(jsonClassStr == "{\"p1\":\"json 1\",\"p2\":\"json 2\"}")));
            jsonClass = JsonTool.ToObject<TestJsonClass>(jsonClassStr);
            Debug.Log(string.Format("SafeString Test. json to obj {0}, {1}", jsonClass.p2, R(jsonClass.p1 == "json 1" && jsonClass.p2 == "json 2")));
            // 结果汇总
            Debug.Log(string.Format("SafeString Test result: {0}", testOK ? "pass" : "failed"));
        }
    }

    // SafeString json转换接口
    public class SafeStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SafeString safeString = (SafeString)value;

            writer.WriteValue((string)safeString);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SafeString safeString = null;
            if (reader.Value is string)
            {
                safeString = new SafeString((string)reader.Value);
            }

            return safeString;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SafeString);
        }
    }

    // 包含数据校验的PlayerPrefs封装
    public class SafePlayerPrefs
    {
        /// <summary>
        /// 需要数据校验的key的集合, 暂时不支持包含通配符的key
        /// 增删数据都需要增加DataVersionUpdate，执行数据更新
        /// </summary>
        public static readonly List<PrefsKeyStruct> lstSafePrefsKey = new List<PrefsKeyStruct>() {
            PrefsKey.OWN_COIN_COUNT,     //金币
            PrefsKey.OWN_DIAMOND_COUNT,  //钻石
            PrefsKey.OWN_POWER_COUNT,    //体力
            PrefsKey.PLAYER_LEVEL,       //等级
            PrefsKey.PLAYER_EXPERIENCE,  //经验
            PrefsKey.PLAYER_NAME,        //名字
            PrefsKey.BLUE_KEY_COUNT,     //蓝色宝箱钥匙
            PrefsKey.PURPLE_KEY_COUNT,   //紫色宝箱钥匙
            PrefsKey.HERO_INFO_DICTIONARY,      //英雄
            PrefsKey.EQUIPED_EQUIP_INFO_LIST,   //穿戴装备
            PrefsKey.UNEQUIPED_EQUIP_INFO_LIST, //未穿戴装备
            PrefsKey.ITEM_INFO,                 //材料
            PrefsKey.TALENT_INFO,               //天赋
            PrefsKey.TIME_REWARD_REWARD_COIN,   //挂机奖励
            PrefsKey.TIME_REWARD_DROPED_ITEM,
            PrefsKey.MATCH_TOKEN_INFO,          //令牌信息
            PrefsKey.DIAMOND_MEMBER_INFO,       //钻石会员
            PrefsKey.REMOTE_EXPEDITION_RANK_ID, //远征排行榜分配的榜单id
        };
        public static readonly SortedSet<string> setSafePrefsKeyStr = new SortedSet<string>(lstSafePrefsKey.ConvertAll(d => d.name));

        public static void SetInt(string key, int value)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                IGameEncrypt gameEncrypt = EncryptTool.CreateEncrypt(safeKey);
                PlayerPrefs.SetString(safeKey, gameEncrypt.AESEncrypt(value.ToString()));
            }

            PlayerPrefs.SetInt(key, value);
        }

        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        public static int GetInt(string key, int defaultValue)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                if (PlayerPrefs.HasKey(safeKey))
                {
                    IGameEncrypt gameEncrypt = EncryptTool.CreateEncrypt(safeKey);
                    return int.Parse(gameEncrypt.AESDecrypt(PlayerPrefs.GetString(safeKey)));
                }
                else
                {
                    return defaultValue;
                }  
            }

            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                IGameEncrypt gameEncrypt = EncryptTool.CreateEncrypt(safeKey);
                PlayerPrefs.SetString(safeKey, gameEncrypt.AESEncrypt(value.ToString()));
            }
            PlayerPrefs.SetFloat(key, value);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                if (PlayerPrefs.HasKey(safeKey))
                {
                    IGameEncrypt gameEncrypt = EncryptTool.CreateEncrypt(safeKey);
                    return float.Parse(gameEncrypt.AESDecrypt(PlayerPrefs.GetString(safeKey)));
                }
                else
                {
                    return defaultValue;
                }
            }

            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static float GetFloat(string key)
        {
            return GetFloat(key, 0);
        }

        public static void SetString(string key, string value)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                IGameEncrypt gameEncrypt = EncryptTool.CreateEncrypt(safeKey);
                PlayerPrefs.SetString(safeKey, gameEncrypt.AESEncrypt(value));
            }
            PlayerPrefs.SetString(key, value);
        }

        public static string GetString(string key, string defaultValue)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                if (PlayerPrefs.HasKey(safeKey))
                {
                    IGameEncrypt gameEncrypt = EncryptTool.CreateEncrypt(safeKey);
                    return gameEncrypt.AESDecrypt(PlayerPrefs.GetString(safeKey));
                }
                else
                {
                    return defaultValue;
                }
            }

            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static string GetString(string key)
        {
            return GetString(key, "");
        }

        public static void DeleteKey(string key)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                PlayerPrefs.DeleteKey(safeKey);
            }

            PlayerPrefs.DeleteKey(key);
        }

        public static bool HasKey(string key)
        {
            if (setSafePrefsKeyStr.Contains(key))
            {
                // 因为解析时是按照safeKey解析的，所以这里判断safeKey是否存在
                string safeKey = PrefsKeyStruct.GetSafeKey(key);
                return PlayerPrefs.HasKey(safeKey);
            }
            return PlayerPrefs.HasKey(key);
        }
    }
}