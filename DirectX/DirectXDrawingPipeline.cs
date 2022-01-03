using DrawingPipelineLibrary;
using DrawingPipelineLibrary.DirectX;
using System.Windows.Input;

namespace DrawingPipeline.DirectX
{
    public class DirectXDrawingPipeline : BaseDrawingPipeline
    {


        public override void SetKeyState(Key key, bool val)
        {
            // handle any generic pipeline keystroke requirements.
            base.SetKeyState(key, val);
        }
        
        public DirectXDrawingPipeline(int width, int height, int timer) : base(width, height, timer)
        {
            // Loads our drawing functions for the DirectX format drawing
            DrawFunc = new DirectXDrawingFunctions();

            // Initializes the direct X drawing system
            GetDSystem = new DSystem();
            GetDSystem.Initialize("DirectX Window", width, height, true, false, 0);
        }

        /// <summary>
        /// Starts the running of the pipeline.
        /// </summary>
        public override void Run()
        {
            // Start running the direct X render loop...
            GetDSystem.RunRenderForm();
        }
    }
}
