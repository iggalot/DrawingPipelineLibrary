using SharpDX;
using System;

namespace DrawingPipelineLibrary.DirectX
{
    public class DCamera                    // 53 lines
    {
        private const float CameraMoveSensitivity = 20;

        // Properties.
        // Original position data
        private float OriginalPosX { get; set; }
        private float OriginalPosY { get; set; }
        private float OriginalPosZ { get; set; }


        // current position data
        private float CurrentPosX { get; set; }
        private float CurrentPosY { get; set; }
        private float CurrentPosZ { get; set; }

        private float CurrentRotX { get; set; }
        private float CurrentRotY { get; set; }
        private float CurrentRotZ { get; set; }
        public Matrix ViewMatrix { get; private set; }

        public float GetX => CurrentPosX;
        public float GetY => CurrentPosY;
        public float GetZ => CurrentPosZ;

        public float GetRotX => CurrentRotX;
        public float GetRotY => CurrentRotY;
        public float GetRotZ => CurrentRotZ;

        // Default direction that the camera is facing.
        public Vector3 LookAt { get; set; } = DXMathFunctions.Vec_Normalize(new Vector3(0, 0, 1));
        public Vector3 Up { get; set; } = new Vector3(0, 1, 0);

        public float Yaw { get; set; } = -90.0f;
        public float Pitch { get; set; } = 0;
        public float Roll { get; set; } = 0;
        public bool HasMoved { get; set; } = false;

        // Is the camera currently active for receiving input?
        public bool IsActiveMode { get; set; } = false;

        public string CurrentPositionString
        {
            get
            {
                string str = "";
                str += "Current Camera Position: (" + CurrentPosX + " , " + CurrentPosY + " , " + CurrentPosZ + ")";
                str += "          Looking At: < " + LookAt.X + " ," + LookAt.Y + " , " + LookAt.Z + ">";
                return str;
            }
        }

        // Constructors
        public DCamera() { }
        public DCamera(float x, float y, float z)
        {
            OriginalPosX = x;
            OriginalPosY = y;
            OriginalPosZ = z;

            // Default the camera to looking at the origin <0,0,0>
            LookAt = DXMathFunctions.Vec_Normalize(new SharpDX.Vector3(0 - x, 0 - y, 0 - z));

            SetPosition(x, y, z);
        }
        // Methods.
        public void SetPosition(float x, float y, float z)
        {
            // if the position hasn't changed, do nothing.
            if ((CurrentPosX == x) && (CurrentPosY == y) && (CurrentPosZ == z))
            {
                HasMoved = false;
                return;
            } else
            {
                HasMoved = true;
            }

            CurrentPosX = x;
            CurrentPosY = y;
            CurrentPosZ = z;
           
            UpdateViewMatrix();
        }

        public void Render()
        {
        
        }
        public void UpdateViewMatrix()
        {
            // Setup the position of the camera in the world.
            Vector3 position = new Vector3(CurrentPosX, CurrentPosY, CurrentPosZ);

            // Setup where the camera is looking by default.
            //Vector3 lookAt = new Vector3(1, 0, 1);

            // Set the yaw (Y axis), pitch (X axis), and roll (Z axis) rotations in radians.
//            float pitch = CurrentRotX * 0.0174532925f;
//            float yaw = CurrentRotY * 0.0174532925f; ;
//            float roll = CurrentRotZ * 0.0174532925f; ;

            // Create the rotation matrix from the yaw, pitch, and roll values.
            Matrix rotationMatrix = Matrix.RotationYawPitchRoll(Yaw, Pitch, Roll);

            // Transform the lookAt and up vector by the rotation matrix so the view is correctly rotated at the origin.
            Vector3 lookAt = Vector3.TransformCoordinate(LookAt, rotationMatrix);
            Vector3 up = Vector3.TransformCoordinate(Vector3.UnitY, rotationMatrix);

            // Translate the rotated camera position to the location of the viewer.
            lookAt = position + lookAt;

            // Finally create the view matrix from the three updated vectors.
            ViewMatrix = Matrix.LookAtLH(position, lookAt, up);
        }

        /// <summary>
        /// Determines the new position of the camera a specified distance 
        /// in the direction it is facing.
        /// '+' distance is forward.
        /// </summary>
        /// <param name="dist"></param>
        public SharpDX.Vector3 MoveForward(bool move_status)
        {
            SharpDX.Vector3 curr = new SharpDX.Vector3(this.GetX, this.GetY, this.GetZ);
            float factor = 1.0f;

            // If we are moving backwards, the multiplier is negative.
            if (!move_status)
                factor = -1.0f;
            
            SharpDX.Vector3 move = DXMathFunctions.Vec_Mul(LookAt, factor * CameraMoveSensitivity);

            curr = DXMathFunctions.Vec_Add(curr, move);

            return curr;
        }

        /// <summary>
        /// Determines the new position of  the camera a specified distance to the right 
        /// (perpendicular) to the direction it is facing.
        /// '+' distance is to the right
        /// </summary>
        /// <param name="dist"></param>
        public SharpDX.Vector3 MoveRight(bool move_status)
        {
            SharpDX.Vector3 curr = new SharpDX.Vector3(this.GetX, this.GetY, this.GetZ);
            SharpDX.Vector3 moveVec = DXMathFunctions.Vec_CrossProduct(LookAt, Up);

            float factor = 1.0f;

            // If we are moving left, the multiplier is negative.
            if (!move_status)
                factor = -1.0f;

            SharpDX.Vector3 move = DXMathFunctions.Vec_Mul(moveVec, factor * CameraMoveSensitivity);
            curr = DXMathFunctions.Vec_Add(curr, move);

            return curr;
        }
    }


}
