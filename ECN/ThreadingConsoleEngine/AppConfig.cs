using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ECN_EngineFramework
{
    public class AppConfig : System.Dynamic.DynamicObject
    {
        public static List<string> SplitCommaSeperatedStringToList(string input)
        {
            return new List<string>(input.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }            
        public static dynamic Get { get { return AppConfig<string>.Get; } }
        public static string LogFile
        {
            get
            {
                return AppConfig<string>.Get.LogFolderPath + AppConfig<string>.Get.LogFileNamePrefix + DateTime.Now.ToString(AppConfig<string>.Get.LogFileNameDateTimeFormat) + AppConfig<string>.Get.LogFileNameSuffix;
            }
        }
    }

    public static class AppConfig<T>
    {
        public static dynamic Get { get { return _config; } }
        static readonly AppConfigProxy _config = new AppConfigProxy();

        class AppConfigProxy : System.Dynamic.DynamicObject
        {
            public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
            {
                var key = binder.Name.ToLower();
                var setting = ConfigurationManager.AppSettings.AllKeys.FirstOrDefault(k => k.Replace(".", "").ToLower().EndsWith(key));
                if (setting != null)
                {
                    string value = ConfigurationManager.AppSettings[setting];
                    Type t = typeof(T);
                    if(t == typeof(string))
                    {
                        result = value;
                    }
                    else if(t.IsEnum)
                    {
                        result = Enum.Parse(t, value);
                    }
                    else
                    {
                        result = Convert.ChangeType(value, t);
                    }
                    return true;
                }
                return base.TryGetMember(binder, out result);
            }
        }
    }    
}
