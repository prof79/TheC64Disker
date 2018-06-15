//----------------------------------------------------------------------------
// <copyright file="CloseEvent.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      A Prism event to signal UI elements like windows to close.
// </description>
// <version>v1.0.0 2018-06-15T02:07:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Events
{
    using Prism.Events;

    /// <summary>
    /// A Prism event to signal UI elements like windows to close.
    /// </summary>
    public class CloseEvent : PubSubEvent
    {
    }
}
