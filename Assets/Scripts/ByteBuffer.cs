using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/**
    ByteBuffer -- Packet class sent over the network.
    The dll file generated can then be used directly in Unity. 
    Following the tutorial : https://www.youtube.com/watch?v=7P8x2PkT71c
    Generate dll file without visual studio :  csc /target:library /out:ByteBuffer.dll ByteBuffer.cs
*/
public class ByteBuffer : IDisposable
{
    private List<byte> Buff;
    private byte[] readBuff;
    private int readops;
    private bool buffUpdated = false;

    public ByteBuffer() {
        Buff = new List<byte>();
        readops = 0;
    }

    public long GetReadPos() {
        return readops;
    }

    public byte[] ToArray() {
        return Buff.ToArray();
    }

    public int Count() {
        return Buff.Count;
    }

    public int Length() {
        return Count() - readops;
    }

    public void Clear() {
        Buff.Clear();
    }

    public void WriteBytes(byte[] Input) {
        Buff.AddRange(Input);
        buffUpdated = true;
    }

    public void WriteShort(short Input) {
        Buff.AddRange(BitConverter.GetBytes(Input));
        buffUpdated = true;
    }

    public void WriteInteger(int Input) {
        Buff.AddRange(BitConverter.GetBytes(Input));
        buffUpdated = true;
    }

    public void WriteFloat(float Input) {
        Buff.AddRange(BitConverter.GetBytes(Input));
        buffUpdated = true;
    }
    public void WriteLong(long Input) {
        Buff.AddRange(BitConverter.GetBytes(Input));
        buffUpdated = true;
    }
    public void WriteString(string Input) {
        Buff.AddRange(BitConverter.GetBytes(Input.Length));
        Buff.AddRange(Encoding.ASCII.GetBytes(Input));
        buffUpdated = true;
    }

    public int ReadInteger(bool Peek = true) {
        if (Buff.Count > readops) {
            if (buffUpdated) {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            int ret = BitConverter.ToInt32(readBuff, readops);
            if (Peek & Buff.Count > readops)
                readops += 4;
            return ret;
        } else
            throw new Exception("Byte Buffer is Past Limit!");
    }
    public byte[] ReadBytes(int Length, bool Peek = true) {
        if (buffUpdated) {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            byte[] ret = Buff.GetRange(readops, Length).ToArray();
            if (Peek)
                readops += Length;
            return ret;
    }

    public string ReadString(bool peek = true) {
        int Len = ReadInteger(true);
         if (buffUpdated) {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            string ret = Encoding.ASCII.GetString(readBuff, readops, Len);
            if (peek & Buff.Count > readops) {
                if (ret.Length > 0) 
                    readops += Len;
            }
            return ret;
    }

    public int ReadShort(bool Peek = true) {
        if (Buff.Count > readops) {
            if (buffUpdated) {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            short ret = BitConverter.ToInt16(readBuff, readops);
            if (Peek & Buff.Count > readops)
                readops += 2;
            return ret;
        } else
            throw new Exception("Byte Buffer is Past Limit!");
    }

    public float ReadFloat(bool Peek=true) {
        if (Buff.Count > readops) {
            if (buffUpdated) {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            float ret = BitConverter.ToSingle(readBuff, readops);
            if (Peek & Buff.Count > readops)
                readops += 4;
            return ret;
        } else
            throw new Exception("Byte Buffer is Past Limit!");
    }

    public float ReadLong(bool Peek=true) {
        if (Buff.Count > readops) {
            if (buffUpdated) {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            long ret = BitConverter.ToInt64(readBuff, readops);
            if (Peek & Buff.Count > readops)
                readops += 8;
            return ret;
        } else
            throw new Exception("Byte Buffer is Past Limit!");
    }
    private bool disposedValue = false; // to detect redundant call
    protected virtual void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing)
                Buff.Clear();
                readops = 0;
        }
        disposedValue = true;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}