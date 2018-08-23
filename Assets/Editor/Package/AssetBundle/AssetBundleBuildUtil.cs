using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorPackage
{
    public class AssetBundleBuildUtil
    {
        [MenuItem("Tools/TestBuildAssetBundle _F1")]
        public static void BuildTest()
        {
            BuildAssetBundle(BuildTarget.Android, true);
        }
        public static void BuildAssetBundle(BuildTarget buildTarget,bool forceBuild)
        {
            ClearAssetBundleNames();
        }

        private static void ClearAssetBundleNames()
        {
            var arr = AssetDatabase.GetAllAssetBundleNames();
            if(arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    AssetDatabase.RemoveAssetBundleName(arr[i], true);
                }
            }
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
    }
}
