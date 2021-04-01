using System;
using System.Data;

namespace AMS.Profile
{
    public abstract class Profile : IProfile, IReadOnlyProfile, ICloneable
    {
        private string m_name;
        private bool m_readOnly;

        public event ProfileChangingHandler Changing;

        public event ProfileChangedHandler Changed;

        protected Profile() => this.m_name = this.DefaultName;

        protected Profile(string name) => this.m_name = name;

        protected Profile(AMS.Profile.Profile profile)
        {
            this.m_name = profile.m_name;
            this.m_readOnly = profile.m_readOnly;
            this.Changing = profile.Changing;
            this.Changed = profile.Changed;
        }

        public string Name
        {
            get => this.m_name;
            set
            {
                this.VerifyNotReadOnly();
                if (this.m_name == value.Trim() || !this.RaiseChangeEvent(true, ProfileChangeType.Name, (string)null, (string)null, (object)value))
                    return;
                this.m_name = value.Trim();
                this.RaiseChangeEvent(false, ProfileChangeType.Name, (string)null, (string)null, (object)value);
            }
        }

        public bool ReadOnly
        {
            get => this.m_readOnly;
            set
            {
                this.VerifyNotReadOnly();
                if (this.m_readOnly == value || !this.RaiseChangeEvent(true, ProfileChangeType.ReadOnly, (string)null, (string)null, (object)value))
                    return;
                this.m_readOnly = value;
                this.RaiseChangeEvent(false, ProfileChangeType.ReadOnly, (string)null, (string)null, (object)value);
            }
        }

        public abstract string DefaultName { get; }

        public abstract object Clone();

        public abstract void SetValue(string section, string entry, object value);

        public abstract object GetValue(string section, string entry);

        public virtual string GetValue(string section, string entry, string defaultValue)
        {
            object obj = this.GetValue(section, entry);
            return obj != null ? obj.ToString() : defaultValue;
        }

        public virtual int GetValue(string section, string entry, int defaultValue)
        {
            object obj = this.GetValue(section, entry);
            if (obj == null)
                return defaultValue;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }

        public virtual double GetValue(string section, string entry, double defaultValue)
        {
            object obj = this.GetValue(section, entry);
            if (obj == null)
                return defaultValue;
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return 0.0;
            }
        }

        public virtual bool GetValue(string section, string entry, bool defaultValue)
        {
            object obj = this.GetValue(section, entry);
            if (obj == null)
                return defaultValue;
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return false;
            }
        }

        public virtual bool HasEntry(string section, string entry)
        {
            string[] entryNames = this.GetEntryNames(section);
            if (entryNames == null)
                return false;
            this.VerifyAndAdjustEntry(ref entry);
            return Array.IndexOf<string>(entryNames, entry) >= 0;
        }

        public virtual bool HasSection(string section)
        {
            string[] sectionNames = this.GetSectionNames();
            if (sectionNames == null)
                return false;
            this.VerifyAndAdjustSection(ref section);
            return Array.IndexOf<string>(sectionNames, section) >= 0;
        }

        public abstract void RemoveEntry(string section, string entry);

        public abstract void RemoveSection(string section);

        public abstract string[] GetEntryNames(string section);

        public abstract string[] GetSectionNames();

        public virtual IReadOnlyProfile CloneReadOnly()
        {
            AMS.Profile.Profile profile = (AMS.Profile.Profile)this.Clone();
            profile.m_readOnly = true;
            return (IReadOnlyProfile)profile;
        }

        public virtual DataSet GetDataSet()
        {
            this.VerifyName();
            string[] sectionNames = this.GetSectionNames();
            if (sectionNames == null)
                return (DataSet)null;
            DataSet dataSet = new DataSet(this.Name);
            foreach (string str1 in sectionNames)
            {
                DataTable dataTable = dataSet.Tables.Add(str1);
                string[] entryNames = this.GetEntryNames(str1);
                DataColumn[] columns = new DataColumn[entryNames.Length];
                object[] objArray = new object[entryNames.Length];
                int index = 0;
                foreach (string str2 in entryNames)
                {
                    object obj = this.GetValue(str1, str2);
                    columns[index] = new DataColumn(str2, obj.GetType());
                    objArray[index++] = obj;
                }
                dataTable.Columns.AddRange(columns);
                dataTable.Rows.Add(objArray);
            }
            return dataSet;
        }

        public virtual void SetDataSet(DataSet ds)
        {
            if (ds == null)
                throw new ArgumentNullException(nameof(ds));
            foreach (DataTable table in (InternalDataCollectionBase)ds.Tables)
            {
                string tableName = table.TableName;
                DataRowCollection rows = table.Rows;
                if (rows.Count != 0)
                {
                    foreach (DataColumn column in (InternalDataCollectionBase)table.Columns)
                    {
                        string columnName = column.ColumnName;
                        object obj = rows[0][column];
                        this.SetValue(tableName, columnName, obj);
                    }
                }
            }
        }

        protected string DefaultNameWithoutExtension
        {
            get
            {
                try
                {
                    string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    return configurationFile.Substring(0, configurationFile.LastIndexOf('.'));
                }
                catch
                {
                    return "profile";
                }
            }
        }

        protected virtual void VerifyAndAdjustSection(ref string section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));
            section = section.Trim();
        }

        protected virtual void VerifyAndAdjustEntry(ref string entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            entry = entry.Trim();
        }

        protected internal virtual void VerifyName()
        {
            if (this.m_name == null || this.m_name == "")
                throw new InvalidOperationException("Operation not allowed because Name property is null or empty.");
        }

        protected internal virtual void VerifyNotReadOnly()
        {
            if (this.m_readOnly)
                throw new InvalidOperationException("Operation not allowed because ReadOnly property is true.");
        }

        protected bool RaiseChangeEvent(
          bool changing,
          ProfileChangeType changeType,
          string section,
          string entry,
          object value)
        {
            if (changing)
            {
                if (this.Changing == null)
                    return true;
                ProfileChangingArgs e = new ProfileChangingArgs(changeType, section, entry, value);
                this.OnChanging(e);
                return !e.Cancel;
            }
            if (this.Changed != null)
                this.OnChanged(new ProfileChangedArgs(changeType, section, entry, value));
            return true;
        }

        protected virtual void OnChanging(ProfileChangingArgs e)
        {
            if (this.Changing == null)
                return;
            foreach (ProfileChangingHandler invocation in this.Changing.GetInvocationList())
            {
                invocation((object)this, e);
                if (e.Cancel)
                    break;
            }
        }

        protected virtual void OnChanged(ProfileChangedArgs e)
        {
            if (this.Changed == null)
                return;
            this.Changed((object)this, e);
        }

        public virtual void Test(bool cleanup)
        {
            string str1 = "";
            try
            {
                string section = "Profile Test";
                str1 = "initializing the profile -- cleaning up the '" + section + "' section";
                this.RemoveSection(section);
                str1 = "getting the sections and their count";
                string[] sectionNames = this.GetSectionNames();
                int num1 = sectionNames == null ? 0 : sectionNames.Length;
                bool flag1 = num1 > 1;
                str1 = "adding some valid entries to the '" + section + "' section";
                this.SetValue(section, "Text entry", (object)"123 abc");
                this.SetValue(section, "Blank entry", (object)"");
                this.SetValue(section, "Null entry", (object)null);
                this.SetValue(section, "  Entry with leading and trailing spaces  ", (object)"The spaces should be trimmed from the entry");
                this.SetValue(section, "Integer entry", (object)17);
                this.SetValue(section, "Long entry", (object)1234567890123456789L);
                this.SetValue(section, "Double entry", (object)17.95);
                this.SetValue(section, "DateTime entry", (object)DateTime.Today);
                this.SetValue(section, "Boolean entry", (object)flag1);
                str1 = "adding a null entry to the '" + section + "' section";
                try
                {
                    this.SetValue(section, (string)null, (object)"123 abc");
                    throw new Exception("Passing a null entry was allowed for SetValue");
                }
                catch (ArgumentNullException ex)
                {
                }
                str1 = "retrieving a null section";
                try
                {
                    this.GetValue((string)null, nameof(Test));
                    throw new Exception("Passing a null section was allowed for GetValue");
                }
                catch (ArgumentNullException ex)
                {
                }
                str1 = "getting the number of entries and their count";
                int num2 = 8;
                string[] entryNames1 = this.GetEntryNames(section);
                str1 = "verifying the number of entries is " + (object)num2;
                if (entryNames1.Length != num2)
                    throw new Exception("Incorrect number of entries found: " + (object)entryNames1.Length);
                str1 = "checking the values for the entries added";
                string str2 = this.GetValue(section, "Text entry", "");
                if (str2 != "123 abc")
                    throw new Exception("Incorrect string value found for the Text entry: '" + str2 + "'");
                int num3 = this.GetValue(section, "Text entry", 321);
                if (num3 != 0)
                    throw new Exception("Incorrect integer value found for the Text entry: " + (object)num3);
                string str3 = this.GetValue(section, "Blank entry", "invalid");
                if (str3 != "")
                    throw new Exception("Incorrect string value found for the Blank entry: '" + str3 + "'");
                int num4 = this.GetValue(section, "Blank entry") != null ? this.GetValue(section, "Blank entry", 321) : throw new Exception("Incorrect null value found for the Blank entry");
                if (num4 != 0)
                    throw new Exception("Incorrect integer value found for the Blank entry: " + (object)num4);
                bool flag2 = this.GetValue(section, "Blank entry", true);
                if (flag2)
                    throw new Exception("Incorrect bool value found for the Blank entry: " + (object)flag2);
                string str4 = this.GetValue(section, "Null entry", "");
                if (str4 != "")
                    throw new Exception("Incorrect string value found for the Null entry: '" + str4 + "'");
                object obj1 = this.GetValue(section, "Null entry");
                if (obj1 != null)
                    throw new Exception("Incorrect object value found for the Blank entry: '" + obj1 + "'");
                string str5 = this.GetValue(section, "  Entry with leading and trailing spaces  ", "");
                if (str5 != "The spaces should be trimmed from the entry")
                    throw new Exception("Incorrect string value found for the Entry with leading and trailing spaces: '" + str5 + "'");
                int num5 = this.HasEntry(section, "Entry with leading and trailing spaces") ? this.GetValue(section, "Integer entry", 0) : throw new Exception("The Entry with leading and trailing spaces (trimmed) was not found");
                if (num5 != 17)
                    throw new Exception("Incorrect integer value found for the Integer entry: " + (object)num5);
                double num6 = this.GetValue(section, "Integer entry", 0.0);
                if (num6 != 17.0)
                    throw new Exception("Incorrect double value found for the Integer entry: " + (object)num6);
                long int64 = Convert.ToInt64(this.GetValue(section, "Long entry"));
                if (int64 != 1234567890123456789L)
                    throw new Exception("Incorrect long value found for the Long entry: " + (object)int64);
                string str6 = this.GetValue(section, "Long entry", "");
                if (str6 != "1234567890123456789")
                    throw new Exception("Incorrect string value found for the Long entry: '" + str6 + "'");
                double num7 = this.GetValue(section, "Double entry", 0.0);
                if (num7 != 17.95)
                    throw new Exception("Incorrect double value found for the Double entry: " + (object)num7);
                int num8 = this.GetValue(section, "Double entry", 321);
                if (num8 != 0)
                    throw new Exception("Incorrect integer value found for the Double entry: " + (object)num8);
                string s = this.GetValue(section, "DateTime entry", "");
                if (s != DateTime.Today.ToString())
                    throw new Exception("Incorrect string value found for the DateTime entry: '" + s + "'");
                if (DateTime.Parse(s) != DateTime.Today)
                    throw new Exception("The DateTime value is not today's date: '" + s + "'");
                bool flag3 = this.GetValue(section, "Boolean entry", !flag1);
                if (flag3 != flag1)
                    throw new Exception("Incorrect bool value found for the Boolean entry: " + (object)flag3);
                string str7 = this.GetValue(section, "Boolean entry", "");
                if (str7 != flag1.ToString())
                    throw new Exception("Incorrect string value found for the Boolean entry: '" + str7 + "'");
                object obj2 = this.GetValue(section, "Nonexistent entry");
                if (obj2 != null)
                    throw new Exception("Incorrect value found for the Nonexistent entry: '" + obj2 + "'");
                string str8 = this.GetValue(section, "Nonexistent entry", "Some Default");
                if (str8 != "Some Default")
                    throw new Exception("Incorrect default value found for the Nonexistent entry: '" + str8 + "'");
                str1 = "creating a ReadOnly clone of the object";
                IReadOnlyProfile readOnlyProfile = this.CloneReadOnly();
                double num9 = readOnlyProfile.HasSection(section) ? readOnlyProfile.GetValue(section, "Double entry", 0.0) : throw new Exception("The section is missing from the cloned read-only profile");
                if (num9 != 17.95)
                    throw new Exception("Incorrect double value in the cloned object: " + (object)num9);
                str1 = "checking if ReadOnly clone can be hacked to allow writing";
                try
                {
                    ((IProfile)readOnlyProfile).ReadOnly = false;
                    throw new Exception("Changing of the ReadOnly flag was allowed on the cloned read-only profile");
                }
                catch (InvalidOperationException ex)
                {
                }
                try
                {
                    ((IProfile)readOnlyProfile).SetValue(section, "Entry which should not be written", (object)"This should not happen");
                    throw new Exception("SetValue did not throw an InvalidOperationException when writing to the cloned read-only profile");
                }
                catch (InvalidOperationException ex)
                {
                }
                if (!cleanup)
                    return;
                str1 = "deleting the entries just added";
                this.RemoveEntry(section, "Text entry");
                this.RemoveEntry(section, "Blank entry");
                this.RemoveEntry(section, "  Entry with leading and trailing spaces  ");
                this.RemoveEntry(section, "Integer entry");
                this.RemoveEntry(section, "Long entry");
                this.RemoveEntry(section, "Double entry");
                this.RemoveEntry(section, "DateTime entry");
                this.RemoveEntry(section, "Boolean entry");
                str1 = "deleting a nonexistent entry";
                this.RemoveEntry(section, "Null entry");
                str1 = "verifying all entries were deleted";
                string[] entryNames2 = this.GetEntryNames(section);
                if (entryNames2.Length != 0)
                    throw new Exception("Incorrect number of entries still found: " + (object)entryNames2.Length);
                str1 = "deleting the section";
                this.RemoveSection(section);
                str1 = "verifying the section was deleted";
                int length = this.GetSectionNames().Length;
                if (num1 != length)
                    throw new Exception("Incorrect number of sections found after deleting: " + (object)length);
                if (this.GetEntryNames(section) != null)
                    throw new Exception("The section was apparently not deleted since GetEntryNames did not return null");
            }
            catch (Exception ex)
            {
                throw new Exception("Test Failed while " + str1, ex);
            }
        }
    }
}
