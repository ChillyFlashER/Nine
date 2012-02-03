﻿#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Statements
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Nine.Graphics.ObjectModel
{
    /// <summary>
    /// Contains extension method for <c>ModelBatch</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ModelBatchExtensions
    {
        /// <summary>
        /// Draws a drawable surface using ModelBatch.
        /// </summary>
        public static void DrawSurface(this ModelBatch modelBatch, DrawableSurface surface, Effect effect)
        {
            foreach (var patch in surface.Patches)
            {
                DrawSurface(modelBatch, patch, effect);
            }
        }

        /// <summary>
        /// Draws a drawable surface using ModelBatch.
        /// </summary>
        public static void DrawSurface(this ModelBatch modelBatch, DrawableSurfacePatch surfacePatch, Effect effect)
        {
            modelBatch.DrawVertices(surfacePatch.VertexBuffer, surfacePatch.IndexBuffer, 0,
                                    surfacePatch.VertexCount, surfacePatch.StartIndex, surfacePatch.PrimitiveCount,
                                    surfacePatch.Transform, effect, null, null, null);
        }
        /// <summary>
        /// Draws a drawable surface using ModelBatch.
        /// </summary>
        public static void DrawSurface(this ModelBatch modelBatch, DrawableSurfacePatch surfacePatch, IEffectInstance effect)
        {
            modelBatch.DrawVertices(surfacePatch.VertexBuffer, surfacePatch.IndexBuffer, 0,
                                    surfacePatch.VertexCount, surfacePatch.StartIndex, surfacePatch.PrimitiveCount,
                                    surfacePatch.Transform, null, effect, null, null);
        }
    }
}
