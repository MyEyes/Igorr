using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Nomad.Archive.SevenZip;

namespace IGORR.Content
{
    class ArchiveExtractCallback : IProgress, IArchiveExtractCallback, ICryptoGetTextPassword
    {
        public uint FileIndex;
        public string FileName;
        public OutStreamWrapper Stream;
        public AutoResetEvent ReadEvent;

        public ArchiveExtractCallback(uint fileIndex, string fileName)
        {
            FileIndex = fileIndex;
            FileName = fileName;
            ReadEvent = new AutoResetEvent(false);
        }

        #region IArchiveExtractCallback Members

        public void SetTotal(ulong total) { }
        public void SetCompleted(ref ulong completeValue) { }

        public int GetStream(uint index, out ISequentialOutStream outStream, AskMode askExtractMode)
        {
            if (index == FileIndex && askExtractMode == AskMode.kExtract)
            {
                Stream = new OutStreamWrapper(new MemoryStream());
                outStream = Stream;
            }
            else
                outStream = null;

            return 0;
        }

        public void PrepareOperation(AskMode askExtractMode) { }

        public void SetOperationResult(OperationResult resultEOperationResult)
        {
            ReadEvent.Set(); // let's the game thread know it's safe to continue
        }

        #endregion

        #region ICryptoGetTextPassword Members

        public int CryptoGetTextPassword(out string password)
        {
            password = "SevenDwarfsHateYou"; // this is where it's asking for the password
            return 0;
        }

        #endregion
    }
}
