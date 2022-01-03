using DrawingHelpersLibrary;
using DrawingPipelineLibrary.DirectX;
using SharpDX;
using System;
using System.Windows.Media;

namespace DrawingPipelineLibrary
{
    public class DirectXDrawingFunctions : acDrawingFunction
    {
        public override void DrawLineSolid(float sx, float sy, float ex, float ey, Brush color, double thick, Linetypes line_type)
        {
            throw new NotImplementedException();
        }

        public override void DrawTriangleFilled()
        {
            throw new NotImplementedException();
        }

        public override void DrawTriangleWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangleFilled()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangleWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawCircleFilled()
        {
            throw new NotImplementedException();
        }

        public override void DrawCircleWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectanglePrismWire()
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangularPrismFilled()
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Creates the vertices array needed for buffer creation for drawing a triangle.
        /// Must ensure that the winding order is clockwise.
        /// Starts at lower left - upper middle - lower right
        /// </summary>
        /// <param name="v1">position of lower left point</param>
        /// <param name="c1">color of lower left point</param>
        /// <param name="v2">position of top middle  point</param>
        /// <param name="c2">color of top middle point</param>
        /// <param name="v3">position of lower right point</param>
        /// <param name="c3">color of lower right point</param>
        /// <returns></returns>
        private DColorShader.DVertex[] CreateTriangleVertices(Vector3 v1, Vector4 c1, Vector3 v2, Vector4 c2, Vector3 v3, Vector4 c3)
        {
            // TODO: verify winding order is clockwise
            var vertices = new[]
            {
                new DColorShader.DVertex()
                {
                    position = v1,
                    color = c1
                },

                new DColorShader.DVertex()
                {
                    position = v2,
                    color = c2
                },

                new DColorShader.DVertex()
                {
                    position = v3,
                    color = c3
                },
            };

            return vertices;
        }

        /// <summary>
        /// Creates the vertices array needed for buffer creation for drawing a triangle.
        /// Must ensure that the winding order is clockwise.
        /// Starts at lower left - upper middle - lower right
        /// </summary>
        /// <param name="v1">position of lower left point</param>
        /// <param name="c1">color of lower left point</param>
        /// <param name="v2">position of lower right  point</param>
        /// <param name="c2">color of lower right point</param>
        /// <param name="v3">position upper right point</param>
        /// <param name="c3">color of upper right point</param>
        /// <param name="v4">position of upper left point</param>
        /// <param name="c4">color of upper left point</param>
        /// <returns></returns>
        private DColorShader.DVertex[] CreateRectangleVertices(Vector3 v1, Vector4 c1, Vector3 v2, Vector4 c2, Vector3 v3, Vector4 c3, Vector3 v4, Vector4 c4)
        {
            // TODO: verify winding order is clockwise
            var vertices = new[]
            {
                new DColorShader.DVertex()
                {
                    position = v1,
                    color = c1
                },

                new DColorShader.DVertex()
                {
                    position = v2,
                    color = c2
                },

                new DColorShader.DVertex()
                {
                    position = v3,
                    color = c3
                },

                new DColorShader.DVertex()
                {
                    position = v4,
                    color = c4
                },
            };

            return vertices;
        }
    }
}
