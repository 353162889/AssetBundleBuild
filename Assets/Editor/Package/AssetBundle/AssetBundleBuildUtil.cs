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
            List<string> files = new List<string>();
            PathTools.GetAllFiles(PathConfig.AssetsRootDir+"/ResourceEx", files, null, "*.*", SearchOption.AllDirectories, new List<string> { ".meta" });
            for (int i = 0; i < files.Count; i++)
            {
                Debug.Log(files[i]);
            }
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
