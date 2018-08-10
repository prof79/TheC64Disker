//----------------------------------------------------------------------------
// <copyright file="Error.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Basic implementation of INotification for error messages.
// </description>
// <version>v1.0.0 2018-06-14T22:59:00+02</version>
//
// Based on:
//
// Prism's Notification
//
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Interactivity.InteractionRequest
{
    // TODO was notification
    using Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// Basic implementation of Prism.Interactivity.InteractionRequest.INotification
    /// for error messages.
    /// </summary>
    public class Error : INotification
    {
        #region Properties

        /// <summary>
        /// Gets or sets the title to use for the notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the notification.
        /// </summary>
        public object Content { get; set; }

        #endregion
    }
}
