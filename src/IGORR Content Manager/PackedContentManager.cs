using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading;
using Nomad.Archive.SevenZip;
using System.Runtime.InteropServices;
using Microsoft.COM;
using Microsoft.Xna.Framework.Content;

namespace IGORR.Content
{
    class PackedContentManager : ContentManager
    {
        static string SevenZipDllPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "7za.dll");
            }
        }

        SevenZipFormat m_format;
        InStreamWrapper m_inStream;
        IInArchive m_archive;
        IArchiveOpenCallback m_callback;
        ulong m_checkPos;
        Hashtable m_hash; // used for file-index lookups
        bool _hasArchive = true;

        public PackedContentManager(IServiceProvider serviceProvider, string archivePath,string contentPath, KnownSevenZipFormat archiveFormat)
            : base(serviceProvider)
        {
            m_format = new SevenZipFormat(SevenZipDllPath);
            m_archive = m_format.CreateInArchive(SevenZipFormat.GetClassIdFromKnownFormat(archiveFormat));
            base.RootDirectory = contentPath;
            try
            {
                m_inStream = new InStreamWrapper(File.OpenRead(archivePath));
                m_callback = new ArchiveOpenCallback();

                m_checkPos = 128 * 1024;
                m_archive.Open(m_inStream, ref m_checkPos, m_callback);

                m_hash = new Hashtable();
                uint count = m_archive.GetNumberOfItems();
                for (uint i = 0; i < count; i++)
                {
                    PropVariant name = new PropVariant();
                    m_archive.GetProperty(i, ItemPropId.kpidPath, ref name);

                    string strName = (name.GetObject() as string).ToLower();
                    int xnbIndex = strName.IndexOf(".xnb");
                    if (xnbIndex >= 0)
                        strName = strName.Remove(xnbIndex, 4);

                    m_hash.Add(strName, i);
                }
            }
            catch (IOException ioe)
            {
                _hasArchive = false;
            }
        }

        uint[] m_extractIndices = new uint[1];

        protected override Stream OpenStream(string assetName)
        {
            string nameLower = assetName.ToLower();
            try
            {
                if (File.Exists(this.RootDirectory + "/" + assetName + ".xnb"))
                {
                    return base.OpenStream(assetName);
                }
            }
            catch (ContentLoadException cle)
            {
            }
            if (_hasArchive && m_hash.ContainsKey(nameLower))
            {
                uint index = (uint)m_hash[nameLower];
                m_extractIndices[0] = index;

                ArchiveExtractCallback extractCallback = new ArchiveExtractCallback(index, assetName);
                m_archive.Extract(m_extractIndices, 1, 0, extractCallback);
                extractCallback.ReadEvent.WaitOne(); // wait for decompress/decryption

                // reset stream position for the content reader
                extractCallback.Stream.BaseStream.Position = 0L;
                return extractCallback.Stream.BaseStream;
            }
            throw new ContentLoadException(assetName);
        }

        public override T Load<T>(string assetName)
        {
            return base.Load<T>(assetName);
        }

        public List<string> GetContentList()
        {
            List<string> list = new List<string>();
            foreach (object obj in m_hash.Keys)
            {
                list.Add(obj.ToString());
            }
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                Marshal.ReleaseComObject(m_archive);
            }
        }

    }
}
