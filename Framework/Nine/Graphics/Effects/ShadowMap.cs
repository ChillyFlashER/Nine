﻿#region Copyright 2009 - 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Nine.Graphics.Effects;
#endregion

namespace Nine.Graphics.Effects
{
    /// <summary>
    /// Represents a shadow drawing technique using shadowmap.
    /// </summary>
    public class ShadowMap : IDisposable
    {
        BlurEffect blur;
        RenderTarget2D renderTarget;
        RenderTarget2D depthBlur;

        public GraphicsDevice GraphicsDevice { get; private set; }

        public int Width { get { return renderTarget.Width; } }
        public int Height { get { return renderTarget.Height; } }
        public SurfaceFormat SurfaceFormat { get { return renderTarget.Format; } }
        public Texture2D Texture { get; private set; }

        public bool BlurEnabled { get; set; }

        public BlurEffect Blur
        {
            get 
            {
                if (blur == null)
                {
                    blur = new BlurEffect(GraphicsDevice);
                    
                    // A 3x3 blur core will produce a good result while still retain framerate.
                    blur.SampleCount = BlurEffect.MinSampleCount;
                }

                return blur; 
            }
        }

        public ShadowMap(GraphicsDevice graphics, int resolution)
            : this(graphics, resolution, resolution)
        {
        }

        public ShadowMap(GraphicsDevice graphics, int width, int height)
        {
            GraphicsDevice = graphics;

            //renderTarget = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Single,
            //                                  graphics.PresentationParameters.DepthStencilFormat);

            renderTarget = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Color,
                                              graphics.PresentationParameters.DepthStencilFormat);
        }

        public void Begin()
        {
            renderTarget.Begin();
        }

        public Texture2D End()
        {
            Texture2D map = renderTarget.End();

            if (BlurEnabled)
            {
                if (depthBlur == null)
                    depthBlur = new RenderTarget2D(GraphicsDevice, Width, Height, false, SurfaceFormat,
                                                   GraphicsDevice.PresentationParameters.DepthStencilFormat);

                // Blur H
                depthBlur.Begin();
                Blur.Direction = MathHelper.PiOver4;

                GraphicsDevice.DrawSprite(map, SamplerState.PointWrap, Color.White, Blur);

                map = depthBlur.End();

                // Blur V
                renderTarget.Begin();
                Blur.Direction = -MathHelper.PiOver4;

                GraphicsDevice.DrawSprite(map, SamplerState.PointWrap, Color.White, Blur);

                map = renderTarget.End();

                // Restore states
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }

            return Texture = map;
        }
        
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (renderTarget != null)
                    renderTarget.Dispose();
                if (Texture != null)
                    Texture.Dispose();
            }
        }

        ~ShadowMap()
        {
            Dispose(false);
        }
    }
}
