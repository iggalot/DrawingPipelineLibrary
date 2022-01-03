using DrawingHelpersLibrary;
using System.Windows.Media;

namespace DrawingPipelineLibrary
{
    /// <summary>
    /// Abstract class to handle drawing of objects to different formats
    /// such as DirectX window or WPF Canvas
    /// </summary>
    public abstract class acDrawingFunction
    {
        // function for drawing a solid line
        public abstract void DrawLineSolid(float sx, float sy, float ex, float ey, Brush color, double thick, Linetypes line_type);

        // function for drawing a filled triangle
        public abstract void DrawTriangleFilled();

        // function for drawing a wired triangle
        public abstract void DrawTriangleWire();

        // function for drawing a filled rectangle
        public abstract void DrawRectangleFilled();

        // function for drawing a wire rectangle
        public abstract void DrawRectangleWire();

        // function to draw a filled circle
        public abstract void DrawCircleFilled();

        // function to draw a wire circle
        public abstract void DrawCircleWire();

        // function to draw a rectangular prism
        public abstract void DrawRectanglePrismWire();

        // function to draw a filled (shaded) rectangular prism
        public abstract void DrawRectangularPrismFilled();
    }
}
