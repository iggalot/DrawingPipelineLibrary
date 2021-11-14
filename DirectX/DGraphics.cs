using System;
using System.Windows;

namespace DrawingPipelineLibrary.DirectX
{
    public class DGraphics                      // 23 lines              // 23 lines
    {
        // Properties
        private DDX11 D3D { get; set; }
        public DTimer Timer { get; set; }

        public bool Initialize(DSystemConfiguration configuration, IntPtr windowsHandle)
        {
            try
            {
                // Create the Direct3D object.
                D3D = new DDX11();

                // Initialize the Direct3D object.
                if (!D3D.Initialize(configuration, windowsHandle))
                    return false;

                // Create the Timer
                Timer = new DTimer();

                // Initialize the Timer
                if (!Timer.Initialize())
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not initialize Direct3D\nError is '" + ex.Message + "'");
                return false;
            }
        }
        public void ShutDown()
        {
            Timer = null;

            D3D?.ShutDown();
            D3D = null;
        }
        public bool Frame()
        {
            // Render the graphics scene.
            return Render();
        }
        public bool Render()
        {
            // Clear the buffer to begin the scene.
            D3D.BeginScene(0.5f, 0.5f, 0.5f, 1.0f);

            // Present the rendered scene to the screen.
            D3D.EndScene();

            return true;
        }
    }
}
