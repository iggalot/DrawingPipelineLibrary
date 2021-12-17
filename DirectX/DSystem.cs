using SharpDX.Windows;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrawingPipelineLibrary.DirectX
{
    public class DSystem                    // 122 lines
    {
        // Properties  DSharpDXRastertek
        private RenderForm RenderForm { get; set; }
        public DSystemConfiguration Configuration { get; private set; }
        public DInput Input { get; private set; }
        public DGraphics Graphics { get; private set; }

        // Constructor
        public DSystem() { }

        /// <summary>
        /// Default renderform loop start point.
        /// DO NOT ERASE!!
        /// </summary>
        public void StartRenderForm(string title, int width, int height, bool vSync, bool fullScreen = true, int testTimeSeconds = 0)
        {
            DSystem system = new DSystem();
            system.Initialize(title, width, height, vSync, fullScreen, testTimeSeconds);
            system.RunRenderForm();
        }

        // Methods
        public virtual bool Initialize(string title, int width, int height, bool vSync, bool fullScreen, int testTimeSeconds)
        {
            bool result = false;

            if (Configuration == null)
                Configuration = new DSystemConfiguration(title, width, height, fullScreen, vSync);

            // Initialize Window.
            InitializeWindows(title);

            if (Input == null)
            {
                Input = new DInput();
                Input.Initialize();
            }
            if (Graphics == null)
            {
                Graphics = new DGraphics();
                result = Graphics.Initialize(Configuration, RenderForm.Handle);
            }

            DPerfLogger.Initialize("RenderForm C# SharpDX: " + Configuration.Width + "x" + Configuration.Height + " VSync:" + DSystemConfiguration.VerticalSyncEnabled + " FullScreen:" + DSystemConfiguration.FullScreen + "   " + RenderForm.Text, testTimeSeconds, Configuration.Width, Configuration.Height);

            return result;
        }
        private void InitializeWindows(string title)
        {
            //int width = Screen.PrimaryScreen.Bounds.Width;
            //int height = Screen.PrimaryScreen.Bounds.Height;

            // Initialize Window.
            RenderForm = new RenderForm(title)
            {
                ClientSize = new Size(Configuration.Width, Configuration.Height),
                FormBorderStyle = DSystemConfiguration.BorderStyle

            };

            // The form must be showing in order for the handle to be used in Input and Graphics objects.
            RenderForm.Show();

            // Set the DirectX window location to the right of the UI window with upper left at Y = 0;
            // TODO:: Fix this location line to determine its position based on the windows position.
            RenderForm.Location = new Point((int)(Configuration.Width*2), 0);

//            RenderForm.Location = new Point((width / 2) - (Configuration.Width / 2), (height / 2) - (Configuration.Height / 2));
        }
        public void RunRenderForm()
        {
            RenderForm.KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            RenderForm.KeyUp += (s, e) => Input.KeyUp(e.KeyCode);

            RenderLoop.Run(RenderForm, () =>
            {
                if (!Frame())
                    ShutDown();
            });
        }
        public bool Frame()
        {
            // Check if the user pressed escape and wants to exit the application.
            if (Input.IsKeyDown(Keys.Escape))
                return false;

            //
            ProcessKeyStrokes();

            // Update the system stats but only run this tutorials test for one second since we cannot get frame rates because it is too fast..
            Graphics.Timer.Frame2();

            if (DPerfLogger.IsTimedTest)
            {
                DPerfLogger.Frame(Graphics.Timer.FrameTime);
                if (Graphics.Timer.CumulativeFrameTime >= DPerfLogger.TestTimeInSeconds * 1000)
                    return false;
            }

            // Do the frame processing for the graphics object.
            return Graphics.Frame();
        }

        private void ProcessKeyStrokes()
        {
            float offset = 20f;
            DCamera c = Graphics.Camera;

            float newX = c.GetX;
            float newY = c.GetY;
            float newZ = c.GetZ;

            if (Input.IsKeyDown(Keys.A))
            {
                //MessageBox.Show("A pressed");
                Input.KeyUp(Keys.A); // turn off the toggle
                newX -= offset;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                //MessageBox.Show("D pressed");
                Input.KeyUp(Keys.D); // turn off the toggle
                newX += offset;
            }
            if (Input.IsKeyDown(Keys.W))
            {
                Input.KeyUp(Keys.W); // turn off the toggle
                newZ -= offset;
                //MessageBox.Show("W pressed");
            }
            if (Input.IsKeyDown(Keys.X))
            {
                Input.KeyUp(Keys.X); // turn off the toggle
                //MessageBox.Show("X pressed");
                newZ += offset;
            }

            if (Input.IsKeyDown(Keys.Space))
            {
                Input.KeyUp(Keys.Space); // turn off the toggle
                //MessageBox.Show("X pressed");
                newY += offset;
            }


            if (Input.IsKeyDown(Keys.R))
            {
                Input.KeyUp(Keys.R); // turn off the toggle
                newX = c.GetX;
                newY = c.GetY;
                newZ = c.GetZ;
            }

            // Set the camera changes
            Graphics.Camera.SetPosition(newX, newY, newZ);
        }

        public void ShutDown()
        {
            ShutdownWindows();

            DPerfLogger.ShutDown();

            // Release graphics and related objects.
            Graphics?.ShutDown();
            Graphics = null;
            Input = null;
            Configuration = null;
        }
        private void ShutdownWindows()
        {
            RenderForm?.Dispose();
            RenderForm = null;
        }
    }
}
