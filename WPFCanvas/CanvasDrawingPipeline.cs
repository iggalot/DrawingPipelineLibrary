using DrawingHelpersLibrary;
using DrawingPipelineLibrary;
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

        public CanvasDrawingPipeline(Canvas c, int width, int height) : base (width, height)
        {
            m_CanvasContext = c;

            // Set up our drawing functions for our canvas object
            DrawFunc = new WPFCanvasDrawingFunctions(c);
        }

        public override int Update(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags)
        {
            // Clear the screen
            m_CanvasContext.Children.Clear();

            // Call the base render function
            return base.Render(TriangleList, LineList, flags);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
