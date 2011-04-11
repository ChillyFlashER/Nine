// -----------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EffectCustomTool v1.3.0.0.
//     Runtime Version: v4.0.30319
//
//     EffectCustomTool is a part of Engine Nine. (http://nine.codeplex.com)
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------------

namespace Nine.Graphics.ScreenEffects
{
#if !WINDOWS_PHONE

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

    partial class ColorMatrixEffect : Effect
    {		
        public ColorMatrixEffect(GraphicsDevice graphics) : base(graphics, effectCode)
        {
            CacheEffectParameters(this);

			OnCreated();
        }

        /// <summary>
        /// Creates a new ColorMatrixEffect by cloning parameter settings from an existing instance.
        /// </summary>
		protected ColorMatrixEffect(ColorMatrixEffect cloneSource) : base(cloneSource)
		{
            CacheEffectParameters(cloneSource);

			OnCreated();

            this._Transform = cloneSource._Transform;

			
			OnClone(cloneSource);
		}

        /// <summary>
        /// Creates a clone of the current ColorMatrixEffect instance.
        /// </summary>
        public override Effect Clone()
        {
            return new ColorMatrixEffect(this);
        }

        private void CacheEffectParameters(Effect cloneSource)
        {
            this._TransformParameter = cloneSource.Parameters["Transform"];

        }

		#region Dirty Flags

		uint dirtyFlag = 0;

        const uint TransformDirtyFlag = 1 << 0;

		#endregion

		#region Properties

        private Matrix _Transform;
        private EffectParameter _TransformParameter;

        /// <summary>
        /// Gets or sets the color transform matrix. See MatrixExtensions.
        /// </summary>
        public Matrix Transform
        {
            get { return _Transform; }
            set { _Transform = value; dirtyFlag |= TransformDirtyFlag; }
        }


		#endregion
		
		#region Apply
        protected override void OnApply()
        {
			OnApplyChanges();

            if ((this.dirtyFlag & TransformDirtyFlag) != 0)
            {
                this._TransformParameter.SetValue(_Transform);
                this.dirtyFlag &= ~TransformDirtyFlag;
            }

            base.OnApply();
        }
		#endregion

        #region ByteCode
        static byte[] effectCode = null;

        static ColorMatrixEffect()
        {
#if XBOX360
            effectCode = new byte[] 
            {
0xBC, 0xF0, 0x0B, 0xCF, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x09, 0x01, 0x00, 0x00, 0x00, 0xF8, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 
0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x3F, 0x80, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 
0x00, 0x00, 0x00, 0x9C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x53, 0x61, 0x73, 0x55, 0x69, 0x44, 0x65, 0x73, 
0x63, 0x72, 0x69, 0x70, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x54, 0x72, 0x61, 0x6E, 0x73, 0x66, 0x6F, 0x72, 
0x6D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x50, 0x61, 0x73, 0x73, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x53, 0x61, 0x74, 0x75, 
0x72, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 
0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x44, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x88, 0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xDC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x5D, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0xC8, 0x00, 0x00, 0x00, 0xC4, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x3F, 
0x47, 0x65, 0x74, 0x73, 0x20, 0x6F, 0x72, 0x20, 0x73, 0x65, 0x74, 0x73, 0x20, 0x74, 0x68, 0x65, 0x20, 0x63, 0x6F, 0x6C, 0x6F, 0x72, 0x20, 0x74, 
0x72, 0x61, 0x6E, 0x73, 0x66, 0x6F, 0x72, 0x6D, 0x20, 0x6D, 0x61, 0x74, 0x72, 0x69, 0x78, 0x2E, 0x20, 0x53, 0x65, 0x65, 0x20, 0x4D, 0x61, 0x74, 
0x72, 0x69, 0x78, 0x45, 0x78, 0x74, 0x65, 0x6E, 0x73, 0x69, 0x6F, 0x6E, 0x73, 0x2E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x78, 0x10, 0x2A, 0x11, 0x00, 0x00, 0x00, 0x01, 0x18, 
0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF4, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xCC, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0xBF, 0xFF, 0xFF, 0x03, 0x00, 0x00, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB8, 0x00, 0x00, 0x00, 0x44, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x4C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x68, 
0x00, 0x00, 0x00, 0x78, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0x00, 0x04, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x54, 0x72, 0x61, 0x6E, 0x73, 0x66, 0x6F, 0x72, 0x6D, 0x00, 0xAB, 0xAB, 0x00, 0x03, 0x00, 0x03, 0x00, 0x04, 0x00, 0x04, 
0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 
0x70, 0x73, 0x5F, 0x33, 0x5F, 0x30, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x31, 0x31, 0x36, 0x32, 0x36, 0x2E, 0x30, 0x00, 0xAB, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x60, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x21, 0x00, 0x01, 0x00, 0x01, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x30, 0x50, 0x00, 0x01, 0x10, 0x02, 0x00, 0x00, 0x12, 0x00, 0xC4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x03, 
0x00, 0x00, 0x22, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x08, 0x00, 0x01, 0x1F, 0x1F, 0xF6, 0x88, 0x00, 0x00, 0x40, 0x00, 0xC8, 0x01, 0x80, 0x00, 
0x00, 0xA7, 0xA7, 0x00, 0xAF, 0x00, 0x00, 0x00, 0xC8, 0x02, 0x80, 0x00, 0x00, 0xA7, 0xA7, 0x00, 0xAF, 0x00, 0x01, 0x00, 0xC8, 0x04, 0x80, 0x00, 
0x00, 0xA7, 0xA7, 0x00, 0xAF, 0x00, 0x02, 0x00, 0xC8, 0x08, 0x80, 0x00, 0x00, 0xA7, 0xA7, 0x00, 0xAF, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  
            };
#else
            effectCode = new byte[] 
            {
0xCF, 0x0B, 0xF0, 0xBC, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x09, 0xFF, 0xFE, 0xF8, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 
0xB4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
0x9C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x53, 0x61, 0x73, 0x55, 0x69, 0x44, 0x65, 0x73, 
0x63, 0x72, 0x69, 0x70, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x65, 0x19, 0x06, 0x0A, 0x00, 0x00, 0x00, 0x54, 0x72, 0x61, 0x6E, 0x73, 0x66, 0x6F, 0x72, 
0x6D, 0x00, 0x19, 0x06, 0x02, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x50, 0x61, 0x73, 0x73, 0x31, 0x00, 0xBB, 0x06, 0x0B, 0x00, 0x00, 0x00, 0x53, 0x61, 0x74, 0x75, 
0x72, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 
0x04, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x88, 0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0xDC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x93, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0xC8, 0x00, 0x00, 0x00, 0xC4, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x00, 0x00, 
0x47, 0x65, 0x74, 0x73, 0x20, 0x6F, 0x72, 0x20, 0x73, 0x65, 0x74, 0x73, 0x20, 0x74, 0x68, 0x65, 0x20, 0x63, 0x6F, 0x6C, 0x6F, 0x72, 0x20, 0x74, 
0x72, 0x61, 0x6E, 0x73, 0x66, 0x6F, 0x72, 0x6D, 0x20, 0x6D, 0x61, 0x74, 0x72, 0x69, 0x78, 0x2E, 0x20, 0x53, 0x65, 0x65, 0x20, 0x4D, 0x61, 0x74, 
0x72, 0x69, 0x78, 0x45, 0x78, 0x74, 0x65, 0x6E, 0x73, 0x69, 0x6F, 0x6E, 0x73, 0x2E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x74, 0x01, 0x00, 0x00, 0x00, 0x02, 0xFF, 0xFF, 0xFE, 0xFF, 0x3D, 0x00, 
0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00, 0xBF, 0x00, 0x00, 0x00, 0x00, 0x02, 0xFF, 0xFF, 0x02, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x20, 0xB8, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x4C, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 
0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0x04, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x54, 0x72, 0x61, 0x6E, 0x73, 0x66, 0x6F, 0x72, 0x6D, 0x00, 0xAB, 0xAB, 0x03, 0x00, 0x03, 0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x70, 0x73, 0x5F, 0x32, 
0x5F, 0x30, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 
0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 0x32, 0x36, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x32, 
0x38, 0x34, 0x34, 0x00, 0x1F, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x03, 0xB0, 0x1F, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x90, 
0x00, 0x08, 0x0F, 0xA0, 0x42, 0x00, 0x00, 0x03, 0x00, 0x00, 0x0F, 0x80, 0x00, 0x00, 0xE4, 0xB0, 0x00, 0x08, 0xE4, 0xA0, 0x09, 0x00, 0x00, 0x03, 
0x01, 0x00, 0x01, 0x80, 0x00, 0x00, 0xE4, 0x80, 0x00, 0x00, 0xE4, 0xA0, 0x09, 0x00, 0x00, 0x03, 0x01, 0x00, 0x02, 0x80, 0x00, 0x00, 0xE4, 0x80, 
0x01, 0x00, 0xE4, 0xA0, 0x09, 0x00, 0x00, 0x03, 0x01, 0x00, 0x04, 0x80, 0x00, 0x00, 0xE4, 0x80, 0x02, 0x00, 0xE4, 0xA0, 0x09, 0x00, 0x00, 0x03, 
0x01, 0x00, 0x08, 0x80, 0x00, 0x00, 0xE4, 0x80, 0x03, 0x00, 0xE4, 0xA0, 0x01, 0x00, 0x00, 0x02, 0x00, 0x08, 0x0F, 0x80, 0x01, 0x00, 0xE4, 0x80, 
0xFF, 0xFF, 0x00, 0x00,   
            };
#endif
        }
        #endregion
    }

#endif
}
