using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace ____.Systems
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeRect  { public float X, Y, W, H; }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeCircle { public float X, Y, Radius; }

    public static class DetectionNative
    {
        const string Lib = "detection";

        [DllImport(Lib)] public static extern int aabb_overlaps(NativeRect a, NativeRect b);
        [DllImport(Lib)] public static extern int point_in_rect(float px, float py, NativeRect r);
        [DllImport(Lib)] public static extern int circles_overlap(NativeCircle a, NativeCircle b);

        public static NativeRect ToNativeRect(Rectangle r)
        {
            return new NativeRect { X = r.X, Y = r.Y, W = r.Width, H = r.Height };
        }
    }

}