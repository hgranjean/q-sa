using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Utility.XML
{
    internal static class FileSystem
    {
#if SILVERLIGHT
        private const string TempFileExtension = "tmp";
        private const string IsolatedTempDir = "TEMP";
        private static readonly IsolatedStorageFile _isolatedStorageFile;
#endif
        private static readonly FileSystemDisposer _disposerInstance;	// Instance used to clean up TempPath when garbage collected

        static FileSystem()
        {
#if SILVERLIGHT
            _isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            _isolatedStorageFile.CreateDirectory(IsolatedTempDir);
#endif

            _disposerInstance = new FileSystemDisposer();
        }

        private class FileSystemDisposer : IDisposable
        {
            public void Dispose()
            {
                DisposeInternal(false); //fromFinalizer
            }

            private void DisposeInternal(bool fromFinalizer)
            {
#if SILVERLIGHT
                if(_isolatedStorageFile != null)
                {
                    _isolatedStorageFile.Dispose();
                }
#endif

                if (!fromFinalizer)
                {
                    GC.SuppressFinalize(this);
                }
            }

            ~FileSystemDisposer()
            {
                try
                {
                    DisposeInternal(true); //fromFinalizer
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception during finalizer: " + ex);
                }
            }

            public override string ToString()
            {
                return typeof(FileSystem).Name + " initialized";
            }
        }

        public static class Directory
        {
            public static bool Exists(string path)
            {
#if SILVERLIGHT
                return _isolatedStorageFile.DirectoryExists(path);
#else
                return System.IO.Directory.Exists(path);
#endif
            }

            public static void Create(string path)
            {
#if SILVERLIGHT
                _isolatedStorageFile.CreateDirectory(path);
#else
                System.IO.Directory.CreateDirectory(path);
#endif
            }

            public static string[] GetFileSystemEntries(string path)
            {
#if SILVERLIGHT
                string[] filePaths = _isolatedStorageFile.GetFileNames(path);
                string[] directoryPaths = _isolatedStorageFile.GetDirectoryNames(path);
                return filePaths.ConcatArray(directoryPaths);
#else
                return System.IO.Directory.GetFileSystemEntries(path);
#endif
            }
        }

        public static class File
        {
            public static Stream Open(string path, FileMode mode)
            {
#if SILVERLIGHT
                return new IsolatedStorageFileStream(path, mode, _isolatedStorageFile);
#else
                return new FileStream(path, mode);
#endif
            }

            public static Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
            {
#if SILVERLIGHT
                return new IsolatedStorageFileStream(path, mode, access, share, _isolatedStorageFile);
#else
                return new FileStream(path, mode, access, share);
#endif
            }

            public static Stream Open(string path, FileMode mode, FileAccess access)
            {
#if SILVERLIGHT
                return new IsolatedStorageFileStream(path, mode, access, FileShare.None, _isolatedStorageFile);
#else
                return new FileStream(path, mode, access, FileShare.None);
#endif
            }

            public static bool Exists(string path)
            {
#if SILVERLIGHT
                return _isolatedStorageFile.FileExists(path);
#else
                return System.IO.File.Exists(path);
#endif
            }

            public static void Delete(string path)
            {
#if SILVERLIGHT
                _isolatedStorageFile.DeleteFile(path);
#else
                System.IO.File.Delete(path);
#endif
            }

            public static void Copy(string sourceFileName, string destFileName)
            {
#if SILVERLIGHT
                _isolatedStorageFile.CopyFile(sourceFileName, destFileName);
#else
                System.IO.File.Copy(sourceFileName, destFileName);
#endif
            }

            public static void Copy(string sourceFileName, string destFileName, bool overwrite)
            {
#if SILVERLIGHT
                _isolatedStorageFile.CopyFile(sourceFileName, destFileName, overwrite);
#else
                System.IO.File.Copy(sourceFileName, destFileName, overwrite);
#endif
            }
        }

        [Serializable]
        public abstract class FileSystemInfo
        {
#if SILVERLIGHT
            protected string _fullPath;
#else
            protected System.IO.FileSystemInfo _fileSystemInfo;
#endif

#if SILVERLIGHT
            protected FileSystemInfo(string fileSystemPath)
            {
                _fullPath = fileSystemPath;
            }

            public static FileSystemInfo Create(string fileSystemPath)
            {
                if(fileSystemPath != null)
                {
                    int lastPathSeparator = fileSystemPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                    int lastAltPathSeparator = fileSystemPath.LastIndexOf(System.IO.Path.AltDirectorySeparatorChar);
                    int lastSeparator = Math.Max(lastPathSeparator, lastAltPathSeparator);
                    int lastExtensionDelimiter = fileSystemPath.LastIndexOf('.');
                    if (lastSeparator > lastExtensionDelimiter)
                    {
                        return new FileSystem.DirectoryInfo(fileSystemPath);
                    }
                    else
                    {
                        return new FileSystem.FileInfo(fileSystemPath);
                    }
                }
                return null;
            }
#else
            protected FileSystemInfo(System.IO.FileSystemInfo fileSystemInfo)
            {
                _fileSystemInfo = fileSystemInfo;
            }

            public static FileSystemInfo Create(System.IO.FileSystemInfo fileSystemInfo)
            {
                if (fileSystemInfo != null)
                {
                    if (fileSystemInfo is System.IO.DirectoryInfo)
                    {
                        return new FileSystem.DirectoryInfo((System.IO.DirectoryInfo)fileSystemInfo);
                    }
                    else if (fileSystemInfo is System.IO.FileInfo)
                    {
                        return new FileSystem.FileInfo((System.IO.FileInfo)fileSystemInfo);
                    }
                }
                return null;
            }
#endif

            public virtual string FullName
            {
                get
                {
#if SILVERLIGHT
                    return _fullPath;
#else
                    return _fileSystemInfo.FullName;
#endif
                }
            }
            public abstract bool Exists { get; }

            public abstract void Delete();
        }

        [Serializable]
        public class DirectoryInfo : FileSystemInfo
        {
#if !SILVERLIGHT
            private System.IO.DirectoryInfo _directoryInfo;
#endif

            public DirectoryInfo(string directoryPath)
#if SILVERLIGHT
                : base(directoryPath)
#else
                : this(new System.IO.DirectoryInfo(directoryPath))
#endif
            {
            }

#if !SILVERLIGHT
            public DirectoryInfo(System.IO.DirectoryInfo directoryInfo)
                : base(directoryInfo)
            {
                _directoryInfo = (System.IO.DirectoryInfo)base._fileSystemInfo;
            }

            //public static implicit operator DirectoryInfo(System.IO.DirectoryInfo directoryInfo)
            //{
            //    return new DirectoryInfo(directoryInfo);
            //}
#endif

            public override bool Exists
            {
                get
                {
#if SILVERLIGHT
                    return Directory.Exists(_fullPath);
#else
                    return _directoryInfo.Exists;
#endif
                }
            }

            public override void Delete()
            {
#if SILVERLIGHT
                //TODO possible in Silverlight?
                //Directory.Delete(_fullPath);
#else
                _directoryInfo.Delete();
#endif
            }

            public DirectoryInfo CreateSubdirectory(string relativePath)
            {
#if SILVERLIGHT
                string subDirectoryPath = Path.Combine(_fullPath, relativePath);
                Directory.Create(subDirectoryPath);
                return new DirectoryInfo(subDirectoryPath);
#else
                return new DirectoryInfo(_directoryInfo.CreateSubdirectory(relativePath));
#endif
            }

            public string Name
            {
                get
                {
#if SILVERLIGHT
                    return System.IO.Path.GetDirectoryName(_fullPath);
#else
                    return _directoryInfo.Name;
#endif
                }
            }

#if SILVERLIGHT
            public FileSystemInfo[] GetFileSystemInfos()
            {
                string[] iofileSystemInfos = Directory.GetFileSystemEntries(_fullPath);
                FileSystemInfo[] fileSystemInfos = new FileSystemInfo[iofileSystemInfos.Length];
                for (int i = 0; i < iofileSystemInfos.Length; i++)
                {
                    fileSystemInfos[i] = FileSystem.FileSystemInfo.Create(iofileSystemInfos[i]);
                }
                return fileSystemInfos;
            }
#else
            public FileSystemInfo[] GetFileSystemInfos()
            {
                System.IO.FileSystemInfo[] iofileSystemInfos = _directoryInfo.GetFileSystemInfos();
                FileSystemInfo[] fileSystemInfos = new FileSystemInfo[iofileSystemInfos.Length];
                for (int i = 0; i < iofileSystemInfos.Length; i++)
                {
                    fileSystemInfos[i] = FileSystem.FileSystemInfo.Create(iofileSystemInfos[i]);
                }
                return fileSystemInfos;
            }
#endif
        }

        [Serializable]
        public class FileInfo : FileSystemInfo
        {
#if !SILVERLIGHT
            private System.IO.FileInfo _fileInfo;
#endif

            public FileInfo(string filePath)
#if SILVERLIGHT
                : base(filePath)
#else
                : this(new System.IO.FileInfo(filePath))
#endif
            {
            }

#if !SILVERLIGHT
            public FileInfo(System.IO.FileInfo fileInfo)
                : base(fileInfo)
            {
                _fileInfo = (System.IO.FileInfo)base._fileSystemInfo;
            }

            //public static implicit operator FileInfo(System.IO.FileInfo fileInfo)
            //{
            //    return new FileInfo(fileInfo);
            //}
#endif

            public override bool Exists
            {
                get
                {
#if SILVERLIGHT
                    return File.Exists(_fullPath);
#else
                    return _fileInfo.Exists;
#endif
                }
            }

            public override void Delete()
            {
#if SILVERLIGHT
                File.Delete(_fullPath);
#else
                _fileInfo.Delete();
#endif
            }

            public long Length
            {
                get
                {
#if SILVERLIGHT
                    throw new NotImplementedException();
#else
                    return _fileInfo.Length;
#endif
                }
            }

            public Stream Open(FileMode mode)
            {
                return File.Open(this.FullName, mode);
            }

            public Stream Open(FileMode mode, FileAccess access)
            {
                return File.Open(this.FullName, mode, access);
            }

            public Stream Open(FileMode mode, FileAccess access, FileShare share)
            {
                return File.Open(this.FullName, mode, access, share);
            }

            public string Name
            {
                get
                {
#if SILVERLIGHT
                    return System.IO.Path.GetFileName(_fullPath);
#else
                    return _fileInfo.Name;
#endif
                }
            }

            public DateTime LastWriteTimeUtc
            {
                get
                {
#if SILVERLIGHT
                    return DateTime.MinValue;
#else
                    return _fileInfo.LastWriteTimeUtc;
#endif
                }
            }

#if !SILVERLIGHT
            public FileInfo CopyTo(string destFileName)
            {
                if (File.Exists(destFileName))
                {
                    File.Delete(destFileName);
                }
                return new FileInfo(_fileInfo.CopyTo(destFileName));

            }
#endif
        }

        public static class Path
        {
            public static string Combine(params string[] paths)
            {
                return System.IO.Path.Combine(paths);
            }

            public static string GetTempFileName(string tempPath)
            {
                return System.IO.Path.Combine(tempPath, System.IO.Path.GetRandomFileName());
            }
            public static string GetTempPath()
            {
                return System.IO.Path.GetTempPath();
            }

            public static string GetFullPath(string ruleApplicationPath)
            {
#if SILVERLIGHT
                return ruleApplicationPath;
#else
                return System.IO.Path.GetFullPath(ruleApplicationPath);
#endif
            }
        }
    }
}
