using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AMS.Profile
{
    public class Ini : AMS.Profile.Profile
    {
        public Ini()
        {
        }

        public Ini(string fileName)
          : base(fileName)
        {
        }

        public Ini(Ini ini)
          : base((AMS.Profile.Profile)ini)
        {
        }

        public override string DefaultName => this.DefaultNameWithoutExtension + ".ini";

        public override object Clone() => (object)new Ini(this);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int WritePrivateProfileString(
          string section,
          string key,
          string value,
          string fileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int WritePrivateProfileString(
          string section,
          string key,
          int value,
          string fileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int WritePrivateProfileString(
          string section,
          int key,
          string value,
          string fileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int GetPrivateProfileString(
          string section,
          string key,
          string defaultValue,
          StringBuilder result,
          int size,
          string fileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int GetPrivateProfileString(
          string section,
          int key,
          string defaultValue,
          [MarshalAs(UnmanagedType.LPArray)] byte[] result,
          int size,
          string fileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int GetPrivateProfileString(
          int section,
          string key,
          string defaultValue,
          [MarshalAs(UnmanagedType.LPArray)] byte[] result,
          int size,
          string fileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int GetPrivateProfileString(
          string section,
          string key,
          string defaultValue,
          [MarshalAs(UnmanagedType.LPArray)] byte[] result,
          int size,
          string fileName);

        public override void SetValue(string section, string entry, object value)
        {
            if (section == null || entry == null)
                return;
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
                if (Ini.WritePrivateProfileString(section, entry, value.ToString(), this.Name) == 0)
                    throw new Win32Exception();
                this.RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
            }
        }

        public override object GetValue(string section, string entry)
        {
            if (section == null || entry == null)
                return (object)null;
            this.VerifyName();
            this.VerifyAndAdjustSection(ref section);
            this.VerifyAndAdjustEntry(ref entry);
            int num1 = 250;
            StringBuilder result;
            int num2;
            while (true)
            {
                result = new StringBuilder(num1);
                num2 = 2 * Ini.GetPrivateProfileString(section, entry, "", result, num1, this.Name);
                if (num2 >= num1 - 2)
                    num1 *= 2;
                else
                    break;
            }
            return num2 == 0 && !this.HasEntry(section, entry) ? (object)null : (object)result.ToString();
        }

        public override void RemoveEntry(string section, string entry)
        {
            if (!this.HasEntry(section, entry))
                return;
            this.VerifyNotReadOnly();
            this.VerifyName();
            this.VerifyAndAdjustSection(ref section);
            this.VerifyAndAdjustEntry(ref entry);
            if (!this.RaiseChangeEvent(true, ProfileChangeType.RemoveEntry, section, entry, (object)null))
                return;
            if (Ini.WritePrivateProfileString(section, entry, 0, this.Name) == 0)
                throw new Win32Exception();
            this.RaiseChangeEvent(false, ProfileChangeType.RemoveEntry, section, entry, (object)null);
        }

        public override void RemoveSection(string section)
        {
            if (!this.HasSection(section))
                return;
            this.VerifyNotReadOnly();
            this.VerifyName();
            this.VerifyAndAdjustSection(ref section);
            if (!this.RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, (string)null, (object)null))
                return;
            if (Ini.WritePrivateProfileString(section, 0, "", this.Name) == 0)
                throw new Win32Exception();
            this.RaiseChangeEvent(false, ProfileChangeType.RemoveSection, section, (string)null, (object)null);
        }

        public override string[] GetEntryNames(string section)
        {
            if (!this.HasSection(section))
                return (string[])null;
            this.VerifyAndAdjustSection(ref section);
            int size = 500;
            byte[] numArray;
            int num;
            while (true)
            {
                numArray = new byte[size];
                num = 2 * Ini.GetPrivateProfileString(section, 0, "", numArray, size, this.Name);
                if (num >= size - 3)
                    size *= 2;
                else
                    break;
            }
            string str = Encoding.Unicode.GetString(numArray, 0, num - (num > 0 ? 2 : 0));
            return str == "" ? new string[0] : str.Split(new char[1]);
        }

        public override string[] GetSectionNames()
        {
            if (!File.Exists(this.Name))
                return (string[])null;
            int size = 500;
            byte[] numArray;
            int num;
            while (true)
            {
                numArray = new byte[size];
                num = 2 * Ini.GetPrivateProfileString(0, "", "", numArray, size, this.Name);
                if (num >= size - 3)
                    size *= 2;
                else
                    break;
            }
            string str = Encoding.Unicode.GetString(numArray, 0, num - (num > 0 ? 2 : 0));
            return str == "" ? new string[0] : str.Split(new char[1]);
        }
    }
}
