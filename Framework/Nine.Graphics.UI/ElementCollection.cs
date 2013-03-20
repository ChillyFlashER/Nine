#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace Nine.Graphics.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ElementCollection : IList<UIElement>, ITemplatedList<UIElement>
    {
        #region Properties

        public ElementCollection(UIElement owner)
        {
            this.owner = owner;
        }

        public int Count
        {
            get { return this.elements.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public UIElement this[int index]
        {
            get { return this.elements[index]; }
            set
            {
                UIElement oldItem = this.elements[index];
                UIElement newItem = value;

                this.elements[index] = newItem;
                this.owner.InvalidateMeasure();
                this.SetParents(oldItem, newItem);
            }
        }

        #endregion

        #region Fields

        private readonly List<UIElement> elements = new List<UIElement>();
        private readonly UIElement owner;

        #endregion

        #region Methods

        public void Add(UIElement item)
        {
            this.elements.Add(item);
            this.owner.InvalidateMeasure();
            this.SetParents(null, item);
        }

        public void Clear()
        {
            this.elements.Clear();
            this.owner.InvalidateMeasure();
        }

        public bool Contains(UIElement item)
        {
            return this.elements.Contains(item);
        }

        public void CopyTo(UIElement[] array, int arrayIndex)
        {
            this.elements.CopyTo(array, arrayIndex);
        }

        public bool Remove(UIElement item)
        {
            bool wasRemoved = this.elements.Remove(item);
            if (wasRemoved)
            {
                this.owner.InvalidateMeasure();
                this.SetParents(item, null);
            }

            return wasRemoved;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        public int IndexOf(UIElement item)
        {
            return this.elements.IndexOf(item);
        }

        public void Insert(int index, UIElement item)
        {
            this.elements.Insert(index, item);
            this.owner.InvalidateMeasure();
            this.SetParents(null, item);
        }

        public void RemoveAt(int index)
        {
            UIElement oldItem = this.elements[index];
            this.elements.RemoveAt(index);
            this.owner.InvalidateMeasure();
            this.SetParents(oldItem, null);
        }

        public void Add(object item, Func<object, UIElement> template)
        {
            this.Add(Realize(item, template));
        }

        public void Insert(int index, object item, Func<object, UIElement> template)
        {
            this.Insert(index, Realize(item, template));
        }

        public void Move(int oldIndex, int newIndex)
        {
            UIElement element = this[oldIndex];
            this.RemoveAt(oldIndex);
            this.Insert(newIndex, element);
        }

        private static UIElement Realize(object item, Func<object, UIElement> template)
        {
            if (template == null)
            {
                throw new InvalidOperationException("An element cannot be created without a template");
            }

            UIElement element = template(item);
            element.DataContext = item;
            return element;
        }

        private void SetParents(UIElement oldItem, UIElement newItem)
        {
            if (oldItem != null)
            {
                oldItem.Parent = null;
            }

            if (newItem != null)
            {
                newItem.Parent = this.owner;
            }
        }

#endregion
    }
}
