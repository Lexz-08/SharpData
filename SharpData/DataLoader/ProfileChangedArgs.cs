using System;

namespace AMS.Profile
{
    public class ProfileChangedArgs : EventArgs
    {
        private readonly ProfileChangeType m_changeType;
        private readonly string m_section;
        private readonly string m_entry;
        private readonly object m_value;

        public ProfileChangedArgs(
          ProfileChangeType changeType,
          string section,
          string entry,
          object value)
        {
            this.m_changeType = changeType;
            this.m_section = section;
            this.m_entry = entry;
            this.m_value = value;
        }

        public ProfileChangeType ChangeType => this.m_changeType;

        public string Section => this.m_section;

        public string Entry => this.m_entry;

        public object Value => this.m_value;
    }
}
