using DrawingPipeline.DirectX;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;

namespace DrawingPipelineLibrary.DirectX
{
    public enum ModelElementTypes
    {
        MODEL_ELEMENT_TRIANGLE,
        MODEL_ELEMENT_LINE
    }
    public class DModel                 // 112 lines
    {
        // Properties
        public SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        public SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        public int VertexCount { get; set; }
        public int IndexCount { get; set; }
        public DTexture Texture { get; private set; }

        public ModelElementTypes ModelElementType { get; set; }

        // Constructor
        public DModel() { }

        public bool Initialize(SharpDX.Direct3D11.Device device, ModelElementTypes element_type, bool use_texture, string textureFileName)
        {
            // Initialize the vertex and index buffer that hold the geometry for the triangle.
            if(use_texture && !(String.IsNullOrEmpty(textureFileName)))
            {
                if (!InitializeBuffer_Texture(device, element_type, textureFileName))
                    return false;

                if (!LoadTexture(device, textureFileName))
                    return false;
            } else
            {
                if (!InitializeBuffer_NoTexture(device, element_type))
                    return false;

                if (!LoadTexture(device, textureFileName))
                    return false;
            }

            return true;
        }

        private bool LoadTexture(Device device, string textureFileName)
        {
            throw new NotImplementedException();
        }

        // Methods.
        public void ShutDown()
        {
            // Release the model texture
            ReleaseTexture();

            // Release the vertex and index buffers.
            ShutDownBuffers();
        }

        private void ReleaseTexture()
        {
            // Release the texture object.
            Texture?.ShutDown();
            Texture = null;
        }

        public void Render(DeviceContext deviceContext)
        {
            // Put the vertex and index buffers on the graphics pipeline to prepare for drawings.
            RenderBuffers(deviceContext);
        }

        /// <summary>
        /// Creates the vertices array needed for buffer creation for drawing a triangle using textures.
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
        private DTextureShader.DVertex[] CreateTriangleVertices_Texture(Vector3 v1, Vector2 c1, Vector3 v2, Vector2 c2, Vector3 v3, Vector2 c3)
        {
            // TODO: verify winding order is clockwise
            var vertices = new[]
            {
                new DTextureShader.DVertex()
                {
                    position = v1,
                    texture = c1
                },

                new DTextureShader.DVertex()
                {
                    position = v2,
                    texture = c2
                },

                new DTextureShader.DVertex()
                {
                    position = v3,
                    texture = c3
                },
            };

            return vertices;
        }


        /// <summary>
        /// Creates the vertices array needed for buffer creation for drawing a triangle without textures.
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
        private DColorShader.DVertex[] CreateTriangleVertices_NoTexture(Vector3 v1, Vector4 c1, Vector3 v2, Vector4 c2, Vector3 v3, Vector4 c3)
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
        /// Creates the vertices array needed for buffer creation for drawing a rectangle with textures.
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
        private DTextureShader.DVertex[] CreateRectangleVertices_Texture(Vector3 v1, Vector2 c1, Vector3 v2, Vector2 c2, Vector3 v3, Vector2 c3, Vector3 v4, Vector2 c4)
        {
            // TODO: verify winding order is clockwise
            var vertices = new[]
            {
                new DTextureShader.DVertex()
                {
                    position = v1,
                    texture = c1
                },

                new DTextureShader.DVertex()
                {
                    position = v2,
                    texture = c2
                },

                new DTextureShader.DVertex()
                {
                    position = v3,
                    texture = c3
                },
                               
                new DTextureShader.DVertex()
                {
                    position = v4,
                    texture = c4
                },
            };

            return vertices;
        }

        /// <summary>
        /// Creates the vertices array needed for buffer creation for drawing a rectangle with no textures.
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
        private DColorShader.DVertex[] CreateRectangleVertices_NoTexture(Vector3 v1, Vector4 c1, Vector3 v2, Vector4 c2, Vector3 v3, Vector4 c3, Vector3 v4, Vector4 c4)
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


        public bool InitializeBuffer_NoTexture(Device device, ModelElementTypes element_type)
        {
            try
            {
                // TODO:  Move this to some sort of data read
                // pt 1 (lower left)
                var p1 = new Vector3(-1, -1, 0);
                var c1 = new Vector4(1, 1, 0, 1);

                // pt2 (upper middle)
                var p2 = new Vector3(0, 1, 0);
                var c2 = new Vector4(1, 1, 0, 1);

                // pt3 (lower right)
                var p3 = new Vector3(1, -1, 0);
                var c3 = new Vector4(1, 0, 0, 1);

                // pt4 (lower middle)

                var p4 = new Vector3(0.0f, -3.0f, 0);
                var c4 = new Vector4(0, 1, 0, 1);


                // Create the vertex array and load it with data.
                var vertices = CreateRectangleVertices_NoTexture(p1, c1, p2, c2, p3, c3, p4, c4);

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

                ModelElementType = element_type;

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

        public bool InitializeBuffer_Texture(Device device, ModelElementTypes element_type, string textureFileName)
        {
            try
            {
                // TODO:  Move this to some sort of data read
                // pt 1 (lower left)
                var p1 = new Vector3(-1, -1, 0);
                var c1 = new Vector2(0, 0);

                // pt2 (upper left)
                var p2 = new Vector3(0, 1, 0);
                var c2 = new Vector2(0, 1);

                // pt3 (lower right)
                var p3 = new Vector3(1, -1, 0);
                var c3 = new Vector2(1, 0);

                // pt4 (upper right)

                var p4 = new Vector3(0.0f, -3.0f, 0);
                var c4 = new Vector2(1, 1);


                // Create the vertex array and load it with data.
                var vertices = CreateRectangleVertices_Texture(p1, c1, p2, c2, p3, c3, p4, c4);

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

                ModelElementType = element_type;

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

        public bool InitializeBufferTestTriangle_NoTexture(Device device, ModelElementTypes element_type)
        {
            try
            {
                // Set number of vertices in the vertex array.
                VertexCount = 3;
                // Set number of vertices in the index array.
                IndexCount = 3;

                var vertices = new[]
                {
                // Bottom left.
                new DColorShader.DVertex()
                               {
                                   position = new Vector3(-1, -1, 0),
                                   color = new Vector4(0, 1, 0, 0)
                               },
                // Top middle. TO DO 3:  Top Left.
                new DColorShader.DVertex()
                               {
                                   position = new Vector3(0, 1, 0),
                                   color = new Vector4(0.5f, 0, 0, 0)
                               },
                // Bottom right.
                new DColorShader.DVertex()
                               {
                                   position = new Vector3(1, -1, 0),
                                   color = new Vector4(1, 1, 0 , 0)
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

                ModelElementType = element_type;

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

        public bool InitializeBufferTestTriangle_Texture(Device device, ModelElementTypes element_type)
        {
            try
            {
                // Set number of vertices in the vertex array.
                VertexCount = 3;
                // Set number of vertices in the index array.
                IndexCount = 3;

                // Create the vertex array and load it with data.
                //           var vertices = new[]
                //           {
                //// Bottom left.
                //new DColorShader.DVertex()
                //               {
                //                   position = new Vector3(-1, -1, 0),
                //                   color = new Vector4(0, 1, 0, 1)
                //               },
                //// Top middle. TO DO 3:  Top Left.
                //new DColorShader.DVertex()
                //               {
                //                   position = new Vector3(0, 1, 0),
                //                   color = new Vector4(0, 1, 0, 1)
                //               },
                //// Bottom right.
                //new DColorShader.DVertex()
                //               {
                //                   position = new Vector3(1, -1, 0),
                //                   color = new Vector4(0, 1, 0, 1)
                //               }
                //           };

                var vertices = new[]
                {
                // Bottom left.
                new DTextureShader.DVertex()
                               {
                                   position = new Vector3(-1, -1, 0),
                                   texture = new Vector2(0, 1)
                               },
                // Top middle. TO DO 3:  Top Left.
                new DTextureShader.DVertex()
                               {
                                   position = new Vector3(0, 1, 0),
                                   texture = new Vector2(0.5f, 0)
                               },
                // Bottom right.
                new DTextureShader.DVertex()
                               {
                                   position = new Vector3(1, -1, 0),
                                   texture = new Vector2(1, 1)
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

                ModelElementType = element_type;

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
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<DTextureShader.DVertex>(), 0));

            // Set the index buffer to active in the input assembler so it can be rendered.
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);

            // Set the type of the primitive that should be rendered from this vertex buffer, in this case triangles.
            switch (ModelElementType)
            {
                case ModelElementTypes.MODEL_ELEMENT_TRIANGLE:
                    deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                    break;
                case ModelElementTypes.MODEL_ELEMENT_LINE:
                    deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
                    break;
                default:
                    break;
            }
        }
    }
}
