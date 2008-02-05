//-----------------------------------------------------------------------------
//  Isles v1.0
//  
//  Copyright 2008 (c) Nightin Games. All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Isles.Engine;

namespace Isles.Graphics
{
    /// <summary>
    /// Provide sprite text drawing functionality
    /// </summary>
    public static class Text
    {
        struct StringValue
        {
            public string Text;
            public float Size;
            public Vector2 Position;
            public Color Color;
        }

        static BaseGame game;
        static SpriteFont font;
        static SpriteBatch sprite;
        static BasicEffect basicEffect;
        static List<StringValue> values = new List<StringValue>();

        /// <summary>
        /// Get sprite font
        /// </summary>
        public static SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// Gets or Sets sprite batch used to drawing the text
        /// </summary>
        public static SpriteBatch Sprite
        {
            get { return sprite; }
        }

        /// <summary>
        /// Initialize text system
        /// </summary>
        /// <param name="game"></param>
        public static void Initialize(BaseGame setGame)
        {
            game = setGame;
            font = game.Content.Load<SpriteFont>(game.Settings.DefaultFont);
            sprite = new SpriteBatch(game.GraphicsDevice);
            basicEffect = new BasicEffect(game.GraphicsDevice, null);

            Log.Write("Text Initialized...");
        }

        /// <summary>
        /// Default text drawing function
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public static void DrawString(string text, float size, Vector3 position, Color color)
        {
            Vector3 v = game.GraphicsDevice.Viewport.Project(
                position, game.Projection, game.View, Matrix.Identity);

            DrawString(text, size, new Vector2(v.X, v.Y), color);
        }

        /// <summary>
        /// Default text drawing function
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public static void DrawString(string text, float size, Vector2 position, Color color)
        {
            StringValue value = new StringValue();
            value.Text = text;
            value.Size = size;
            value.Position = position;
            value.Color = color;

            values.Add(value);
        }

        /// <summary>
        /// Draw a line strip
        /// </summary>
        public static void DrawLineStrip(Vector3[] vertices, Vector3 color)
        {
            basicEffect.View = game.View;
            basicEffect.Projection = game.Projection;
            basicEffect.DiffuseColor = color;
            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                game.GraphicsDevice.DrawUserPrimitives<Vector3>(
                    PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);

                pass.End();
            }
            basicEffect.End();
        }

        /// <summary>
        /// Draw a line list
        /// </summary>
        public static void DrawLineList(Vector3[] vertices, Vector3 color)
        {
            basicEffect.View = game.View;
            basicEffect.Projection = game.Projection;
            basicEffect.DiffuseColor = color;
            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                game.GraphicsDevice.DrawUserPrimitives<Vector3>(
                    PrimitiveType.LineList, vertices, 0, vertices.Length / 2);

                pass.End();
            }
            basicEffect.End();
        }

        /// <summary>
        /// Call this at the end of the frame to draw all strings
        /// </summary>
        public static void Present()
        {
            sprite.Begin();
            foreach (StringValue value in values)
                sprite.DrawString(
                    font, value.Text, value.Position, value.Color, 0,
                    Vector2.Zero, value.Size / font.LineSpacing, SpriteEffects.None, 0);
            sprite.End();

            // Clear all string in this frame
            values.Clear();
        }
        
        /// <summary>
        /// Each charactor has a different with and height. But sadly SpriteFont
        /// hides all those charactor map and cropping stuff inside, making it
        /// difficult to measure and format text ourself.
        /// Unless you have any better idea, just use a fixed charactor width and
        /// height. This would yield incorrect result.
        /// </summary>
        public const int CharactorWidth = 10;
        public const int CharactorHeight = 25;

        /// <summary>
        /// Format the given text to make it fit in a rectangle area.
        /// What if we could recognize and split english word?
        /// </summary>
        /// <returns>The formatted text</returns>
        /// <example>
        /// FormatString("ABCD", 20): "AB\nCD"
        /// FormatString("What is your name?", 100): "What is\nyour name?"
        /// </example>
        public static string FormatString(string text, int width)
        {
            StringBuilder rtvSB = new StringBuilder();
            
            //Charactors per line
            int charNumPerLine = width / CharactorWidth;
            
            if(charNumPerLine == 0)
            {
                throw new Exception("No enough space for even one charactor per line.");
            }

            //Num of lines
            int lineNum = text.Length / charNumPerLine;
            if(text.Length % charNumPerLine != 0)
            {
                lineNum++;
            }

            //Construct the formatted string
            for (int i = 0; i < lineNum - 1; i++)
            {
                rtvSB.Append(text.Substring(i * charNumPerLine, charNumPerLine));
                rtvSB.Append('\n');
            }
            rtvSB.Append(text.Substring( (lineNum - 1)* charNumPerLine));

            return rtvSB.ToString();
        }

        /// <summary>
        /// Format the given text to make it fit in a rectangle area.
        /// Clip and append "..." at the end if the text excceed the rectangle.
        /// </summary>
        /// <example>
        /// FormatString("ABCDEFGH", 40, 50)    : "ABCD\nEFGH"
        /// FormatString("ABCDEFGHIJ", 40, 50)  : "ABCD\nE..."
        /// </example>
        public static string FormatString(string text, int width, int height)
        {
            //Mux num of chars that can be held in this rectangle
            int muxChars = (height / CharactorHeight) * (width / CharactorWidth) ;

            if (muxChars < 3)
            {
                throw new Exception("No enough space for even \"...\"");
            }

            bool exceed = text.Length > muxChars;

            if (exceed)
            {
                return FormatString(text.Substring(0, muxChars - 3) + "...", width);
            }
            else
            {
                return FormatString(text, width);
            }
        }
    }
}
