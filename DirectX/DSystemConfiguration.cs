using System.Windows.Forms;

namespace DrawingPipelineLibrary.DirectX
{
    public class DSystemConfiguration                   // 41 lines
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Static Properties
        public static FormBorderStyle BorderStyle { get; set; }
        public static bool FullScreen { get; private set; }
        public static bool VerticalSyncEnabled { get; private set; }

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
            BorderStyle = FormBorderStyle.None;
        }
    }
}
