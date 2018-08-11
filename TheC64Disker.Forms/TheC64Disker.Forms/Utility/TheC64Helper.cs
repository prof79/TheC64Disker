//----------------------------------------------------------------------------
// <copyright file="TheC64Helper.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Helper class to handle C64 disk images and I/O.
// </description>
// <version>v0.9.1 2018-08-11T21:32:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
        /// Gets the extension of CBM 8250 disk images.
        /// Please note that <see cref="FileInfo"/> includes
        /// the dot (".") in the extension eg. ".txt".
        /// </summary>
        /// <remarks>
        /// Added by request due to some compilation disk
        /// images around using this format and being
        /// compatible to the THEC64 Mini.
        /// </remarks>
        public static string ImageExtensionCbm8250
            => ".d82";

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
                        .Union(
                            Directory
                                .EnumerateFiles(
                                DriveRoot,
                                $"*{ImageExtensionCbm8250}",
                                SearchOption.AllDirectories)
                        )
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
                throw new FileNotFoundException("Root path does not exist.", DriveRoot);
            }
        }

        public static OperationResult CopyToDefaultImage(
            this DiskImage image,
            Func<bool> overwriteHandler = null,
            Action<Exception> errorHandler = null)
        {
            if (image == null)
            {
                throw new ArgumentException(
                    "Disk image argument must not be null.",
                    nameof(image));
            }

            if (!image.Exists)
            {
                errorHandler?.Invoke(
                    new ArgumentException(
                        $"Disk image with path '{image.FullName}' not found on disk.",
                        nameof(image)));

                return OperationResult.Error;
            }

            if (!image.IsValid)
            {
                throw new ArgumentException(
                    $"Judging by the extension '{image.FullName}' is not a valid C64 disk image.",
                    nameof(image));
            }

            var targetPath = DriveRoot.Combine(TheC64MiniDiskName);

            var overwrite = false;

            // If the target file already exists we must ask the user for
            // permission to replace it.
            if (targetPath.Exists())
            {
                // Is there a callback to ask the user?
                if (overwriteHandler == null)
                {
                    // Developer most likely forgot to supply an overwrite handler.
                    return OperationResult.Error;
                }
                else
                {
                    overwrite = overwriteHandler();

                    if (!overwrite)
                    {
                        return OperationResult.Cancelled;
                    }
                }
            }

            // We're clear

            try
            {
                File.Copy(image.FullName, targetPath, overwrite);

                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);

                return OperationResult.Error;
            }
        }

        #endregion
    }
}
