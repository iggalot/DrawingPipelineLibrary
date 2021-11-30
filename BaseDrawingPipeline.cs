using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static MathLibrary.MathVectors;

namespace DrawingPipeline
{
    public class BaseDrawingPipeline : IDisposable
    {
        private static float[] m_DepthBuffer;

        private Mat4x4 matProj = new Mat4x4();
        private Mat4x4 matView = new Mat4x4();
        private Mat4x4 matWorld = new Mat4x4();

        private float fViewX;
        private float fViewY;
        private float fViewW;
        private float fViewH;

        private sLight[] lights = new sLight[4];

        public virtual void RunPipeline() { }

        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 100.0f;

        public float Width { get; set; } = 800.0f;
        public float Height { get; set; } = 800.0f;

        public Mat4x4 GetMatProj { get => matProj; set { matProj = value; } }
        public Mat4x4 GetMatView { get => matView; set { matView = value; } }
        public Mat4x4 GetMatWorld { get => matWorld; set { matWorld = value; } }
        public sLight[] GetLights { get => lights; set { lights = value; } }


        [Flags]
        public enum RENDERFLAGS
        {
            RENDER_WIRE = 0x01,
            RENDER_FLAT = 0x02,
            RENDER_TEXTURED = 0x04,
            RENDER_CULL_CW = 0x08,
            RENDER_CULL_CCW = 0x16,
            RENDER_DEPTH = 0x32,
            RENDER_LIGHTS = 0x64,
        }

        [Flags]
        public enum LightsType
        {
            LIGHT_DISABLED,
            LIGHT_AMBIENT,
            LIGHT_DIRECTIONAL,
            LIGHT_POINT
        }

        public class sLight
        {
            public LightsType type;
            public Vec3D pos;
            public Vec3D dir;
            public Pixel col;
            public float param;
        }



        Pixel PixelF(float red, float green, float blue, float alpha = 1.0f)
        {

            return new Pixel((uint)red, (uint)green, (uint)blue, (uint)alpha);
        }

        // Constants
        public readonly Pixel GREY = new Pixel(192, 192, 192);
        public readonly Pixel DARK_GREY = new Pixel(128, 128, 128);
        public readonly Pixel VERY_DARK_GREY = new Pixel(64, 64, 64);
        public readonly Pixel RED = new Pixel(255, 0, 0);
        public readonly Pixel DARK_RED = new Pixel(128, 0, 0);
        public readonly Pixel VERY_DARK_RED = new Pixel(64, 0, 0);
        public readonly Pixel YELLOW = new Pixel(255, 255, 0);
        public readonly Pixel DARK_YELLOW = new Pixel(128, 128, 0);
        public readonly Pixel VERY_DARK_YELLOW = new Pixel(64, 64, 0);
        public readonly Pixel GREEN = new Pixel(0, 255, 0);
        public readonly Pixel DARK_GREEN = new Pixel(0, 128, 0);
        public readonly Pixel VERY_DARK_GREEN = new Pixel(0, 64, 0);
        public readonly Pixel CYAN = new Pixel(0, 255, 255);
        public readonly Pixel DARK_CYAN = new Pixel(0, 128, 128);
        public readonly Pixel VERY_DARK_CYAN = new Pixel(0, 64, 64);
        public readonly Pixel BLUE = new Pixel(0, 0, 255);
        public readonly Pixel DARK_BLUE = new Pixel(0, 0, 128);
        public readonly Pixel VERY_DARK_BLUE = new Pixel(0, 0, 64);
        public readonly Pixel MAGENTA = new Pixel(255, 0, 255);
        public readonly Pixel DARK_MAGENTA = new Pixel(128, 0, 128);
        public readonly Pixel VERY_DARK_MAGENTA = new Pixel(64, 0, 64);
        public readonly Pixel WHITE = new Pixel(255, 255, 255);
        public readonly Pixel BLACK = new Pixel(0, 0, 0);
        public readonly Pixel BLANK = new Pixel(0, 0, 0, 0);


        public virtual void Dispose() { }



        public virtual LineObject RasterSingleLine(LineObject lineRaster, RENDERFLAGS flags)
        {
            // Scale to viewport
            Vec3D vOffsetView = new Vec3D(1, 1, 0);
            lineRaster.p[0] = MathOps.Vec_Add(lineRaster.p[0], vOffsetView);
            lineRaster.p[1] = MathOps.Vec_Add(lineRaster.p[1], vOffsetView);
            lineRaster.p[0].X *= 0.5f * fViewW;
            lineRaster.p[0].Y *= 0.5f * fViewH;
            lineRaster.p[1].X *= 0.5f * fViewW;
            lineRaster.p[1].Y *= 0.5f * fViewH;
            vOffsetView = new Vec3D(fViewX, fViewY, 0);
            lineRaster.p[0] = MathOps.Vec_Add(lineRaster.p[0], vOffsetView);
            lineRaster.p[1] = MathOps.Vec_Add(lineRaster.p[1], vOffsetView);

            return lineRaster;
        }

        public virtual TriangleObject RasterSingleTriangle(TriangleObject triRaster, RENDERFLAGS flags)
        {
            // Scale to viewport
            Vec3D vOffsetView = new Vec3D(1, 1, 0);
            triRaster.p[0] = MathOps.Vec_Add(triRaster.p[0], vOffsetView);
            triRaster.p[1] = MathOps.Vec_Add(triRaster.p[1], vOffsetView);
            triRaster.p[2] = MathOps.Vec_Add(triRaster.p[2], vOffsetView);
            triRaster.p[0].X *= 0.5f * fViewW;
            triRaster.p[0].Y *= 0.5f * fViewH;
            triRaster.p[1].X *= 0.5f * fViewW;
            triRaster.p[1].Y *= 0.5f * fViewH;
            triRaster.p[2].X *= 0.5f * fViewW;
            triRaster.p[2].Y *= 0.5f * fViewH;
            vOffsetView = new Vec3D(fViewX, fViewY, 0);
            triRaster.p[0] = MathOps.Vec_Add(triRaster.p[0], vOffsetView);
            triRaster.p[1] = MathOps.Vec_Add(triRaster.p[1], vOffsetView);
            triRaster.p[2] = MathOps.Vec_Add(triRaster.p[2], vOffsetView);


            //if (flags & RENDER_TEXTURED)
            //{/*
            //	TexturedTriangle(
            //		triRaster.p[0].x, triRaster.p[0].y, triRaster.t[0].x, triRaster.t[0].y, triRaster.t[0].z,
            //		triRaster.p[1].x, triRaster.p[1].y, triRaster.t[1].x, triRaster.t[1].y, triRaster.t[1].z,
            //		triRaster.p[2].x, triRaster.p[2].y, triRaster.t[2].x, triRaster.t[2].y, triRaster.t[2].z,
            //		sprTexture);*/

            //	RasterTriangle(
            //		triRaster.p[0].x, triRaster.p[0].y, triRaster.t[0].x, triRaster.t[0].y, triRaster.t[0].z, triRaster.col,
            //		triRaster.p[1].x, triRaster.p[1].y, triRaster.t[1].x, triRaster.t[1].y, triRaster.t[1].z, triRaster.col,
            //		triRaster.p[2].x, triRaster.p[2].y, triRaster.t[2].x, triRaster.t[2].y, triRaster.t[2].z, triRaster.col,
            //		sprTexture, nFlags);

            //}
            return triRaster;
        }

        public virtual int Update(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags) { return -1; }

        public virtual int Render(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags)
        {
            int itemsDrawn = 0;
            // Draw triangle objects
            itemsDrawn += RenderTriangle(TriangleList, flags, 0, TriangleList.Count);

            // Draw the line objects
            itemsDrawn += RenderLine(LineList, flags, 0, LineList.Count);

            Console.WriteLine(itemsDrawn.ToString() + " items drawn.");
            return itemsDrawn = 0;
        }
        public BaseDrawingPipeline()
        {
            matProj = MathOps.Mat_MakeIdentity();
            matWorld = MathOps.Mat_MakeIdentity();
            matView = MathOps.Mat_MakeIdentity();
        }
        public int RenderLine(List<LineObject> lines, RENDERFLAGS flags = RENDERFLAGS.RENDER_DEPTH)
        {
            return RenderLine(lines, flags, 0, lines.Count);
        }

        public int RenderLine(List<LineObject> lines, RENDERFLAGS flags, int nOffset, int nCount)
        {
            Console.WriteLine(lines.Count.ToString() + " lines were sent to render!");

            // Calculate the Transformation Matrix
            Mat4x4 matWorldView = MathOps.Mat_MultiplyMatrix(matWorld, matView);

            int nLineDrawnCount = 0;

            // Process Lines
            for (int tx = 0; tx < lines.Count; tx++)
            {
                LineObject line = lines[tx];
                LineObject lineTransformed = new LineObject();

                // Copy end point colors
                lineTransformed.col[0] = line.col[0];
                lineTransformed.col[1] = line.col[1];

                // Transform line from object (world) space into projected space
                lineTransformed.p[0] = MathOps.Mat_MultiplyVector(matWorldView, line.p[0]);
                lineTransformed.p[1] = MathOps.Mat_MultiplyVector(matWorldView, line.p[1]);

                // Now clip the boundaries
                nLineDrawnCount += ClipLine_ToViewportBoundary(lineTransformed, flags);
            }

            return nLineDrawnCount;
        }

        public int RenderTriangle(List<TriangleObject> triangles, RENDERFLAGS flags = RENDERFLAGS.RENDER_CULL_CW | RENDERFLAGS.RENDER_TEXTURED | RENDERFLAGS.RENDER_DEPTH)
        {
            return RenderTriangle(triangles, flags, 0, triangles.Count);
        }

        public int RenderTriangle(List<TriangleObject> triangles, RENDERFLAGS flags, int nOffset, int nCount)
        {
            Console.WriteLine(triangles.Count.ToString() + " triangles were sent to Render!");

            // Calculate Transformation Matrix
            Mat4x4 matWorldView = MathOps.Mat_MultiplyMatrix(GetMatWorld, GetMatView);

            int nTriangleDrawnCount = 0;

            //Process Triangles
            for (int tx = 0; tx < triangles.Count; tx++)
            {
                TriangleObject tri = triangles[tx];
                TriangleObject triTransformed = new TriangleObject();

                // Just copy through texture coordinates
                triTransformed.t[0] = tri.t[0];
                triTransformed.t[1] = tri.t[1];
                triTransformed.t[2] = tri.t[2];

                // Copy Vertex colors
                triTransformed.col[0] = tri.col[0];
                triTransformed.col[1] = tri.col[1];
                triTransformed.col[2] = tri.col[2];

                // Transform triangle from object (world) space into projected space
                triTransformed.p[0] = MathOps.Mat_MultiplyVector(matWorldView, tri.p[0]);
                triTransformed.p[1] = MathOps.Mat_MultiplyVector(matWorldView, tri.p[1]);
                triTransformed.p[2] = MathOps.Mat_MultiplyVector(matWorldView, tri.p[2]);

                // Calculate triangle normal in WorldView SPace
                Vec3D normal, line1, line2;
                line1 = MathOps.Vec_Sub(triTransformed.p[1], triTransformed.p[0]);
                line2 = MathOps.Vec_Sub(triTransformed.p[2], triTransformed.p[0]);
                normal = MathOps.Vec_CrossProduct(line1, line2);
                normal = MathOps.Vec_Normalize(normal);

                // Cull triangles that face away from viewer
                if (((flags & RENDERFLAGS.RENDER_CULL_CW) == flags) && (MathOps.Vec_DotProduct(normal, triTransformed.p[0]) > 0.0f)) continue;
                if (((flags & RENDERFLAGS.RENDER_CULL_CCW) == flags) && (MathOps.Vec_DotProduct(normal, triTransformed.p[0]) < 0.0f)) continue;

                // If lighting, calculate shading
                if ((flags & RENDERFLAGS.RENDER_LIGHTS) == flags)
                {
                    Pixel ambient_clamp = new Pixel(0, 0, 0, 1);
                    Pixel light_combined = new Pixel(0, 0, 0, 1);
                    int nLightSources = 0;
                    float nLightR = 0;
                    float nLightG = 0;
                    float nLightB = 0;

                    for (int i = 0; i < 4; i++)
                    {
                        switch (GetLights[i].type)
                        {
                            case LightsType.LIGHT_DISABLED:
                                break;
                            case LightsType.LIGHT_AMBIENT:
                                ambient_clamp = GetLights[i].col;
                                break;
                            case LightsType.LIGHT_DIRECTIONAL:
                                {
                                    nLightSources++;
                                    Vec3D light_dir = MathOps.Vec_Normalize(GetLights[i].dir);
                                    float light = MathOps.Vec_DotProduct(light_dir, normal);

                                    light = Math.Max(light, 0.0f);
                                    nLightR += light * (GetLights[i].col.r / 255.0f);
                                    nLightG += light * (GetLights[i].col.g / 255.0f);
                                    nLightB += light * (GetLights[i].col.b / 255.0f);
                                }
                                break;
                            case LightsType.LIGHT_POINT:
                                break;
                        }
                    }

                    nLightR = Math.Max(nLightR, ambient_clamp.r / 255.0f);
                    nLightG = Math.Max(nLightG, ambient_clamp.g / 255.0f);
                    nLightB = Math.Max(nLightB, ambient_clamp.b / 255.0f);

                    triTransformed.col[0] = new Pixel((uint)(nLightR * triTransformed.col[0].r), (uint)(nLightG * triTransformed.col[0].g), (uint)(nLightB * triTransformed.col[0].b));
                    triTransformed.col[1] = new Pixel((uint)(nLightR * triTransformed.col[1].r), (uint)(nLightG * triTransformed.col[1].g), (uint)(nLightB * triTransformed.col[1].b));
                    triTransformed.col[2] = new Pixel((uint)(nLightR * triTransformed.col[2].r), (uint)(nLightG * triTransformed.col[2].g), (uint)(nLightB * triTransformed.col[2].b));
                }

                // Now clip the boundaries
                nTriangleDrawnCount += ClipTriangle_ToViewportBoundary(triTransformed, flags);
            }

            return nTriangleDrawnCount;
        }





        public void SetProjection(float fFovDegrees, float fAspectRatio, float fNear, float fFar, float fLeft, float fTop, float fWidth, float fHeight)
        {
            matProj = MathOps.Mat_MakeProjection(fFovDegrees, fAspectRatio, fNear, fFar);
            fViewX = fLeft;
            fViewY = fTop;
            fViewW = fWidth;
            fViewH = fHeight;

            NearPlane = fNear;
            FarPlane = fFar;
        }

        public void SetCamera(Vec3D pos, Vec3D lookat, Vec3D up)
        {
            matView = MathOps.Mat_PointAt(pos, lookat, up);
            matView = MathOps.Mat_QuickInverse(matView);
        }

        public void SetTransform(Mat4x4 transform)
        {
            matWorld = transform;
        }

        private int ClipLine_ToViewportBoundary(LineObject line, RENDERFLAGS flags)
        {
            int nLineDrawnCount = 0;
            // Clip line against near plane
            int nClippedLines = 0;
            LineObject clipped = new LineObject();

            // The n_plane should be the Z-plane of the screen <0,0,1>
            nClippedLines = MathOps.Line_ClipAgainstPlane(new Vec3D(0.0f, 0.0f, NearPlane), new Vec3D(0.0f, 0.0f, 1.0f), line, ref clipped);

            for (int n = 0; n < nClippedLines; n++)
            {
                LineObject lineProjected = clipped;

                // Project the new lines end points
                lineProjected.p[0] = MathOps.Mat_MultiplyVector(matProj, clipped.p[0]);
                lineProjected.p[1] = MathOps.Mat_MultiplyVector(matProj, clipped.p[1]);

                // Apply Projection to end points and scale to homogeneous units (dividing by W coeff) to get screen space coordinates
                lineProjected.p[0].X = lineProjected.p[0].X / lineProjected.p[0].W;
                lineProjected.p[1].X = lineProjected.p[1].X / lineProjected.p[1].W;

                lineProjected.p[0].Y = lineProjected.p[0].Y / lineProjected.p[0].W;
                lineProjected.p[1].Y = lineProjected.p[1].Y / lineProjected.p[1].W;

                lineProjected.p[0].Z = lineProjected.p[0].Z / lineProjected.p[0].W;
                lineProjected.p[1].Z = lineProjected.p[1].Z / lineProjected.p[1].W;

                // Clip against viewport in screen space
                // Clip line against all four screen edges
                LineObject sclipped = new LineObject();
                List<LineObject> listLines = new List<LineObject>();

                // Add initial triangle
                listLines.Add(lineProjected);

                int nNewLines = 1;

                for (int p = 0; p < 4; p++)
                {
                    int nLinesToAdd = 0;
                    while (nNewLines > 0)
                    {
                        sclipped = new LineObject();

                        // Take triangle from front of queue
                        LineObject test = listLines[0];

                        // remove it from the queue and subtract from total in the queue
                        listLines.RemoveAt(0);
                        nNewLines--;

                        // Clip it against a plane. We only need to test each 
                        // subsequent plane, against subsequent new triangles
                        // as all triangles after a plane clip are guaranteed
                        // to lie on the inside of the plane. I like how this
                        // comment is almost completely and utterly justified

                        switch (p)
                        {
                            case 0:
                                {
                                    nLinesToAdd = MathOps.Line_ClipAgainstPlane(new Vec3D(0.0f, -1.0f, 0.0f), new Vec3D(0.0f, 1.0f, 0.0f), test, ref sclipped);
                                    break;
                                }
                            case 1:
                                {
                                    nLinesToAdd = MathOps.Line_ClipAgainstPlane(new Vec3D(0.0f, +1.0f, 0.0f), new Vec3D(0.0f, -1.0f, 0.0f), test, ref sclipped);
                                    break;
                                }
                            case 2:
                                {
                                    nLinesToAdd = MathOps.Line_ClipAgainstPlane(new Vec3D(-1.0f, 0.0f, 0.0f), new Vec3D(1.0f, 0.0f, 0.0f), test, ref sclipped);
                                    break;
                                }
                            case 3:
                                {
                                    nLinesToAdd = MathOps.Line_ClipAgainstPlane(new Vec3D(+1.0f, 0.0f, 0.0f), new Vec3D(-1.0f, 0.0f, 0.0f), test, ref sclipped);
                                    //listLines.Add(sclipped);
                                    //RasterLines(listLines, flags);

                                    break;
                                }
                        }

                        // Add the newly clipped line to the end of the list for the next pass
                        for (int w = 0; w < nLinesToAdd; w++)
                        {
                            listLines.Add(sclipped);
                        }
                    }

                    nNewLines = listLines.Count;
                }

                RasterLines(listLines, flags);
                nLineDrawnCount++;

            }

            return nLineDrawnCount;
        }




        /// <summary>
        /// Clip the drawing objects to our viewport boundary
        ///                         //
        // JHA:  Let y be vertical, and x be horizontal.  Calcs assume a bounding box with coordinates as shown:
        // 
        //          (-1, 1, 0) ------------------------------------- (1, 1 , 0)
        //               |                                               |
        //               |                                               |
        //               |                                               |
        //               |                                               |
        //               |                                               |
        //          (-1, -1, 0) ------------------------------------- (1, -1, 0)
        //
        //         1st term:  "plane_n" is the normal vector of each line in the rectangle pointing to the interior of the rectangle
        //         2nd term:  "plane_p" is the point on the clipping plane which is the midpoint of each of the sides of the rectangle. 
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public int ClipTriangle_ToViewportBoundary(TriangleObject triangle, RENDERFLAGS flags)
        {
            int nTriangleDrawnCount = 0;
            // Clip triangle against near plane
            int nClippedTriangles = 0;
            TriangleObject[] clipped = new TriangleObject[2];
            clipped[0] = new TriangleObject();
            clipped[1] = new TriangleObject();

            // The n_plane should be the Z-plane of the screen <0,0,1>
            nClippedTriangles = MathOps.Triangle_ClipAgainstPlane(new Vec3D(0.0f, 0.0f, NearPlane), new Vec3D(0.0f, 0.0f, 1.0f), triangle, ref clipped[0], ref clipped[1]);

            // This may yield two new triangles
            for (int n = 0; n < nClippedTriangles; n++)
            {
                TriangleObject triProjected = clipped[n];

                // Project new triangle
                triProjected.p[0] = MathOps.Mat_MultiplyVector(matProj, clipped[n].p[0]);
                triProjected.p[1] = MathOps.Mat_MultiplyVector(matProj, clipped[n].p[1]);
                triProjected.p[2] = MathOps.Mat_MultiplyVector(matProj, clipped[n].p[2]);

                // Apply Projection to Verts
                triProjected.p[0].X = triProjected.p[0].X / triProjected.p[0].W;
                triProjected.p[1].X = triProjected.p[1].X / triProjected.p[1].W;
                triProjected.p[2].X = triProjected.p[2].X / triProjected.p[2].W;

                triProjected.p[0].Y = triProjected.p[0].Y / triProjected.p[0].W;
                triProjected.p[1].Y = triProjected.p[1].Y / triProjected.p[1].W;
                triProjected.p[2].Y = triProjected.p[2].Y / triProjected.p[2].W;

                triProjected.p[0].Z = triProjected.p[0].Z / triProjected.p[0].W;
                triProjected.p[1].Z = triProjected.p[1].Z / triProjected.p[1].W;
                triProjected.p[2].Z = triProjected.p[2].Z / triProjected.p[2].W;

                // Apply Projection to Tex coords
                triProjected.t[0].X = triProjected.t[0].X / triProjected.p[0].W;
                triProjected.t[1].X = triProjected.t[1].X / triProjected.p[1].W;
                triProjected.t[2].X = triProjected.t[2].X / triProjected.p[2].W;

                triProjected.t[0].Y = triProjected.t[0].Y / triProjected.p[0].W;
                triProjected.t[1].Y = triProjected.t[1].Y / triProjected.p[1].W;
                triProjected.t[2].Y = triProjected.t[2].Y / triProjected.p[2].W;

                triProjected.t[0].Z = 1.0f / triProjected.p[0].W;
                triProjected.t[1].Z = 1.0f / triProjected.p[1].W;
                triProjected.t[2].Z = 1.0f / triProjected.p[2].W;

                // Clip against viewport in screen space
                // Clip triangles against all four screen edges, this could yield
                // a bunch of triangles, so create a queue that we traverse to 
                //  ensure we only test new triangles generated against planes
                TriangleObject[] sclipped = new TriangleObject[2];
                sclipped[0] = new TriangleObject();
                sclipped[1] = new TriangleObject();
                List<TriangleObject> listTriangles = new List<TriangleObject>();

                // Add initial triangle
                listTriangles.Add(triProjected);

                int nNewTriangles = 1;

                for (int p = 0; p < 4; p++)
                {
                    int nTrisToAdd = 0;
                    while (nNewTriangles > 0)
                    {
                        sclipped[0] = new TriangleObject();
                        sclipped[1] = new TriangleObject();

                        // Take triangle from front of queue
                        TriangleObject test = listTriangles[0];

                        // remove it from the queue and subtract from total in the queue
                        listTriangles.RemoveAt(0);
                        nNewTriangles--;

                        // Clip it against a plane. We only need to test each 
                        // subsequent plane, against subsequent new triangles
                        // as all triangles after a plane clip are guaranteed
                        // to lie on the inside of the plane. I like how this
                        // comment is almost completely and utterly justified

                        switch (p)
                        {
                            case 0:
                                {
                                    nTrisToAdd = MathOps.Triangle_ClipAgainstPlane(new Vec3D(0.0f, -1.0f, 0.0f), new Vec3D(0.0f, 1.0f, 0.0f), test, ref sclipped[0], ref sclipped[1]);
                                    break;
                                }
                            case 1:
                                {
                                    nTrisToAdd = MathOps.Triangle_ClipAgainstPlane(new Vec3D(0.0f, +1.0f, 0.0f), new Vec3D(0.0f, -1.0f, 0.0f), test, ref sclipped[0], ref sclipped[1]);
                                    break;
                                }
                            case 2:
                                {
                                    nTrisToAdd = MathOps.Triangle_ClipAgainstPlane(new Vec3D(-1.0f, 0.0f, 0.0f), new Vec3D(1.0f, 0.0f, 0.0f), test, ref sclipped[0], ref sclipped[1]);
                                    break;
                                }
                            case 3:
                                {
                                    nTrisToAdd = MathOps.Triangle_ClipAgainstPlane(new Vec3D(+1.0f, 0.0f, 0.0f), new Vec3D(-1.0f, 0.0f, 0.0f), test, ref sclipped[0], ref sclipped[1]);
                                    break;
                                }
                        }

                        // Clipping may yield a variable number of triangles, so
                        // add these new ones to the back of the queue for subsequent
                        // clipping against next planes
                        for (int w = 0; w < nTrisToAdd; w++)
                        {
                            listTriangles.Add(sclipped[w]);
                        }
                    }

                    nNewTriangles = listTriangles.Count;
                }

                RasterTriangles(listTriangles, flags);
                nTriangleDrawnCount++;

            }
            return nTriangleDrawnCount;
        }

        public void RasterTriangles (List<TriangleObject> listTriangles, RENDERFLAGS flags)
        {
            foreach (TriangleObject tri in listTriangles)
            {
                RasterSingleTriangle(tri, flags);
            }
        }

        public void RasterLines (List<LineObject> listLines, RENDERFLAGS flags)
        {
            foreach (LineObject line in listLines)
            {
                RasterSingleLine(line, flags);
            }
        }


        //public UInt32 RenderLine(Vec3D p1, Vec3D p2, Pixel col)
        //{

        //}

        //public UInt32 RenderCircleXZ(Vec3D p1, float r, Pixel col);



        public void ConfigureDisplay()
        {
            m_DepthBuffer = new float[(int)(Width * Height)];
        }

        public static void ClearDepth()
        {
            for (int i = 0; i < m_DepthBuffer.Length; i++)
            {
                m_DepthBuffer[i] = 0.0f;
            }
        }

        public static void AddTriangleToScene(TriangleObject tri)
        {

        }

        public virtual void RenderScene()
        {

        }

        public virtual void DrawTriangleFlat(TriangleObject tri) { throw new NotImplementedException("in BaseDrawingPipeline:  DrawTriangleFlat not implemented for base clase"); }

        public virtual void DrawTriangleWire(TriangleObject tri, Pixel col) { throw new NotImplementedException("in BaseDrawingPipeline:  DrawTriangleWire not implemented for base clase"); }

        public virtual void DrawLineObject(LineObject line, Pixel col1, Pixel col2) { throw new NotImplementedException("in BaseDrawingPipeline:  DrawLineObject not implemented for base clase"); }

        public virtual void DrawTriangleTex(TriangleObject tri, Pixel col)
        {
            throw new NotImplementedException("In DrawTriangleTex() -- Textured Triangles not supported at this time");
        }

        public virtual void TexturedTriangle(int x1, int y1, float u1, float v1, float w1, 
            int x2, int y2, float u2, float v2, float w2, 
            int x3, int y3, float u3, float v3, float w3)
        {
            throw new NotImplementedException("in BaseDrawingPipeline:  Textured Triangle not implemented for base clase");

        }

        public virtual void RasterTriangle(int x1, int y, float u1, float v1, float w1, Pixel c1,
            int x2, int y2, float u2, float v2, float w2, Pixel c2,
            int x3, int y3, float u3, float v3, float w3, Pixel c3, UInt32 nFlags)
        {
            throw new NotImplementedException("in BaseDrawingPipeline:  RasterTriangle not implemented for base clase");
        }

        public virtual void DrawTriangle(double x1, double y1, double x2, double y2, double x3, double y3, Pixel p)
        {
            throw new NotImplementedException("in BaseDrawingPipeline:  DrawTriangle not implemented for base clase");
        }

        /// <summary>
        /// Wrapper function that returns an interpolated Pixel color
        /// </summary>
        /// <param name="p">percentage value to interpolate (0 < p < 1.0)</param>
        /// <param name="pixel1">pixel of the first node</param>
        /// <param name="pixel2">pixel of the second node</param>
        /// <returns></returns>
        public virtual Pixel InterpolateColors_ToPixel(float p, Pixel pixel1, Pixel pixel2)
        {
            throw new NotImplementedException("in BaseDrawingPipeline:  InterpoateColors_ToPixel not implemented for base clase");
        }

    }
}
