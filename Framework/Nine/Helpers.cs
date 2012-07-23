namespace Nine
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines the rotation order for 3D rotations. 
    /// The default value is Zxy which is equivalent to Matrix.CreateFromYawPitchRoll.
    /// </summary>
    public enum RotationOrder
    {
        /// <summary>
        /// The rotation will be around z, x and y axis in that order.
        /// This is equivalent to Matrix.CreateFromYawPitchRoll.
        /// </summary>
        Zxy,

        /// <summary>
        /// The rotation will be around y, x and z axis in that order.
        /// </summary>
        Yxz,
    }

    /// <summary>
    /// A method used to interpolate the specified type.
    /// </summary>
    public delegate T Interpolate<T>(T x, T y, float amount);

    /// <summary>
    /// Helper class to interpolate common types.
    /// </summary>
    internal static class LerpHelper
    {
        public static byte Lerp(byte x, byte y, float amount)
        {
            return (byte)(x * (1 - amount) + y * amount);
        }
        
        public static char Lerp(char x, char y, float amount)
        {
            return (char)(x * (1 - amount) + y * amount);
        }

        public static int Lerp(int x, int y, float amount)
        {
            return (int)(x * (1 - amount) + y * amount);
        }

        public static short Lerp(short x, short y, float amount)
        {
            return (short)(x * (1 - amount) + y * amount);
        }

        public static long Lerp(long x, long y, float amount)
        {
            return (long)(x * (1 - amount) + y * amount);
        }

        public static double Lerp(double x, double y, float amount)
        {
            return x * (1 - amount) + y * amount;
        }

        public static decimal Lerp(decimal x, decimal y, float amount)
        {
            return x * (decimal)(1 - amount) + y * (decimal)amount;
        }

        public static bool Lerp(bool x, bool y, float amount)
        {
            return amount < 0.5f ? x : y;
        }

        public static Rectangle Lerp(Rectangle x, Rectangle y, float amount)
        {
            Rectangle rc = new Rectangle();

            rc.X = Lerp(x.X, y.X, amount);
            rc.Y = Lerp(x.Y, y.Y, amount);
            rc.Width = Lerp(x.Width, y.Width, amount);
            rc.Height = Lerp(x.Height, y.Height, amount);

            return rc;
        }

        public static Point Lerp(Point x, Point y, float amount)
        {
            Point rc = new Point();

            rc.X = Lerp(x.X, y.X, amount);
            rc.Y = Lerp(x.Y, y.Y, amount);

            return rc;
        }

        public static Ray Lerp(Ray x, Ray y, float amount)
        {
            Ray r;

            r.Position = Vector3.Lerp(x.Position, y.Position, amount);
            r.Direction = Vector3.Lerp(x.Direction, y.Direction, amount);

            return r;
        }

        /// <summary>
        /// Roughly decomposes two matrices and performs spherical linear interpolation
        /// </summary>
        /// <param name="a">Source matrix for interpolation</param>
        /// <param name="b">Destination matrix for interpolation</param>
        /// <param name="amount">Ratio of interpolation</param>
        /// <returns>The interpolated matrix</returns>
        public static Matrix Slerp(Matrix a, Matrix b, float amount)
        {
            Quaternion rotationA, rotationB, rotation;
            Vector3 scaleA, scaleB, scale;
            Vector3 translationA, translationB, translation;
            Matrix mxScale, mxRotation;

            if (a.Decompose(out scaleA, out  rotationA, out translationA) &&
                b.Decompose(out scaleB, out  rotationB, out translationB))
            {
                Vector3.Lerp(ref scaleA, ref scaleB, amount, out scale);
                Vector3.Lerp(ref translationA, ref translationB, amount, out translation);
                Quaternion.Slerp(ref rotationA, ref rotationB, amount, out rotation);

                Matrix.CreateScale(ref scale, out mxScale);
                Matrix.CreateFromQuaternion(ref rotation, out mxRotation);
                Matrix.Multiply(ref mxScale, ref mxRotation, out mxRotation);

                mxRotation.M41 += translation.X;
                mxRotation.M42 += translation.Y;
                mxRotation.M43 += translation.Z;
                return mxRotation;
            }

            throw new InvalidOperationException(Strings.CannotDecomposeMatrix);
        }

        /// <summary>
        /// Implemements a weighted sum algorithm of quaternions.
        /// See http://en.wikipedia.org/wiki/Generalized_quaternion_interpolation for detailed math formulars.
        /// </summary>
        public static Quaternion WeightedSum(IEnumerable<Quaternion> quaternions, IEnumerable<float> weights, int steps)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Contains helper method for Matrix.
    /// </summary>
    internal static class MatrixHelper
    {
        public static Matrix CreateRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            Matrix result = new Matrix();
            CreateRotation(ref fromDirection, ref toDirection, out result);
            return result;
        }
        
        public static void CreateRotation(ref Vector3 fromDirection, ref Vector3 toDirection, out Matrix matrix)
        {
            Vector3 axis = new Vector3();
            Vector3.Cross(ref fromDirection, ref toDirection, out axis);
            axis.Normalize();

            if (float.IsNaN(axis.X))
            {
                matrix = Matrix.Identity;
                return;
            }

            float angle;
            Vector3.Dot(ref fromDirection, ref toDirection, out angle);
            Matrix.CreateFromAxisAngle(ref axis, (float)Math.Acos(angle), out matrix);
        }

        public static Matrix CreateRotation(Vector3 rotation, RotationOrder rotationOrder)
        {
            Matrix result;
            CreateRotation(ref rotation, rotationOrder, out result);
            return result;
        }

        public static void CreateRotation(ref Vector3 rotation, RotationOrder rotationOrder, out Matrix result)
        {
            if (rotationOrder == RotationOrder.Zxy)
                Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z, out result);
            else
            {
                Matrix temp2;
                Matrix.CreateRotationY(rotation.Y, out result);
                Matrix.CreateRotationX(rotation.X, out temp2);
                Matrix.Multiply(ref result, ref temp2, out result);
                Matrix.CreateRotationZ(rotation.Z, out temp2);
                Matrix.Multiply(ref result, ref temp2, out result);
            }
        }

        public static Matrix CreateWorld(Vector3 position, Vector3 direction)
        {
            direction.Normalize();
            if (Math.Abs(direction.X * direction.Z) < 0.0001f)
                return Matrix.CreateWorld(position, direction, Vector3.Up);
            return Matrix.CreateWorld(position, direction, Vector3.UnitZ);
        }

        static BoundingFrustum frustum = new BoundingFrustum(Matrix.Identity);

        internal static float GetFarClip(this Matrix projection)
        {
            frustum.Matrix = projection;
            return (frustum.Near.Normal * frustum.Near.D - frustum.Far.Normal * frustum.Far.D).Length();
            //return Math.Abs(projection.M43 / (Math.Abs(projection.M33) - 1));
        }

        [Obsolete]
        internal static float GetNearClip(this Matrix projection)
        {
            return Math.Abs(projection.M43 / projection.M33);
        }
    }
}