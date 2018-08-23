using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace CustomizeEditor
{
    public class PackageSettingUtil
    {
        private static string BundleIdentifier = "com.zhongju.buyu";
        private static string CompanyName = "zhongju";
        private static string[] LanguageCodes = new string[] {
            "zh_CN",
            "en_US",
            "uig",
        };

        private static string[] LanguageNameDesc = new string[] {
            "捕鱼帝国",
            "fish",
            "بېلىق تۇتۇش ئىمپېرىيەسى"
        };
        private static int LanguageCodeIndex = 0;
        private static string GetGameName()
        {
            return "";
        }

        public static void ChangeSetting()
        {
            PlayerSettings.productName = GetGameName();
            PlayerSettings.companyName = CompanyName;
            //PlayerSettings.bundleVersion = GameConfig.ShowVersion;
        }
    }
}
