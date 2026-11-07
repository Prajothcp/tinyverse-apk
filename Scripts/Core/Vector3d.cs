using System;
using Godot;

namespace TinyverseCSharp.Core
{
    /// <summary>
    /// 64-bit Double-Precision 3D Vector for astronomical coordinates in C#.
    /// Prevents 32-bit float precision loss across 28 orders of magnitude.
    /// </summary>
    public struct Vector3d
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3d Zero => new Vector3d(0, 0, 0);

        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);
        public double LengthSquared => X * X + Y * Y + Z * Z;

        public Vector3 ToVector3() => new Vector3((float)X, (float)Y, (float)Z);

        public static Vector3d Lerp(Vector3d a, Vector3d b, double t)
        {
            t = Math.Clamp(t, 0.0, 1.0);
            return new Vector3d(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t
            );
        }

        public static Vector3d operator +(Vector3d a, Vector3d b) => new Vector3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3d operator -(Vector3d a, Vector3d b) => new Vector3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3d operator *(Vector3d a, double s) => new Vector3d(a.X * s, a.Y * s, a.Z * s);
        public override string ToString() => $"[{X:F2}, {Y:F2}, {Z:F2}]";
    }
}
