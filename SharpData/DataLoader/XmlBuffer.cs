using System;
using System.IO;
using System.Xml;

namespace AMS.Profile
{
    public class XmlBuffer : IDisposable
    {
        private XmlBased m_profile;
        private XmlDocument m_doc;
        private FileStream m_file;
        internal bool m_needsFlushing;

        internal XmlBuffer(XmlBased profile, bool lockFile)
        {
            this.m_profile = profile;
            if (!lockFile)
                return;
            this.m_profile.VerifyName();
            if (!File.Exists(this.m_profile.Name))
                return;
            this.m_file = new FileStream(this.m_profile.Name, FileMode.Open, this.m_profile.ReadOnly ? FileAccess.Read : FileAccess.ReadWrite, FileShare.Read);
        }

        internal void Load(XmlTextWriter writer)
        {
            writer.Flush();
            writer.BaseStream.Position = 0L;
            this.m_doc.Load(writer.BaseStream);
            this.m_needsFlushing = true;
        }

        internal XmlDocument XmlDocument
        {
            get
            {
                if (this.m_doc == null)
                {
                    this.m_doc = new XmlDocument();
                    if (this.m_file != null)
                    {
                        this.m_file.Position = 0L;
                        this.m_doc.Load((Stream)this.m_file);
                    }
                    else
                    {
                        this.m_profile.VerifyName();
                        if (File.Exists(this.m_profile.Name))
                            this.m_doc.Load(this.m_profile.Name);
                    }
                }
                return this.m_doc;
            }
        }

        internal bool IsEmpty => this.XmlDocument.InnerXml == string.Empty;

        public bool NeedsFlushing => this.m_needsFlushing;

        public bool Locked => this.m_file != null;

        public void Flush()
        {
            if (this.m_profile == null)
                throw new InvalidOperationException("Cannot flush an XmlBuffer object that has been closed.");
            if (this.m_doc == null)
                return;
            if (this.m_file == null)
            {
                this.m_doc.Save(this.m_profile.Name);
            }
            else
            {
                this.m_file.SetLength(0L);
                this.m_doc.Save((Stream)this.m_file);
            }
            this.m_needsFlushing = false;
        }

        public void Reset()
        {
            if (this.m_profile == null)
                throw new InvalidOperationException("Cannot reset an XmlBuffer object that has been closed.");
            this.m_doc = (XmlDocument)null;
            this.m_needsFlushing = false;
        }

        public void Close()
        {
            if (this.m_profile == null)
                return;
            if (this.m_needsFlushing)
                this.Flush();
            this.m_doc = (XmlDocument)null;
            if (this.m_file != null)
            {
                this.m_file.Close();
                this.m_file = (FileStream)null;
            }
            if (this.m_profile != null)
                this.m_profile.m_buffer = (XmlBuffer)null;
            this.m_profile = (XmlBased)null;
        }

        public void Dispose() => this.Close();
    }
}
