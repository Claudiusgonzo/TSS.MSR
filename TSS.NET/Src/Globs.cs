﻿/*++

Copyright (c) 2010-2015 Microsoft Corporation
Microsoft Confidential

*/
using System;
using System.Globalization;
using System.Resources;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

#if !TSS_USE_BCRYPT
using System.Security.Cryptography;
#endif


namespace Tpm2Lib
{
    /// <summary>
    /// Summary description for Globs.
    /// </summary>
    public class Globs
    {
        internal const int TpmMaxBufferSize = 0x1000;

        public static byte[] HostToNet(object o)
        {
            return ReverseByteOrder(GetBytes(o));
        }

        public static byte[] BytesFromString(string s)
        {
            return AddZeroToEnd(System.Text.Encoding.Unicode.GetBytes(s), sizeof(char));
        }
        
        public static byte[] GetBytes(object o)
        {
            Type t = o.GetType();
            if (t == typeof(byte))
            {
                return new byte[] { (byte)o };
            }
            if (t == typeof(ushort))
            {
                return BitConverter.GetBytes((ushort)o);
            }
            if (t == typeof(uint))
            {
                return BitConverter.GetBytes((uint)o);
            }
            if (t == typeof(ulong))
            {
                return BitConverter.GetBytes((ulong)o);
            }
            if (t == typeof(short))
            {
                return BitConverter.GetBytes((short)o);
            }
            if (t == typeof(int))
            {
                return BitConverter.GetBytes((int)o);
            }
            if (t == typeof(long))
            {
                return BitConverter.GetBytes((long)o);
            }
            if (t == typeof(sbyte))
            {
                return new byte[]{ (byte)(sbyte)o };
            }
            // Unsupported type
            Debug.Assert(false);
            return null;
        }

        public static long NetToHost8(byte[] x)
        {
            if (x.Length != 8)
            {
                throw new ArgumentException("Globs.NetToHost8() needs an 8 byte input");
            }
            return x[7] + (x[6] << 8) + (x[5] << 16) + (x[4] << 24) +
                   (x[3] << 32) + (x[2] << 40) + (x[1] << 48) + (x[0] << 56);
        }

        public static ulong NetToHost8U(byte[] x)
        {
            if (x.Length != 8)
            {
                throw new ArgumentException("Globs.NetToHost8 needs an 8 byte input");
            }
            return x[7] + ((ulong)x[6] << 8) + ((ulong)x[5] << 16) + ((ulong)x[4] << 24) +
                   ((ulong)x[3] << 32) + ((ulong)x[2] << 40) + ((ulong)x[1] << 48) + ((ulong)x[0] << 56);
        }

        public static int NetToHost4(byte[] x)
        {
            if (x.Length != 4)
            {
                throw new ArgumentException("Globs.NetToHost4() needs a 4 byte input");
            }
            return x[3] + (x[2] << 8) + (x[1] << 16) + (x[0] << 24);
        }

        public static uint NetToHost4U(byte[] x)
        {
            if (x.Length != 4)
            {
                throw new ArgumentException("Globs.NetToHost4U() needs a 4 byte input");
            }
            return x[3] + (uint)(x[2] << 8) + (uint)(x[1] << 16) + (uint)(x[0] << 24);
        }

        public static short NetToHost2(byte[] x)
        {
            if (x.Length != 2)
            {
                throw new ArgumentException("Globs.NetToHost2() needs a 2 byte input");
            }
            return (short)(x[1] + (x[0] << 8));
        }

        public static ushort NetToHost2U(byte[] x)
        {
            if (x.Length != 2)
            {
                throw new ArgumentException("Globs.NetToHost2U() needs a 2 byte input");
            }
            return (ushort)(x[1] + (x[0] << 8));
        }

        public static uint NetToHostVar(byte[] x)
        {
            if (x.Length == 2)
            {
                return NetToHost2U(x);
            }
            if (x.Length == 4)
            {
                return NetToHost4U(x);
            }
            throw new ArgumentException("Globs.NetToHostVar(): Unsupported array length " + x.Length);
        }

        public static object NetToHostValue(Type t, byte[] data)
        {
            if (t == typeof(byte))
            {
                return data[0];
            }
            if (t == typeof(ushort))
            {
                return NetToHost2U(data);
            }
            if (t == typeof(uint))
            {
                return NetToHost4U(data);
            }
            if (t == typeof(ulong))
            {
                return NetToHost8U(data);
            }
            if (t == typeof(short))
            {
                return NetToHost2(data);
            }
            if (t == typeof(int))
            {
                return NetToHost4(data);
            }
            if (t == typeof(long))
            {
                return NetToHost8(data);
            }
            if (t == typeof(sbyte))
            {
                return (sbyte)data[0];
            }
            // Unsupported type
            Debug.Assert(false);
            return null;
        }

        public static object FromBytes(Type t, byte[] data)
        {
            //return NetToHostValue(t, ReverseByteOrder(data));

            if (t == typeof(byte))
            {
                return data[0];
            }
            if (t == typeof(ushort))
            {
                return BitConverter.ToUInt16(data, 0);
            }
            if (t == typeof(uint))
            {
                return BitConverter.ToUInt32(data, 0);
            }
            if (t == typeof(ulong))
            {
                return BitConverter.ToUInt64(data, 0);
            }
            if (t == typeof(short))
            {
                return BitConverter.ToInt16(data, 0);
            }
            if (t == typeof(int))
            {
                return BitConverter.ToInt32(data, 0);
            }
            if (t == typeof(long))
            {
                return BitConverter.ToInt64(data, 0);
            }
            if (t == typeof(sbyte))
            {
                return (sbyte)data[0];
            }
            // Unsupported type
            Debug.Assert(false);
            return null;
        }

        public static int SizeOf(Type t)
        {
            //return NetToHostValue(t, ReverseByteOrder(data));

            if (t == typeof(byte) || t == typeof(sbyte))
            {
                return sizeof(byte);
            }
            if (t == typeof(ushort) || t == typeof(short))
            {
                return sizeof(ushort);
            }
            if (t == typeof(uint) || t == typeof(int))
            {
                return sizeof(uint);
            }
            if (t == typeof(ulong) || t == typeof(long))
            {
                return sizeof(ulong);
            }
            // Unsupported type
            Debug.Assert(false);
            return 0;
        }

        public static object IncrementValue(object o, sbyte delta)
        {
            Type t = o.GetType();
            byte[] data = GetBytes(o);
            int s = delta;
            for (int i = 0; s != 0 && i < data.Length; ++i )
            {
                s += data[i];
                data[i] = (byte)s;
                s >>= 8;
            }
            return FromBytes(t, data);
        }

        // RNG used when seeded random numbers are required

#if !TSS_USE_BCRYPT
        /// <summary>
        /// Default RNG used by the library (for nonces, and if a random auth-value is required, etc.)
        /// </summary>
        private static readonly RNGCryptoServiceProvider CryptoRand = new RNGCryptoServiceProvider();
#endif

        /// <summary>
        /// Seed for the PRNG for this run.  Maybe set from the system cryptographic random sounce, or
        /// may be set through SetRngSeed().
        /// </summary>
        private static byte[] _randSeed;

        /// <summary>
        /// A buffer of random data that is emptied on calls to GetRandom() and filled when the buffer
        /// is empty through FillRandBuf().
        /// </summary>
        private static ByteBuf _randBuf = new ByteBuf();

        /// <summary>
        /// Counter for each round of buffer filling.
        /// </summary>
        private static int _randRound;

        public static Object RandLock = new Object();

        /// <summary>
        /// Set the PRNG seed used by the tester. If this routine is not called then the seed is 
        /// extracted from the system RNG. Note that there is on RNG shared by all threads using
        /// TPM library services, so non-determinism is to be expected in multi-threaded programs
        /// even when the RNG is seeded.
        /// </summary>
        /// <param name="seed"></param>
        public static void SetRngSeed(string seed)
        {
            lock (RandLock)
            {
                _randSeed = seed == null ? new byte[0] :
                                CryptoLib.HashData(TpmAlgId.Sha256, Encoding.UTF8.GetBytes(seed));
                _randRound = 0;
                _randBuf = null;
            }
        }

        /// <summary>
        /// Set the tester PRNG seed to random value from the system RNG
        /// </summary>
        public static void SetRngRandomSeed()
        {
            lock (RandLock)
            {
                if (_randSeed == null)
                {
                    _randSeed = new byte[32];
#if TSS_USE_BCRYPT
                    var rnd = new Random();
                    rnd.NextBytes(_randSeed);
#else
                    CryptoRand.GetBytes(_randSeed);
#endif
                    _randRound = 0;
                    _randBuf = null;
                }
            }
        }

        public static byte[] GetRandomBytes(int numBytes)
        {
            if (numBytes > RandMaxBytes)
            {
                throw new Exception("Too many random bytes requested.");
            }
            // Make sure that the RNG is properly seeded
            if (_randSeed == null)
            {
                SetRngRandomSeed();
            }
            // Fill or refill the buffer
            lock (RandLock)
            {
                    // ReSharper disable once PossibleNullReferenceException
                if (_randBuf == null || _randBuf.BytesRemaining() < numBytes)
                {
                    FillRandBuf();
                }
                // And return the data
                return _randBuf.Extract(numBytes);
            }
        }

        private const int RandMaxBytes = 1024 * 1024;

        private static void FillRandBuf()
        {
            // Fill the buffer with random data
            byte[] contextU = HostToNet(_randRound);
            byte[] data = KDF.KDFa(TpmAlgId.Sha256, _randSeed, "RNG", contextU, new byte[0], (uint)RandMaxBytes * 8);
            _randRound++;
            _randBuf = new ByteBuf(data);
        }

        public static int GetRandomInt(int maxVal = Int32.MaxValue)
        {
            byte[] randUint = GetRandomBytes(4);
            var val = Math.Abs(BitConverter.ToInt32(randUint, 0)) % maxVal;
            return val;
        }

        public static ulong GetRandomUlong(ulong maxVal = UInt64.MaxValue)
        {
            return BitConverter.ToUInt64(GetRandomBytes(8), 0) % maxVal;
        }

        public static double GetRandomDouble()
        {
            int randInt = GetRandomInt(1000000);
            return randInt / 1.0e6;
        }

        public static byte[] GetZeroBytes(int numBytes)
        {
            return new byte[numBytes];
        }

        public static byte[] ByteArray2Dto1D(byte[][] arr)
        {
            int len = arr.Sum(t => t.Length);
            var retVal = new byte[len];
            int pos = 0;
            foreach (byte[] t in arr)
            {
                t.CopyTo(retVal, pos);
                pos += t.Length;
            }
            return retVal;
        }

        public static byte[] ByteArrayFromHex(string hexString, bool removeSpaces = false)
        {
            if (removeSpaces)
            {
                int lastLen;
                do
                {
                    lastLen = hexString.Length;
                    hexString = hexString.Replace(" ", "");
                } while (lastLen != hexString.Length);
            }
            var res = new byte[1 + (hexString.Length - 1) / 2];
            string temp = hexString;
            if ((temp.Length / 2) * 2 != temp.Length)
            {
                temp = "0" + temp;
            }

            int pos = 0;
            while (pos < temp.Length)
            {
                int posx = pos / 2;
                res[posx] = (byte)(HexNibbleToInt(temp[pos++]) << 4);
                res[posx] |= (byte)HexNibbleToInt(temp[pos++]);
            }
            return res;
        }

        public static string HexFromByteArray(byte[] b)
        {
            var s = new StringBuilder();
            foreach (byte t in b)
            {
                s.AppendFormat("{0:x2}", t);
            }
            return s.ToString();
        }

        /// <summary>
        /// Just prints out length and first and last bytes of a (long) byte-array.  
        /// Useful for debugging.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string HexFromByteArrayCompact(byte[] b)
        {
            var s = new StringBuilder();
            const int bytesToPrint = 8;

            s.AppendFormat("[{0}] ", b.Length);

            for (int j = 0; j < b.Length; j++)
            {
                if (j == bytesToPrint)
                {
                    s.AppendFormat(".. ");
                    continue;
                }
                if (j >= bytesToPrint && b.Length - j > bytesToPrint)
                {
                    continue;
                }
                s.AppendFormat("{0:x2} ", b[j]);
            }
            return s.ToString();
        }

        /// <summary>
        /// Just prints out length and first and last bytes of a (long) byte-array.  
        /// Useful for debugging.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string HexFromByteArrayCompactNoSpaces(byte[] b, int bytesToPrint = 8)
        {
            var s = new StringBuilder();
            if (bytesToPrint == 0)
            {
                bytesToPrint = Int32.MaxValue;
            }

            for (int j = 0; j < b.Length; j++)
            {
                if (j == bytesToPrint)
                {
                    s.AppendFormat("..");
                    continue;
                }
                if (j >= bytesToPrint && b.Length - j > bytesToPrint)
                {
                    continue;
                }
                s.AppendFormat("{0:x2}", b[j]);
            }
            return s.ToString();
        }

        public static string HexFromValueType(ValueType x)
        {
            var s = new StringBuilder();
            if (x is UInt32)
            {
                s.AppendFormat("{0:X8}", (UInt32)x);
            }
            if (x is UInt16)
            {
                s.AppendFormat("{0:X4}", (UInt16)x);
            }
            if (x is byte)
            {
                s.AppendFormat("{0:X2}", (byte)x);
            }
            if (String.IsNullOrEmpty(s.ToString()))
            {
                throw new Exception("Unsupported ValueType conversion");
            }
            return s.ToString();
        }

        public static string ToBinaryString(byte[] x)
        {
            string s = "";
            foreach (byte t in x)
            {
                for (int k = 0; k < 8; k++)
                {
                    if ((t & (0x80 >> k)) != 0)
                    {
                        s += "1";
                    }
                    else
                    {
                        s += "0";
                    }
                }
            }
            return s;
        }

        public static byte[] ByteArray(int numBytes, byte val)
        {
            var x = new byte[numBytes];
            for (int j = 0; j < numBytes; j++)
            {
                x[j] = val;
            }
            return x;
        }

        public static void DebugIf(bool x)
        {
            if (x)
            {
                Debug.WriteLine("");
            }
        }

        public static uint GetEnumFieldAsUint(Enum val, int lowestBit, int highestBit)
        {
            uint x = Convert.ToUInt32(val);
            x = x >> lowestBit;
            int numBits = highestBit - lowestBit;
            uint mask = 0;
            for (int j = 0; j <= numBits; j++)
            {
                mask = (mask << 1) | 1;
            }
            uint ret = x & mask;
            return ret;
        }

        private static int HexNibbleToInt(char c)
        {
            if ((c >= '0') && (c <= '9'))
            {
                return c - '0';
            }
            if (Char.ToLower(c) >= 'a' && Char.ToLower(c) <= 'f')
            {
                return 10 + Char.ToLower(c) - 'a';
            }
            throw new ArgumentException("Character + " + c + "is not hex");
        }

        public static bool BigEndianArraysAreEqual(byte[] a1, byte[] a2)
        {
            int maxLen = Math.Min(a1.Length, a2.Length);
            for (int j = 0; j < maxLen; j++)
            {
                if (a1[a1.Length - 1 - j] != a2[a2.Length - 1 - j])
                {
                    return false;
                }
            }
            // Rest of the other array must be all zeros
            if (a1.Length > a2.Length)
            {
                for (int j = 0; j < a1.Length - a2.Length; j++)
                {
                    if (a1[j] != 0)
                    {
                        return false;
                    }
                }
            }
            if (a2.Length > a1.Length)
            {
                for (int j = 0; j < a2.Length - a1.Length; j++)
                {
                    if (a2[j] != 0)
                        return false;
                }
            }
            return true;
        }

        public static byte[] CopyData(byte[] from, int start = 0, int len = -1)
        {
            if (from == null)
            {
                return null;
            }
            if (len == -1)
            {
                len = from.Length - start;
            }
            var to = new byte[len];
            Array.Copy(from, start, to, 0, len);
            return to;
        }

        public static bool ArraysAreEqual(Array a1, Array a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }
            return !a1.Cast<object>().Where((t, j) => !a1.GetValue(j).Equals(a2.GetValue(j))).Any();
        }

        public static ElementType[] CopyArray<ElementType>(ElementType[] from)
        {
            if (from == null)
            {
                return null;
            }
            var to = new ElementType[from.Length];
            Array.Copy(from, to, from.Length);
            return to;
        }

        // Not all modern .Net framework editions support ConvertAll extension method on collections.
        public static List<DstType> ConvertAll<SrcType, DstType>(IEnumerable<SrcType> coll, Func<SrcType, DstType> conv)
        {
            var dstList = new List<DstType>();
            foreach (var elt in coll)
            {
                dstList.Add(conv(elt));
            }
            return dstList;
        }

        public static string RepeatChars(int count, char charToRepeat)
        {
            string ss = "";
            for (int j = 0; j < count; j++)
            {
                ss += charToRepeat;
            }
            return ss;
        }

        public static int Mix(int x, int y)
        {
            return (x + 1) * (y + 1);
        }

        public static int GetSizeOfValueType(Object o)
        {
            if (o is byte)
            {
                return 1;
            }
            if (o is ushort || o is short)
            {
                return 2;
            }
            if (o is uint || o is int)
            {
                return 4;
            }
            if (o is ulong || o is long)
            {
                return 8;
            }
            throw new Exception("Unrecognized value type");
        }

        public static byte[] Concatenate(byte[][] fragments)
        {
            int len = fragments.Sum(t => t != null ? t.Length : 0);
            var temp = new byte[len];
            int pos = 0;
            foreach (byte[] t in fragments)
            {
                if (t == null)
                {
                    continue;
                }
                Array.Copy(t, 0, temp, pos, t.Length);
                pos += t.Length;
            }
            return temp;
        }

        public static byte[] Concatenate(byte[] f0, byte[] f1)
        {
            var arr = new byte[2][];
            arr[0] = f0;
            arr[1] = f1;
            return Concatenate(arr);
        }

        private static byte[] ShiftRightInternal(byte[] x, int numBits)
        {
            if (numBits > 7)
                throw new ArgumentException("Can only shift up to 7 bits");
            int numCarryBits = 8 - numBits;
            var y = new byte[x.Length];
            for (int j = x.Length - 1; j >= 0; j--)
            {
                y[j] = (byte)(x[j] >> numBits);
                if (j != 0)
                {
                    y[j] |= (byte)(x[j - 1] << numCarryBits);
                }
            }
            return y;
        }

        public static byte[] ShiftRight(byte[] x, int numBits)
        {
            var y = new byte[x.Length - numBits / 8];
            Array.Copy(x, y, y.Length);
            return ShiftRightInternal(y, numBits % 8);
        }

        public static void DebugPrint(string s, Object o, bool doTrace = true)
        {
            if (!doTrace)
            {
                return;
            }
            var bytes = o as byte[];

            if (bytes != null)
            {
                Debug.WriteLine("{0} [{1},0x{1:x}] {2}", s, bytes.Length, HexFromByteArray(bytes));
                return;
            }
            Debug.WriteLine(s + " " + o);
        }

        public static byte[] ReverseByteOrder(byte[] b)
        {
            int len = b.Length;
            var b2 = new byte[len];
            for (int j = 0; j < len; j++)
            {
                b2[j] = b[len - 1 - j];
            }
            return b2;
        }

        public static byte[] AddZeroToEnd(byte[] b, int numZeroBytes = 1)
        {
            var bb = new byte[b.Length + numZeroBytes];
            Array.Copy(b, bb, b.Length);
            return bb;
        }

        public static byte[] AddZeroToBeginning(byte[] b)
        {
            var bb = new byte[b.Length + 1];
            Array.Copy(b, 0, bb, 1, b.Length);
            return bb;
        }

        private static ResourceManager _resMgr;

        public static string GetResourceString(string name)
        {
            if (_resMgr == null)
            {
                // Assembly.GetExecutingAssembly()
                Assembly thisAsm = typeof(Globs).GetTypeInfo().Assembly;
                _resMgr = new ResourceManager("Tpm2Lib.Messages", thisAsm);
            }
            // We use *message to avoid the lookup for use of strings in internal debugging
            if (!IsAlphanumeric(name))
            {
                return name;
            }
            try
            {
                string res = _resMgr.GetString(name);
                return res;
            }
            catch (Exception)
            {
                return name + "[[NOTE: string resource is missing]]";
            }
        }

        private static bool IsAlphanumeric(string s)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (char c in s)
            {
                if (!Char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }

        public static string GetFlags(Enum e, string separator)
        {
            bool first = true;
            Array values = Enum.GetValues(e.GetType());
            string flags = "";
            foreach (Enum v in values)
            {
                if (e.HasFlag(v))
                {
                    if (!first)
                    {
                        flags += separator;
                    }
                    else
                    {
                        first = false;
                    }
                    flags += Enum.GetName(e.GetType(), v);
                }
            }
            return flags;
        }

        public static string ToString<T> (IEnumerable<T> list, string separator = ", ", string emptyListDesignator = "")
        {
            if (list.Count() == 0)
            {
                return emptyListDesignator;
            }
            string s = "";
            foreach (T val in list)
            {
                if (s.Length > 0)
                {
                    s += separator;
                }
                s += val;
            }
            return s;
        }

        public static Type GetLengthType (int lengthSize)
        {
            switch(lengthSize)
            {
                case 1: return typeof(byte);
                case 2: return typeof(UInt16);
                case 4: return typeof(UInt32);
                case 8: return typeof(UInt64);
            }
            throw new Exception("Unsupported length size " + lengthSize);
        }

        public static string ToCSharpStyle (string typeName)
        {
            if (typeName.EndsWith("[]"))
            {
                typeName = typeName.Substring(0, typeName.Length - 2);
            }
            if (typeName == "bool")     { return "bool"; }
            if (typeName == "Byte")     { return "byte"; }
            if (typeName == "SByte")    { return "sbyte"; }
            if (typeName == "Char")     { return "char"; }
            if (typeName == "Decimal")  { return "decimal"; }
            if (typeName == "Double")   { return "double"; }
            if (typeName == "Single")   { return "float"; }
            if (typeName == "Int32")    { return "int"; }
            if (typeName == "UInt32")   { return "uint"; }
            if (typeName == "Int64")    { return "long"; }
            if (typeName == "UInt64")   { return "ulong"; }
            if (typeName == "Object")   { return "object"; }
            if (typeName == "Int16")    { return "short"; }
            if (typeName == "UInt16")   { return "ushort"; }
            if (typeName == "String")   { return "string"; }
            return typeName;
        }

        public static string GetTypeName(object obj)
        {
            string fullName = obj.GetType().ToString();
            return fullName.Substring(fullName.LastIndexOf('.') + 1);
        }

        /// <summary>
        /// Returns unqualified name of the object's type or value.
        /// </summary>
        public static string GetShortName(object obj)
        {
            string fullName = obj.ToString();
            string name = fullName.Substring(fullName.LastIndexOf('.') + 1);
            if (name.Contains(","))
            {
                name = name.Replace(" ", "");
                string[] tokens = name.Split(new[] { ',' });
                foreach (var token in tokens)
                {
                    if ((uint)obj == (uint)Enum.Parse(obj.GetType(), token))
                    {
                        return token;
                    }
                }
            }
            return name;
        }

        public static string TrimEnd(string str, string substr)
        {
            if (str.Length < substr.Length || str.Substring(str.Length - substr.Length) != substr)
            {
                return str;
            }
            return str.Substring(0, str.Length - substr.Length);
        }

        public static string Join(string separator, params string[] items)
        {
            return string.Join(separator, items.Where(item => !string.IsNullOrEmpty(item)));
        }

        private static volatile int _volatileCounter;

        public static void SpinLoop(int spinCount)
        {
            for (int i = 0; i < spinCount; ++i)
            {
                for (_volatileCounter = 0; _volatileCounter < 1000; ++_volatileCounter)
                {
                }
            }
        }

        /// <summary>
        /// Returns .Net type of a member described by the memInfo argument.
        /// </summary>
        public static Type GetMemberType(MemberInfo memInfo)
        {
            if (memInfo is FieldInfo)
            {
                return (memInfo as FieldInfo).FieldType;
            }
            else if (memInfo is PropertyInfo)
            {
                return (memInfo as PropertyInfo).PropertyType;
            }
            return null;
        }

        /// <summary>
        /// Returns reference to containingObject's member described by the memInfo argument.
        /// </summary>
        public static object GetMember(MemberInfo memInfo, object containingObject)
        {
            if (memInfo is FieldInfo)
            {
                return (memInfo as FieldInfo).GetValue(containingObject);
            }
            else if (memInfo is PropertyInfo)
            {
                return (memInfo as PropertyInfo).GetValue(containingObject);
            }
            return null;
        }

        /// <summary>
        /// Sets the value of containingObject's member described by the memInfo argument.
        /// </summary>
        public static void SetMember(MemberInfo memInfo, object containingObject, object val)
        {
            if (memInfo is FieldInfo)
            {
                (memInfo as FieldInfo).SetValue(containingObject, val);
            }
            else if (memInfo is PropertyInfo)
            {
                (memInfo as PropertyInfo).SetValue(containingObject, val);
            }
            else
            {
                // Unsupported type
                Debug.Assert(false);
            }
        }

        public static A GetAttr<A>(MemberInfo memInfo) where A : class
        {
#if TSS_MIN_API
            Object[] attr = memInfo.GetCustomAttributes(typeof(A), false).ToArray();
#else
            Object[] attr = memInfo.GetCustomAttributes(typeof(A), false);
#endif
            if (attr.Length == 0)
            {
                return null;
            }
            Debug.Assert(attr.Length == 1);
            return (A)attr[0];
        }

        public static string[] ReadAllLines(string fileName, Encoding enc = null)
        {
#if TSS_MIN_API
            string t = enc == null ? File.ReadAllText(fileName)
                                   : File.ReadAllText(fileName, enc);
            return t.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
#else
            return enc == null ? File.ReadAllLines(fileName)
                               : File.ReadAllLines(fileName, enc);
#endif
        }

        public static void Throw<E>(string exceptionMsg) where E : Exception
        {
            if (!Tpm2._TssBehavior.Passthrough)
            {
                throw Activator.CreateInstance(typeof(E), exceptionMsg) as E;
            }
        }

        public static void Throw<E>(string failedFunction, int errorCode) where E : Exception
        {
            Throw<E>(failedFunction + " failed: 0x" + errorCode.ToString("X"));
        }

        public static void Throw(string exceptionMsg)
        {
            Throw<Exception>(exceptionMsg);
        }

        public static void Free(ref IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptr);
                ptr = IntPtr.Zero;
            }
        }
    } // class Globs

    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        Tpm2 my_tpm;

        public ByteArrayComparer(Tpm2 tpm)
        {
            my_tpm = tpm;
        }

        bool IEqualityComparer<byte[]>.Equals(byte[] x, byte[] y)
        {
            bool res = x.IsEqual(y);
            return res;
        }

        int IEqualityComparer<byte[]>.GetHashCode(byte[] obj)
        {
            TkHashcheck validation;
            return BitConverter.ToInt32(my_tpm.Hash(obj as byte[], TpmAlgId.Sha1, TpmRh.Owner, out validation), 0);
        }
    } // class ByteArrayComparer

    public static class ExtensionMethods
    {
        public static bool IsEqual(this Array a1, Array otherArray)
        {
            return Globs.ArraysAreEqual(a1, otherArray);
        }

        public static bool TypeIsEnum(this Type t)
        {
            return t.GetTypeInfo().IsEnum;
        }
    }

    public class Dbg
    {
        public bool Enabled = false;
        private string CurIndent = "";

        public Dbg(bool enabled = false)
        {
            Enabled = enabled;
        }

        public void Trace (string format, params object[] args)
        {
            if (Enabled)
            {
                Console.WriteLine(CurIndent + format, args);
            }
        }

        public void Indent ()
        {
            if (Enabled)
            {
                CurIndent += "    ";
            }
        }

        public void Unindent ()
        {
            if (Enabled && CurIndent.Length > 3)
            {
                CurIndent = CurIndent.Substring(0, CurIndent.Length - 4);
            }
        }
    }
}
