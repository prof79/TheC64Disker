//----------------------------------------------------------------------------
// <copyright file="TheC64Helper.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Helper class to handle C64 disk images and I/O.
// </description>
// <version>v0.1.0 2018-06-13T01:24:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Models;

    /// <summary>
    /// Helper class to handle C64 disk images and I/O.
    /// </summary>
    public static class TheC64Helper
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default extension of C64 disk images.
        /// Please note that <see cref="FileInfo"/> includes
        /// the dot (".") in the extension eg. ".txt".
        /// </summary>
        public static string ImageExtension
            => ".d64";

        /// <summary>
        /// Gets the hard-coded default disk image name used
        /// in the THEC64 Mini up to at least firmware 1.0.8
        /// (no disk swapping/loading UI).
        /// </summary>
        public static string TheC64MiniDiskName
            => "THEC64-drive8.d64";

        public static string DriveRoot
        {
            get
            {
                // Enable development/debugging - if set prefer specific path
                // as "root" instead of the application's drive root.
                if (ConfigurationManager.AppSettings.HasKeys()
                    && !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[SettingKey.ForceRootPath]))
                {
                    return
                        ConfigurationManager.AppSettings[SettingKey.ForceRootPath];
                }
                else
                {
                    return GetAssemblyPath().GetDriveForPath();
                }
            }
        }

        #endregion

        #region Methods

        public static string GetAssemblyPath()
            => Assembly.GetExecutingAssembly().Location;

        public static IEnumerable<DiskImage> GetDiskImages()
        {
            if (DriveRoot.IsDirectory())
            {
                var images =
                    Directory
                        .EnumerateFiles(
                            DriveRoot,
                            $"*{ImageExtension}",
                            SearchOption.AllDirectories)
                        .Select(path => new DiskImage { FullName = path })
                        .ToList();

                var defaultImage =
                    images.SingleOrDefault(image => image.IsC64MiniDefaultImage);

                if (defaultImage != null)
                {
                    var defaultImageHash = defaultImage.Aes256Hash;

                    foreach (var image in images)
                    {
                        if (image.Aes256Hash.Equals(defaultImageHash, StringComparison.InvariantCultureIgnoreCase))
                        {
                            image.IsActive = true;
                        }
                    }
                }

                return images;
            }
            else
            {
                return null;
            }
        }

        public static bool CopyToDefaultImage(this DiskImage image, Func<bool> overwriteHandler = null)
        {
            if (image == null)
            {
                throw new ArgumentException(
                    "Disk image argument must not be null.",
                    nameof(image));
            }

            if (!image.Exists)
            {
                throw new ArgumentException(
                    $"Disk image with path '{image.FullName}' not found on disk.",
                    nameof(image));
            }

            if (!image.IsValid)
            {
                throw new ArgumentException(
                    $"Judging by the extension '{image.FullName}' is not a valid C64 disk image.",
                    nameof(image));
            }

            var targetPath = DriveRoot.Combine(TheC64MiniDiskName);

            var overwrite = false;

            if (targetPath.Exists() && overwriteHandler == null)
            {
                // TODO: Proper error feedback
                return false;
            }
            else
            {
                overwrite = overwriteHandler();
            }

            // We're clear

            try
            {
                File.Copy(image.FullName, targetPath, overwrite);

                return true;
            }
            catch (Exception)
            {
                // TODO: We need better error handling.
                return false;
            }
        }

        #endregion
    }
}
