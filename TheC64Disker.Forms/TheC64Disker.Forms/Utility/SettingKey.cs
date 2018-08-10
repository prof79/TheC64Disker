//----------------------------------------------------------------------------
// <copyright file="SettingKey.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Strongly-typed access to configuration file keys.
// </description>
// <version>v0.9.0 2018-06-13T02:08:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Utility
{
    using System;

    /// <summary>
    /// Strongly-typed access to configuration file keys.
    /// </summary>
    public static class SettingKey
    {
        #region Properties

        public static string ForceRootPath
            => nameof(ForceRootPath);

        #endregion
    }
}
