using SharpDX;
using System;

namespace DrawingPipelineLibrary.DirectX
{
    public class DCamera                    // 53 lines
    {
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
        public Vector3 LookAt { get; set; } = new Vector3(1, 0, 1);

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public float Roll { get; set; } = 0;
        public bool HasMoved { get; set; } = false;

        // Is the camera currently active for receiving input?
        public bool IsActiveMode { get; set; } = false;

        // Constructors
        public DCamera() { }
        public DCamera(float x, float y, float z)
        {
            OriginalPosX = x;
            OriginalPosY = y;
            OriginalPosZ = z;
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
    }
}
