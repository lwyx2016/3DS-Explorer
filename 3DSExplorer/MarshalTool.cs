﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace _3DSExplorer
{
    static class MarshalTool
    {
/*
        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }
*/

        public static byte[] StructureToByteArray<T>(T structure) where T : struct
        {
            var size = Marshal.SizeOf(structure);
            var byteArray = new byte[size];
            var pointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structure, pointer, false);
            Marshal.Copy(pointer, byteArray, 0, size);
            Marshal.FreeHGlobal(pointer);
            return byteArray;
        }

        public static T ReadStruct<T>(Stream fs)
        {
            var buffer = new byte[Marshal.SizeOf(typeof(T))];

            fs.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }

        public static T ReadStructBE<T>(Stream fs)
        {
            var buffer = new byte[Marshal.SizeOf(typeof(T))];

            fs.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            var t = temp.GetType();
            var fieldInfo = t.GetFields();
            foreach (var fi in fieldInfo)
            {
                if (fi.FieldType == typeof(Int16))
                {
                    var i16 = (Int16)fi.GetValue(temp);
                    var b16 = BitConverter.GetBytes(i16);
                    var b16R = b16.Reverse().ToArray();
                    fi.SetValueDirect(__makeref(temp), BitConverter.ToInt16(b16R, 0));
                }
                else if (fi.FieldType == typeof(Int32))
                {
                    var i32 = (Int32)fi.GetValue(temp);
                    var b32 = BitConverter.GetBytes(i32);
                    var b32R = b32.Reverse().ToArray();
                    fi.SetValueDirect(__makeref(temp), BitConverter.ToInt32(b32R, 0));
                }
                else if (fi.FieldType == typeof(Int64))
                {
                    var i64 = (Int64)fi.GetValue(temp);
                    var b64 = BitConverter.GetBytes(i64);
                    var b64R = b64.Reverse().ToArray();
                    fi.SetValueDirect(__makeref(temp), BitConverter.ToInt64(b64R, 0));
                }
                else if (fi.FieldType == typeof(UInt16))
                {
                    var i16 = (UInt16)fi.GetValue(temp);
                    var b16 = BitConverter.GetBytes(i16);
                    var b16R = b16.Reverse().ToArray();
                    fi.SetValueDirect(__makeref(temp), BitConverter.ToUInt16(b16R, 0));
                }
                else if (fi.FieldType == typeof(UInt32))
                {
                    var i32 = (UInt32)fi.GetValue(temp);
                    var b32 = BitConverter.GetBytes(i32);
                    var b32R = b32.Reverse().ToArray();
                    fi.SetValueDirect(__makeref(temp), BitConverter.ToUInt32(b32R, 0));
                }
                else if (fi.FieldType == typeof(UInt64))
                {
                    var i64 = (UInt64)fi.GetValue(temp);
                    var b64 = BitConverter.GetBytes(i64);
                    var b64R = b64.Reverse().ToArray();
                    fi.SetValueDirect(__makeref(temp), BitConverter.ToUInt64(b64R, 0));
                }
            }
            return temp;
        }

    }
}
