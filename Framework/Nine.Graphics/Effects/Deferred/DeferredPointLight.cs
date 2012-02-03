#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nine.Graphics.Primitives;
#endregion


namespace Nine.Graphics.Effects.Deferred
{
    public partial class DeferredPointLight : IDeferredLight, IEffectMatrices, IEffectTexture, IPointLight
    {
        SphereInvert primitive;

        bool viewProjectionChanged;

        public Matrix View
        {
            get { return view; }
            set
            {
                view = value;
                viewProjectionChanged = true;
            }
        }
        private Matrix view;

        public Matrix Projection
        {
            get { return projection; }
            set
            {
                viewProjectionChanged = true;
                projection = value;
            }
        }
        private Matrix projection;

        private void OnCreated()
        {
            primitive = GraphicsResources<SphereInvert>.GetInstance(GraphicsDevice);

            Range = 10;
            Attenuation = MathHelper.E;
            DiffuseColor = Vector3.One;
        }

        private void OnClone(DeferredPointLight cloneSource)
        {
            view = cloneSource.view;
            projection = cloneSource.projection;
        }

        private void OnApplyChanges()
        {
            if (viewProjectionChanged)
            {
                viewProjection = view * Projection;
                viewProjectionInverse = Matrix.Invert(viewProjection);
                eyePosition = Matrix.Invert(view).Translation;
                viewProjectionChanged = false;
            }
            halfPixel = new Vector2(0.5f / GraphicsDevice.Viewport.Width, 0.5f / GraphicsDevice.Viewport.Height);
        }

        VertexBuffer IDeferredLight.VertexBuffer
        {
            get { return primitive.VertexBuffer; }
        }

        IndexBuffer IDeferredLight.IndexBuffer
        {
            get { return primitive.IndexBuffer; }
        }

        Effect IDeferredLight.Effect
        {
            get { return this; }
        }

        Matrix IEffectMatrices.World
        {
            get { return Matrix.Identity; }
            set { }
        }

        Texture2D IEffectTexture.Texture
        {
            get { return null; }
            set { }
        }

        void IEffectTexture.SetTexture(TextureUsage usage, Texture texture)
        {
            if (usage == TextureUsage.NormalMap)
                NormalBuffer = texture as Texture2D;
            else if (usage == TextureUsage.DepthBuffer)
                DepthBuffer = texture as Texture2D;
        }
    }
}
