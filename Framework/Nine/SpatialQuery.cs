#region Copyright 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nine
{
    /// <summary>
    /// Defines Spatial relations between objects.
    /// </summary>
    public interface ISpatialQuery<T>
    {
        /// <summary>
        /// Finds the nearest object intersects with the specified ray.
        /// </summary>
        T FindFirst(Ray ray);
        
        /// <summary>
        /// Finds all the objects resides within the specified bounding sphere.
        /// </summary>
        IEnumerable<T> Find(Vector3 position, float radius);

        /// <summary>
        /// Finds all the objects that intersects with the specified ray.
        /// </summary>
        IEnumerable<T> Find(Ray ray);

        /// <summary>
        /// Finds all the objects resides within the specified bounding frustum.
        /// </summary>
        IEnumerable<T> Find(BoundingFrustum frustum);
    }

    /// <summary>
    /// Represents an adapter class that filters and converts the result of
    /// an existing <c>SpatialQuery</c>.
    /// </summary>
    public class SpatialQuery<TInput, TOutput> : ISpatialQuery<TOutput>
    {
        /// <summary>
        /// Gets or sets the inner query.
        /// </summary>
        public IList<ISpatialQuery<TInput>> InnerQueries { get; set; }

        /// <summary>
        /// Gets or sets a predicate that filters the result of the inner query.
        /// </summary>
        public Predicate<object> Filter { get; set; }

        /// <summary>
        /// Gets or sets a predicate that filters the result of the inner query.
        /// </summary>
        public Converter<TInput, TOutput> Converter { get; set; }

        public SpatialQuery(params ISpatialQuery<TInput>[] queries) 
        {
            if (queries == null)
                throw new ArgumentNullException("query");

            this.InnerQueries = new List<ISpatialQuery<TInput>>(queries);
        }

        private TOutput Convert(TInput input)
        {
            if (Filter != null && Filter(input))
                return default(TOutput);

            if (Converter != null)
                return Converter(input);

            if (input is TOutput)
                return (TOutput)(object)input;

            return default(TOutput);
        }

        private IEnumerable<TOutput> Convert(IEnumerable<TInput> inputs)
        {
            foreach (TInput input in inputs)
            {
                if (Filter != null && Filter(input))
                    continue;

                if (Converter != null)
                    yield return Converter(input);

                if (input is TOutput)
                    yield return (TOutput)(object)input;
            }
        }
        
        public TOutput FindFirst(Ray ray)
        {
            return Convert(InnerQueries[0].FindFirst(ray));
        }

        public IEnumerable<TOutput> Find(Vector3 position, float radius)
        {
            foreach (ISpatialQuery<TInput> query in InnerQueries)
                foreach (TOutput output in Convert(query.Find(position, radius)))
                    yield return output;
        }

        public IEnumerable<TOutput> Find(Ray ray)
        {
            foreach (ISpatialQuery<TInput> query in InnerQueries)
                foreach (TOutput output in Convert(query.Find(ray)))
                    yield return output;
        }

        public IEnumerable<TOutput> Find(BoundingFrustum frustum)
        {
            foreach (ISpatialQuery<TInput> query in InnerQueries)
                foreach (TOutput output in Convert(query.Find(frustum)))
                    yield return output;
        }
    }
}