using DrawingHelpersLibrary;
using DrawingPipelineLibrary;
using DrawingPipelineLibrary.DirectX;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        /// <summary>
        /// Update function for drawing to a WPF Canvas object
        /// </summary>
        /// <returns></returns>
        public override int Update()
        {
            int ex = GetDSystem.Configuration.Width;
            int ey = GetDSystem.Configuration.Height;

            // Clear the screen
            m_CanvasContext.Children.Clear();


            //// DRAW STUFF
            DrawFunc.DrawLineSolid(0, 0, (float)ex, (float)ey, Brushes.Red, 10, Linetypes.LINETYPE_PHANTOM_X2);

         //   // Call the base render function
            return 1;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// To handle key states
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public override void SetKeyState(Key key, bool val)
        {
            // handle any generic pipeline keystroke requirements.
            base.SetKeyState(key, val);
        }
    }
}
