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

        // The last x-position of the mouse
        public float LastMouseX {get; set;}

        // The last y-position of the mouse
        public float LastMouseY { get; set; }

        // Flag for the first time the mouse is moved.
        private bool bFirstMouse { get; set; } = true;

        // Flag to determine if the UI needs an update
        public bool bNeedsUpdate { get; set; } = true;

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
            // Add event handlers for key down, key up, and mouse move
            RenderForm.KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            RenderForm.KeyUp += (s, e) => Input.KeyUp(e.KeyCode);
            RenderForm.MouseMove += (s, e) =>
            {
                MouseMoveCallBack(e.X, e.Y);

            };
            RenderForm.MouseUp += (s, e) =>
            {
                MessageBox.Show("Mouse button up");
            };
            RenderForm.MouseDown += (s, e) =>
            {
                MessageBox.Show("Mouse button down");
            };
            RenderForm.MouseWheel += (s, e) =>
            {
                MessageBox.Show("Mouse wheel detected");
            };
            RenderForm.MouseClick += (s, e) =>
            {
                MessageBox.Show("Mouse click detected");
            };

            RenderLoop.Run(RenderForm, () =>
            {
                if (!Frame())
                    ShutDown();
            });
        }

        /// <summary>
        /// Handles the vector updates for the camera orientation based on movement within the DirectX window.
        /// </summary>
        /// <param name="x_pos"></param>
        /// <param name="y_pos"></param>
        private void MouseMoveCallBack(float x_pos, float y_pos)
        {
            var camera = Graphics.Camera;

            // If our camera isn't active.  Do nothing.
            if (camera.IsActiveMode == false)
                return;

            if (bFirstMouse)
            {
                LastMouseX = x_pos;
                LastMouseY = y_pos;
                bFirstMouse = false;
            }

            float xoffset = x_pos - LastMouseX;
            float yoffset = LastMouseY - y_pos;  // last is first since y+ is down
            LastMouseX = x_pos;
            LastMouseY = y_pos;

            float sensitivity = 0.005f;
            xoffset *= sensitivity;
            yoffset *= sensitivity;

            camera.Yaw += xoffset;
            camera.Pitch += yoffset;

            if (camera.Pitch > 89.0f)
                camera.Pitch = 89.0f;
            if (camera.Pitch < -89.0f)
                camera.Pitch = -89.0f;

            SharpDX.Vector3 direction = new SharpDX.Vector3();
            direction.X = (float)(Math.Cos(camera.Yaw * 3.14159 / 180.0f) * Math.Cos(camera.Pitch * 3.14159 / 180.0f));
            direction.Y = (float)(Math.Sin(camera.Pitch * 3.14159 / 180.0f));
            direction.Z = (float)(Math.Sin(camera.Yaw * 3.14159 / 180.0f) * Math.Cos(camera.Pitch * 3.14159 / 180.0f));
            camera.LookAt = DXMathFunctions.Vec_Normalize(direction);
            camera.UpdateViewMatrix();

            //MessageBox.Show("X: " + x_pos + "   Y: " + y_pos);


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
            DCamera c = Graphics.Camera;

            float newX = c.GetX;
            float newY = c.GetY;
            float newZ = c.GetZ;

            if (Input.IsKeyDown(Keys.C))
            {
                Input.KeyUp(Keys.C); // toggle the camera

                c.IsActiveMode = !(c.IsActiveMode);
                MessageBox.Show("Camera is " + (c.IsActiveMode ? "" : " NOT ") + " active now!");
            }

            SharpDX.Vector3 newPos = new SharpDX.Vector3(newX, newY, newZ);

            // Camera related functionality
            if (c.IsActiveMode)
            {   
                SharpDX.Vector3 move = new SharpDX.Vector3(0,0,0);
                if (Input.IsKeyDown(Keys.A))
                {
                    Input.KeyUp(Keys.A); // turn off the toggle
                    move =c.MoveRight(false);

                    c.SetPosition(move.X, move.Y, move.Z);
                }
                if (Input.IsKeyDown(Keys.D))
                {
                    Input.KeyUp(Keys.D); // turn off the toggle
                    move = c.MoveRight(true);

                    c.SetPosition(move.X, move.Y, move.Z);
                }
                if (Input.IsKeyDown(Keys.W))
                {
                    Input.KeyUp(Keys.W); // turn off the toggle
                    move = c.MoveForward(true);

                    c.SetPosition(move.X, move.Y, move.Z);
                }
                if (Input.IsKeyDown(Keys.X))
                {
                    Input.KeyUp(Keys.X); // turn off the toggle
                    move = c.MoveForward(false);

                    c.SetPosition(move.X, move.Y, move.Z);

                }

                //if (Input.IsKeyDown(Keys.Space))
                //{
                //    Input.KeyUp(Keys.Space); // turn off the toggle
                //}

                //if (Input.IsKeyDown(Keys.R))
                //{
                //    Input.KeyUp(Keys.R); // turn off the toggle
                //}

            }

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
