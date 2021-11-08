using DrawingPipeline;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using static DrawingPipeline.BaseDrawingPipeline;
using static MathLibrary.MathVectors;

namespace MathLibraryDriver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BaseDrawingPipeline Pipeline { get; set; }
        List<TriangleObject> TriangleList { get; set; }
        List<LineObject> LineList { get; set; }

        Vec3D CameraPos { get; set; }
        Vec3D CameraTarget { get; set; }
        MathLibrary.MathVectors.Vec3D CameraUp { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            TriangleList = new List<TriangleObject>();
            LineList = new List<LineObject>();
            //Pipeline = new CanvasDrawingPipeline(MainCanvas);
            Pipeline = new DirectXDrawingPipeline();

            OnUserCreate();


            // create a thread for UI
            Thread UIThread = new Thread(new ThreadStart(UIThreadFunction));
            UIThread.Start();


            // Perform our rendering.
            OnUserUpdate();

            UIThread.Join();


        }

        private void UIThreadFunction()
        {
            var directInput = new DirectInput();
            var keyboard = new Keyboard(directInput);


            keyboard.Properties.BufferSize = 128;
            keyboard.Acquire();


            bool validKeyPressed = false;
            while (true)
            {
                keyboard.Poll();
                var datas = keyboard.GetBufferedData();
                foreach (var state in datas)
                {
                    if (state.IsPressed == false)
                        continue;

                    if (state.Key == Key.D)
                    {
                        //Console.WriteLine(state.Key + " was pressed here!");
                        TriangleList[0].p[0].X += -10.0f;
                        validKeyPressed = true;
                    }
                    if(state.Key == Key.A)
                    {
                        //Console.WriteLine(state.Key + " was pressed here!");
                        TriangleList[0].p[0].X += 10.0f;
                        validKeyPressed = true;
                    }
                    if (state.Key == Key.W)
                    {
                        //Console.WriteLine(state.Key + " was pressed here!");
                        TriangleList[0].p[0].Y += -10.0f;
                        validKeyPressed = true;
                    }
                    if (state.Key == Key.S)
                    {
                        //Console.WriteLine(state.Key + " was pressed here!");
                        TriangleList[0].p[0].Y += +10.0f;
                        validKeyPressed = true;
                    }

                    if (validKeyPressed)
                    {
                        Dispatcher.BeginInvoke((Action)(() => OnUserUpdate()));
                        validKeyPressed = false;
                    }

                    // Show the key press state information
                    Console.WriteLine(state);
                }
            }
        }


        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.W)
            {
                Console.WriteLine("forward");
                CameraPos = MathOps.Vec_Add(CameraPos,new Vec3D(0, 0, 50));
                Pipeline.SetCamera(CameraPos, CameraTarget, CameraUp);
            }

            if (e.Key == System.Windows.Input.Key.S)
            {
                Console.WriteLine("backward");
                CameraPos = MathOps.Vec_Add(CameraPos, new Vec3D(0, 0, -50));
                Pipeline.SetCamera(CameraPos, CameraTarget, CameraUp);
            }

            if (e.Key == System.Windows.Input.Key.A)
            {
                Console.WriteLine("backward");
                CameraPos = MathOps.Vec_Add(CameraPos, new Vec3D(-50, 0, 0));
                Pipeline.SetCamera(CameraPos, CameraTarget, CameraUp);
            }

            if (e.Key == System.Windows.Input.Key.D)
            {
                Console.WriteLine("backward");
                CameraPos = MathOps.Vec_Add(CameraPos, new Vec3D(+50, 0, 0));
                Pipeline.SetCamera(CameraPos, CameraTarget, CameraUp);
            }

            OnUserUpdate();
        }

        private void OnUserUpdate()
        {
            Pipeline.Update(TriangleList, LineList, RENDERFLAGS.RENDER_CULL_CCW | RENDERFLAGS.RENDER_WIRE);
        }

        private void OnUserCreate()
        {
            // Make some test data
            // position data
            Vec3D vec1 = new Vec3D(-100.0f, -100.0f, 100.0f);
            Vec3D vec2 = new Vec3D(100.0f, -100.0f, 100.0f);
            Vec3D vec3 = new Vec3D(100.0f, 100.0f, 100.0f);
            Vec3D vec4 = new Vec3D(-100.0f, 100.0f, 100.0f);
            Vec3D vec5 = new Vec3D(-100.0f, -100.0f, -100.0f);
            Vec3D vec6 = new Vec3D(100.0f, -100.0f, -100.0f);
            Vec3D vec7 = new Vec3D(100.0f, 100.0f, -100.0f);
            Vec3D vec8 = new Vec3D(-100.0f, 100.0f, -100.0f);

            // texture coords
            Vec2D t1 = new Vec2D(-100.0f, -100.0f, 100.0f);
            Vec2D t2 = new Vec2D(100.0f, -100.0f, 100.0f);
            Vec2D t3 = new Vec2D(100.0f, 100.0f, 100.0f);
            Vec2D t4 = new Vec2D(-100.0f, 100.0f, 100.0f);
            Vec2D t5 = new Vec2D(-100.0f, -100.0f, -100.0f);
            Vec2D t6 = new Vec2D(100.0f, -100.0f, -100.0f);
            Vec2D t7 = new Vec2D(100.0f, 100.0f, -100.0f);
            Vec2D t8 = new Vec2D(-100.0f, 100.0f, -100.0f);

            // colors
            Pixel pixel1 = new Pixel(255, 0, 0);
            Pixel pixel2 = new Pixel(0, 255, 0);
            Pixel pixel3 = new Pixel(0, 0, 255);
            Pixel pixel4 = new Pixel(0, 0, 0);

            // Create Triangle 1
            TriangleObject tri1 = new TriangleObject();
            tri1.p[0] = vec1;
            tri1.p[1] = vec2;
            tri1.p[2] = vec3;
            tri1.t[0] = t1;
            tri1.t[1] = t2;
            tri1.t[2] = t3;
            tri1.col[0] = pixel1;
            tri1.col[1] = pixel2;
            tri1.col[2] = pixel3;

            // Create Triangle 2
            TriangleObject tri2 = new TriangleObject();
            tri2.p[0] = vec1;
            tri2.p[1] = vec3;
            tri2.p[2] = vec4;
            tri2.t[0] = t1;
            tri2.t[1] = t3;
            tri2.t[2] = t4;
            tri2.col[0] = pixel1;
            tri2.col[1] = pixel3;
            tri2.col[2] = pixel4;

            // Line #1
            LineObject line = new LineObject();
            line.p[0] = new Vec3D(-100.0f, -100.0f, -100.0f);
            line.p[1] = new Vec3D(1000.0f, 1000.0f, 1000.0f);
            line.col[0] = new Pixel(0, 0, 255);
            line.col[1] = new Pixel(255, 0, 0);

            // Add to our lists of triangles and lines
            TriangleList.Add(tri1);
            TriangleList.Add(tri2);
            //LineList.Add(line);

            // Create our camera for our view
            CameraPos = new Vec3D(0.0f, 50.0f, -250.0f);
            CameraTarget = new Vec3D(0.0f, 0.0f, 0.0f);
            CameraUp = new Vec3D(0.0f, 1.0f, 0.0f);

            // Create our default view matrix transform.
            Pipeline.SetTransform(MathOps.Mat_MakeIdentity());
            Pipeline.SetCamera(CameraPos, CameraTarget, CameraUp);
            Pipeline.SetProjection(45, ((float)MainCanvas.Width / (float)MainCanvas.Height), 0.1f, 1000, 0.0f, 0.0f, (float)MainCanvas.Width, (float)MainCanvas.Height);
        }
    }
}
