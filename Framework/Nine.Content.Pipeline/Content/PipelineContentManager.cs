﻿#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Nine.Content.Pipeline
{
    class PipelineContentManager : ContentManager
    {
        public Dictionary<string, Stream> MemoryStreams { get; private set; }

        public PipelineContentManager(GraphicsDevice graphics)
            : base(new PipelineGraphicsDeviceService() { GraphicsDevice = graphics })
        {
            MemoryStreams = new Dictionary<string, Stream>(StringComparer.OrdinalIgnoreCase);
        }

        public PipelineContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            MemoryStreams = new Dictionary<string, Stream>(StringComparer.OrdinalIgnoreCase);
        }

        public override T Load<T>(string assetName)
        {
            return base.Load<T>(assetName);
        }

        protected override Stream OpenStream(string assetName)
        {
            Stream memoryStream;
            if (MemoryStreams.TryGetValue(assetName, out memoryStream))
                return memoryStream;

            if (!assetName.EndsWith(".xnb", StringComparison.OrdinalIgnoreCase))
                assetName = assetName + ".xnb";
            return File.OpenRead(assetName);
        }
    }

    class PipelineGraphicsDeviceService : IGraphicsDeviceService, IServiceProvider
    {
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        public GraphicsDevice GraphicsDevice { get; set; }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IGraphicsDeviceService) ? this : null;
        }
    }
}
