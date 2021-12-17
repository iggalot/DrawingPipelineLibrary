using System;
using System.Windows.Controls;

namespace DrawingPipelineLibrary
{
    /// <summary>
    /// A class that contains the drawing functions for being used on a WPF Canvas object
    /// </summary>
    public class WPFCanvasDrawingFunctions : acDrawingFunction
    {
        // Holds the canvas object these functions are tied to.
        private Canvas m_Canvas;

        public WPFCanvasDrawingFunctions(Canvas canvas)
        {
            m_Canvas = canvas;
        }

        /// <summary>
        //TODO:  Original WPF drawing code.  Need to generalize for WPF functions
        /// </summary>
        //public TriangleObject RasterSingleTriangle(TriangleObject triRaster, RENDERFLAGS flags)
        //{
        //    // Perform the scaling of the viewport
        //    triRaster = base.RasterSingleTriangle(triRaster, flags);

        //    // Set the  colors
        //    Brush br0 = ToBrush(triRaster.col[0]);
        //    Brush br1 = ToBrush(triRaster.col[1]);
        //    Brush br2 = ToBrush(triRaster.col[2]);

        //    if ((flags & RENDERFLAGS.RENDER_WIRE) == flags)
        //    {
        //        DrawTriangleWire(triRaster, base.BLACK);
        //    }

        //    if ((flags & RENDERFLAGS.RENDER_FLAT) == flags)
        //    {
        //        DrawingHelpers.DrawTriangleFilled(m_CanvasContext, triRaster.p[0].X, triRaster.p[0].Y, triRaster.p[1].X, triRaster.p[1].Y, triRaster.p[2].X, triRaster.p[2].Y, Brushes.Green, br0, br0, br0);
        //    }
        //    if ((flags & RENDERFLAGS.RENDER_TEXTURED) == flags)
        //    {
        //        DrawTriangleTex(triRaster, base.BLACK);
        //    }

        //    if ((flags & RENDERFLAGS.RENDER_DEPTH) == flags)
        //    {

        //    }
        //    else
        //    {
        //        DrawingHelpers.DrawTriangleFilled(m_CanvasContext, triRaster.p[0].X, triRaster.p[0].Y, triRaster.p[1].X, triRaster.p[1].Y, triRaster.p[2].X, triRaster.p[2].Y, Brushes.Green, br0, br1, br2)
        //    }

        //    return triRaster;
        //}

        //public override LineObject RasterSingleLine(LineObject lineRaster, RENDERFLAGS flags)
        //{
        //    // Perform the scaling of the line objects
        //    lineRaster = base.RasterSingleLine(lineRaster, flags);

        //    // For now, just draw the line
        //    SolidColorBrush br0 = (SolidColorBrush)ToBrush(lineRaster.col[0]);
        //    SolidColorBrush br1 = (SolidColorBrush)ToBrush(lineRaster.col[1]);

        //    DrawLineObject(lineRaster, ToPixel(br0), ToPixel(br1));

        //    return lineRaster;
        //}

        //public override void DrawTriangleFlat(TriangleObject tri)
        //{
        //    SolidColorBrush stroke = new SolidColorBrush(Color.FromArgb((byte)tri.col[0].a, (byte)tri.col[0].r, (byte)tri.col[0].g, (byte)tri.col[0].b));
        //    DrawingHelpers.DrawTriangleFilled(m_CanvasContext, tri.p[0].X, tri.p[0].Y, tri.p[1].X, tri.p[1].Y, tri.p[2].X, tri.p[2].Y, stroke, stroke, stroke, stroke);
        //}

        //public override void DrawTriangleWire(TriangleObject tri, Pixel col)
        //{
        //    DrawingHelpers.DrawTriangle(m_CanvasContext, tri.p[0].X, tri.p[0].Y, tri.p[1].X, tri.p[1].Y, tri.p[2].X, tri.p[2].Y, ToBrush(col));
        //}

        //public override void DrawLineObject(LineObject line, Pixel col1, Pixel col2)
        //{
        //    DrawingHelpers.DrawLine_ColorGradient(m_CanvasContext, line.p[0].X, line.p[0].Y, line.p[1].X, line.p[1].Y, ToBrush(col1), ToBrush(col2));
        //}

        //public override void DrawTriangleTex(TriangleObject tri, Pixel col)
        //{
        //    throw new NotImplementedException("In DrawTriangleTex() -- Textured Triangles not supported at this time");
        //}

        //public override void DrawTriangle(double x1, double y1, double x2, double y2, double x3, double y3, Pixel p)
        //{
        //    SolidColorBrush stroke = new SolidColorBrush(Color.FromArgb((byte)p.a, (byte)p.r, (byte)p.g, (byte)p.b));
        //    DrawingHelpers.DrawTriangle(m_CanvasContext, x1, y1, x2, y2, x3, y3, stroke);
        //}

        public override void DrawLineSolid()
        {
            throw new NotImplementedException();
        }

        public override void DrawTriangleFilled()
        {
            throw new NotImplementedException();
        }

        public override void DrawTriangleWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangleFilled()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangleWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawCircleFilled()
        {
            throw new NotImplementedException();
        }

        public override void DrawCircleWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectanglePrismWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangularPrismFilled()
        {
            throw new NotImplementedException();
        }
    }
}
