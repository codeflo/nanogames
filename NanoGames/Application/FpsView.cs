﻿// Copyright (c) the authors of NanoGames. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the project root.

using System.Collections.Generic;
using System.Diagnostics;

namespace NanoGames.Application
{
    /// <summary>
    /// A view that measures and draws the current frames per second.
    /// </summary>
    internal sealed class FpsView : IView
    {
        private readonly Queue<long> _times = new Queue<long>();

        /// <inheritdoc/>
        public void Update(Terminal terminal)
        {
            var time = Stopwatch.GetTimestamp();
            if (_times.Count > 0)
            {
                var fps = (double)Stopwatch.Frequency * _times.Count / (time - _times.Peek());
                var fpsString = ((int)(fps + 0.5)).ToString("D2");

                double size = 8;
                terminal.Text(new Color(0.8, 0.6, 0.2), size, new Vector(Terminal.Width - fpsString.Length * size, Terminal.Height - size), fpsString);
            }

            if (_times.Count > 128)
            {
                _times.Dequeue();
            }

            _times.Enqueue(time);
        }
    }
}