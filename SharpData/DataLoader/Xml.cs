using System.IO;
using System.Xml;

namespace AMS.Profile
{
    public class Xml : XmlBased
    {
        private string m_rootName = "profile";

        public Xml()
        {
        }

        public Xml(string fileName)
          : base(fileName)
        {
        }

        public Xml(AMS.Profile.Xml xml)
          : base((XmlBased)xml)
          => this.m_rootName = xml.m_rootName;

        public override string DefaultName => this.DefaultNameWithoutExtension + ".xml";

        public override object Clone() => (object)new AMS.Profile.Xml(this);

        private string GetSectionsPath(string section) => "section[@name=\"" + section + "\"]";

        private string GetEntryPath(string entry) => "entry[@name=\"" + entry + "\"]";

        public string RootName
        {
            get => this.m_rootName;
            set
            {
                this.VerifyNotReadOnly();
                if (this.m_rootName == value.Trim() || !this.RaiseChangeEvent(true, ProfileChangeType.Other, (string)null, nameof(RootName), (object)value))
                    return;
                this.m_rootName = value.Trim();
                this.RaiseChangeEvent(false, ProfileChangeType.Other, (string)null, nameof(RootName), (object)value);
            }
        }

        public override void SetValue(string section, string entry, object value)
        {
            if (value == null)
            {
                this.RemoveEntry(section, entry);
            }
            else
            {
                this.VerifyNotReadOnly();
                this.VerifyName();
                this.VerifyAndAdjustSection(ref section);
                this.VerifyAndAdjustEntry(ref entry);
                if (!this.RaiseChangeEvent(true, ProfileChangeType.SetValue, section, entry, value))
                    return;
                string text = value.ToString();
                if ((this.m_buffer == null || this.m_buffer.IsEmpty) && !File.Exists(this.Name))
                {
                    XmlTextWriter writer = this.m_buffer != null ? new XmlTextWriter((Stream)new MemoryStream(), this.Encoding) : new XmlTextWriter(this.Name, this.Encoding);
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();
                    writer.WriteStartElement(this.m_rootName);
                    writer.WriteStartElement(nameof(section));
                    writer.WriteAttributeString("name", (string)null, section);
                    writer.WriteStartElement(nameof(entry));
                    writer.WriteAttributeString("name", (string)null, entry);
                    writer.WriteString(text);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    if (this.m_buffer != null)
                        this.m_buffer.Load(writer);
                    writer.Close();
                    this.RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
                }
                else
                {
                    XmlDocument xmlDocument = this.GetXmlDocument();
                    XmlElement documentElement = xmlDocument.DocumentElement;
                    XmlNode xmlNode1 = documentElement.SelectSingleNode(this.GetSectionsPath(section));
                    if (xmlNode1 == null)
                    {
                        XmlElement element = xmlDocument.CreateElement(nameof(section));
                        XmlAttribute attribute = xmlDocument.CreateAttribute("name");
                        attribute.Value = section;
                        element.Attributes.Append(attribute);
                        xmlNode1 = documentElement.AppendChild((XmlNode)element);
                    }
                    XmlNode xmlNode2 = xmlNode1.SelectSingleNode(this.GetEntryPath(entry));
                    if (xmlNode2 == null)
                    {
                        XmlElement element = xmlDocument.CreateElement(nameof(entry));
                        XmlAttribute attribute = xmlDocument.CreateAttribute("name");
                        attribute.Value = entry;
                        element.Attributes.Append(attribute);
                        xmlNode2 = xmlNode1.AppendChild((XmlNode)element);
                    }
                    xmlNode2.InnerText = text;
                    this.Save(xmlDocument);
                    this.RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
                }
            }
        }

        public override object GetValue(string section, string entry)
        {
            this.VerifyAndAdjustSection(ref section);
            this.VerifyAndAdjustEntry(ref entry);
            try
            {
                return (object)this.GetXmlDocument().DocumentElement.SelectSingleNode(this.GetSectionsPath(section) + "/" + this.GetEntryPath(entry)).InnerText;
            }
            catch
            {
                return (object)null;
            }
        }

        public override void RemoveEntry(string section, string entry)
        {
            this.VerifyNotReadOnly();
            this.VerifyAndAdjustSection(ref section);
            this.VerifyAndAdjustEntry(ref entry);
            XmlDocument xmlDocument = this.GetXmlDocument();
            if (xmlDocument == null)
                return;
            XmlNode oldChild = xmlDocument.DocumentElement.SelectSingleNode(this.GetSectionsPath(section) + "/" + this.GetEntryPath(entry));
            if (oldChild == null || !this.RaiseChangeEvent(true, ProfileChangeType.RemoveEntry, section, entry, (object)null))
                return;
            oldChild.ParentNode.RemoveChild(oldChild);
            this.Save(xmlDocument);
            this.RaiseChangeEvent(false, ProfileChangeType.RemoveEntry, section, entry, (object)null);
        }

        public override void RemoveSection(string section)
        {
            this.VerifyNotReadOnly();
            this.VerifyAndAdjustSection(ref section);
            XmlDocument xmlDocument = this.GetXmlDocument();
            if (xmlDocument == null)
                return;
            XmlElement documentElement = xmlDocument.DocumentElement;
            if (documentElement == null)
                return;
            XmlNode oldChild = documentElement.SelectSingleNode(this.GetSectionsPath(section));
            if (oldChild == null || !this.RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, (string)null, (object)null))
                return;
            documentElement.RemoveChild(oldChild);
            this.Save(xmlDocument);
            this.RaiseChangeEvent(false, ProfileChangeType.RemoveSection, section, (string)null, (object)null);
        }

        public override string[] GetEntryNames(string section)
        {
            if (!this.HasSection(section))
                return (string[])null;
            this.VerifyAndAdjustSection(ref section);
            XmlNodeList xmlNodeList = this.GetXmlDocument().DocumentElement.SelectNodes(this.GetSectionsPath(section) + "/entry[@name]");
            if (xmlNodeList == null)
                return (string[])null;
            string[] strArray = new string[xmlNodeList.Count];
            int num = 0;
            foreach (XmlNode xmlNode in xmlNodeList)
                strArray[num++] = xmlNode.Attributes["name"].Value;
            return strArray;
        }

        public override string[] GetSectionNames()
        {
            XmlDocument xmlDocument = this.GetXmlDocument();
            if (xmlDocument == null)
                return (string[])null;
            XmlElement documentElement = xmlDocument.DocumentElement;
            if (documentElement == null)
                return (string[])null;
            XmlNodeList xmlNodeList = documentElement.SelectNodes("section[@name]");
            if (xmlNodeList == null)
                return (string[])null;
            string[] strArray = new string[xmlNodeList.Count];
            int num = 0;
            foreach (XmlNode xmlNode in xmlNodeList)
                strArray[num++] = xmlNode.Attributes["name"].Value;
            return strArray;
        }
    }
}
