// -----------------------------------------------------------------------
// <copyright file="ProcessFlag.cs" company="Microsoft">
// </copyright>
// -----------------------------------------------------------------------

namespace APP.Manager.Common.Utilities
{
    using System.Threading;

    /// <summary>
    /// Brief Description
    /// </summary>
    public class ProcessFlag
    {
        private const int PROCESSING = 1, STOPPED = 0;
        private int usingResource = STOPPED;

        public bool StopProcess()
        {
            if (PROCESSING == Interlocked.Exchange(ref usingResource, STOPPED))
            {
                return true;
            }
            return false;
        }

        public bool SetInProcess()
        {
            if (STOPPED == Interlocked.Exchange(ref usingResource, PROCESSING))
            {
                return true;
            }
            return false;
        }

        public bool IsProcessing
        {
            get
            {
                return Thread.VolatileRead(ref usingResource) == PROCESSING;
            }
        }
    }
}
