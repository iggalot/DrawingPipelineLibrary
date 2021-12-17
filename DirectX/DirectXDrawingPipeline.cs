using DrawingPipelineLibrary;
using DrawingPipelineLibrary.DirectX;
using System.Windows.Input;

namespace DrawingPipeline.DirectX
{
    public class DirectXDrawingPipeline : BaseDrawingPipeline
    {
        // The DirectX System parameter
        public DSystem GetDSystem { get; set; }
        public override void SetKeyState(Key key, bool val)
        {
            // convert the Application's WPF keystrokes to DirectX key input
            GetDSystem.Input.SetKeyState(key, val);

            // handle any generic pipeline keystroke requirements.
            base.SetKeyState(key, val);
        }
        
        public DirectXDrawingPipeline(int width, int height) : base(width, height)
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
        public override void RunPipeline()
        {
            // Start running the direct X render loop...
            GetDSystem.RunRenderForm();
        }
    }
}
