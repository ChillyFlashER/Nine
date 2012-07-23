﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nine.Content.Pipeline.Properties
{



    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Nine.Content.Pipeline.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.

        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Block type (parameters, headercode, interpolators, vertexattributes, textures, vertexoutputs, vs, ps) expected, but found &apos;{0}&apos;..
        /// </summary>
        internal static string FragmentParserParameterBlockTypeExpected {
            get {
                return ResourceManager.GetString("FragmentParserParameterBlockTypeExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected character: &apos;{0}&apos;..
        /// </summary>
        internal static string LexerUnexpectedCharacter {
            get {
                return ResourceManager.GetString("LexerUnexpectedCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected end of file found. &apos;&quot;&apos; expected..
        /// </summary>
        internal static string LexerUnexpectedEndOfFileString {
            get {
                return ResourceManager.GetString("LexerUnexpectedEndOfFileString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected array index, which must be either a positive integer &gt;= 1, or an identifier..
        /// </summary>
        internal static string ParserArrayIndexExpected {
            get {
                return ResourceManager.GetString("ParserArrayIndexExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data type (bool, int, int2, int3, int4, intNxM, float, float2, float3, float4, floatNxM, matrix, Texture1D, Texture2D, Texture3D, TextureCube) expected..
        /// </summary>
        internal static string ParserDataTypeExpected {
            get {
                return ResourceManager.GetString("ParserDataTypeExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initial value was not expected for this type of parameter block..
        /// </summary>
        internal static string ParserInitialValueUnexpected {
            get {
                return ResourceManager.GetString("ParserInitialValueUnexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shader profile &apos;{0}&apos; is not supported. Only 2_0 and 3_0 are supported..
        /// </summary>
        internal static string ParserShaderProfileNotSupported {
            get {
                return ResourceManager.GetString("ParserShaderProfileNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Syntax error: &apos;{0}&apos; expected..
        /// </summary>
        internal static string ParserTokenExpected {
            get {
                return ResourceManager.GetString("ParserTokenExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Syntax error: &apos;{0}&apos; unexpected..
        /// </summary>
        internal static string ParserTokenUnexpected {
            get {
                return ResourceManager.GetString("ParserTokenUnexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Block type (headercode, fragments, techniques) expected, but found &apos;{0}&apos;..
        /// </summary>
        internal static string StitchedEffectParserParameterBlockTypeExpected {
            get {
                return ResourceManager.GetString("StitchedEffectParserParameterBlockTypeExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to String literal or identifier expected..
        /// </summary>
        internal static string StitchedEffectParserStringLiteralOrIdentifierExpected {
            get {
                return ResourceManager.GetString("StitchedEffectParserStringLiteralOrIdentifierExpected", resourceCulture);
            }
        }
    }
}
