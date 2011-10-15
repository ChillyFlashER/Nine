#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Nine.Animations;
#endregion

namespace Nine.Graphics.ObjectModel
{
    /// <summary>
    /// Defines a display object that contains a list of objects.
    /// </summary>
    /// <remarks>
    /// This class serves as a container to composite other objects.
    /// If you wish to create your own display object, derive from <c>Transformable</c> instead.
    /// </remarks>
    public sealed class DisplayObject : Transformable, IUpdateable, IEnumerable<object>, INotifyCollectionChanged<object>, IDisposable
    {
        #region Children
        /// <summary>
        /// Gets the child drawable owned used by this drawable.
        /// </summary>
        [ContentSerializer(Optional=true)]
        public IList<object> Children
        {
            get { return children; }

            // To be used by content reader
            internal set
            {
                children.Clear();
                children.AddRange(value);
            }
        }
        private NotificationCollection<object> children;

        void children_Added(object sender, NotifyCollectionChangedEventArgs<object> e)
        {
            if (e.Value == null)
                throw new ArgumentNullException("item");

            // This method is called after the object has been added to children, so check against 1 instead.
            if (children.Count(c => c == e.Value) > 1)
                throw new InvalidOperationException("The object is already a child of this display object");

            ISpatialQueryable boundable = e.Value as ISpatialQueryable;
            if (boundable != null)
            {
                boundingBoxDirty = true;
                boundable.BoundingBoxChanged += new EventHandler<EventArgs>(boundable_BoundingBoxChanged);
            }

            Transformable transformable = e.Value as Transformable;
            if (transformable != null)
            {
                if (transformable.Parent != null)
                    throw new InvalidOperationException("The object is already added to a display object.");
                transformable.Parent = this;
            }

            if (Added != null)
                Added(sender, e);
        }

        void children_Removed(object sender, NotifyCollectionChangedEventArgs<object> e)
        {
            ISpatialQueryable boundable = e.Value as ISpatialQueryable;
            if (boundable != null)
            {
                boundingBoxDirty = true;
                boundable.BoundingBoxChanged -= new EventHandler<EventArgs>(boundable_BoundingBoxChanged);
            }
            
            Transformable transformable = e.Value as Transformable;
            if (transformable != null)
            {
                if (transformable.Parent == null)
                    throw new InvalidOperationException("The object does not belong to this display object.");
                transformable.Parent = null;

                // Remove all the transform bindings associated with this child
                transformBindings.RemoveAll(b => b.Source == transformable || b.Target == transformable);
            }

            if (Removed != null)
                Removed(sender, e);
        }

        public event EventHandler<NotifyCollectionChangedEventArgs<object>> Added;
        public event EventHandler<NotifyCollectionChangedEventArgs<object>> Removed;
        #endregion

        #region BoundingBox
        public override BoundingBox BoundingBox
        {
            get 
            {
                if (boundingBoxDirty)
                {
                    boundingBox = BoundingBoxExtensions.CreateMerged(children.OfType<IBoundable>().Select(b => b.BoundingBox));
                    boundingBoxDirty = false;
                }
                return boundingBox; 
            }
        }

        BoundingBox boundingBox;
        bool boundingBoxDirty;

        void boundable_BoundingBoxChanged(object sender, EventArgs e)
        {
            boundingBoxDirty = true;
        }
        #endregion

        #region Transform Binding
        /// <summary>
        /// Gets all the transform bindings owned by this <c>DisplayObject</c>.
        /// </summary>
        public IList<TransformBinding> TransformBindings
        {
            get { return transformBindings; }

            // To be used by content reader
            internal set
            {
                transformBindings.Clear();
                transformBindings.AddRange(value);
            }
        }
        private NotificationCollection<TransformBinding> transformBindings;

        void transformBindings_Added(object sender, NotifyCollectionChangedEventArgs<TransformBinding> e)
        {
            if (e.Value == null)
                throw new ArgumentNullException("item");

            if (!string.IsNullOrEmpty(e.Value.SourceName))
            {
                e.Value.Source = Find<Transformable>(e.Value.SourceName);
                if (e.Value.Source == null)
                    throw new ContentLoadException("Cannot find a child object with name: " + e.Value.SourceName);
                e.Value.SourceName = null;
            }

            if (!string.IsNullOrEmpty(e.Value.TargetName))
            {
                e.Value.Target = Find<Transformable>(e.Value.TargetName);
                if (e.Value.Target == null)
                    throw new ContentLoadException("Cannot find a child object with name: " + e.Value.TargetName);
                e.Value.TargetName = null;
            }

            if (!children.Contains(e.Value.Source) || !children.Contains(e.Value.Target))
                throw new InvalidOperationException("The source and target object for the binding must be a child of this display object.");

            if (transformBindings.Count(b => b.Source == e.Value.Source) > 1)
                throw new InvalidOperationException("Cannot bind the source object multiple times.");

            // TODO: Dependency sorting
        }
        
        /// <summary>
        /// Binds the transformation of the source object to the target object.
        /// </summary>
        public void Bind(Transformable source, Transformable target)
        {
            Bind(source, target, null, null);
        }

        /// <summary>
        /// Binds the transformation of the source object to the target object.
        /// </summary>
        public void Bind(Transformable source, Transformable target, string boneName)
        {
            Bind(source, target, boneName, null);
        }

        /// <summary>
        /// Binds the transformation of the source object to the target object.
        /// </summary>
        public void Bind(Transformable source, Transformable target, string boneName, Matrix? biasTransform)
        {
            transformBindings.Add(new TransformBinding(source, target) { TargetBone = boneName, Transform = biasTransform });
        }

        /// <summary>
        /// Unbinds the transformation of the source object to the target object.
        /// </summary>
        public void Unbind(Transformable source, Transformable target)
        {
            transformBindings.RemoveAll(b => b.Source == source && b.Target == target);
        }
        #endregion

        #region Animations
        /// <summary>
        /// Gets all the animations in this display object.
        /// </summary>
        [ContentSerializer]
        public AnimationPlayer Animations
        {
            get { return animations; }
            
            // For serialization
            internal set 
            {
                if (value != null && value.Animations != null)
                {
                    UpdateTweenAnimationTargets(value.Animations.Values);
                    animations = value;
                }
                animations.Play(); 
            }
        }
        
        private void UpdateTweenAnimationTargets(IEnumerable value)
        {
            if (value != null)
            {
                foreach (var supportTarget in value.ForEachRecursive<ISupportTarget>())
                {
                    var property = supportTarget.TargetProperty;
                    if (!string.IsNullOrEmpty(property))
                    {
                        var nameEnds = property.IndexOf(".");
                        if (nameEnds < 0)
                        {
                            supportTarget.Target = this;
                        }
                        else
                        {
                            var name = property.Substring(0, nameEnds);
                            var target = Find<object>(name);
                            if (target == null)
                                throw new ContentLoadException("Cannot find a child object with name " + name);
                            supportTarget.Target = target;
                            supportTarget.TargetProperty = property.Substring(nameEnds + 1, property.Length - nameEnds - 1);
                        }
                    }
                }
            }
        }

        private AnimationPlayer animations = new AnimationPlayer();
        #endregion

        #region Find
        public T Find<T>(string name)
        {
            if (Name == name && this is T)
                return (T)(object)this;
            var result = children.OfType<Transformable>().FirstOrDefault(t => t.Name == name);
            if (result is T)
                return (T)(object)result;
            return default(T);
        }

        public IEnumerable<T> FindAll<T>(string name)
        {
            if (Name == name && this is T)
                yield return (T)(object)this;
            foreach (var result in children.OfType<Transformable>().Where(t => t.Name == name))
            {
                if (result is T)
                    yield return (T)(object)result;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <c>DisplayObject</c>.
        /// </summary>
        public DisplayObject()
        {
            children = new NotificationCollection<object>();
            children.Sender = this;
            children.Added += children_Added;
            children.Removed += children_Removed;

            transformBindings = new NotificationCollection<TransformBinding>();
            transformBindings.Sender = this;
            transformBindings.Added += transformBindings_Added;
        }
        #endregion

        #region Update
        public void Update(TimeSpan elapsedTime)
        {
            Animations.Update(elapsedTime);
            UpdateTransformBindings();
        }

        private void UpdateTransformBindings()
        {
            foreach (var binding in transformBindings)
            {
                if (string.IsNullOrEmpty(binding.TargetBone))
                {
                    if (binding.Transform != null)
                        binding.Source.Transform = binding.Transform.Value * binding.Target.Transform;
                    else
                        binding.Source.Transform = binding.Target.Transform;
                }
                else
                {
                    DrawableModel model = binding.Target as DrawableModel;
                    if (model == null)
                        throw new InvalidOperationException("The target object must be a DrawableModel when a bone name is specified.");

                    var boneTransform = model.Skeleton.GetAbsoluteBoneTransform(binding.TargetBone);
                    if (!binding.UseBoneScale)
                    {
                        Vector3 translation, scale;
                        Quaternion rotation;
                        if (!boneTransform.Decompose(out scale, out rotation, out translation))
                            throw new InvalidOperationException();
                        Matrix.CreateFromQuaternion(ref rotation, out boneTransform);
                        boneTransform.M41 = translation.X;
                        boneTransform.M42 = translation.Y;
                        boneTransform.M43 = translation.Z;
                    }
                    if (binding.Transform != null)
                        binding.Source.Transform = binding.Transform.Value * boneTransform * model.Transform;
                    else
                        binding.Source.Transform = boneTransform * model.Transform;
                }
            }
        }
        #endregion

        #region IEnumerable
        public IEnumerator<object> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            foreach (var child in Children)
            {
                IDisposable disposable = child as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }
        #endregion
    }
}