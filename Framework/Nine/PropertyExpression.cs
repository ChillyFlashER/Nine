#region Copyright 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
#endregion

namespace Nine
{
    /// <summary>
    /// Represents a property access expression.
    /// </summary>
    /// <example>
    /// "Name"                  -> Target.Name
    /// "Name.FirstName"        -> Target.Name.FirstName
    /// "Names[0].FirstName"    -> Target.Names[0].FirstName
    /// "Names["n"].FirstName"  -> Target.Names["n"].FirstName
    /// </example>
    class PropertyExpression
    {
        MemberInfo invocationMember;
        object invocationTarget;

        /// <summary>
        /// Gets or sets the value of the target evaluated using this expression.
        /// </summary>
        public object Value
        {
            get { return GetValue(invocationTarget, invocationMember); }
            set { SetValue(invocationTarget, invocationMember, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyExpression"/> class.
        /// </summary>
        public PropertyExpression(object target, string property)
        {
            Parse(target, property, out invocationTarget, out invocationMember);
        }

        private static void Parse(object target, string property, out object invocationTarget, out MemberInfo invocationMember)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (property == null)
                throw new ArgumentNullException("property");

            invocationMember = null;

            var targetType = target.GetType();
            var dot = property.IndexOf('.');
            var currentProperty = dot >= 0 ? property.Substring(0, dot).Trim() : property;
            
            var leftBracket = currentProperty.IndexOf('[');
            var rightBracket = currentProperty.IndexOf(']');
            if (leftBracket * rightBracket <= 0)
            {
                throw new ArgumentException("Invalid expression format: " + currentProperty);
            }
            if (leftBracket > 0)
            {
                if (dot < 0)
                    throw new NotSupportedException();
                var content = currentProperty.Substring(leftBracket + 1, rightBracket - leftBracket - 1).Trim();
                currentProperty = currentProperty.Substring(0, leftBracket);
                invocationTarget = GetValue(target, GetMember(target.GetType(), currentProperty));
                if (content.StartsWith("\"") && content.EndsWith("\""))
                {
                    invocationMember = invocationTarget.GetType().GetProperty("Item", null, new[] { typeof(string) });
                    invocationTarget = GetValue(invocationTarget, invocationMember, content.Substring(1, content.Length - 2));
                }
                else
                {
                    invocationMember = invocationTarget.GetType().GetProperty("Item", null, new[] { typeof(int) });
                    invocationTarget = GetValue(invocationTarget, invocationMember, Convert.ToInt32(content));
                }
            }
            else if (dot < 0)
            {
                invocationTarget = target;
                invocationMember = GetMember(target.GetType(), currentProperty);
                if (invocationMember == null)
                {
                    throw new ArgumentException("Type " + targetType.FullName + " does not have a valid public property or field: " + currentProperty);
                }
            }
            else
            {
                invocationTarget = GetValue(target, GetMember(target.GetType(), currentProperty));
            }

            if (dot >= 0)
            {
                Parse(invocationTarget, property.Substring(dot + 1, property.Length - dot - 1).Trim(), out invocationTarget, out invocationMember);
            }
        }

        private static MemberInfo GetMember(Type targetType, string property)
        {
            return (MemberInfo)targetType.GetProperty(property) ?? targetType.GetField(property);
        }
                
        private static object GetValue(object target, MemberInfo member)
        {
            return (member is FieldInfo) ? ((FieldInfo)(member)).GetValue(target) : ((PropertyInfo)(member)).GetValue(target, null);
        }

        private static object GetValue(object target, MemberInfo member, int index)
        {
            return ((PropertyInfo)(member)).GetValue(target, new object[] { index });
        }

        private static object GetValue(object target, MemberInfo member, string key)
        {
            return ((PropertyInfo)(member)).GetValue(target, new object[] { key });
        }

        private static void SetValue(object target, MemberInfo member, object value)
        {
            if (member is FieldInfo)
                ((FieldInfo)(member)).SetValue(target, value);
            else
                ((PropertyInfo)(member)).SetValue(target, value, null);
        }
    }
}