namespace Nine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Markup;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Nine.Animations;

    /// <summary>
    /// Defines a instance that is created from a template.
    /// </summary>
    [ContentSerializable]
    [ContentProperty("Properties")]
    public class Instance : Nine.Object, IObjectFactory
    {
        /// <summary>
        /// Gets or sets the file name that contains the template.
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Gets a dictionary of properties that will override the properties
        /// in the created instance.
        /// </summary>
        public IDictionary<string, object> Properties
        {
            get { return properties; }
        }
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        /// <summary>
        /// Create a new instance of the object from template.
        /// </summary>
        public T CreateInstance<T>(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Template))
                return default(T);

            var contentManager = serviceProvider.GetService<ContentManager>();
            if (contentManager == null)
                throw new InvalidOperationException("Cannot find content manager in the service provider");

            return ApplyProperties(contentManager.Create<T>(Template));
        }

        /// <summary>
        /// Create a new instance of the object from template.
        /// </summary>
        public object CreateInstance(IServiceProvider serviceProvider)
        {
            return CreateInstance<object>(serviceProvider);
        }

        private T ApplyProperties<T>(T result)
        {
            foreach (var pair in properties)
            {
                new PropertyExpression<object>(result, pair.Key).Value = pair.Value;
            }
            return result;
        }
    }
}