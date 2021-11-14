using DrawingHelpersLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using static MathLibrary.MathVectors;

namespace DrawingPipeline
{
    public class CanvasDrawingPipeline : BaseDrawingPipeline
    {
        /// <summary>
        /// The canvas objet for our drawing context
        /// </summary>
        private Canvas m_CanvasContext { get; set; }
        public CanvasDrawingPipeline(Canvas c)
        {
            m_CanvasContext = c;
            Width = (float)m_CanvasContext.Width;
            Height =(float)m_CanvasContext.Height;
        }

        public override int Update(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags)
        {
            // Clear the screen
            m_CanvasContext.Children.Clear();

            // Call the base render function
            return base.Render(TriangleList, LineList, flags);
        }

        public override TriangleObject RasterSingleTriangle(TriangleObject triRaster, RENDERFLAGS flags)
        {
            // Perform the scaling of the viewport
            triRaster = base.RasterSingleTriangle(triRaster, flags);

            // Set the  colors
            Brush br0 = ToBrush(triRaster.col[0]);
            Brush br1 = ToBrush(triRaster.col[1]);
            Brush br2 = ToBrush(triRaster.col[2]);

            if ((flags & RENDERFLAGS.RENDER_WIRE) == flags)
            {
                DrawTriangleWire(triRaster, base.BLACK);
            }

            if ((flags & RENDERFLAGS.RENDER_FLAT) == flags)
            {
                DrawingHelpers.DrawTriangleFilled(m_CanvasContext, triRaster.p[0].X, triRaster.p[0].Y, triRaster.p[1].X, triRaster.p[1].Y, triRaster.p[2].X, triRaster.p[2].Y, Brushes.Green, br0, br0, br0);
            }
            if ((flags & RENDERFLAGS.RENDER_TEXTURED) == flags)
            {
                DrawTriangleTex(triRaster, base.BLACK);
            }

            if ((flags & RENDERFLAGS.RENDER_DEPTH) == flags)
            {

            }
            else
            {
                DrawingHelpers.DrawTriangleFilled(m_CanvasContext, triRaster.p[0].X, triRaster.p[0].Y, triRaster.p[1].X, triRaster.p[1].Y, triRaster.p[2].X, triRaster.p[2].Y, Brushes.Green, br0, br1, br2);

                //    //RasterTriangle(
                //    //    (int)triRaster.p[0].X, (int)triRaster.p[0].Y, triRaster.t[0].X, triRaster.t[0].Y, triRaster.t[0].Z, triRaster.col[0],
                //    //    (int)triRaster.p[1].X, (int)triRaster.p[1].Y, triRaster.t[1].X, triRaster.t[1].Y, triRaster.t[1].Z, triRaster.col[1],
                //    //    (int)triRaster.p[2].X, (int)triRaster.p[2].Y, triRaster.t[2].X, triRaster.t[2].Y, triRaster.t[2].Z, triRaster.col[2],
                //    //    sprTexture, flags);

            }

            return triRaster;
        }

        public override LineObject RasterSingleLine(LineObject lineRaster, RENDERFLAGS flags)
        {
            // Perform the scaling of the line objects
            lineRaster = base.RasterSingleLine(lineRaster, flags);

            // For now, just draw the line
            SolidColorBrush br0 = (SolidColorBrush)ToBrush(lineRaster.col[0]);
            SolidColorBrush br1 = (SolidColorBrush)ToBrush(lineRaster.col[1]);

            DrawLineObject(lineRaster, ToPixel(br0), ToPixel(br1));

            return lineRaster;
        }

        public override void DrawTriangleFlat(TriangleObject tri)
        {
            SolidColorBrush stroke = new SolidColorBrush(Color.FromArgb((byte)tri.col[0].a, (byte)tri.col[0].r, (byte)tri.col[0].g, (byte)tri.col[0].b));
            DrawingHelpers.DrawTriangleFilled(m_CanvasContext, tri.p[0].X, tri.p[0].Y, tri.p[1].X, tri.p[1].Y, tri.p[2].X, tri.p[2].Y, stroke, stroke, stroke, stroke);
        }

        public override void DrawTriangleWire(TriangleObject tri, Pixel col)
        {
            DrawingHelpers.DrawTriangle(m_CanvasContext, tri.p[0].X, tri.p[0].Y, tri.p[1].X, tri.p[1].Y, tri.p[2].X, tri.p[2].Y, ToBrush(col));
        }

        public override void DrawLineObject(LineObject line, Pixel col1, Pixel col2)
        {
            DrawingHelpers.DrawLine_ColorGradient(m_CanvasContext, line.p[0].X, line.p[0].Y, line.p[1].X, line.p[1].Y, ToBrush(col1), ToBrush(col2));
        }

        public override void DrawTriangleTex(TriangleObject tri, Pixel col)
        {
            throw new NotImplementedException("In DrawTriangleTex() -- Textured Triangles not supported at this time");
        }

        public override void DrawTriangle(double x1, double y1, double x2, double y2, double x3, double y3, Pixel p)
        {
            SolidColorBrush stroke = new SolidColorBrush(Color.FromArgb((byte)p.a, (byte)p.r, (byte)p.g, (byte)p.b));
            DrawingHelpers.DrawTriangle(m_CanvasContext, x1, y1, x2, y2, x3, y3, stroke);
        }

        /// <summary>
        /// Wrapper function that returns an interpolated Pixel color
        /// </summary>
        /// <param name="p">percentage value to interpolate (0 < p < 1.0)</param>
        /// <param name="pixel1">pixel of the first node</param>
        /// <param name="pixel2">pixel of the second node</param>
        /// <returns></returns>
        public override Pixel InterpolateColors_ToPixel(float p, Pixel pixel1, Pixel pixel2)
        {
            // interpolate the colors using DrawingHelpers color interpolation function.
            return ToPixel(DrawingHelpers.InterpolateColors(p, ToBrush(pixel1), ToBrush(pixel2)));
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
