using DrawingHelpersLibrary;
using DrawingPipelineLibrary;
using DrawingPipelineLibrary.DirectX;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using static MathLibrary.MathVectors;

namespace DrawingPipeline
{
    public class CanvasDrawingPipeline : BaseDrawingPipeline
    {
        /// <summary>
        /// The canvas objet for our drawing context
        /// </summary>
        private Canvas m_CanvasContext;

        public CanvasDrawingPipeline(Canvas c, int width, int height, int timer) : base (width, height, timer)
        {
            m_CanvasContext = c;

            GetDSystem = new DSystem();
            GetDSystem.Initialize("Canvas Window", width, height, true, false, 0);

            // Set up our drawing functions for our canvas object
            DrawFunc = new WPFCanvasDrawingFunctions(c);
        }

        public override void Run()
        {
            Update();
        }
        public override int Update()
        {
            var ex = GetDSystem.Configuration.Width;
            var ey = GetDSystem.Configuration.Height;

            // Clear the screen
            m_CanvasContext.Children.Clear();


                // Draw to the canvas
                DrawingHelpers.DrawLine(m_CanvasContext, 0, 0, ex, ey, Brushes.Red, 10, Linetypes.LINETYPE_PHANTOM_X2);

         //   // Call the base render function
            return 1;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
