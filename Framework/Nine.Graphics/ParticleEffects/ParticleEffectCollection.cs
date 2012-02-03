﻿#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

#endregion

namespace Nine.Graphics.ParticleEffects
{
    /// <summary>
    /// Represents a collection of particle effects.
    /// </summary>
    [EditorBrowsable()]
    public class ParticleEffectCollection : Collection<ParticleEffect>
    {
        protected override void InsertItem(int index, ParticleEffect item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (item.ActiveEmitters.Count > 0)
                throw new ArgumentException(Strings.ParticleEffectAlreadyTriggered);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, ParticleEffect item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (item.ActiveEmitters.Count > 0)
                throw new ArgumentException(Strings.ParticleEffectAlreadyTriggered);
            base.SetItem(index, item);
        }
    }
}