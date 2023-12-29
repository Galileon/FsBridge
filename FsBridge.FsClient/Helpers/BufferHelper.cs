using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FsBridge.Helpers
{
    internal class BufferHelper
    {
        internal static bool HasCompletedSegment (ArraySegment<byte> buffer)
        {
            for (int i = 0; i < buffer.Count - 1; i++) if (buffer[i] == 10 && buffer[i + 1] == 10) return true;
            return false;
        }
        internal static bool FetchMessageSegment (NetCoreServer.Buffer buffer, int? fixedSegmentLenght, out string msg )
        {
            if (fixedSegmentLenght.HasValue && buffer.Size >= fixedSegmentLenght.Value) 
            {
                msg = buffer.ExtractString(0, fixedSegmentLenght.Value);
                buffer.Remove(0, fixedSegmentLenght.Value); // 10 
                return true;
            }
            for (long a = 0; a< buffer.Size - 1;a++)
            {
                if (buffer[a] == 10 && buffer[a + 1] == 10)
                {
                    msg = buffer.ExtractString(0, a);
                    buffer.Remove(0, a + 2); // 10 
                    return true;
                }
            }
            msg = string.Empty;
            return false;
        }
    }
}
