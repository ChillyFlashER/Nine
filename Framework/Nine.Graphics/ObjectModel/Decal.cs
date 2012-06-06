#region Copyright 2012 (c) Engine Nine
//=============================================================================
//
//  Copyright 2012 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nine.Graphics.Materials;
using Nine.Graphics.Drawing;
#endregion

namespace Nine.Graphics.ObjectModel
{
    /// <summary>
    /// Defines a textured decal that can be projected to the triangle surfaces of the scene.
    /// </summary>
    [Obsolete("Not Implemented")]
    public class Decal : Transformable, ISpatialQueryable, IDrawableObject, IDisposable
    {
        #region Const
        const int GeometryDirtyMask = 1 << 0;
        const int BoundingBoxDirtyMask = 1 << 1;
        const int OrientedBoundingBoxDirtyMask = 1 << 2;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Gets whether this object is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the opaque of this decal.
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// Gets or sets the color of this decal.
        /// </summary>
        public Vector3 Color { get; set; }

        /// <summary>
        /// Gets or sets the size of this decal.
        /// </summary>
        public Vector3 Size
        {
            get { return size; }
            set
            {
                size = value; 
                dirtyMask |= (GeometryDirtyMask | BoundingBoxDirtyMask | OrientedBoundingBoxDirtyMask); 
            }
        }

        /// <summary>
        /// Gets the material of the object.
        /// </summary>
        public Material Material
        {
            get { return material; }
#if !TEXT_TEMPLATE && !WINDOWS_PHONE
#else
#endif
            //set { material = value ?? (new DecalMaterial(GraphicsDevice)); UpdateDecalMaterial(); }
            set { }
        }

        /// <summary>
        /// Gets or sets a small offset value that is used as the depth bias to eliminate depth fighting.
        /// </summary>
        public float DepthBias
        {
            get { return -rasterizeState.DepthBias; }
            set { rasterizeState.DepthBias = -value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether lighting is enabled for this decal.
        /// </summary>
        public bool LightingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether normal mapping is enabled for this decal.
        /// </summary>
        public bool NormalMappingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the decal texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets an optional normal map for the decal texture.
        /// </summary>
        public Texture2D NormalMap { get; set; }

        /// <summary>
        /// Gets or sets the duration of this decal or null if the decal is persistent.
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets the duration for this decal to fade out.
        /// </summary>
        public TimeSpan FadeDuration { get; set; }

        /// <summary>
        /// Gets a list of geometries that projects this decal.
        /// </summary>
        [ContentSerializerIgnore]
        public IList<IGeometry> DecalGeometries 
        {
            get { return decalGeometries; } 
        }
        #endregion

        #region ISpatialQueryable
        /// <summary>
        /// Gets the axis aligned bounding box in world space.
        /// </summary>
        public BoundingBox BoundingBox
        {
            get
            {
                if ((dirtyMask & BoundingBoxDirtyMask) != 0)
                {
                    boundingBox = OrientedBoundingBox.CreateAxisAligned(AbsoluteTransform);
                    dirtyMask &= ~BoundingBoxDirtyMask;
                    OnBoundingBoxChanged();
                }
                return boundingBox;
            }
        }

        /// <summary>
        /// Gets the oriented bounding box of this decal in local space.
        /// </summary>
        public BoundingBox OrientedBoundingBox
        {
            get
            {
                if ((dirtyMask & OrientedBoundingBoxDirtyMask) != 0)
                {
                    orientedBoundingBox.Max = Size * 0.5f;
                    orientedBoundingBox.Min = -Size * 0.5f;
                    dirtyMask &= ~OrientedBoundingBoxDirtyMask;
                }
                return orientedBoundingBox;
            }
        }

        /// <summary>
        /// Occurs when the bounding box changed.
        /// </summary>
        public event EventHandler<EventArgs> BoundingBoxChanged;

        /// <summary>
        /// Called when the bounding box changed.
        /// </summary>
        protected virtual void OnBoundingBoxChanged()
        {
            if (BoundingBoxChanged != null)
                BoundingBoxChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets the data used for spatial query.
        /// </summary>
        object ISpatialQueryable.SpatialData { get; set; }
        #endregion

        #region Fields
        private int vertexCount;
        private int indexCount;
        private int dirtyMask = 1;
        private Matrix invertWorld;
        private Vector3 size = Vector3.One;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private Material material;
        private RasterizerState rasterizeState;
        private BoundingBox boundingBox;
        private BoundingBox orientedBoundingBox;
        private NotificationCollection<IGeometry> decalGeometries;

        internal bool Initialized = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Decal"/> class.
        /// </summary>
        public Decal(GraphicsDevice graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");

            rasterizeState = new RasterizerState();
            rasterizeState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizeState.FillMode = FillMode.Solid;
            rasterizeState.MultiSampleAntiAlias = false;
            rasterizeState.ScissorTestEnable = false;

            Alpha = 1;
            Size = Vector3.One;
            Color = Vector3.One;
            FadeDuration = TimeSpan.FromSeconds(1);
            GraphicsDevice = graphics;
            Visible = true;
            DepthBias = 0.001f;
            
            decalGeometries = new NotificationCollection<IGeometry>();
            decalGeometries.Added += (sender, e) => { dirtyMask |= GeometryDirtyMask; };
            decalGeometries.Removed += (sender, e) => { dirtyMask |= GeometryDirtyMask; };
            
#if !TEXT_TEMPLATE
#if !WINDOWS_PHONE
            //material = new DecalMaterial(graphics);
#else
            throw new NotSupportedException("Decals aren't supported on Windows Phone.");
#endif
#endif
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws this object with the specified material.
        /// </summary>
        public void Draw(DrawingContext context, Material material)
        {

        }

        /// <summary>
        /// Draws the object using the graphics context.
        /// </summary>
        /// <param name="context"></param>
        public void Draw(DrawingContext context)
        {
#if !WINDOWS_PHONE
            if ((dirtyMask & GeometryDirtyMask) != 0)
            {
                UpdateDecalGeometry();
                dirtyMask &= ~GeometryDirtyMask;
            }

            if (vertexCount > 0 && indexCount > 0)
            {
                /*
                material.Apply();

                var matrices = material.Effect.Find<IEffectMatrices>();
                if (matrices != null)
                {
                    matrices.World = Matrix.Identity;
                    matrices.View = context.View;
                    matrices.Projection = context.Projection;
                }

                var textureTransform = material.Effect.Find<IEffectTextureTransform>();
                if (textureTransform != null)
                {
                    Matrix matrix;
                    Matrix.CreateScale(1.0f / size.X, 1.0f / size.Y, 1.0f / size.Z, out matrix);
                    Matrix.Multiply(ref invertWorld, ref matrix, out matrix);
                    
                    matrix.M41 += 0.5f;
                    matrix.M42 += 0.5f;
                    matrix.M43 += 0.5f;

                    textureTransform.TextureTransform = matrix;
                }
                material.Effect.CurrentTechnique.Passes[0].Apply();
                
                GraphicsDevice.RasterizerState = rasterizeState;
                GraphicsDevice.SetVertexBuffer(vertexBuffer);
                GraphicsDevice.Indices = indexBuffer;
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexCount, 0, indexCount / 3);
                 */
            }
#endif
        }

        protected override void OnTransformChanged()
        {
            dirtyMask |= (BoundingBoxDirtyMask | GeometryDirtyMask);
        }

        private void UpdateDecalMaterial()
        {
            var effectTexture = material.Find<IEffectTexture>();
            if (effectTexture != null)
            {
                effectTexture.Texture = Texture;
                effectTexture.SetTexture(TextureUsage.Decal, Texture);
                effectTexture.SetTexture(TextureUsage.NormalMap, NormalMap);
            }
            
            var effectMaterial = material.Find<IEffectMaterial>();
            if (effectMaterial != null)
            {
                effectMaterial.Alpha = Alpha;
                effectMaterial.DiffuseColor = Color;
            }
        }

        /// <summary>
        /// Forces an update to the geometry used to render the decal.
        /// </summary>
        private void UpdateDecalGeometry()
        {
            vertexCount = 0;
            indexCount = 0;
            invertWorld = Matrix.Invert(AbsoluteTransform);

            Triangle localTriangle;
            Triangle worldTriangle;
            ContainmentType containmentType;
            BoundingBox obb = OrientedBoundingBox;

            // Initialize geometry primitive
            for (int i = 0; i < decalGeometries.Count; i++)
            {
                var geometry = decalGeometries[i];

                if (IndexTracker == null || IndexTracker.Length < geometry.Positions.Length)
                    IndexTracker = new ushort[geometry.Positions.Length];
                else
                    Array.Clear(IndexTracker, 0, IndexTracker.Length);

                for (int t = 0; t < geometry.Indices.Length; t += 3)
                {
                    var i1 = geometry.Indices[t];
                    var i2 = geometry.Indices[t + 1];
                    var i3 = geometry.Indices[t + 2];

                    worldTriangle.V1 = geometry.Positions[i1];
                    worldTriangle.V2 = geometry.Positions[i2];
                    worldTriangle.V3 = geometry.Positions[i3];

                    // Transform to world space
                    if (geometry.Transform.HasValue)
                    {
                        var transform = geometry.Transform.Value;
                        Vector3.Transform(ref worldTriangle.V1, ref transform, out worldTriangle.V1);
                        Vector3.Transform(ref worldTriangle.V2, ref transform, out worldTriangle.V2);
                        Vector3.Transform(ref worldTriangle.V3, ref transform, out worldTriangle.V3);
                    }

                    // Transform to decal local space
                    Vector3.Transform(ref worldTriangle.V1, ref invertWorld, out localTriangle.V1);
                    Vector3.Transform(ref worldTriangle.V2, ref invertWorld, out localTriangle.V2);
                    Vector3.Transform(ref worldTriangle.V3, ref invertWorld, out localTriangle.V3);                    
                    
                    // Box triangle test using oriented bounding box
                    obb.Contains(ref localTriangle, out containmentType);

                    if (containmentType != ContainmentType.Disjoint)
                    {
                        // Reserve at least 3 vertices
                        if (Vertices == null)
                            Vertices = new VertexPositionNormalTexture[geometry.Positions.Length];
                        else if (Vertices.Length < (vertexCount + 3))
                            Array.Resize(ref Vertices, (vertexCount + 3) * 2);

                        // Reserve at least 3 indices
                        if (Indices == null)
                            Indices = new ushort[geometry.Indices.Length];
                        else if (Indices.Length < (indexCount + 3))
                            Array.Resize(ref Indices, (indexCount + 3) * 2);

                        // Use 0 to represent that the vertex has not yet been discovered.
                        // This restricts the maximum vertex count to be ushort.MaxValue - 1.
                        if (IndexTracker[i1] == 0)
                        {
                            Vertices[vertexCount].Position = worldTriangle.V1;
                            Indices[indexCount++] = (ushort)vertexCount;
                            IndexTracker[i1] = (ushort)(++vertexCount);
                        }
                        else
                        {
                            Indices[indexCount++] = (ushort)(IndexTracker[i1] - 1);
                        }

                        if (IndexTracker[i2] == 0)
                        {
                            Vertices[vertexCount].Position = worldTriangle.V2;
                            Indices[indexCount++] = (ushort)vertexCount;
                            IndexTracker[i2] = (ushort)(++vertexCount);
                        }
                        else
                        {
                            Indices[indexCount++] = (ushort)(IndexTracker[i2] - 1);
                        }

                        if (IndexTracker[i3] == 0)
                        {
                            Vertices[vertexCount].Position = worldTriangle.V3;
                            Indices[indexCount++] = (ushort)vertexCount;
                            IndexTracker[i3] = (ushort)(++vertexCount);
                        }
                        else
                        {
                            Indices[indexCount++] = (ushort)(IndexTracker[i3] - 1);
                        }
                    }
                }
            }

            // Nothing to project the decal
            if (vertexCount <= 0 || indexCount <= 0)
                return;

            ComputeNormals(Vertices, Indices, vertexCount, indexCount);

            // Create vertex buffer & index buffer
            if (vertexBuffer == null || vertexBuffer.VertexCount < vertexCount)
            {
                if (vertexBuffer != null)
                    vertexBuffer.Dispose();
                vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.WriteOnly);
            }

            if (indexBuffer == null || indexBuffer.IndexCount < indexCount)
            {
                if (indexBuffer != null)
                    indexBuffer.Dispose();
                indexBuffer = new IndexBuffer(GraphicsDevice, typeof(ushort), indexCount, BufferUsage.WriteOnly);
            }

            vertexBuffer.SetData<VertexPositionNormalTexture>(Vertices, 0, vertexCount);
            indexBuffer.SetData<ushort>(Indices, 0, indexCount);
        }

        /// <summary>
        /// Computes the normal for each vertex of the input primitive.
        /// </summary>
        private static void ComputeNormals(VertexPositionNormalTexture[] vertices, ushort[] indices, int vertexCount, int indexCount)
        {
            // Clear normals for each vertex
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.X = 0;
                vertices[i].Normal.Y = 0;
                vertices[i].Normal.Z = 0;
            }

            Vector3 edge1 = new Vector3();
            Vector3 edge2 = new Vector3();
            Vector3 normal = new Vector3();

            // Accumulate vertex normals on adjacent faces
            for (int t = 0; t < indices.Length; t += 3)
            {
                var i1 = indices[t];
                var i2 = indices[t + 1];
                var i3 = indices[t + 2];

                edge1.X = vertices[i2].Position.X - vertices[i1].Position.X;
                edge1.Y = vertices[i2].Position.Y - vertices[i1].Position.Y;
                edge1.Z = vertices[i2].Position.Z - vertices[i1].Position.Z;

                edge2.X = vertices[i3].Position.X - vertices[i1].Position.X;
                edge2.Y = vertices[i3].Position.Y - vertices[i1].Position.Y;
                edge2.Z = vertices[i3].Position.Z - vertices[i1].Position.Z;

                Vector3.Cross(ref edge2, ref edge1, out normal);

                vertices[i1].Normal.X += normal.X;
                vertices[i1].Normal.Y += normal.Y;
                vertices[i1].Normal.Z += normal.Z;
            }

            // Average normals for each vertex
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
            }
        }

        static VertexPositionNormalTexture[] Vertices;
        static ushort[] Indices;
        static ushort[] IndexTracker;

        void IDrawableObject.BeginDraw(DrawingContext context) { }
        void IDrawableObject.EndDraw(DrawingContext context) { }
        #endregion

        #region Dispose
        ~Decal()
        {
            Dispose(false);
        }
        
        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                    vertexBuffer = null;
                }
                if (indexBuffer != null)
                {
                    indexBuffer.Dispose();
                    indexBuffer = null;
                }
            }
        }
        #endregion
    }
}