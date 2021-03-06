using System;

namespace DrawingPipelineLibrary
{
    /// <summary>
    /// SharpDX.Vector related math operations.
    /// </summary>
    public class DXMathFunctions
    {
        public static SharpDX.Vector3 Vec_CrossProduct(SharpDX.Vector3 v1, SharpDX.Vector3 v2)
        {
            SharpDX.Vector3 v = new SharpDX.Vector3(0.0f, 0.0f, 0.0f);
            v.X = v1.Y * v2.Z - v1.Z * v2.Y;
            v.Y = v1.Z * v2.X - v1.X * v2.Z;
            v.Z = v1.X * v2.Y - v1.Y * v2.X;
            return v;
        }

        public static SharpDX.Vector4 Vec_CrossProduct(SharpDX.Vector4 v1, SharpDX.Vector4 v2)
        {
            SharpDX.Vector4 v = new SharpDX.Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            v.X = v1.Y * v2.Z - v1.Z * v2.Y;
            v.Y = v1.Z * v2.X - v1.X * v2.Z;
            v.Z = v1.X * v2.Y - v1.Y * v2.X;
            v.W = 0.0f;
            return v;
        }

        public static SharpDX.Vector4 Vec_Normalize(SharpDX.Vector4 v)
        {
            SharpDX.Vector4 temp = new SharpDX.Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            float l = Vec_Length(v);
            temp.X = v.X / l;
            temp.Y = v.Y / l;
            temp.Z = v.Z / l;
            temp.W = 0.0f;
            return temp;
        }

        public static SharpDX.Vector3 Vec_Normalize(SharpDX.Vector3 v)
        {
            SharpDX.Vector3 temp = new SharpDX.Vector3(0.0f, 0.0f, 0.0f);
            float l = Vec_Length(v);
            temp.X = v.X / l;
            temp.Y = v.Y / l;
            temp.Z = v.Z / l;
            return temp;
        }

        public static float Vec_Length(SharpDX.Vector4 v)
        {
            return (float)Math.Sqrt(Vec_DotProduct(v, v));
        }
        
        public static float Vec_Length(SharpDX.Vector3 v)
        {
            return (float)Math.Sqrt(Vec_DotProduct(v, v));
        }

        public static float Vec_DotProduct(SharpDX.Vector4 v1, SharpDX.Vector4 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static float Vec_DotProduct(SharpDX.Vector3 v1, SharpDX.Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static SharpDX.Vector4 Vec_Add(SharpDX.Vector4 v1, SharpDX.Vector4 v2)
        {
            SharpDX.Vector4 temp = new SharpDX.Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            temp.X = v1.X + v2.X;
            temp.Y = v1.Y + v2.Y;
            temp.Z = v1.Z + v2.Z;
            temp.W = 0.0f;
            return temp;
        }

        public static SharpDX.Vector3 Vec_Add(SharpDX.Vector3 v1, SharpDX.Vector3 v2)
        {
            SharpDX.Vector3 temp = new SharpDX.Vector3(0.0f, 0.0f, 0.0f);
            temp.X = v1.X + v2.X;
            temp.Y = v1.Y + v2.Y;
            temp.Z = v1.Z + v2.Z;
            return temp;
        }


        public static SharpDX.Vector4 Vec_Sub(SharpDX.Vector4 v1, SharpDX.Vector4 v2)
        {
            SharpDX.Vector4 temp = new SharpDX.Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            temp.X = v1.X - v2.X;
            temp.Y = v1.Y - v2.Y;
            temp.Z = v1.Z - v2.Z;
            temp.W = 0.0f;
            return temp;
        }

        public static SharpDX.Vector4 Vec_Mul(SharpDX.Vector4 v1, float k)
        {
            SharpDX.Vector4 temp = new SharpDX.Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            temp.X = v1.X * k;
            temp.Y = v1.Y * k;
            temp.Z = v1.Z * k;
            temp.W = v1.W * k;
            return temp;
        }

        public static SharpDX.Vector3 Vec_Mul(SharpDX.Vector3 v1, float k)
        {
            SharpDX.Vector3 temp = new SharpDX.Vector3(0.0f, 0.0f, 0.0f);
            temp.X = v1.X * k;
            temp.Y = v1.Y * k;
            temp.Z = v1.Z * k;
            return temp;
        }

        public static SharpDX.Vector3 Vec_Div(SharpDX.Vector3 v1, float k)
        {
            if (k == 0)
                throw new InvalidOperationException("Division by 0 detected");

            SharpDX.Vector3 temp = new SharpDX.Vector3(0.0f, 0.0f, 0.0f);
            temp.X = v1.X / k;
            temp.Y = v1.Y / k;
            temp.Z = v1.Z / k;
            return temp;
        }

        public static SharpDX.Vector4 Vec_Div(SharpDX.Vector4 v1, float k)
        {
            if (k == 0)
                throw new InvalidOperationException("Division by 0 detected");

            SharpDX.Vector4 temp = new SharpDX.Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            temp.X = v1.X / k;
            temp.Y = v1.Y / k;
            temp.Z = v1.Z / k;
            temp.W = v1.W / k;

            return temp;
        }
    }
}
