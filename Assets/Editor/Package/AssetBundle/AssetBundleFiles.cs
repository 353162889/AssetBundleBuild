using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace EditorPackage
{
    public static class AssetBundleFiles
    {
        private static Dictionary<string, ABFile> m_dicFile = new Dictionary<string, ABFile>();
        public static void Init()
        {
            m_dicFile.Clear();
            AssetBundleConfig.Init();
            var lst = AssetBundleConfig.lstBuildInfo;
            for (int i = 0; i < lst.Count; i++)
            {
                BuildABFile(lst[i]);
            }
            AssetBundleConfig.Clear();
        }

        public static void Clear()
        {
            m_dicFile.Clear();
        }

        private static void BuildABFile(AssetBundleBuildInfo buildInfo)
        {
            List<string> files = new List<string>();
            switch(buildInfo.packingType)
            {
                case AssetBundlePackingType.Whole:
                    PathTools.GetAllFiles(buildInfo.searchDirectory, files, null, "*.*", SearchOption.AllDirectories, new List<string> { ".meta"});
                    break;
                case AssetBundlePackingType.SubDir:
                    string[] folders = AssetDatabase.GetSubFolders(buildInfo.searchDirectory);
                    //if(folders != null)
                    //{
                    //    List<string> lst = new List<string>();
                    //    for (int i = 0; i < folders.Length; i++)
                    //    {
                    //        var arrFile = AssetDatabase.FindAssets("Object", new string[] { folders });

                    //    }
                    //}
                    break;
                case AssetBundlePackingType.SingleFile:
                    break;
            }
        }
    }

    public class ABFile
    {
        public string file;
        public string bundleName;
        public int refCount;
    }
}
