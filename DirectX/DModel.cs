using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace DrawingPipelineLibrary.DirectX
{
    public class DModel                 // 112 lines
    {
        // Properties
        public SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        public SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        public int VertexCount { get; set; }
        public int IndexCount { get; set; }

        // Constructor
        public DModel() { }

        // Methods.
        public bool Initialize(Device device)
        {
            // Initialize the vertex and index buffer that hold the geometry for the triangle.
            return InitializeBuffer(device);
        }
        public void ShutDown()
        {
            // Release the vertex and index buffers.
            ShutDownBuffers();
        }
        public void Render(DeviceContext deviceContext)
        {
            // Put the vertex and index buffers on the graphics pipeline to prepare for drawings.
            RenderBuffers(deviceContext);
        }

        public bool InitializeBuffer(Device device)
        {
            try
            {
                // Create the vertex array and load it with data.
                var vertices = new[]
                {
					// Bottom left.
					new DColorShader.DVertex()
                    {
                        position = new Vector3(-1, -1, 0),
                        color = new Vector4(1, 1, 0, 1)
                    },
					// Top middle. TO DO 3:  Top Left.
					new DColorShader.DVertex()
                    {
                        position = new Vector3(0, 1, 0),
                        color = new Vector4(1, 1, 0, 1)
                    },
					// Bottom right.
					new DColorShader.DVertex()
                    {
                        position = new Vector3(1, -1, 0),
                        color = new Vector4(1, 0, 0, 1)
                    },
                    					// Bottom right.
					new DColorShader.DVertex()
                    {
                        position = new Vector3(0.0f, -3.0f, 0),
                        color = new Vector4(0, 1, 0, 1)
                    }
                };

                // Create Indicies for the IndexBuffer.
                int[] indicies = new int[]
                {
                    0, // Bottom left.
					2, // Top middle.
					3,  // Bottom right.
                    0,
                    1,
                    2
                };
                // Set number of vertices in the vertex array.
                VertexCount = vertices.Length;

                // Set number of vertices in the index array.
                IndexCount = indicies.Length;

                // Create the vertex buffer.
                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, vertices);

                // Create the index buffer.
                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indicies, IndexCount * sizeof(int));

                // Delete arrays now that they are in their respective vertex and index buffers.
                vertices = null;
                indicies = null;

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool InitializeBufferTestTriangle(Device device)
        {
            try
            {
                // Set number of vertices in the vertex array.
                VertexCount = 3;
                // Set number of vertices in the index array.
                IndexCount = 3;

                // Create the vertex array and load it with data.
                var vertices = new[]
                {
					// Bottom left.
					new DColorShader.DVertex()
                    {
                        position = new Vector3(-1, -1, 0),
                        color = new Vector4(0, 1, 0, 1)
                    },
					// Top middle. TO DO 3:  Top Left.
					new DColorShader.DVertex()
                    {
                        position = new Vector3(0, 1, 0),
                        color = new Vector4(0, 1, 0, 1)
                    },
					// Bottom right.
					new DColorShader.DVertex()
                    {
                        position = new Vector3(1, -1, 0),
                        color = new Vector4(0, 1, 0, 1)
                    }
                };

                // Create Indicies for the IndexBuffer.
                int[] indicies = new int[]
                {
                    0, // Bottom left.
					1, // Top middle.
					2  // Bottom right.
                };

                // Create the vertex buffer.
                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, vertices);

                // Create the index buffer.
                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indicies);

                // Delete arrays now that they are in their respective vertex and index buffers.
                vertices = null;
                indicies = null;

                return true;
            }
            catch
            {
                return false;
            }
        }
        private void ShutDownBuffers()
        {
            // Release the index buffer.
            IndexBuffer?.Dispose();
            IndexBuffer = null;
            // Release the vertex buffer.
            VertexBuffer?.Dispose();
            VertexBuffer = null;
        }
        private void RenderBuffers(DeviceContext deviceContext)
        {
            // Set the vertex buffer to active in the input assembler so it can be rendered.
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<DColorShader.DVertex>(), 0));

            // Set the index buffer to active in the input assembler so it can be rendered.
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);

            // Set the type of the primitive that should be rendered from this vertex buffer, in this case triangles.
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }
    }
}
