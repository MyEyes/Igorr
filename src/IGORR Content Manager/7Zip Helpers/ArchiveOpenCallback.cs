using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nomad.Archive.SevenZip;

namespace IGORR.Content
{
    class ArchiveOpenCallback : IArchiveOpenCallback
    {
        #region IArchiveOpenCallback Members

        public void SetTotal(IntPtr files, IntPtr bytes) { }
        public void SetCompleted(IntPtr files, IntPtr bytes) { }

        #endregion
    }
}
