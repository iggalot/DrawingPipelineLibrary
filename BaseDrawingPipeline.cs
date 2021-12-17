using DrawingPipelineLibrary;
using DrawingPipelineLibrary.DirectX;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using static MathLibrary.MathVectors;

namespace DrawingPipeline
{
    [Flags]
    public enum RENDERFLAGS
    {
        RENDER_WIRE = 0x01,
        RENDER_FLAT = 0x02,
        RENDER_TEXTURED = 0x04,
        RENDER_CULL_CW = 0x08,
        RENDER_CULL_CCW = 0x16,
        RENDER_DEPTH = 0x32,
        RENDER_LIGHTS = 0x64,
    }

    [Flags]
    public enum LightsType
    {
        LIGHT_DISABLED,
        LIGHT_AMBIENT,
        LIGHT_DIRECTIONAL,
        LIGHT_POINT
    }

    public class BaseDrawingPipeline : IDisposable
    {
        private int m_RefreshTimer;

        public acDrawingFunction DrawFunc;

        // The DirectX System parameter
        public DSystem GetDSystem { get; set; }
        public int RefreshTimer { get; set; }

        private int DisplayWidth { get; set; }
        private int DisplayHeight { get; set; }

        public BaseDrawingPipeline(int display_width, int display_height, int timer)
        {
            DisplayWidth = display_width;
            DisplayHeight = display_height;

            RefreshTimer = timer;
        }

        public virtual void Run() { }

        public virtual void Dispose() { }

        public virtual void SetKeyState(Key key, bool val) { }

        public virtual int Update() 
        { 
            throw new NotImplementedException("in BaseDrawingPipeline:  Update not implemented for base clase");
        }

        public virtual int Render(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags)
        {
            throw new NotImplementedException("in BaseDrawingPipeline:  Render not implemented for base clase");
        }
    }
}
