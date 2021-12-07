using System;
using System.Collections.Generic;
using System.Windows;

namespace DrawingPipelineLibrary.DirectX
{
    public class DGraphics                      // 23 lines              // 23 lines
    {
        private List<DModel> lstModelList = new List<DModel>();
        // Properties
        public DDX11 D3D { get; set; }
        public DCamera Camera { get; set; }
        public DModel Model { get; set; }

        public List<DModel> ModelList {
            get {
                lock(this)
                {
                    return lstModelList;
                }
            }
            set 
            {
                if (value != null)
                {
                    lock(this)
                    {
                        lstModelList = value;
                    }
                }
            }
        }
        public void AddModel(DModel model)
        {
            if (model != null)
                ModelList.Add(model);

            return;
        }

        public DColorShader ColorShader { get; set; }
        public DTimer Timer { get; set; }

        // Constructor
        public DGraphics() { }

        public bool Initialize(DSystemConfiguration configuration, IntPtr windowHandle)
        {
            try
            {
                // Create the Direct3D object.
                D3D = new DDX11();

                // Initialize the Direct3D object.
                if (!D3D.Initialize(configuration, windowHandle))
                    return false;

                // Create the camera object and set its initial position
                Camera = new DCamera(0, 0, -10);

                // Create the list of models
                ModelList = new List<DModel>();
                //// Create the model object.
                //Model = new DModel();

                //// Initialize the model object.
                //if (!Model.Initialize(D3D.Device))
                //    return false;

                // Create the color shader object.
                ColorShader = new DColorShader();

                // Initialize the color shader object.
                if (!ColorShader.Initialize(D3D.Device, windowHandle))
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
            // Release the camera object.
            Camera = null;
            
            // Release the timer object
            Timer = null;

            // Release the color shader object.
            ColorShader?.ShutDown();
            ColorShader = null;

            for (int i = 0; i < ModelList.Count; i++)
            {
                // Release the model object.
                ModelList[i]?.ShutDown();
                ModelList[i] = null;
            }
            ModelList.Clear();

            //// Release the model object.
            //Model?.ShutDown();
            //Model = null;

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

            // Generate the view matrix based on the camera position.
            Camera.Render();

            // Get the world, view, and projection matrices from camera and d3d objects.
            var viewMatrix = Camera.ViewMatrix;
            var worldMatrix = D3D.WorldMatrix;
            var projectionMatrix = D3D.ProjectionMatrix;

            // Put the model vertex and index buffers on the graphics pipeline to prepare them for drawing.
            for (int i = 0; i < ModelList.Count; i++)
            {
                var model = ModelList[i];

                if (model == null)
                    break;

                // TODO:: Fix the race condition that occurs here for somereason.
                // if the modelist has changed or an element has broken.

                model.Render(D3D.DeviceContext);

                // Render the model using the color shader.
                if (!ColorShader.Render(D3D.DeviceContext, model.IndexCount, worldMatrix, viewMatrix, projectionMatrix))
                    return false;
            }



            // Present the rendered scene to the screen.
            D3D.EndScene();

            return true;
        }
    }
}
