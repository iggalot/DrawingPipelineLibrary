using System;
using System.Windows.Media;

namespace MathLibrary
{
    public class MathVectors
    {
        public class Vec2D
        {
            public Vec2D()
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
            public Vec2D(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            public float X { get => v[0]; set { v[0] = value; } }
            public float Y { get => v[1]; set { v[1] = value; } }
            public float Z { get => v[2]; set { v[2] = value; } }

            float[] v = new float[3] { 0, 0, 0};
        }

        public class Vec3D
        {
            public Vec3D()
            {
                X = 0;
                Y = 0;
                Z = 0;
                W = 0;
            }
            public Vec3D(float x, float y, float z, float w = 1.0f)
            {
                X = x;
                Y = y;
                Z = z;
                W = w;
            }
            public float X { get => v[0]; set { v[0] = value; } }
            public float Y { get => v[1]; set { v[1] = value; } }
            public float Z { get => v[2]; set { v[2] = value; } }
            public float W { get => v[3]; set { v[3] = value; } } // needs a 4th term to work common 4x4 matrix multiplication

            float[] v = new float[4] { 0, 0, 0, 1 };
        }

        public class Mat4x4
        {
            /// <summary>
            /// Private members
            /// </summary>
            public float[,] m = new float[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

            /// <summary>
            /// First row
            /// </summary>
            public float M00 { get => m[0,0]; set { m[0, 0] = value; } }
            public float M01 { get => m[0,1]; set { m[0, 1] = value; } }
            public float M02 { get => m[0,2]; set { m[0, 2] = value; } }
            public float M03 { get => m[0,3]; set { m[0, 3] = value; } }

            /// <summary>
            /// Second row
            /// </summary>
            public float M10 { get => m[1, 0]; set { m[1, 0] = value; } }
            public float M11 { get => m[1, 1]; set { m[1, 1] = value; } }
            public float M12 { get => m[1, 2]; set { m[1, 2] = value; } }
            public float M13 { get => m[1, 3]; set { m[1, 3] = value; } }
            
            /// <summary>
            /// Third row
            /// </summary>
            public float M20 { get => m[2, 0]; set { m[2, 0] = value; } }
            public float M21 { get => m[2, 1]; set { m[2, 1] = value; } }
            public float M22 { get => m[2, 2]; set { m[2, 2] = value; } }
            public float M23 { get => m[2, 3]; set { m[2, 3] = value; } }

            /// <summary>
            /// First row
            /// </summary>
            public float M30 { get => m[3, 0]; set { m[3, 0] = value; } }
            public float M31 { get => m[3, 1]; set { m[3, 1] = value; } }
            public float M32 { get => m[3, 2]; set { m[3, 2] = value; } }
            public float M33 { get => m[3, 3]; set { m[3, 3] = value; } }
        }

        public class TriangleObject
        {
            public Vec3D[] p { get; set; } = new Vec3D[3];
            public Vec2D[] t { get; set; } = new Vec2D[3];

            public Pixel[] col { get; set; } = new Pixel[3];

            public TriangleObject()
            {
                p[0] = new Vec3D();
                p[1] = new Vec3D();
                p[2] = new Vec3D();
                t[0] = new Vec2D();
                t[1] = new Vec2D();
                t[2] = new Vec2D();
                col[0] = new Pixel(120,120,120);
                col[1] = new Pixel(120,120,120);
                col[2] = new Pixel(120,120,120);
            }
        }

        public class LineObject
        {
            public Vec3D[] p { get; set; } = new Vec3D[2];
            public Pixel[] col {get; set; } = new Pixel[2];

            public LineObject()
            {
                p[0] = new Vec3D();
                p[1] = new Vec3D();
                col[0] = new Pixel(120, 120, 120);
                col[1] = new Pixel(120, 120, 120);
            }
        }

        public class Pixel
        {
            public uint r = 0;
            public uint g = 0;
            public uint b = 0;
            public uint a = 0;

            public enum Mode { NORMAL, MASK, ALPHA, CUSTOM }

            public Pixel()
            {
                r = 120;
                b = 120;
                g = 120;
                a = 120;
            }
            public Pixel(uint red, uint green, uint blue, uint alpha = 0xFF)
            {
                r = red;
                g = green;
                b = blue;
                a = alpha;
            }
        }

        public static class MathOps
        {
            public static Vec3D Mat_MultiplyVector(Mat4x4 m, Vec3D i)
            {
                Vec3D v = new Vec3D(0, 0, 0, 1.0f);
                v.X = i.X * m.M00 + i.Y * m.M10 + i.Z * m.M20 + i.W * m.M30;
                v.Y = i.X * m.M01 + i.Y * m.M11 + i.Z * m.M21 + i.W * m.M31;
                v.Z = i.X * m.M02 + i.Y * m.M12 + i.Z * m.M22 + i.W * m.M32;
                v.W = i.X * m.M03 + i.Y * m.M13 + i.Z * m.M23 + i.W * m.M33;
                return v;
            }

            public static Mat4x4 Mat_MultiplyMatrix(Mat4x4 m1, Mat4x4 m2)
            {
                Mat4x4 matrix = new Mat4x4();
                for (int c = 0; c < 4; c++)
                    for (int r = 0; r < 4; r++)
                        matrix.m[r,c] = m1.m[r,0] * m2.m[0,c] + m1.m[r,1] * m2.m[1,c] + m1.m[r,2] * m2.m[2,c] + m1.m[r,3] * m2.m[3,c];
                return matrix;
            }

            public static Mat4x4 Mat_MakeIdentity()
            {
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = 1.0f;
                matrix.M11 = 1.0f;
                matrix.M22 = 1.0f;
                matrix.M33 = 1.0f;
                return matrix;
            }

            public static Mat4x4 Mat_MakeRotationX(float fAngleRad)
            {
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = 1.0f;
                matrix.M11 = (float)Math.Cos(fAngleRad);
                matrix.M12 = (float)Math.Sin(fAngleRad);
                matrix.M21 = (float)(-Math.Sin(fAngleRad));
                matrix.M22 = (float)Math.Cos(fAngleRad);
                matrix.M33 = 1.0f;
                return matrix;
            }

            public static Mat4x4 Mat_MakeRotationY(float fAngleRad)
            {
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = (float)Math.Cos(fAngleRad);
                matrix.M01 = (float)Math.Sin(fAngleRad);
                matrix.M10 = (float)(-Math.Sin(fAngleRad));
                matrix.M11 = 1.0f;
                matrix.M22 = (float)Math.Cos(fAngleRad);
                matrix.M33 = 1.0f;
                return matrix;
            }

            public static Mat4x4 Mat_MakeRotationZ(float fAngleRad)
            {
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = (float)Math.Cos(fAngleRad);
                matrix.M01 = (float)Math.Sin(fAngleRad);
                matrix.M10 = (float)(-Math.Sin(fAngleRad));
                matrix.M11 = (float)Math.Cos(fAngleRad);
                matrix.M22 = 1.0f;
                matrix.M33 = 1.0f;
                return matrix;
            }

            public static Mat4x4 Mat_MakeScale(float x, float y, float z)
            {
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = x;
                matrix.M11 = y;
                matrix.M22 = z;
                matrix.M33 = 1.0f;
                return matrix;
            }

            public static Mat4x4 Mat_MakeTranslation(float x, float y, float z)
            {
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = 1.0f;
                matrix.M11 = 1.0f;
                matrix.M33 = 1.0f;
                matrix.M33 = 1.0f;
                matrix.M30 = x;
                matrix.M31 = y;
                matrix.M32 = z;
                return matrix;
            }

            public static Mat4x4 Mat_MakeProjection(float fFovDegrees, float fAspectRatio, float fNear, float fFar)
            {
                float fFovRad = 1.0f / ((float)(Math.Tan(fFovDegrees * 0.5f / 180.0f * 3.14159f)));
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = fAspectRatio * fFovRad;
                matrix.M11 = fFovRad;
                matrix.M22 = fFar / (fFar - fNear);
                matrix.M32 = (-fFar * fNear) / (fFar - fNear);
                matrix.M23 = 1.0f;
                matrix.M33 = 0.0f;
                return matrix;
            }

            public static Mat4x4 Mat_PointAt(Vec3D pos, Vec3D target, Vec3D up)
            {
                // Calculate new forward direction
                Vec3D newForward = Vec_Sub(target, pos);
                newForward = Vec_Normalize(newForward);

                // Calculate new Up direction
                Vec3D a = Vec_Mul(newForward, Vec_DotProduct(up, newForward));
                Vec3D newUp = Vec_Sub(up, a);
                newUp = Vec_Normalize(newUp);

                // New Right direction is easy, its just cross product
                Vec3D newRight = Vec_CrossProduct(newUp, newForward);

                // Construct Dimensioning and Translation Matrix	
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = newRight.X; matrix.M01 = newRight.Y; matrix.M02 = newRight.Z; matrix.M03 = 0.0f;
                matrix.M10 = newUp.X; matrix.M11 = newUp.Y; matrix.M12 = newUp.Z; matrix.M13 = 0.0f;
                matrix.M20 = newForward.X; matrix.M21 = newForward.Y; matrix.M22 = newForward.Z; matrix.M23 = 0.0f;
                matrix.M30 = pos.X; matrix.M31 = pos.Y; matrix.M32 = pos.Z; matrix.M33 = 1.0f;
                return matrix;
            }

            public static Mat4x4 Mat_QuickInverse(Mat4x4 m)  // only for rotation / translation matrices
            {
                Mat4x4 matrix = new Mat4x4();
                matrix.M00 = m.M00; matrix.M01 = m.M10; matrix.M02 = m.M20; matrix.M03 = 0.0f;
                matrix.M10 = m.M01; matrix.M11 = m.M11; matrix.M12 = m.M21; matrix.M13 = 0.0f;
                matrix.M20 = m.M02; matrix.M21 = m.M12; matrix.M22 = m.M22; matrix.M23 = 0.0f;
                matrix.M30 = -(m.M30 * matrix.M00 + m.M31 * matrix.M10 + m.M32 * matrix.M20);
                matrix.M31 = -(m.M30 * matrix.M01 + m.M31 * matrix.M11 + m.M32 * matrix.M21);
                matrix.M32 = -(m.M30 * matrix.M02 + m.M31 * matrix.M12 + m.M32 * matrix.M22);
                matrix.M33 = 1.0f;
                return matrix;
            }

            public static Mat4x4 Mat_Inverse(Mat4x4 m)
            {
                double det;

                Mat4x4 matInv = new Mat4x4();

                matInv.m[0,0] =  m.m[1,1] * m.m[2,2] * m.m[3,3] - m.m[1,1] * m.m[2,3] * m.m[3,2] - m.m[2,1] * m.m[1,2] * m.m[3,3] + m.m[2,1] * m.m[1,3] * m.m[3,2] + m.m[3,1] * m.m[1,2] * m.m[2,3] - m.m[3,1] * m.m[1,3] * m.m[2,2];
                matInv.m[1,0] = -m.m[1,0] * m.m[2,2] * m.m[3,3] + m.m[1,0] * m.m[2,3] * m.m[3,2] + m.m[2,0] * m.m[1,2] * m.m[3,3] - m.m[2,0] * m.m[1,3] * m.m[3,2] - m.m[3,0] * m.m[1,2] * m.m[2,3] + m.m[3,0] * m.m[1,3] * m.m[2,2];
                matInv.m[2,0] =  m.m[1,0] * m.m[2,1] * m.m[3,3] - m.m[1,0] * m.m[2,3] * m.m[3,1] - m.m[2,0] * m.m[1,1] * m.m[3,3] + m.m[2,0] * m.m[1,3] * m.m[3,1] + m.m[3,0] * m.m[1,1] * m.m[2,3] - m.m[3,0] * m.m[1,3] * m.m[2,1];
                matInv.m[3,0] = -m.m[1,0] * m.m[2,1] * m.m[3,2] + m.m[1,0] * m.m[2,2] * m.m[3,1] + m.m[2,0] * m.m[1,1] * m.m[3,2] - m.m[2,0] * m.m[1,2] * m.m[3,1] - m.m[3,0] * m.m[1,1] * m.m[2,2] + m.m[3,0] * m.m[1,2] * m.m[2,1];
                matInv.m[0,1] = -m.m[0,1] * m.m[2,2] * m.m[3,3] + m.m[0,1] * m.m[2,3] * m.m[3,2] + m.m[2,1] * m.m[0,2] * m.m[3,3] - m.m[2,1] * m.m[0,3] * m.m[3,2] - m.m[3,1] * m.m[0,2] * m.m[2,3] + m.m[3,1] * m.m[0,3] * m.m[2,2];
                matInv.m[1,1] =  m.m[0,0] * m.m[2,2] * m.m[3,3] - m.m[0,0] * m.m[2,3] * m.m[3,2] - m.m[2,0] * m.m[0,2] * m.m[3,3] + m.m[2,0] * m.m[0,3] * m.m[3,2] + m.m[3,0] * m.m[0,2] * m.m[2,3] - m.m[3,0] * m.m[0,3] * m.m[2,2];
                matInv.m[2,1] = -m.m[0,0] * m.m[2,1] * m.m[3,3] + m.m[0,0] * m.m[2,3] * m.m[3,1] + m.m[2,0] * m.m[0,1] * m.m[3,3] - m.m[2,0] * m.m[0,3] * m.m[3,1] - m.m[3,0] * m.m[0,1] * m.m[2,3] + m.m[3,0] * m.m[0,3] * m.m[2,1];
                matInv.m[3,1] =  m.m[0,0] * m.m[2,1] * m.m[3,2] - m.m[0,0] * m.m[2,2] * m.m[3,1] - m.m[2,0] * m.m[0,1] * m.m[3,2] + m.m[2,0] * m.m[0,2] * m.m[3,1] + m.m[3,0] * m.m[0,1] * m.m[2,2] - m.m[3,0] * m.m[0,2] * m.m[2,1];
                matInv.m[0,2] =  m.m[0,1] * m.m[1,2] * m.m[3,3] - m.m[0,1] * m.m[1,3] * m.m[3,2] - m.m[1,1] * m.m[0,2] * m.m[3,3] + m.m[1,1] * m.m[0,3] * m.m[3,2] + m.m[3,1] * m.m[0,2] * m.m[1,3] - m.m[3,1] * m.m[0,3] * m.m[1,2];
                matInv.m[1,2] = -m.m[0,0] * m.m[1,2] * m.m[3,3] + m.m[0,0] * m.m[1,3] * m.m[3,2] + m.m[1,0] * m.m[0,2] * m.m[3,3] - m.m[1,0] * m.m[0,3] * m.m[3,2] - m.m[3,0] * m.m[0,2] * m.m[1,3] + m.m[3,0] * m.m[0,3] * m.m[1,2];
                matInv.m[2,2] =  m.m[0,0] * m.m[1,1] * m.m[3,3] - m.m[0,0] * m.m[1,3] * m.m[3,1] - m.m[1,0] * m.m[0,1] * m.m[3,3] + m.m[1,0] * m.m[0,3] * m.m[3,1] + m.m[3,0] * m.m[0,1] * m.m[1,3] - m.m[3,0] * m.m[0,3] * m.m[1,1];
                matInv.m[3,2] = -m.m[0,0] * m.m[1,1] * m.m[3,2] + m.m[0,0] * m.m[1,2] * m.m[3,1] + m.m[1,0] * m.m[0,1] * m.m[3,2] - m.m[1,0] * m.m[0,2] * m.m[3,1] - m.m[3,0] * m.m[0,1] * m.m[1,2] + m.m[3,0] * m.m[0,2] * m.m[1,1];
                matInv.m[0,3] = -m.m[0,1] * m.m[1,2] * m.m[2,3] + m.m[0,1] * m.m[1,3] * m.m[2,2] + m.m[1,1] * m.m[0,2] * m.m[2,3] - m.m[1,1] * m.m[0,3] * m.m[2,2] - m.m[2,1] * m.m[0,2] * m.m[1,3] + m.m[2,1] * m.m[0,3] * m.m[1,2];
                matInv.m[1,3] =  m.m[0,0] * m.m[1,2] * m.m[2,3] - m.m[0,0] * m.m[1,3] * m.m[2,2] - m.m[1,0] * m.m[0,2] * m.m[2,3] + m.m[1,0] * m.m[0,3] * m.m[2,2] + m.m[2,0] * m.m[0,2] * m.m[1,3] - m.m[2,0] * m.m[0,3] * m.m[1,2];
                matInv.m[2,3] = -m.m[0,0] * m.m[1,1] * m.m[2,3] + m.m[0,0] * m.m[1,3] * m.m[2,1] + m.m[1,0] * m.m[0,1] * m.m[2,3] - m.m[1,0] * m.m[0,3] * m.m[2,1] - m.m[2,0] * m.m[0,1] * m.m[1,3] + m.m[2,0] * m.m[0,3] * m.m[1,1];
                matInv.m[3,3] =  m.m[0,0] * m.m[1,1] * m.m[2,2] - m.m[0,0] * m.m[1,2] * m.m[2,1] - m.m[1,0] * m.m[0,1] * m.m[2,2] + m.m[1,0] * m.m[0,2] * m.m[2,1] + m.m[2,0] * m.m[0,1] * m.m[1,2] - m.m[2,0] * m.m[0,2] * m.m[1,1];

                det = m.m[0,0] * matInv.m[0,0] + m.m[0,1] * matInv.m[1,0] + m.m[0,2] * matInv.m[2,0] + m.m[0,3] * matInv.m[3,0];

                det = 1.0 / det;

                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        matInv.m[i,j] *= (float)det;

                return matInv;
            }

            public static Mat4x4 Mat_Transpose(Mat4x4 mat)
            {

                Mat4x4 matTranspose = new Mat4x4();

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        matTranspose.m[i, j] = mat.m[j, i];
                    }
                }
                return matTranspose;

            }

            public static Vec3D Vec_Add(Vec3D v1, Vec3D v2)
            {
                Vec3D temp = new Vec3D(0.0f, 0.0f, 0.0f);
                temp.X = v1.X + v2.X;
                temp.Y = v1.Y + v2.Y;
                temp.Z = v1.Z + v2.Z;
                temp.W = v1.W + v2.W;
                return temp;
            }

            public static Vec3D Vec_Sub(Vec3D v1, Vec3D v2)
            {
                Vec3D temp = new Vec3D(0.0f, 0.0f, 0.0f);
                temp.X = v1.X - v2.X;
                temp.Y = v1.Y - v2.Y;
                temp.Z = v1.Z - v2.Z;
                temp.W = v1.W - v2.W;
                return temp;
            }

            public static Vec3D Vec_Mul(Vec3D v1, float k)
            {
                Vec3D temp = new Vec3D(0.0f, 0.0f, 0.0f);
                temp.X = v1.X * k;
                temp.Y = v1.Y * k;
                temp.Z = v1.Z * k;
                temp.W = v1.W * k;
                return temp;
            }

            public static Vec3D Vec_Div(Vec3D v1, float k)
            {
                Vec3D temp = new Vec3D(0.0f, 0.0f, 0.0f);
                temp.X = v1.X / k;
                temp.Y = v1.Y / k;
                temp.Z = v1.Z / k;
                temp.W = v1.W / k;
                return temp;
            }

            public static float Vec_DotProduct(Vec3D v1, Vec3D v2)
            {
                return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            }

            public static float Vec_Length(Vec3D v)
            {
                return (float)Math.Sqrt(Vec_DotProduct(v, v));
            }

            public static Vec3D Vec_Normalize(Vec3D v)
            {
                Vec3D temp = new Vec3D(0.0f, 0.0f, 0.0f);
                float l = Vec_Length(v);
                temp.X = v.X / l;
                temp.Y = v.Y / l;
                temp.Z = v.Z / l;
                return temp;
            }

            public static Vec3D Vec_CrossProduct(Vec3D v1, Vec3D v2)
            {
                Vec3D v = new Vec3D(0.0f, 0.0f, 0.0f);
                v.X = v1.Y * v2.Z - v1.Z * v2.Y;
                v.Y = v1.Z * v2.X - v1.X * v2.Z;
                v.Z = v1.X * v2.Y - v1.Y * v2.X;
                return v;
            }

            public static Vec3D Vec_IntersectPlane(Vec3D plane_p, Vec3D plane_n, Vec3D lineStart, Vec3D lineEnd, out float t)
            {
                plane_n = Vec_Normalize(plane_n);
                float plane_d = -Vec_DotProduct(plane_n, plane_p);
                float ad = Vec_DotProduct(lineStart, plane_n);
                float bd = Vec_DotProduct(lineEnd, plane_n);

                // TODO::  This blows up if bd = ad
                t = (-plane_d - ad) / (bd - ad);

                Vec3D lineStartToEnd = Vec_Sub(lineEnd, lineStart);
                Vec3D lineToIntersect = Vec_Mul(lineStartToEnd, t);
                return Vec_Add(lineStart, lineToIntersect);
            }

            private static float dist(Vec3D plane_p, Vec3D plane_n, Vec3D p)
            {
                Vec3D n = MathOps.Vec_Normalize(p);
                return (plane_n.X * p.X + plane_n.Y * p.Y + plane_n.Z * p.Z - MathOps.Vec_DotProduct(plane_n, plane_p));

            }

            /// <summary>
            /// Function to return triangle points that are clipped by a plane.
            /// </summary>
            /// <param name="plane_p"></param>
            /// <param name="plane_n"></param>
            /// <param name="in_tri"></param>
            /// <param name="out_tri1"></param>
            /// <param name="out_tri2"></param>
            /// <returns></returns>
            public static int Triangle_ClipAgainstPlane(Vec3D plane_p, Vec3D plane_n, TriangleObject in_tri, ref TriangleObject out_tri1, ref TriangleObject out_tri2)
            {
                // Make sure plane normal is indeed normal
                plane_n = Vec_Normalize(plane_n);

                out_tri1.t[0] = in_tri.t[0];
                out_tri2.t[0] = in_tri.t[0];
                out_tri1.t[1] = in_tri.t[1];
                out_tri2.t[1] = in_tri.t[1];
                out_tri1.t[2] = in_tri.t[2];
                out_tri2.t[2] = in_tri.t[2];

                out_tri1.col[0] = in_tri.col[0];
                out_tri2.col[0] = in_tri.col[0];
                out_tri1.col[1] = in_tri.col[1];
                out_tri2.col[1] = in_tri.col[1];
                out_tri1.col[2] = in_tri.col[2];
                out_tri2.col[2] = in_tri.col[2];

                // Return signed shortest distance from point to plane, plane normal must be normalised
                float d0 = dist(plane_p, plane_n, in_tri.p[0]);
                float d1 = dist(plane_p, plane_n, in_tri.p[1]);
                float d2 = dist(plane_p, plane_n, in_tri.p[2]);

                Console.WriteLine("d0: " + d0.ToString() + "  d1: " + d1.ToString() + "  d2: " + d2.ToString());


                Vec3D[] inside_pts = new Vec3D[3];

                // Create two temporary storage arrays to classify points either side of plane
                // If distance sign is positive, point lies on "inside" of plane
                Vec3D[] inside_points = new Vec3D[3];  int nInsidePointCount = 0;
                Vec3D[] outside_points = new Vec3D[3]; int nOutsidePointCount = 0;
                Vec2D[] inside_tex = new Vec2D[3]; int nInsideTexCount = 0;
                Vec2D[] outside_tex = new Vec2D[3]; int nOutsideTexCount = 0;
                Pixel[] inside_color = new Pixel[3]; int nInsideColorCount = 0;
                Pixel[] outside_color = new Pixel[3]; int nOutsideColorCount = 0;


                if (d0 >= 0) { 
                    inside_points[nInsidePointCount++] = in_tri.p[0]; 
                    inside_tex[nInsideTexCount++] = in_tri.t[0];
                    inside_color[nInsideColorCount++] = in_tri.col[0];
                }
                else
                {
                    outside_points[nOutsidePointCount++] = in_tri.p[0]; 
                    outside_tex[nOutsideTexCount++] = in_tri.t[0];
                    outside_color[nOutsideColorCount++] = in_tri.col[0];

                }
                if (d1 >= 0)
                {
                    inside_points[nInsidePointCount++] = in_tri.p[1]; 
                    inside_tex[nInsideTexCount++] = in_tri.t[1];
                    inside_color[nInsideColorCount++] = in_tri.col[1];
                }
                else
                {
                    outside_points[nOutsidePointCount++] = in_tri.p[1]; 
                    outside_tex[nOutsideTexCount++] = in_tri.t[1];
                    outside_color[nOutsideColorCount++] = in_tri.col[1];
                }
                if (d2 >= 0)
                {
                    inside_points[nInsidePointCount++] = in_tri.p[2]; 
                    inside_tex[nInsideTexCount++] = in_tri.t[2];
                    inside_color[nInsideColorCount++] = in_tri.col[2];
                }
                else
                {
                    outside_points[nOutsidePointCount++] = in_tri.p[2]; 
                    outside_tex[nOutsideTexCount++] = in_tri.t[2];
                    outside_color[nOutsideColorCount++] = in_tri.col[2];
                }

                // Now classify triangle points, and break the input triangle into 
                // smaller output triangles if required. There are four possible
                // outcomes...

                if (nInsidePointCount == 0)
                {
                    Console.WriteLine("Triangle outside view area -- fully clipped");
                    // All points lie on the outside of plane, so clip whole triangle
                    // It ceases to exist

                    return 0; // No returned triangles are valid
                }

                if (nInsidePointCount == 3)
                {
                    Console.WriteLine("No clipping");

                    // All points lie on the inside of plane, so do nothing
                    // and allow the triangle to simply pass through
                    out_tri1 = in_tri;

                    return 1; // Just the one returned original triangle is valid
                }

                if (nInsidePointCount == 1 && nOutsidePointCount == 2)
                {
                    Console.WriteLine("Two points clipped");

                    // Triangle should be clipped. As two points lie outside
                    // the plane, the triangle simply becomes a smaller triangle

                    // Copy appearance info to new triangle
                    out_tri1.col[0] = in_tri.col[0];
                    out_tri1.col[1] = in_tri.col[1];
                    out_tri1.col[2] = in_tri.col[2];
                    out_tri2.col[0] = in_tri.col[0];
                    out_tri2.col[1] = in_tri.col[1];
                    out_tri2.col[2] = in_tri.col[2];

                    // The inside point is valid, so keep that...
                    out_tri1.p[0] = inside_points[0];
                    out_tri1.t[0] = inside_tex[0];
                    out_tri1.col[0] = inside_color[0];

                    // but the two new points are at the locations where the 
                    // original sides of the triangle (lines) intersect with the plane
                    
                    float t;
                    out_tri1.p[1] = Vec_IntersectPlane(plane_p, plane_n, inside_points[0], outside_points[0], out t);
                    out_tri1.t[1].X = t * (outside_tex[0].X - inside_tex[0].X) + inside_tex[0].X;
                    out_tri1.t[1].Y = t * (outside_tex[0].Y - inside_tex[0].Y) + inside_tex[0].Y;
                    out_tri1.t[1].Z = t * (outside_tex[0].Z - inside_tex[0].Z) + inside_tex[0].Z;

                    // Inteprolate the color of the 1st point
                    out_tri1.col[1] = ToPixel(InterpolateColors(t, ToBrush(outside_color[0]), ToBrush(inside_color[0])));

                    out_tri1.p[2] = Vec_IntersectPlane(plane_p, plane_n, inside_points[0], outside_points[1], out t);
                    out_tri1.t[2].X = t * (outside_tex[1].X - inside_tex[0].X) + inside_tex[0].X;
                    out_tri1.t[2].Y = t * (outside_tex[1].Y - inside_tex[0].Y) + inside_tex[0].Y;
                    out_tri1.t[2].Z = t * (outside_tex[1].Z - inside_tex[0].Z) + inside_tex[0].Z;

                    // Interpolate the color of the 2nd point
                    out_tri1.col[2] = ToPixel(InterpolateColors(t, ToBrush(outside_color[1]), ToBrush(inside_color[0])));

                    return 1; // Return the newly formed single triangle
                }

                if (nInsidePointCount == 2 && nOutsidePointCount == 1)
                {
                    Console.WriteLine("One point clipped -- quad created");

                    // Triangle should be clipped. As two points lie inside the plane,
                    // the clipped triangle becomes a "quad". Fortunately, we can
                    // represent a quad with two new triangles

                    // Copy appearance info to new triangles
                    out_tri1.col[0] = in_tri.col[0];
                    out_tri2.col[0] = in_tri.col[0];
                    out_tri1.col[1] = in_tri.col[1];
                    out_tri2.col[1] = in_tri.col[1];
                    out_tri1.col[2] = in_tri.col[2];
                    out_tri2.col[2] = in_tri.col[2];

                    // The first triangle consists of the two inside points and a new
                    // point determined by the location where one side of the triangle
                    // intersects with the plane
                    out_tri1.p[0] = inside_points[0];
                    out_tri1.t[0] = inside_tex[0];
                    out_tri1.col[0] = inside_color[0];

                    out_tri1.p[1] = inside_points[1];
                    out_tri1.t[1] = inside_tex[1];
                    out_tri1.col[1] = inside_color[1];

                    float t;
                    out_tri1.p[2] = Vec_IntersectPlane(plane_p, plane_n, inside_points[0], outside_points[0], out t);
                    out_tri1.t[2].X = t * (outside_tex[0].X - inside_tex[0].X) + inside_tex[0].X;
                    out_tri1.t[2].Y = t * (outside_tex[0].Y - inside_tex[0].Y) + inside_tex[0].Y;
                    out_tri1.t[2].Z = t * (outside_tex[0].Z - inside_tex[0].Z) + inside_tex[0].Z;

                    // Interpolate the color of the 2nd point
                    out_tri1.col[2] = ToPixel(InterpolateColors(t, ToBrush(outside_color[0]), ToBrush(inside_color[0])));

                    // The second triangle is composed of one of the inside points, a
                    // new point determined by the intersection of the other side of the 
                    // triangle and the plane, and the newly created point above
                    out_tri2.p[1] = inside_points[1];
                    out_tri2.t[1] = inside_tex[1];
                    out_tri2.col[1] = inside_color[1];

                    out_tri2.p[0] = out_tri1.p[2];
                    out_tri2.t[0] = out_tri1.t[2];
                    out_tri2.col[0] = out_tri1.col[2];

                    out_tri2.p[2] = Vec_IntersectPlane(plane_p, plane_n, inside_points[1], outside_points[0], out t);
                    out_tri2.t[2].X = t * (outside_tex[0].X - inside_tex[1].X) + inside_tex[1].X;
                    out_tri2.t[2].Y = t * (outside_tex[0].Y - inside_tex[1].Y) + inside_tex[1].Y;
                    out_tri2.t[2].Z = t * (outside_tex[0].Z - inside_tex[1].Z) + inside_tex[1].Z;

                    // Interpolate the color of the 2nd point
                    out_tri2.col[2] = ToPixel(InterpolateColors(t, ToBrush(outside_color[0]), ToBrush(inside_color[1])));

                    return 2; // Return two newly formed triangles which form a quad
                }

                return 0;
            }

            /// <summary>
            /// Function to return the points where a line object is clipped by a plane.
            /// </summary>
            /// <param name="plane_p"></param>
            /// <param name="plane_n"></param>
            /// <param name="in_line"></param>
            /// <param name="out_line"></param>
            /// <returns></returns>
            public static int Line_ClipAgainstPlane(Vec3D plane_p, Vec3D plane_n, LineObject in_line, ref LineObject out_line)
            {
                // Make sure plane normal is indeed normal
                plane_n = Vec_Normalize(plane_n);

                out_line.col[0] = in_line.col[0];
                out_line.col[1] = in_line.col[1];

                // Return signed shortest distance from point to plane, plane normal must be normalised
                float d0 = dist(plane_p, plane_n, in_line.p[0]);
                float d1 = dist(plane_p, plane_n, in_line.p[1]);

                Console.WriteLine("d0: " + d0.ToString() + "  d1: " + d1.ToString());

                Vec3D[] inside_pts = new Vec3D[3];

                // Create two temporary storage arrays to classify points either side of plane
                // If distance sign is positive, point lies on "inside" of plane
                Vec3D[] inside_points = new Vec3D[3]; int nInsidePointCount = 0;
                Vec3D[] outside_points = new Vec3D[3]; int nOutsidePointCount = 0;
                Pixel[] inside_color = new Pixel[3]; int nInsideColorCount = 0;
                Pixel[] outside_color = new Pixel[3]; int nOutsideColorCount = 0;


                // For the start point
                if (d0 >= 0)
                {
                    inside_points[nInsidePointCount++] = in_line.p[0];
                    inside_color[nInsideColorCount++] = in_line.col[0];
                }
                else
                {
                    outside_points[nOutsidePointCount++] = in_line.p[0];
                    outside_color[nOutsideColorCount++] = in_line.col[0];

                }

               // For the end point
                if (d1 >= 0)
                {
                    inside_points[nInsidePointCount++] = in_line.p[1];
                    inside_color[nInsideColorCount++] = in_line.col[1];
                }
                else
                {
                    outside_points[nOutsidePointCount++] = in_line.p[1];
                    outside_color[nOutsideColorCount++] = in_line.col[1];
                }

                // Now classify line points, 

                if (nInsidePointCount == 0)
                {
                    Console.WriteLine("Line outside view area -- fully clipped");

                    // All points lie on the outside of plane, so clip whole triangle
                    // It ceases to exist

                    return 0; // No returned triangles are valid
                }

                if (nInsidePointCount == 2)
                {
                    Console.WriteLine("No clipping");

                    // All points lie on the inside of plane, so do nothing
                    // and allow the triangle to simply pass through
                    out_line = in_line;

                    return 1; // Just the one returned original triangle is valid
                }

                if (nInsidePointCount == 1 && nOutsidePointCount == 1)
                {
                    Console.WriteLine("One points clipped");

                    // Triangle should be clipped. As two points lie outside
                    // the plane, the triangle simply becomes a smaller triangle

                    // Copy appearance info to new triangle
                    out_line.col[0] = in_line.col[0];
                    out_line.col[1] = in_line.col[1];

                    // The inside point is valid, so keep that...
                    out_line.p[0] = inside_points[0];
                    out_line.col[0] = inside_color[0];

                    // but the second new point is at the locations where the 
                    // original line intersects with the plane

                    float t;
                    out_line.p[1] = Vec_IntersectPlane(plane_p, plane_n, inside_points[0], outside_points[0], out t);

                    // Interpolate the color of the 1st point
                    out_line.col[1] = ToPixel(InterpolateColors(t, ToBrush(outside_color[0]), ToBrush(inside_color[0])));
                    return 1; // Return the newly formed line
                }

                return 0;
            }
        }

        /// <summary>
        /// Interpolates colors based on a percentile where fill2 location is 0% and fill1 location is 100%
        /// </summary>
        /// <param name="p">percentile measured from 2nd point</param>
        /// <param name="fill1">1st point color</param>
        /// <param name="fill2">2nd point color</param>
        /// <returns></returns>
        public static SolidColorBrush InterpolateColors(float p, Brush fill1, Brush fill2)
        {
            SolidColorBrush col;

            if (fill2 != fill1)
            {
                // RGBA colors for our two vertices
                float r1 = ((SolidColorBrush)fill1).Color.R;
                float r2 = ((SolidColorBrush)fill2).Color.R;
                float g1 = ((SolidColorBrush)fill1).Color.G;
                float g2 = ((SolidColorBrush)fill2).Color.G;
                float b1 = ((SolidColorBrush)fill1).Color.B;
                float b2 = ((SolidColorBrush)fill2).Color.B;
                float a1 = ((SolidColorBrush)fill1).Color.A;
                float a2 = ((SolidColorBrush)fill2).Color.A;

                // Interpolate the colors
                float c23_r = (float)Math.Round(r2 - p * (r2 - r1));
                float c23_g = (float)Math.Round(g2 - p * (g2 - g1));
                float c23_b = (float)Math.Round(b2 - p * (b2 - b1));
                float c23_a = (float)Math.Round(a2 - p * (a2 - a1));

                col = new SolidColorBrush(Color.FromArgb((byte)c23_a, (byte)c23_r, (byte)c23_g, (byte)c23_b));
            }
            else
            {
                col = (SolidColorBrush)fill1;
            }

            return col;
        }

        public static Brush ToBrush(Pixel p)
        {
            return new SolidColorBrush(Color.FromArgb((byte)p.a, (byte)p.r, (byte)p.g, (byte)p.b));
        }

        public static Pixel ToPixel(SolidColorBrush b)
        {
            return new Pixel(b.Color.R, b.Color.G, b.Color.B, b.Color.A);
        }
    }
}
