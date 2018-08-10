//----------------------------------------------------------------------------
// <copyright file="DiskImageEnumerationError.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Event fired on disk image enumeration errors most likely caused
//      by I/O path and access errors.
// </description>
// <version>v1.0.0 2018-06-15T20:21:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Events
{
    using System;
    using Prism.Events;

    /// <summary>
    /// Event fired on disk image enumeration errors most likely caused
    /// by I/O path and access errors.
    /// </summary>
    public class DiskImageEnumerationError : PubSubEvent<Exception>
    {
    }
}
