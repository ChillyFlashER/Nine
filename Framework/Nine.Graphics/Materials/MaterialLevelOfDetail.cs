#region Copyright 2009 - 2012 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2012 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace Nine.Graphics.Materials
{
    /// <summary>
    /// Defines a list of materials that are sorted from lowest quanlity to highest quanlity.
    /// </summary>
    [ContentSerializable]
    [ContentProperty("MaterialLevels")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MaterialLevelOfDetail
    {
        /// <summary>
        /// Gets a collection containing all the material levels.
        /// </summary>
        public IList<Material> MaterialLevels
        {
            get { return materialLevels; }
        }
        private List<Material> materialLevels;

        /// <summary>
        /// Gets the current material level.
        /// </summary>
        public Material Current
        {
            get { return current; }
        }
        internal Material current;
        
        /// <summary>
        /// Gets or sets the distance at which to use the highest quanlity material.
        /// </summary>
        public float LevelOfDetailStart
        {
            get { return levelOfDetailStart; }
            set { levelOfDetailStart = value; }
        }
        private float levelOfDetailStart;

        /// <summary>
        /// Gets or sets the distance at which to use the lowest quanlity material.
        /// </summary>
        public float LevelOfDetailEnd
        {
            get { return levelOfDetailEnd; }
            set { levelOfDetailEnd = value; }
        }
        private float levelOfDetailEnd;

        /// <summary>
        /// Gets or sets the overall material quality that is multiplied with the quanlity
        /// determined by LevelOfDetailStart and LevelOfDetailEnd.
        /// </summary>
        public float MaterialQuality
        {
            get { return materialQuality; }
            set { materialQuality = value; }
        }
        private float materialQuality;

        /// <summary>
        /// Gets the count of material levels.
        /// </summary>
        public int Count
        {
            get { return materialLevels.Count; } 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialLevelOfDetail"/> class.
        /// </summary>
        public MaterialLevelOfDetail()
        {
            LevelOfDetailStart = 100;
            LevelOfDetailEnd = 1000;
            MaterialQuality = 1;
            materialLevels = new List<Material>(1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialLevelOfDetail"/> class.
        /// </summary>
        public MaterialLevelOfDetail(IEnumerable<Material> materialLevels) : this()
        {
            MaterialLevels.AddRange(materialLevels);
            UpdateLevelOfDetail(0);
        }

        /// <summary>
        /// Manually updates the current selected material based on level of detail settings.
        /// </summary>
        public Material UpdateLevelOfDetail(float distanceToEye)
        {
            var count = materialLevels.Count;
            if (count <= 0)
                return current = null;

            if (count == 1)
                return current = materialLevels[0];

            float lod = 1 - (distanceToEye - LevelOfDetailStart) / (LevelOfDetailEnd - LevelOfDetailStart);
            lod = MathHelper.Clamp(MaterialQuality * lod, 0, 0.99f);
            return current = materialLevels[(int)(count * lod)];
        }
    }
}