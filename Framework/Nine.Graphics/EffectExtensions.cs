namespace Nine.Graphics
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Gets or sets skinning parameters for the current effect.
    /// </summary>
    public interface IEffectSkinned
    {
        /// <summary>
        /// Gets or sets if vertex skinning is enabled by this effect.
        /// </summary>
        bool SkinningEnabled { get; set; }

        /// <summary>
        /// Sets the bones transforms for the skinned effect.
        /// </summary>
        void SetBoneTransforms(Matrix[] boneTransforms);
    }

    /// <summary>
    /// Gets or sets lighting parameters for the current effect.
    /// </summary>
    public interface IEffectLights<T>
    {
        bool Bind(int ordinal, T light);

        /// <summary>
        /// Gets a read only collection of lights exposed by this effect.
        /// </summary>
        ReadOnlyCollection<T> Lights { get; }
    }

    /// <summary>
    /// Contains commonly used material parameters.
    /// </summary>
    public interface IEffectMaterial
    {
        /// <summary>
        /// Gets or sets the opaque of the effect.
        /// </summary>
        float Alpha { get; set; }

        /// <summary>
        /// Gets or sets the diffuse color of the effect.
        /// </summary>
        Vector3 DiffuseColor { get; set; }

        /// <summary>
        /// Gets or sets the emissive color of the effect.
        /// </summary>
        Vector3 EmissiveColor { get; set; }

        /// <summary>
        /// Gets or sets the specular color of the effect.
        /// </summary>
        Vector3 SpecularColor { get; set; }

        /// <summary>
        /// Gets or sets the specular power of the effect.
        /// </summary>
        float SpecularPower { get; set; }
    }

    /// <summary>
    /// Gets or sets texture parameters for the current effect.
    /// </summary>
    public interface IEffectTexture
    {
        /// <summary>
        /// Gets or sets the primiary diffuse texture of the current effect.
        /// </summary>
        Texture2D Texture { get; set; }

        /// <summary>
        /// Sets the texture with the specified texture usage.
        /// </summary>
        void SetTexture(TextureUsage usage, Texture texture);
    }

    /// <summary>
    /// Defines a wrapper around effect. Each effect instance stores
    /// a local copy of effect parameter values, this values are pushed to
    /// the underlying effect when Apply is called.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IEffectInstance
    {
        /// <summary>
        /// Gets the underlying effect.
        /// </summary>
        Effect Effect { get; }

        /// <summary>
        /// Applys the parameter values.
        /// </summary>
        void Apply();
    }
    
    /// <summary>
    /// Contains extension methods for Effects.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EffectExtensions
    {
        internal static T Find<T>(this Effect effect) where T : class
        {
#if WINDOWS_PHONE
            return effect as T;
#else
            T result = effect as T;
            if (result != null)
                return result;
            return result;
#endif
        }

        #region Copy Material
        internal static void CopyMaterialsFrom(this Effect effect, Effect sourceEffect)
        {
            if (effect == null || sourceEffect == null)
                return;

            Vector3 DiffuseColor = Vector3.One;
            Vector3 SpecularColor = Vector3.Zero;
            Vector3 EmissiveColor = Vector3.Zero;
            float SpecularPower = 16;
            float Alpha = 1;

            Texture2D Texture = sourceEffect.GetTexture();
            if (Texture != null)
                effect.SetTexture(Texture);

            // Extract from source
            if (sourceEffect is IEffectMaterial)
            {
                IEffectMaterial source = sourceEffect as IEffectMaterial;
                DiffuseColor = source.DiffuseColor;
                EmissiveColor = source.EmissiveColor;
                SpecularColor = source.SpecularColor;
                SpecularPower = source.SpecularPower;
                Alpha = source.Alpha;
            }
            else if (sourceEffect is BasicEffect)
            {
                BasicEffect source = sourceEffect as BasicEffect;
                DiffuseColor = source.DiffuseColor;
                EmissiveColor = source.EmissiveColor;
                SpecularColor = source.SpecularColor;
                SpecularPower = source.SpecularPower;
                Alpha = source.Alpha;
            }
            else if (sourceEffect is SkinnedEffect)
            {
                SkinnedEffect source = sourceEffect as SkinnedEffect;
                DiffuseColor = source.DiffuseColor;
                EmissiveColor = source.EmissiveColor;
                SpecularColor = source.SpecularColor;
                SpecularPower = source.SpecularPower;
                Alpha = source.Alpha;
            }
            else if (sourceEffect is EnvironmentMapEffect)
            {
                EnvironmentMapEffect source = sourceEffect as EnvironmentMapEffect;
                DiffuseColor = source.DiffuseColor;
                EmissiveColor = source.EmissiveColor;
                Alpha = source.Alpha;
            }
            else if (sourceEffect is DualTextureEffect)
            {
                DualTextureEffect source = sourceEffect as DualTextureEffect;
                DiffuseColor = source.DiffuseColor;
                Alpha = source.Alpha;
            }
            else if (sourceEffect is AlphaTestEffect)
            {
                AlphaTestEffect source = sourceEffect as AlphaTestEffect;
                DiffuseColor = source.DiffuseColor;
                Alpha = source.Alpha;
            }


            // Apply to target
            if (effect is IEffectMaterial)
            {
                IEffectMaterial target = effect as IEffectMaterial;
                target.DiffuseColor = DiffuseColor;
                target.EmissiveColor = EmissiveColor;
                target.SpecularColor = SpecularColor;
                target.SpecularPower = SpecularPower;
                target.Alpha = Alpha;
            }
            else if (effect is BasicEffect)
            {
                BasicEffect target = effect as BasicEffect;
                target.DiffuseColor = DiffuseColor;
                target.EmissiveColor = EmissiveColor;
                target.SpecularColor = SpecularColor;
                target.SpecularPower = SpecularPower;
                target.Alpha = Alpha;
            }
            else if (effect is SkinnedEffect)
            {
                SkinnedEffect target = effect as SkinnedEffect;
                target.DiffuseColor = DiffuseColor;
                target.EmissiveColor = EmissiveColor;
                target.SpecularColor = SpecularColor;
                target.SpecularPower = SpecularPower;
                target.Alpha = Alpha;
            }
            else if (effect is EnvironmentMapEffect)
            {
                EnvironmentMapEffect target = effect as EnvironmentMapEffect;
                target.DiffuseColor = DiffuseColor;
                target.EmissiveColor = EmissiveColor;
                target.Alpha = Alpha;
            }
            else if (effect is DualTextureEffect)
            {
                DualTextureEffect target = effect as DualTextureEffect;
                target.DiffuseColor = DiffuseColor;
                target.Alpha = Alpha;
            }
            else if (effect is AlphaTestEffect)
            {
                AlphaTestEffect target = effect as AlphaTestEffect;
                target.DiffuseColor = DiffuseColor;
                target.Alpha = Alpha;
            }
        }

        internal static void SetTexture(this Effect effect, Texture2D texture)
        {
            if (effect is IEffectTexture)
            {
                IEffectTexture source = effect as IEffectTexture;
                source.Texture = texture;
            }
            else if (effect is BasicEffect)
            {
                BasicEffect source = effect as BasicEffect;
                source.Texture = texture;
                source.TextureEnabled = texture != null;
            }
            else if (effect is SkinnedEffect)
            {
                SkinnedEffect source = effect as SkinnedEffect;
                source.Texture = texture;
            }
            else if (effect is EnvironmentMapEffect)
            {
                EnvironmentMapEffect source = effect as EnvironmentMapEffect;
                source.Texture = texture;
            }
            else if (effect is DualTextureEffect)
            {
                DualTextureEffect source = effect as DualTextureEffect;
                source.Texture = texture;
            }
            else if (effect is AlphaTestEffect)
            {
                AlphaTestEffect source = effect as AlphaTestEffect;
                source.Texture = texture;
            }
        }

        internal static Texture2D GetTexture(this Effect sourceEffect)
        {
            Texture2D texture = null;

            if (sourceEffect is IEffectTexture)
            {
                IEffectTexture source = sourceEffect as IEffectTexture;
                texture = source.Texture;
            }
            else if (sourceEffect is BasicEffect)
            {
                BasicEffect source = sourceEffect as BasicEffect;
                texture = source.Texture;
            }
            else if (sourceEffect is SkinnedEffect)
            {
                SkinnedEffect source = sourceEffect as SkinnedEffect;
                texture = source.Texture;
            }
            else if (sourceEffect is EnvironmentMapEffect)
            {
                EnvironmentMapEffect source = sourceEffect as EnvironmentMapEffect;
                texture = source.Texture;
            }
            else if (sourceEffect is DualTextureEffect)
            {
                DualTextureEffect source = sourceEffect as DualTextureEffect;
                texture = source.Texture;
            }
            else if (sourceEffect is AlphaTestEffect)
            {
                AlphaTestEffect source = sourceEffect as AlphaTestEffect;
                texture = source.Texture;
            }

            return texture;
        }
        #endregion

        #region SetValue
        public static void SetValue(this EffectParameter parameter, object value)
        {
            if (value is Texture)
                parameter.SetValue((Texture)value);
            else if (value is string)
                parameter.SetValue((string)value);
            else if (value is float)
                parameter.SetValue((float)value);
            else if (value is float[])
                parameter.SetValue((float[])value);
            else if (value is int)
                parameter.SetValue((int)value);
            else if (value is int[])
                parameter.SetValue((int[])value);
            else if (value is bool)
                parameter.SetValue((bool)value);
            else if (value is bool[])
                parameter.SetValue((bool[])value);
            else if (value is Quaternion)
                parameter.SetValue((Quaternion)value);
            else if (value is Quaternion[])
                parameter.SetValue((Quaternion[])value);
            else if (value is Matrix)
                parameter.SetValue((Matrix)value);
            else if (value is Matrix[])
                parameter.SetValue((Matrix[])value);
            else if (value is Vector2)
                parameter.SetValue((Vector2)value);
            else if (value is Vector2[])
                parameter.SetValue((Vector2[])value);
            else if (value is Vector3)
                parameter.SetValue((Vector3)value);
            else if (value is Vector3[])
                parameter.SetValue((Vector3[])value);
            else if (value is Vector4)
                parameter.SetValue((Vector4)value);
            else if (value is Vector4[])
                parameter.SetValue((Vector4[])value);
            else
                throw new NotSupportedException("Unexpected effect parameter type: " + value.GetType());
        }
        #endregion

        #region GetValue
        public static object GetValue(this EffectParameter parameter)
        {
            if (parameter.ParameterClass == EffectParameterClass.Object)
            {
                if (parameter.ParameterType == EffectParameterType.Texture2D)
                    return parameter.GetValueTexture2D();
                if (parameter.ParameterType == EffectParameterType.Texture3D)
                    return parameter.GetValueTexture3D();
                if (parameter.ParameterType == EffectParameterType.TextureCube)
                    return parameter.GetValueTextureCube();
            }

            if (parameter.ParameterType == EffectParameterType.Int32)
                return parameter.GetValueInt32();
            if (parameter.ParameterClass == EffectParameterClass.Matrix)
                return parameter.GetValueMatrix();

            if (parameter.ParameterClass == EffectParameterClass.Vector)
            {
                if (parameter.ColumnCount == 1)
                    return parameter.GetValueSingle();
                if (parameter.ColumnCount == 2)
                    return parameter.GetValueVector2();
                if (parameter.ColumnCount == 3)
                    return parameter.GetValueVector3();
                if (parameter.ColumnCount == 4)
                    return parameter.GetValueVector4();
            }

            throw new NotSupportedException();
        }
        #endregion
    }
}