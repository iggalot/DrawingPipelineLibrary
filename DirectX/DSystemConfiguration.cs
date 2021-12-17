using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace DrawingPipelineLibrary.DirectX
{
    public class DSystemConfiguration                   // 41 lines
    {
        public string Title { get; set; }

        // Dimension of the DirectX Window
        public int Width { get; set; } = 500;
        public int Height { get; set; } = 500;

        // Static Properties
        public static bool FullScreen { get; private set; }
        public static bool VerticalSyncEnabled { get; private set; }
        public static float ScreenDepth { get; private set; }
        public static float ScreenNear { get; private set; }
        public static FormBorderStyle BorderStyle { get; set; }
        public static string ShaderFilePath { get; private set; }

        // Constructors
        public DSystemConfiguration(bool fullScreen, bool vSync) : this("SharpDX Demo", fullScreen, vSync) { }
        public DSystemConfiguration(string title, bool fullScreen, bool vSync) : this(title, 800, 600, fullScreen, vSync) { }
        public DSystemConfiguration(string title, int width, int height, bool fullScreen, bool vSync)
        {
            FullScreen = fullScreen;
            Title = title;
            VerticalSyncEnabled = vSync;

            if (!FullScreen)
            {
                Width = width;
                Height = height;
            }
            else
            {
                Width = Screen.PrimaryScreen.Bounds.Width;
                Height = Screen.PrimaryScreen.Bounds.Height;
            }
        }

        // Static Constructor
        static DSystemConfiguration()
        {
            VerticalSyncEnabled = false;
            ScreenDepth = 1000.0f;
            ScreenNear = 0.1f;
            BorderStyle = FormBorderStyle.None;

            // TODO: Find a better way to locate shader files in the directory structure.
            // For the MathLibraryDriverProject, it needs another "..\..\.." instead of two "..\..Watch this path...
            ShaderFilePath = @"../../../DirectX/Shaders/";

            // For the WindProvisionsDriverProject
            ShaderFilePath = @"./DirectX/Shaders/";

        }
    }
}
