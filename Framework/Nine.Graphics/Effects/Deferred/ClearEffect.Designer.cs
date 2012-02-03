// -----------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EffectCustomTool v1.5.1.0.
//     Runtime Version: v4.0.30319
//
//     EffectCustomTool is a part of Engine Nine. (http://nine.codeplex.com)
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------------

namespace Nine.Graphics.Effects.Deferred
{
#if !WINDOWS_PHONE

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    [System.CodeDom.Compiler.GeneratedCode("Nine.Tools.EffectCustomTool", "1.5.1.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class ClearEffect : Effect
    {
        public ClearEffect(GraphicsDevice graphics) 
            : base(graphics, graphics.GraphicsProfile == GraphicsProfile.Reach ? ReachEffectCode :
                                                                                 HiDefEffectCode)
        {
            CacheEffectParameters(Parameters);

            OnCreated();
        }

        /// <summary>
        /// Creates a new ClearEffect by cloning parameter settings from an existing instance.
        /// </summary>
        protected ClearEffect(ClearEffect cloneSource) : base(cloneSource)
        {
            CacheEffectParameters(cloneSource.Parameters);

            OnCreated();

            CloneFrom(cloneSource);

            OnClone(cloneSource);
        }

        /// <summary>
        /// Creates a clone of the current ClearEffect instance.
        /// </summary>
        public override Effect Clone()
        {
            return new ClearEffect(this);
        }

        protected override void OnApply()
        {
            OnApplyChanges();

            ApplyChanges();

            base.OnApply();
        }

        private void CacheEffectParameters(EffectParameterCollection cloneSource)
        {
            this._normalParameter = cloneSource["normal"];
            this._depthParameter = cloneSource["depth"];

        }

        #region Dirty Flags

        uint dirtyFlag = 0;

        const uint normalDirtyFlag = 1 << 0;
        const uint depthDirtyFlag = 1 << 1;

        #endregion

        #region Properties

        private Vector3 _normal;
        private EffectParameter _normalParameter;

        internal Vector3 normal
        {
            get { return _normal; }
            set { _normal = value; dirtyFlag |= normalDirtyFlag; }
        }

        private float _depth;
        private EffectParameter _depthParameter;

        internal float depth
        {
            get { return _depth; }
            set { if (_depth != value) { _depth = value; dirtyFlag |= depthDirtyFlag; } }
        }


        #endregion

        #region Apply
        private void ApplyChanges()
        {
            if ((this.dirtyFlag & normalDirtyFlag) != 0)
            {
                this._normalParameter.SetValue(_normal);
                this.dirtyFlag &= ~normalDirtyFlag;
            }
            if ((this.dirtyFlag & depthDirtyFlag) != 0)
            {
                this._depthParameter.SetValue(_depth);
                this.dirtyFlag &= ~depthDirtyFlag;
            }

        }
        #endregion

        #region Clone
        private void CloneFrom(ClearEffect cloneSource)
        {
            this._normal = cloneSource._normal;
            this._depth = cloneSource._depth;

        }
        #endregion

        #region Structures

        #endregion

        #region ByteCode
        internal static byte[] ReachEffectCode = null;
        internal static byte[] HiDefEffectCode = null;

        static ClearEffect()
        {
#if XBOX360
            ReachEffectCode = HiDefEffectCode = new byte[] 
            {
0xBC, 0xF0, 0x0B, 0xCF, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x09, 0x01, 0x00, 0x00, 0x00, 0xA4, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 
0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x64, 0x65, 0x70, 0x74, 
0x68, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6C, 0x74, 0x00, 0x00, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x38, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x98, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x5C, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x5D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 
0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x10, 0x2A, 0x11, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x00, 0x00, 0x00, 0x30, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0xA8, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x9B, 0xFF, 0xFF, 0x03, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x1C, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00, 0x44, 0x00, 0x02, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4C, 
0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00, 0x6C, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x74, 0x00, 0x00, 0x00, 0x84, 
0x64, 0x65, 0x70, 0x74, 0x68, 0x00, 0xAB, 0xAB, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C, 0x00, 0xAB, 
0x00, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x73, 0x5F, 0x33, 0x5F, 0x30, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x31, 0x31, 0x36, 0x32, 0x36, 
0x2E, 0x30, 0x00, 0xAB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x20, 0x01, 0xC4, 0x00, 0x22, 0x00, 0x00, 0x00, 
0x14, 0x10, 0xC0, 0x01, 0x00, 0x00, 0x00, 0x6C, 0xC2, 0x00, 0x00, 0x01, 0xC8, 0x8F, 0xC0, 0x00, 0x00, 0xC0, 0xC0, 0x00, 0x22, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xCC, 0x10, 0x2A, 0x11, 0x01, 0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0x48, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x23, 0xFF, 0xFE, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x76, 0x73, 0x5F, 0x33, 0x5F, 0x30, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x31, 0x31, 0x36, 0x32, 0x36, 
0x2E, 0x30, 0x00, 0xAB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x90, 0x00, 0x00, 0x00, 0x03, 
0x10, 0x01, 0x10, 0x03, 0x00, 0x00, 0x12, 0x00, 0xC2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x04, 0x00, 0x00, 0x12, 0x00, 0xC4, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x22, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0xF8, 0x00, 0x00, 0x00, 0x00, 0x06, 0x88, 0x00, 0x00, 0x00, 0x00, 
0xC8, 0x0F, 0x80, 0x3E, 0x00, 0x00, 0x00, 0x00, 0xE2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            };
#else
            ReachEffectCode = new byte[] 
            {
0xCF, 0x0B, 0xF0, 0xBC, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x09, 0xFF, 0xFE, 0xA8, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x07, 0x00, 0x00, 0x00, 
0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C, 0x00, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x06, 0x00, 0x00, 0x00, 0x64, 0x65, 0x70, 0x74, 
0x68, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6C, 0x74, 0x00, 
0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x38, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x9C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 
0x92, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x93, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x80, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD0, 0x02, 0x00, 0x00, 0x00, 0x02, 0xFF, 0xFF, 0xFE, 0xFF, 0x16, 0x00, 
0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00, 0x23, 0x00, 0x00, 0x00, 0x00, 0x02, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x20, 0x1C, 0x00, 0x00, 0x00, 0x70, 0x73, 0x5F, 0x32, 0x5F, 0x30, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 
0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 
0x72, 0x20, 0x39, 0x2E, 0x32, 0x36, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x32, 0x38, 0x34, 0x34, 0x00, 0xFE, 0xFF, 0x94, 0x00, 0x50, 0x52, 0x45, 0x53, 
0x01, 0x02, 0x58, 0x46, 0xFE, 0xFF, 0x33, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00, 0x97, 0x00, 0x00, 0x00, 0x01, 0x02, 0x58, 0x46, 
0x02, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x20, 0x94, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x74, 0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0x64, 0x65, 0x70, 0x74, 0x68, 0x00, 0xAB, 0xAB, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C, 0x00, 0xAB, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x74, 0x78, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 
0x73, 0x6F, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D, 
0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 0x32, 0x36, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x32, 0x38, 0x34, 0x34, 0x00, 0xFE, 0xFF, 0x0C, 0x00, 
0x50, 0x52, 0x53, 0x49, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0xFE, 0xFF, 0x1A, 0x00, 0x43, 0x4C, 0x49, 0x54, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x34, 0x00, 0x46, 0x58, 0x4C, 0x43, 0x06, 0x00, 0x00, 0x00, 
0x03, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x04, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 
0xF0, 0xF0, 0xF0, 0xF0, 0x0F, 0x0F, 0x0F, 0x0F, 0xFF, 0xFF, 0x00, 0x00, 0x01, 0x00, 0x00, 0x02, 0x00, 0x08, 0x0F, 0x80, 0x00, 0x00, 0xE4, 0xA0, 
0x01, 0x00, 0x00, 0x02, 0x01, 0x08, 0x0F, 0x80, 0x01, 0x00, 0xE4, 0xA0, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x02, 0xFE, 0xFF, 0xFE, 0xFF, 0x16, 0x00, 
0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00, 0x23, 0x00, 0x00, 0x00, 0x00, 0x02, 0xFE, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x20, 0x1C, 0x00, 0x00, 0x00, 0x76, 0x73, 0x5F, 0x32, 0x5F, 0x30, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 
0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 
0x72, 0x20, 0x39, 0x2E, 0x32, 0x36, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x32, 0x38, 0x34, 0x34, 0x00, 0x1F, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x80, 
0x00, 0x00, 0x0F, 0x90, 0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x0F, 0xC0, 0x00, 0x00, 0xE4, 0x90, 0xFF, 0xFF, 0x00, 0x00, 
            };
            HiDefEffectCode = ReachEffectCode;
#endif
        }
        #endregion
    }

#endif
}
