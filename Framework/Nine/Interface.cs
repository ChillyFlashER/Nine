#region Copyright 2009 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nine
{
    /// <summary>
    /// Object that react to game updates.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Updates the internal state of the object based on game time.
        /// </summary>
        void Update(TimeSpan elapsedTime);
    }

    /// <summary>
    /// Object that react to game draws.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws the internal state of the object.
        /// </summary>
        void Draw(TimeSpan elapsedTime);
    }

    /// <summary>
    /// Interface for a 3D geometry made up of triangles
    /// </summary>
    public interface IGeometry
    {
        /// <summary>
        /// Gets a readonly list of vertex positions.
        /// </summary>
        IList<Vector3> Positions { get; }

        /// <summary>
        /// Gets a read-only list of geometry indices.
        /// </summary>
        IList<ushort> Indices { get; }
    }

    /// <summary>
    /// Interface for an object with a bounding box that can be queryed.
    /// </summary>
    public interface IBoundable
    {
        /// <summary>
        /// Gets the axis aligned bounding box in world space.
        /// </summary>
        BoundingBox BoundingBox { get; }
    }

    /// <summary>
    /// Defines Spatial relations between objects.
    /// </summary>
    public interface ISpatialQuery<T>
    {
        /// <summary>
        /// Finds all the objects that intersects with the specified ray.
        /// </summary>
        /// <param name="result">The caller is responsable for clearing the result collection</param>
        void FindAll(ref Ray ray, ICollection<T> result);

        /// <summary>
        /// Finds all the objects resides within the specified bounding sphere.
        /// </summary>
        /// <param name="result">The caller is responsable for clearing the result collection</param>
        void FindAll(ref BoundingSphere boundingSphere, ICollection<T> result);

        /// <summary>
        /// Finds all the objects that intersects with the specified bounding box.
        /// </summary>
        /// <param name="result">The caller is responsable for clearing the result collection</param>
        void FindAll(ref BoundingBox boundingBox, ICollection<T> result);

        /// <summary>
        /// Finds all the objects resides within the specified bounding frustum.
        /// </summary>
        /// <param name="result">The caller is responsable for clearing the result collection</param>
        void FindAll(ref BoundingFrustum boundingFrustum, ICollection<T> result);
    }

    /// <summary>
    /// Interface for an object that can be queried by a scene manager.
    /// </summary>
    public interface ISpatialQueryable : IBoundable
    {
        /// <summary>
        /// Occurs when the bounding box changed.
        /// </summary>
        event EventHandler<EventArgs> BoundingBoxChanged;

        /// <summary>
        /// Gets or sets the data used for spatial query.
        /// </summary>
        object SpatialData { get; set; }
    }

    /// <summary>
    /// Interface for a scene manager that manages the spatial relationships
    /// between objects.
    /// </summary>
    public interface ISceneManager<T> : ICollection<T>, ISpatialQuery<T>
    {

    }

    /// <summary>
    /// This interface supports the infrastructure of the framework and is not 
    /// intended to be used by externals.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISupportTarget
    {
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        object Target { get; set; }

        /// <summary>
        /// Gets or sets the target property.
        /// </summary>
        string TargetProperty { get; set; }
    }
}