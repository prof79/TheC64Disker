//----------------------------------------------------------------------------
// <copyright file="DiskImage.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Abstraction of a C64 disk image on a modern computer file system.
// </description>
// <version>v0.8.1 2018-06-13T02:42:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Models
{
    using System;
    using System.IO;
    using System.Linq;
    using Utility;

    /// <summary>
    /// Abstraction of a C64 disk image on a modern computer file system.
    /// </summary>
    public class DiskImage
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DiskImage class.
        /// Default parameterless constructor.
        /// </summary>
        public DiskImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DiskImage class.
        /// </summary>
        /// <param name="fullName">
        /// The fully qualified file system path of the image file.
        /// </param>
        public DiskImage(string fullName)
        {
            FullName = fullName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the fully qualified file system path of the image file.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the "active" flag which means that this image is
        /// currently used as the default disk image on the THEC64 Mini.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets the short name of the image file (without path).
        /// </summary>
        public string Name
            => new FileInfo(FullName).Name;

        /// <summary>
        /// Gets whether the image is a valid C64 disk file
        /// (based on extension only for the time being).
        /// </summary>
        public bool IsValid
            => new FileInfo(FullName)
                .Extension.Equals(
                    TheC64Helper.ImageExtension,
                    StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets whether the <see cref="FullName"/> property specifies
        /// a valid existing file name.
        /// </summary>
        public bool Exists
            => FullName.IsFile();

        /// <summary>
        /// Gets whether the disk image name equals the hard-coded
        /// default disk image name of the THEC64 Mini.
        /// </summary>
        public bool IsC64MiniDefaultImage
            => FullName.Equals(
                TheC64Helper.DriveRoot?.Combine(TheC64Helper.TheC64MiniDiskName),
                StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets the AES256 hash of the disk image file in
        /// lower-case hexadecimal string form.
        /// </summary>
        public string Aes256Hash
        {
            get
            {
                var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();

                var hash = new byte[] { };

                if (!Exists)
                {
                    hash = sha256.ComputeHash(new byte[] { });
                }
                else
                {
                    using (var reader = new StreamReader(FullName))
                    {
                        hash = sha256.ComputeHash(reader.BaseStream);
                    }
                }

                return
                    hash
                        .Select(by => String.Format("{0:x2}", by))
                        .Aggregate(String.Empty, (accu, str) => accu + str);
            }
        }

        #endregion

        #region Overridden Methods

        public override string ToString()
            => Name;

        #endregion
    }
}
