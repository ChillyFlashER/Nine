﻿#region Copyright 2009 - 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Nine.Graphics
{
    using Nine.Animations;

    #region BoneAnimationClip
    /// <summary>
    /// Defines a bone animation clip.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BoneAnimationClip
    {
        /// <summary>
        /// Gets animation frame rate.
        /// </summary>
        [ContentSerializer]
        public int FramesPerSecond { get; internal set; }

        /// <summary>
        /// Gets total number of frames.
        /// </summary>
        [ContentSerializer]
        public int TotalFrames { get; internal set; }

        /// <summary>
        /// Gets the preferred ending style.
        /// </summary>
        [ContentSerializer]
        public KeyframeEnding PreferredEnding { get; internal set; }

        /// <summary>
        /// Gets all the channels in this animation clip.
        /// </summary>
        [ContentSerializer]
        public Matrix[][] Transforms { get; internal set; }
    }
    #endregion

    #region BoneAnimation
    /// <summary>
    /// An animation player used to play bone animations.
    /// </summary>
    public class BoneAnimation : KeyframeAnimation
    {
        /// <summary>
        /// Gets or sets whether this BoneAnimation should automatically
        /// perform matrix interpolation when the playing speed is less then 1.
        /// </summary>
        public bool InterpolationEnabled { get; set; }

        /// <summary>
        /// Gets or sets whether this BoneAnimation should blend with the
        /// previous bone poses when started playing the animation.
        /// </summary>
        public bool BlendEnabled { get; set; }

        /// <summary>
        /// Gets or sets blend time.
        /// </summary>
        public TimeSpan BlendDuration { get; set; }


        private Model model;
        private BoneAnimationClip clip;
        private double blendTimer = 0;
        private Matrix[] blendTarget;
        private bool[] disabled;
        private bool shouldLerp;
        static ICurve blendCurve = new SinCurve();


        internal BoneAnimation(Model model, BoneAnimationClip clip)
        {
            if (clip == null || model == null)
                throw new ArgumentNullException();

            this.clip = clip;
            this.model = model;
            this.BlendEnabled = true;
            this.Ending = clip.PreferredEnding;
            this.BlendDuration = TimeSpan.FromSeconds(0.5);
            this.disabled = new bool[model.Bones.Count];

            InterpolationEnabled = true;
            FramesPerSecond = clip.FramesPerSecond;
        }

        /// <summary>
        /// Gets wether the animation on target bone is enabled.
        /// </summary>
        public bool IsEnabled(int bone)
        {
            return !disabled[bone];
        }

        /// <summary>
        /// Gets wether the animation on target bone is enabled.
        /// </summary>
        public bool IsEnabled(string bone)
        {
            return !disabled[model.Bones[bone].Index];
        }

        /// <summary>
        /// Enables the animation on the target and its child bones.
        /// </summary>
        public void Enabled(int bone, bool enableChildBones)
        {
            SetEnabled(bone, true, enableChildBones);
        }

        /// <summary>
        /// Enables the animation on the target and its child bones.
        /// </summary>
        public void Enabled(string bone, bool enableChildBones)
        {
            SetEnabled(model.Bones[bone].Index, true, enableChildBones);
        }

        /// <summary>
        /// Disables the animation on the target and its child bones.
        /// </summary>
        public void Disable(int bone, bool disableChildBones)
        {
            SetEnabled(bone, false, disableChildBones);
        }

        /// <summary>
        /// Disables the animation on the target and its child bones.
        /// </summary>
        public void Disable(string bone, bool disableChildBones)
        {
            SetEnabled(model.Bones[bone].Index, false, disableChildBones);
        }

        private void SetEnabled(int bone, bool enabled, bool recursive)
        {
            disabled[bone] = !enabled;

            if (recursive)
            {
                foreach (ModelBone child in model.Bones[bone].Children)
                {
                    SetEnabled(child.Index, enabled, true);
                }
            }
        }

        protected override int GetTotalFrames()
        {
            return clip.TotalFrames;
        }

        protected override void OnStarted()
        {
            if (BlendEnabled)
            {
                if (blendTarget == null)
                    blendTarget = new Matrix[model.Bones.Count];

                blendTimer = 0;
                model.CopyBoneTransformsTo(blendTarget);
            }

            base.OnStarted();
        }

        public override void Update(GameTime gameTime)
        {
            if (State == Animations.AnimationState.Playing)
            {
                blendTimer += gameTime.ElapsedGameTime.TotalSeconds;
                shouldLerp = (gameTime.ElapsedGameTime.TotalSeconds * Speed) < (1.0 / FramesPerSecond);
            }

            base.Update(gameTime);
        }

        protected override void OnSeek(int startFrame, int endFrame, float percentage)
        {
            float blendLerp = -1;

            if (BlendEnabled && blendTimer < BlendDuration.TotalSeconds)
            {
                blendLerp = (float)(blendTimer / BlendDuration.TotalSeconds);
            }

            for (int bone = 0; bone < clip.Transforms.Length; bone++)
            {
                if (disabled[bone] || clip.Transforms[bone] == null)
                    continue;

                Matrix transform;

                if (InterpolationEnabled && shouldLerp)
                {
                    transform = LerpHelper.Slerp(
                                 clip.Transforms[bone][startFrame],
                                 clip.Transforms[bone][endFrame], percentage);
                }
                else
                {
                   transform = clip.Transforms[bone][startFrame];
                }

                if (blendLerp >= 0 && blendLerp < 1)
                {
                    model.Bones[bone].Transform = LerpHelper.Slerp(
                                blendTarget[bone], transform, blendCurve.Evaluate(blendLerp));
                }
                else
                {
                    model.Bones[bone].Transform = transform;
                }
            }
        }
    }
    #endregion
}
