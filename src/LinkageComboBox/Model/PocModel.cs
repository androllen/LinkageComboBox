using LinkageComboBox.Converter;
using System;
using System.ComponentModel;
using System.Reflection;

namespace LinkageComboBox.Model
{
    [TypeConverter(typeof(EnumDefaultValueTypeConverter))]
    public enum PocName
    {
        [Description("CVE_2017_12149"), DefaultValue("Tomcat_LoginURL_Leak")]
        Tomcat_LoginURL_Leak,
        [Description("CVE_2017_12149"), DefaultValue("Tomcat_InfoLeak")]
        Tomcat_InfoLeak,
        [Description("CVE_2017_12149"), DefaultValue("Tomcat_Get_Version")]
        Tomcat_Get_Version,
        [Description("CVE_2017_12149"), DefaultValue("Tomcat_file_upload_verify_20170920")]
        Tomcat_file_upload_verify_20170920,
        [Description("CVE_2017_12149"), DefaultValue("Tomcat_file_upload_attack_20170920")]
        Tomcat_file_upload_attack_20170920,
        [Description("CVE_2018_12150"), DefaultValue("Tomcat_file_upload_attack_20180920")]
        Tomcat_file_upload_attack_20180920
    }

    [TypeConverter(typeof(EnumDefaultValueTypeConverter))]
    public enum PocEnv
    {
        [Description("CVE_2017_12149"), DefaultValue("CVE-2017-12149")]
        CVE_2017_12149,
        [Description("CVE_2018_12150"), DefaultValue("CVE-2018-12150")]
        CVE_2017_12150
    }

    public static class EnumHelper
    {
        public static T GetEnumAttribute<T>(Enum source) where T : Attribute
        {
            Type type = source.GetType();
            var sourceName = Enum.GetName(type, source);
            FieldInfo field = type.GetField(sourceName);
            object[] attributes = field.GetCustomAttributes(typeof(T), false);
            foreach (var o in attributes)
            {
                if (o is T)
                    return (T)o;
            }
            return null;
        }

        public static string GetDescription(this Enum source)
        {
            var str = GetEnumAttribute<DescriptionAttribute>(source);
            if (str == null)
                return null;
            return str.Description;
        }
    }
}
