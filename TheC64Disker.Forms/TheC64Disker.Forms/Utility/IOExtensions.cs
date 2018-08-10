//----------------------------------------------------------------------------
// <copyright file="IOExtensions.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Custom I/O helper extensions.
// </description>
// <version>v0.8.0 2018-06-13T02:17:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Utility
{
    using System;
    using System.IO;

    /// <summary>
    /// Custom I/O helper extensions.
    /// </summary>
    public static class IOExtensions
    {
        #region Extension Methods

        public static FileInfo GetFileInfo(this string fileSystemPath)
            => new FileInfo(fileSystemPath);

        public static DirectoryInfo GetDirectoryInfo(this string fileSystemPath)
            => new DirectoryInfo(fileSystemPath);

        public static DirectoryInfo GetParentDirectory(this string fileSystemPath)
        {
            if (fileSystemPath.IsFile())
            {
                var fileInfo = fileSystemPath.GetFileInfo();

                return fileInfo.Directory;
            }
            else if (fileSystemPath.IsDirectory())
            {
                var directoryInfo = fileSystemPath.GetDirectoryInfo();

                return directoryInfo.Parent;
            }
            else
            {
                return null;
            }
        }

        public static string GetDriveForPath(this string fileSystemPath)
        {
            var info = new FileInfo(fileSystemPath);

            var drive = Array.Find(
                DriveInfo.GetDrives(),
                driveInfo => fileSystemPath.StartsWith(driveInfo.RootDirectory.FullName));

            return drive.RootDirectory.FullName;
        }

        public static bool IsDirectory(this string fileSystemPath)
            => Directory.Exists(fileSystemPath);

        public static bool IsFile(this string fileSystemPath)
            => File.Exists(fileSystemPath);

        public static bool Exists(this string fileSystemPath)
            => fileSystemPath.IsDirectory()
                || fileSystemPath.IsFile();

        public static string Combine(this string fileSystemPath1, string fileSystemPath2)
            => Path.Combine(fileSystemPath1, fileSystemPath2);

        #endregion
    }
}
