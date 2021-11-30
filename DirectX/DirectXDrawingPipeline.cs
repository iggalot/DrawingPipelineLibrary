using DrawingPipelineLibrary.DirectX;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using static MathLibrary.MathVectors;
using D3D11 = SharpDX.Direct3D11;

namespace DrawingPipeline.DirectX
{
    public class DirectXDrawingPipeline : BaseDrawingPipeline
    {
        private SharpDX.Mathematics.Interop.RawViewportF viewport;
        private D3D11.VertexShader vertexShader;
        private D3D11.PixelShader pixelShader;

        public ConstantBuffer<BasicEffectVertexConstants> _vertexConstantBuffer;

        private D3D11.InputElement[] inputElements = new D3D11.InputElement[]
        {
            new D3D11.InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, D3D11.InputClassification.PerVertexData, 0),
            new D3D11.InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0, D3D11.InputClassification.PerVertexData, 0)
        };
        private ShaderSignature inputSignature;
        private D3D11.InputLayout inputLayout;

        private VertexPositionColor[] vertices = new VertexPositionColor[]
        {
            new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.0f), SharpDX.Color.Red),
            new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.0f), SharpDX.Color.Blue),
            new VertexPositionColor(new Vector3(0.0f, -0.5f, 0.0f), SharpDX.Color.Green)
        };

        [StructLayout(LayoutKind.Explicit, Size = 128)]
        public struct BasicEffectVertexConstants
        {
            [FieldOffset(0)]
            public Mat4x4 WorldViewProjection;

            [FieldOffset(64)]
            public Mat4x4 World;
        }


        public class ConstantBuffer<T> : IDisposable where T : struct
        {
            private readonly D3D11.Device _device;
            private readonly D3D11.Buffer _buffer;
            private readonly DataStream _dataStream;

            public D3D11.Buffer Buffer
            {
                get { return _buffer; }
            }

            public ConstantBuffer(D3D11.Device device)
            {
                _device = device;

                // If no specific marshalling is needed, can use
                // SharpDX.Utilities.SizeOf<T>() for better performance.
                int size = Marshal.SizeOf(typeof(T));

                _buffer = new D3D11.Buffer(device, new BufferDescription
                {
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.ConstantBuffer,
                    SizeInBytes = size,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                });

                _dataStream = new DataStream(size, true, true);
            }

            public void UpdateValue(T value)
            {
                // If no specific marshalling is needed, can use 
                // dataStream.Write(value) for better performance.
                Marshal.StructureToPtr(value, _dataStream.DataPointer, false);

                var dataBox = new SharpDX.DataBox(_dataStream.DataPointer);
                _device.ImmediateContext.UpdateSubresource(dataBox, _buffer, 0);
            }

            public void Dispose()
            {
                if (_dataStream != null)
                    _dataStream.Dispose();
                if (_buffer != null)
                    _buffer.Dispose();
            }
        }

        private D3D11.Buffer triangleVertexBuffer;
        private D3D11.Device d3dDevice;
        private D3D11.DeviceContext d3dDeviceContext;
        private SwapChain swapChain;

        private D3D11.RenderTargetView renderTargetView;
        private RenderForm renderForm;

        public DirectXDrawingPipeline()
        {
            // Start of the new system...
            DSystem system = new DSystem();
            DSystem.StartRenderForm("DirectX Window",1200,900, true, false);

            //// Old Pipeline system stuff
            //renderForm = new RenderForm("My first SharpDX game");
            //renderForm.ClientSize = new Size((int)Width, (int)Height);
            //renderForm.AllowUserResizing = false;

            //InitializeDeviceResources();
            //InitializeShaders();

            //InitializeTriangles();
        }

        public override int Render(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags)
        {
            InitializeDeviceResources();
            InitializeShaders();
            InitializeTriangles(TriangleList);

            //base.Render(TriangleList, LineList, flags);

            //Draw();
            Run();
            return 0;
        }

        public override int Update(List<TriangleObject> TriangleList, List<LineObject> LineList, RENDERFLAGS flags)
        {
            Render(TriangleList, LineList, flags);
            return 0;
        }

        public void Run()
        {
            // Start the render loop
            RenderLoop.Run(renderForm, RenderCallback);
        }

        private void RenderCallback()
        {
            Draw();
        }

        public override void Dispose()
        {
            Console.WriteLine("Disposing");
            inputLayout.Dispose();
            inputSignature.Dispose();
            triangleVertexBuffer.Dispose();
            vertexShader.Dispose();
            pixelShader.Dispose();
            renderTargetView.Dispose();
            swapChain.Dispose();
            d3dDevice.Dispose();
            d3dDeviceContext.Dispose();
            renderForm.Dispose();
        }

        private void InitializeDeviceResources()
        {
            ModeDescription backBufferDesc = new ModeDescription((int)Width, (int)Height, new Rational(60, 1), Format.R8G8B8A8_UNorm);

            // Descriptor for the swap chain
            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = renderForm.Handle,
                IsWindowed = true
            };

            RasterizerStateDescription rasterStateDesc = new RasterizerStateDescription
            {
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = true,   // default for DirectX is clockwise
                IsMultisampleEnabled = true,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0
            };

            // Set the swap chain
            D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out d3dDevice, out swapChain);
            d3dDeviceContext = d3dDevice.ImmediateContext;

            // Set the raster state for the device context
            D3D11.RasterizerState rasterState = new RasterizerState(d3dDevice, rasterStateDesc);
            d3dDeviceContext.Rasterizer.State = rasterState;

            using (D3D11.Texture2D backBuffer = swapChain.GetBackBuffer<D3D11.Texture2D>(0))
            {
                renderTargetView = new D3D11.RenderTargetView(d3dDevice, backBuffer);
            }

            // Set viewport
            viewport = new SharpDX.Viewport(0, 0, (int) Width, (int) Height);

            d3dDeviceContext.Rasterizer.SetViewport(viewport);

            // create our constant buffer
            _vertexConstantBuffer = new ConstantBuffer<BasicEffectVertexConstants>(d3dDevice);

            var vertexConstants = new BasicEffectVertexConstants();

            // TODO verify the order here.
            Mat4x4 wvp = MathOps.Mat_MultiplyMatrix(GetMatWorld, GetMatView);
            wvp = MathOps.Mat_MultiplyMatrix(wvp, GetMatProj);
            wvp = MathOps.Mat_Transpose(wvp);

            vertexConstants.WorldViewProjection = wvp;
            vertexConstants.World = MathOps.Mat_Transpose(GetMatWorld);

            _vertexConstantBuffer.UpdateValue(vertexConstants);
            d3dDeviceContext.VertexShader.SetConstantBuffer(0, _vertexConstantBuffer.Buffer);

        }

        private void InitializeTriangles(List<TriangleObject> triList)
        {
            // set the vertices
            vertices = CreateVerticesFromTriangleList(triList);

            if (vertices.Length == 0)
                return;

            triangleVertexBuffer = D3D11.Buffer.Create<VertexPositionColor>(d3dDevice, D3D11.BindFlags.VertexBuffer, vertices);
        }

        private void InitializeShaders()
        {
            using (var vertexShaderByteCode = ShaderBytecode.CompileFromFile("vertexShader.hlsl", "main", "vs_4_0", ShaderFlags.Debug))
            {
                inputSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
                vertexShader = new D3D11.VertexShader(d3dDevice, vertexShaderByteCode);
            }
            using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile("pixelShader.hlsl", "main", "ps_4_0", ShaderFlags.Debug))
            {
                pixelShader = new D3D11.PixelShader(d3dDevice, pixelShaderByteCode);
            }

            // Set as current vertex and pixel shaders
            d3dDeviceContext.VertexShader.Set(vertexShader);
            d3dDeviceContext.PixelShader.Set(pixelShader);

            // Declare that the data is a list of triangles
            d3dDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            inputLayout = new D3D11.InputLayout(d3dDevice, inputSignature, inputElements);
            d3dDeviceContext.InputAssembler.InputLayout = inputLayout;
        }

        private void Draw()
        {
            // Set back buffer as current render target view
            d3dDeviceContext.OutputMerger.SetRenderTargets(renderTargetView);

            // Clear the screen -- Blue color RGBA(32, 103, 178, 255)
            d3dDeviceContext.ClearRenderTargetView(renderTargetView, new SharpDX.Color(32, 103, 178));

            d3dDeviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(triangleVertexBuffer, Utilities.SizeOf<VertexPositionColor>(), 0));
            d3dDeviceContext.Draw(vertices.Count(), 0);

            // Swap the front and back buffer
            if (swapChain.Present(1, PresentFlags.None) != Result.Ok)
                Console.WriteLine("error in presenting"); ;
        }

        public override void DrawTriangleFlat(TriangleObject tri)
        {
        }

        public override void DrawTriangleWire(TriangleObject tri, Pixel col)
        {

        }

        public override void DrawTriangleTex(TriangleObject tri, Pixel col)
        {
            base.DrawTriangleTex(tri, col);
        }

        public override void DrawLineObject(LineObject line, Pixel col1, Pixel col2)
        {

        }

        public override void TexturedTriangle(int x1, int y1, float u1, float v1, float w1, int x2, int y2, float u2, float v2, float w2, int x3, int y3, float u3, float v3, float w3)
        {
            base.TexturedTriangle(x1, y1, u1, v1, w1, x2, y2, u2, v2, w2, x3, y3, u3, v3, w3);
        }

        public override void RasterTriangle(int x1, int y, float u1, float v1, float w1, Pixel c1, int x2, int y2, float u2, float v2, float w2, Pixel c2, int x3, int y3, float u3, float v3, float w3, Pixel c3, uint nFlags)
        {
            base.RasterTriangle(x1, y, u1, v1, w1, c1, x2, y2, u2, v2, w2, c2, x3, y3, u3, v3, w3, c3, nFlags);
        }


        public override LineObject RasterSingleLine(LineObject lineRaster, RENDERFLAGS flags)
        {
            return base.RasterSingleLine(lineRaster, flags);    
        }

        public override TriangleObject RasterSingleTriangle(TriangleObject triRaster, RENDERFLAGS flags)
        {
            return base.RasterSingleTriangle(triRaster, flags);
        }

        /// <summary>
        /// Converts a list of TriangleObjects to the vertices format used by DirectX11
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private VertexPositionColor[] CreateVerticesFromTriangleList(List<TriangleObject> list)
        {
            VertexPositionColor[] newList;
            // if there are no items in the list, do nothing.
            if (list.Count <= 0)
                return new VertexPositionColor[0];

            // Three vertices per list item
            newList = new VertexPositionColor[list.Count * 3];

            // Convert the TriangleObject to a vertices object.
            int count = 0;
            foreach (TriangleObject item in list)
            {
                for (int j = 0; j < 3; j++)
                {

                    //TODO:  Fix the arbitrary scaling by using the homogenous coordinates instead of the triangle list coordinates.
                                        newList[count] = new VertexPositionColor(new Vector3(item.p[j].X / 400, item.p[j].Y / 300, 0.0f),
                                            new Color4(item.col[j].r/255.0f, item.col[j].g / 255.0f, item.col[j].b / 255.0f, item.col[j].a / 255.0f));
                    count++;
                }
            }

            //newList = new VertexPositionColor[2 * 3];


            //newList[0] = new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0), new Color4(1, 0, 0, 1));
            //newList[1] = new VertexPositionColor(new Vector3(0.5f, 0.5f, 0), new Color4(0, 0, 1, 1));
            //newList[2] = new VertexPositionColor(new Vector3(0.0f, -0.5f, 0), new Color4(0, (float)0.5019608, 0, 1));

            //newList[3] = new VertexPositionColor(new Vector3(0.5f, 0.5f, 0), new Color4(1, 0, 0, 1));
            //newList[4] = new VertexPositionColor(new Vector3(1.0f, 0.5f, 0), new Color4(1, 0, 1, 1));
            //newList[5] = new VertexPositionColor(new Vector3(0.75f, -0.5f, 0), new Color4(1, 0, 0, 1));

            return newList;
            //return vertices;
        }
    }
}
