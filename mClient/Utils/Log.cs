using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace mClient.Shared
{
    public static class Log
    {
        static ReaderWriterLock packetLock = new ReaderWriterLock();
        static ReaderWriterLock networkLock = new ReaderWriterLock();
        static ReaderWriterLock debugkLock = new ReaderWriterLock();
        static ReaderWriterLock normalLock = new ReaderWriterLock();
        static ReaderWriterLock errorLock = new ReaderWriterLock();
        static int writerTimeouts = 0;

        public static void WriteLine(LogType type, string format, params object[] parameters)
        {          

            format = string.Format("[{0}][{1}]{2}", Time.GetTime(), type, (string)format);
            string msg = format;
            if (parameters.Length > 0)
                msg = string.Format(format, parameters);

            if (Config.LogToFile)
            {
                if (type == LogType.Packet)
                {
                    try
                    {
                        packetLock.AcquireWriterLock(1000);
                        try
                        {
                            using (var packetFile = File.AppendText("log_packets.txt"))
                                packetFile.WriteLine(msg);
                        }
                        finally
                        {
                            packetLock.ReleaseWriterLock();
                        }
                    }
                    catch(Exception e)
                    {
                        Interlocked.Increment(ref writerTimeouts);
                    }
                }
                else if (type == LogType.Network)
                {
                    try
                    {
                        networkLock.AcquireWriterLock(1000);
                        try
                        {
                            using (var networkFile = File.AppendText("network.txt"))
                                networkFile.WriteLine(msg);
                        }
                        finally
                        {
                            networkLock.ReleaseWriterLock();
                        }
                    }
                    catch (Exception e)
                    {
                        Interlocked.Increment(ref writerTimeouts);
                    }
                }
                else if (type == LogType.Debug)
                {
                    try
                    {
                        debugkLock.AcquireWriterLock(1000);
                        try
                        {
                            using (var debugFile = File.AppendText("debug.txt"))
                                debugFile.WriteLine(msg);
                        }
                        finally
                        {
                            debugkLock.ReleaseWriterLock();
                        }
                    }
                    catch (Exception e)
                    {
                        Interlocked.Increment(ref writerTimeouts);
                    }
                }
                else if (type == LogType.Normal)
                {
                    try
                    {
                        normalLock.AcquireWriterLock(1000);
                        try
                        {
                            using (var normalFile = File.AppendText("normal.txt"))
                                normalFile.WriteLine(msg);
                        }
                        finally
                        {
                            normalLock.ReleaseWriterLock();
                        }
                    }
                    catch (Exception e)
                    {
                        Interlocked.Increment(ref writerTimeouts);
                    }
                }
                else if (type == LogType.Error)
                {
                    try
                    {
                        errorLock.AcquireWriterLock(1000);
                        try
                        {
                            using (var errorFile = File.AppendText("error.txt"))
                                errorFile.WriteLine(msg);
                        }
                        finally
                        {
                            errorLock.ReleaseWriterLock();
                        }
                    }
                    catch (Exception e)
                    {
                        Interlocked.Increment(ref writerTimeouts);
                    }
                }
            }



            if (((UInt32)type & Config.LogFilter) > 0)
                return;
            else
                mCore.Event(new Event(EventType.EVENT_LOG, "0", new object[] { msg }));
        }

    }

    public enum LogType : long
    {
        Command = 0x1000000000000000,
        Normal  = 0x0100000000000000,
        Success = 0x0010000000000000,
        Error   = 0x0001000000000000,
        Debug   = 0x0000100000000000,
        Test    = 0x0000010000000000,
        Chat    = 0x0000001000000000,
        Terrain = 0x0000000100000000,
        Network = 0x0000000010000000,
        Packet  = 0x0000000001000000,
    }

}
