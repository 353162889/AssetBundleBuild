using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EditorPackage
{
    public static class PathTools
    {
        public static string UnityAssetPath(string path)
        {
            path = UnityPath(path);
            if (path.StartsWith("Assets")) return path;
            return path.Replace(Application.dataPath, "Assets");
        }
        public static string UnityPath(string path)
        {
            return path.Replace("\\", "/");
        }

        public static void GetAllFiles(string dir, List<string> files, string ignoreDir = null,string searchPattern = "*.*",SearchOption searchOption = SearchOption.TopDirectoryOnly, List<string> excludeExtensions = null)
        {
            if (!Directory.Exists(dir))
            {
                return;
            }
            dir = UnityPath(dir);
            //获取所有文件
            string[] innerFiles = Directory.GetFiles(dir,searchPattern, SearchOption.TopDirectoryOnly);
            if (innerFiles != null)
            {
                for (int i = 0; i < innerFiles.Length; i++)
                {
                    bool add = true;
                    if (excludeExtensions != null)
                    {
                        for (int j = 0; j < excludeExtensions.Count; j++)
                        {
                            if(innerFiles[i].EndsWith(excludeExtensions[j]))
                            {
                                add = false;
                                break;
                            }
                        }
                    }
                    if(add)
                    {
                        files.Add(UnityPath(innerFiles[i]));
                    }
                }
            }

            if (searchOption == SearchOption.AllDirectories)
            {
                //递归获取子目录的文件
                var subdirs = Directory.GetDirectories(dir);
                foreach (var subdir in subdirs)
                {
                    if (string.IsNullOrEmpty(ignoreDir) || subdir != ignoreDir)
                    {
                        GetAllFiles(subdir, files,ignoreDir,searchPattern,searchOption, excludeExtensions);
                    }
                }
            }
        }

        public static string GetFileNameForPath(string path,bool includeExt)
        {
            path = UnityPath(path);
            if(!File.Exists(path))
            {
                Debug.LogError("路径:"+path+"不是一个文件");
                return "";
            }
            string file = "";
            int index = path.LastIndexOf("/");
            if (index > -1)
            {
                file = path.Substring(index + 1);
            }
            if(!includeExt)
            {
                file = file.Substring(0, file.LastIndexOf("."));
            }
            return file;
        }

        public static string GetFileExt(string file)
        {
            string ext = "";
            int index = file.LastIndexOf(".");
            if (index > -1)
            {
                ext = file.Substring(index + 1, file.Length - index - 1);
            }
            return ext;
        }

        public static string GetFileExtForPath(string path)
        {
            string fileName = GetFileNameForPath(path,true);
            return GetFileExt(fileName);
        }

        public static string GetFileDirForPath(string path)
        {
            path = UnityPath(path);
            if (path.EndsWith("/")) path = path.Substring(0, path.Length - 1);
            if(Directory.Exists(path))
            {
                return path;
            }
            int index = path.LastIndexOf("/");
            if (index > -1)
            {
                return path.Substring(0,index);
            }
            return path;
        }

        public static void CopyToRenameExtension(string from, string to, string searchParttern, string extension, List<string> movedFiles = null)
        {
            if (Directory.Exists(from))
            {
                if (!Directory.Exists(to))
                {
                    Directory.CreateDirectory(to);
                }
                to = UnityPath(to);

                if (to[to.Length - 1] != '/')
                    to = to + "/";
                string[] files = Directory.GetFiles(from, searchParttern);

                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = UnityPath(Path.GetFileName(files[i]));
                    CopyAndRenameExtension(files[i], to + fileName, extension, movedFiles);
                }

                string[] directorys = Directory.GetDirectories(from);
                for (int i = 0; i < directorys.Length; i++)
                {
                    string tempDir = UnityPath(directorys[i]);
                    int index = tempDir.LastIndexOf("/");
                    if (index > -1)
                    {
                        string directoryName = tempDir.Substring(index + 1);
                        CopyToRenameExtension(UnityPath(directorys[i]), to + directoryName, searchParttern, extension, movedFiles);
                    }
                }
            }
            else if (File.Exists(from))
            {
                CopyAndRenameExtension(from, to, extension, movedFiles);
            }
        }

        private static void CopyAndRenameExtension(string src, string desc, string extension, List<string> movedFiles = null)
        {
            desc = UnityPath(desc);
            int index = desc.LastIndexOf(".");
            if (index > -1)
            {
                string prePath = desc.Substring(0, index);
                File.Copy(src, prePath + extension, true);
                if (movedFiles != null)
                {
                    movedFiles.Add(prePath + extension);
                }
            }
        }

        //拷贝文件夹内容到新的文件夹下
        public static void CopyTo(string from, string to,string searchPattern = "*.*", List<string> excludeExtensions = null)
        {
            if (Directory.Exists(from))
            {
                if (!Directory.Exists(to))
                {
                    Directory.CreateDirectory(to);
                }
                to = UnityPath(to);
                if (to[to.Length - 1] != '/')
                    to = to + "/";
                string[] files = Directory.GetFiles(from, searchPattern);

                for (int i = 0; i < files.Length; i++)
                {
                    if(excludeExtensions != null)
                    {
                        bool isExcludeExtension = false;
                        for (int j = 0; j < excludeExtensions.Count; j++)
                        {
                            if(files[i].EndsWith(excludeExtensions[j]))
                            {
                                isExcludeExtension = true;
                                break;
                            }
                        }
                        if (isExcludeExtension) continue;
                    }
                    string fileName = UnityPath(Path.GetFileName(files[i]));
                    File.Copy(UnityPath(files[i]), to + fileName, true);
                }

                string[] directorys = Directory.GetDirectories(from);
                for (int i = 0; i < directorys.Length; i++)
                {
                    string tempDir = UnityPath(directorys[i]);
                    int index = tempDir.LastIndexOf("/");
                    if (index > -1)
                    {
                        string directoryName = tempDir.Substring(index + 1);
                        CopyTo(UnityPath(directorys[i]), to + directoryName, searchPattern,excludeExtensions);
                    }
                }
            }
            else if (File.Exists(from))
            {
                if (excludeExtensions != null)
                {
                    for (int j = 0; j < excludeExtensions.Count; j++)
                    {
                        if (from.EndsWith(excludeExtensions[j]))
                        {
                            return;
                        }
                    }
                }
                File.Copy(from, to, true);
            }
        }

        //更改文件夹下所有文件的扩展名
        public static void RenameExtensionInDirectory(string dir, string searchParttern, string extension)
        {
            if (Directory.Exists(dir))
            {
                string[] files = Directory.GetFiles(dir, searchParttern, SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    string filePath = UnityPath(f);
                    RenameFileExtension(filePath, extension);
                }
            }
        }

        //更改文件扩展名
        public static void RenameFileExtension(string path, string extension)
        {
            if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                int index = path.LastIndexOf(".");
                if (index > -1)
                {
                    string prePath = path.Substring(0, index);
                    File.Copy(path, prePath + extension);
                    File.Delete(path);
                }
            }
        }

        public static void RemoveDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return;
            }
            string[] files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
            foreach (var path in files)
            {
                string filePath = UnityPath(path);
                File.Delete(filePath);
            }
            RemoveEmptyDir(dirPath);
        }

        public static void RemoveEmptyDir(string dirPath)
        {
            foreach (string path in Directory.GetDirectories(dirPath))
            {
                RemoveEmptyDir(path);
            }
            if (Directory.GetDirectories(dirPath).Length == 0 && Directory.GetFiles(dirPath, "*.*").Length == 0)
            {
                Directory.Delete(dirPath, true);
            }
        }

        public static void RemoveFile(string path)
        {
            if (!File.Exists(path)) return;
            File.Delete(path);
        }
    }
}
