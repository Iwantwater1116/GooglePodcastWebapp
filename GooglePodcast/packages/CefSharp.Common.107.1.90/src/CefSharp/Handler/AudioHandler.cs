// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Structs;

namespace CefSharp.Handler
{
    /// <summary>
    /// Inherit from this class to handle audio events
    /// All methods will be called on the CEF UI thread
    /// </summary>
    public class AudioHandler : IAudioHandler
    {
        private bool isDisposed;

        /// <summary>
        /// Gets a value indicating this <see cref="AudioHandler"/> instance
        /// has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        /// <inheritdoc/>
        bool IAudioHandler.GetAudioParameters(IWebBrowser chromiumWebBrowser, IBrowser browser, ref AudioParameters parameters)
        {
            return GetAudioParameters(chromiumWebBrowser, browser, ref parameters);
        }

        /// <summary>
        /// Called on the CEF UI thread to allow configuration of audio stream parameters.
        /// Audio stream paramaters can optionally be configured by modifying <paramref name="parameters"/>
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="parameters">audio stream parameters can optionally be configured here, they are
        /// pre-filled with some sensible defaults.</param>
        /// <returns>Return true to proceed with audio stream capture, or false to cancel it</returns>
        protected virtual bool GetAudioParameters(IWebBrowser chromiumWebBrowser, IBrowser browser, ref AudioParameters parameters)
        {
            return false;
        }

        /// <inheritdoc/>
        void IAudioHandler.OnAudioStreamStarted(IWebBrowser chromiumWebBrowser,
            IBrowser browser,
            AudioParameters parameters,
            int channels)
        {
            OnAudioStreamStarted(chromiumWebBrowser, browser, parameters, channels);
        }

        /// <summary>
        /// Called on a browser audio capture thread when the browser starts streaming audio.
        /// OnAudioStreamStopped will always be called after OnAudioStreamStarted; both methods may be called multiple
        /// times for the same browser.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="parameters">contains the audio parameters like sample rate and channel layout.
        /// Changing the param values will have no effect here.</param>
        /// <param name="channels">is the number of channels</param>
        protected virtual void OnAudioStreamStarted(IWebBrowser chromiumWebBrowser,
            IBrowser browser,
            AudioParameters parameters,
            int channels)
        {

        }

        /// <inheritdoc/>
        void IAudioHandler.OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr data, int noOfFrames, long pts)
        {
            OnAudioStreamPacket(chromiumWebBrowser, browser, data, noOfFrames, pts);
        }

        /// <summary>
        /// Called on the audio stream thread when a PCM packet is received for the stream.
        /// Based on and the <see cref="AudioParameters.ChannelLayout"/> value passed to <see cref="OnAudioStreamStarted"/>
        /// you can calculate the size of the <paramref name="data"/> array in bytes.
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser">the browser object</param>
        /// <param name="data">is an array representing the raw PCM data as a floating point type, i.e. 4-byte value(s).</param>
        /// <param name="noOfFrames">is the number of frames in the PCM packet</param>
        /// <param name="pts">is the presentation timestamp (in milliseconds since the Unix Epoch)
        /// and represents the time at which the decompressed packet should be presented to the user</param>
        protected virtual void OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr data, int noOfFrames, long pts)
        {

        }

        /// <inheritdoc/>
        void IAudioHandler.OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            OnAudioStreamStopped(chromiumWebBrowser, browser);
        }

        /// <summary>
        /// Called on the CEF UI thread when the stream has stopped. OnAudioStreamStopped will always be called after <see cref="OnAudioStreamStarted"/>;
        /// both methods may be called multiple times for the same stream.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        protected virtual void OnAudioStreamStopped(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        /// <inheritdoc/>
        void IAudioHandler.OnAudioStreamError(IWebBrowser chromiumWebBrowser, IBrowser browser, string errorMessage)
        {
            OnAudioStreamError(chromiumWebBrowser, browser, errorMessage);
        }

        /// <summary>
        /// Called on the CEF UI thread or audio stream thread when an error occurred. During the
        /// stream creation phase this callback will be called on the UI thread while
        /// in the capturing phase it will be called on the audio stream thread. The
        /// stream will be stopped immediately.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="errorMessage">error message</param>
        protected virtual void OnAudioStreamError(IWebBrowser chromiumWebBrowser, IBrowser browser, string errorMessage)
        {

        }

        /// <summary>
        /// Releases unmanaged and managed resources
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            isDisposed = true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
