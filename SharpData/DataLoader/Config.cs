using System.IO;
using System.Xml;

namespace AMS.Profile
{
    public class Config : XmlBased
    {
        private const string SECTION_TYPE = "System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null";
        private string m_groupName = "profile";

        public Config()
        {
        }

        public Config(string fileName)
          : base(fileName)
        {
        }

        public Config(Config config)
          : base((XmlBased)config)
          => this.m_groupName = config.m_groupName;

        public override string DefaultName => this.DefaultNameWithoutExtension + ".config";

        public override object Clone() => (object)new Config(this);

        public string GroupName
        {
            get => this.m_groupName;
            set
            {
                this.VerifyNotReadOnly();
                if (this.m_groupName == value || !this.RaiseChangeEvent(true, ProfileChangeType.Other, (string)null, nameof(GroupName), (object)value))
                    return;
                this.m_groupName = value;
                if (this.m_groupName != null)
                {
                    this.m_groupName = this.m_groupName.Replace(' ', '_');
                    if (this.m_groupName.IndexOf(':') >= 0)
                        throw new XmlException("GroupName may not contain a namespace prefix.");
                }
                this.RaiseChangeEvent(false, ProfileChangeType.Other, (string)null, nameof(GroupName), (object)value);
            }
        }

        private bool HasGroupName => this.m_groupName != null && this.m_groupName != "";

        private string GroupNameSlash => !this.HasGroupName ? "" : this.m_groupName + "/";

        private bool IsAppSettings(string section) => !this.HasGroupName && section != null && section == "appSettings";

        protected override void VerifyAndAdjustSection(ref string section)
        {
            base.VerifyAndAdjustSection(ref section);
            if (section.IndexOf(' ') < 0)
                return;
            section = section.Replace(' ', '_');
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
                bool hasGroupName = this.HasGroupName;
                bool flag = this.IsAppSettings(section);
                if ((this.m_buffer == null || this.m_buffer.IsEmpty) && !File.Exists(this.Name))
                {
                    XmlTextWriter writer = this.m_buffer != null ? new XmlTextWriter((Stream)new MemoryStream(), this.Encoding) : new XmlTextWriter(this.Name, this.Encoding);
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();
                    writer.WriteStartElement("configuration");
                    if (!flag)
                    {
                        writer.WriteStartElement("configSections");
                        if (hasGroupName)
                        {
                            writer.WriteStartElement("sectionGroup");
                            writer.WriteAttributeString("name", (string)null, this.m_groupName);
                        }
                        writer.WriteStartElement(nameof(section));
                        writer.WriteAttributeString("name", (string)null, section);
                        writer.WriteAttributeString("type", (string)null, "System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null");
                        writer.WriteEndElement();
                        if (hasGroupName)
                            writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    if (hasGroupName)
                        writer.WriteStartElement(this.m_groupName);
                    writer.WriteStartElement(section);
                    writer.WriteStartElement("add");
                    writer.WriteAttributeString("key", (string)null, entry);
                    writer.WriteAttributeString(nameof(value), (string)null, value.ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    if (hasGroupName)
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
                    if (!flag)
                    {
                        XmlNode xmlNode1 = documentElement.SelectSingleNode("configSections") ?? documentElement.AppendChild((XmlNode)xmlDocument.CreateElement("configSections"));
                        XmlNode xmlNode2 = xmlNode1;
                        if (hasGroupName)
                        {
                            xmlNode2 = xmlNode1.SelectSingleNode("sectionGroup[@name=\"" + this.m_groupName + "\"]");
                            if (xmlNode2 == null)
                            {
                                XmlElement element = xmlDocument.CreateElement("sectionGroup");
                                XmlAttribute attribute = xmlDocument.CreateAttribute("name");
                                attribute.Value = this.m_groupName;
                                element.Attributes.Append(attribute);
                                xmlNode2 = xmlNode1.AppendChild((XmlNode)element);
                            }
                        }
                        XmlNode xmlNode3 = xmlNode2.SelectSingleNode("section[@name=\"" + section + "\"]");
                        if (xmlNode3 == null)
                        {
                            XmlElement element = xmlDocument.CreateElement(nameof(section));
                            XmlAttribute attribute = xmlDocument.CreateAttribute("name");
                            attribute.Value = section;
                            element.Attributes.Append(attribute);
                            xmlNode3 = xmlNode2.AppendChild((XmlNode)element);
                        }
                        XmlAttribute attribute1 = xmlDocument.CreateAttribute("type");
                        attribute1.Value = "System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null";
                        xmlNode3.Attributes.Append(attribute1);
                    }
                    XmlNode xmlNode4 = (XmlNode)documentElement;
                    if (hasGroupName)
                        xmlNode4 = documentElement.SelectSingleNode(this.m_groupName) ?? documentElement.AppendChild((XmlNode)xmlDocument.CreateElement(this.m_groupName));
                    XmlNode xmlNode5 = xmlNode4.SelectSingleNode(section) ?? xmlNode4.AppendChild((XmlNode)xmlDocument.CreateElement(section));
                    XmlNode xmlNode6 = xmlNode5.SelectSingleNode("add[@key=\"" + entry + "\"]");
                    if (xmlNode6 == null)
                    {
                        XmlElement element = xmlDocument.CreateElement("add");
                        XmlAttribute attribute = xmlDocument.CreateAttribute("key");
                        attribute.Value = entry;
                        element.Attributes.Append(attribute);
                        xmlNode6 = xmlNode5.AppendChild((XmlNode)element);
                    }
                    XmlAttribute attribute2 = xmlDocument.CreateAttribute(nameof(value));
                    attribute2.Value = value.ToString();
                    xmlNode6.Attributes.Append(attribute2);
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
                return (object)this.GetXmlDocument().DocumentElement.SelectSingleNode(this.GroupNameSlash + section + "/add[@key=\"" + entry + "\"]").Attributes["value"].Value;
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
            XmlNode oldChild = xmlDocument.DocumentElement.SelectSingleNode(this.GroupNameSlash + section + "/add[@key=\"" + entry + "\"]");
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
            XmlNode oldChild1 = documentElement.SelectSingleNode(this.GroupNameSlash + section);
            if (oldChild1 == null || !this.RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, (string)null, (object)null))
                return;
            oldChild1.ParentNode.RemoveChild(oldChild1);
            if (!this.IsAppSettings(section))
            {
                XmlNode oldChild2 = documentElement.SelectSingleNode("configSections/" + (this.HasGroupName ? "sectionGroup[@name=\"" + this.m_groupName + "\"]" : "") + "/section[@name=\"" + section + "\"]");
                oldChild2?.ParentNode.RemoveChild(oldChild2);
            }
            this.Save(xmlDocument);
            this.RaiseChangeEvent(false, ProfileChangeType.RemoveSection, section, (string)null, (object)null);
        }

        public override string[] GetEntryNames(string section)
        {
            if (!this.HasSection(section))
                return (string[])null;
            this.VerifyAndAdjustSection(ref section);
            XmlNodeList xmlNodeList = this.GetXmlDocument().DocumentElement.SelectNodes(this.GroupNameSlash + section + "/add[@key]");
            if (xmlNodeList == null)
                return (string[])null;
            string[] strArray = new string[xmlNodeList.Count];
            int num = 0;
            foreach (XmlNode xmlNode in xmlNodeList)
                strArray[num++] = xmlNode.Attributes["key"].Value;
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
            XmlNode xmlNode1 = this.HasGroupName ? documentElement.SelectSingleNode(this.m_groupName) : (XmlNode)documentElement;
            if (xmlNode1 == null)
                return (string[])null;
            XmlNodeList childNodes = xmlNode1.ChildNodes;
            if (childNodes == null)
                return (string[])null;
            string[] strArray = new string[childNodes.Count];
            int num = 0;
            foreach (XmlNode xmlNode2 in childNodes)
                strArray[num++] = xmlNode2.Name;
            return strArray;
        }
    }
}
