//----------------------------------------------------------------------------
// <copyright file="OperationResult.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      An enumeration of results a general operation can yield.
// </description>
// <version>v1.0.0 2018-06-14T23:39:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Utility
{
    /// <summary>
    /// An enumeration of results a general operation can yield.
    /// </summary>
    public enum OperationResult
    {
        Cancelled,
        Error,
        Success,
    }
}
