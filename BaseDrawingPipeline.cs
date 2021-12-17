using DrawingPipelineLibrary;
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
        public acDrawingFunction DrawFunc;
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }

        public BaseDrawingPipeline(int display_width, int display_height)
        {
            DisplayWidth = display_width;
            DisplayHeight = display_height;
        }

        public virtual void RunPipeline() { }

        public virtual void Dispose() { }

        public virtual void SetKeyState(Key key, bool val) { }

        public virtual int Update(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags) 
        { 
            throw new NotImplementedException("in BaseDrawingPipeline:  Update not implemented for base clase");
        }

        public virtual int Render(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags)
        {
            throw new NotImplementedException("in BaseDrawingPipeline:  Render not implemented for base clase");
        }
    }
}
