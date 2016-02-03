using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Engines;

namespace Engine.Components.Cameras
{
    public class FixedCamera : CameraComponent
    {
        float xDirection = .05f;
        float zDirection = .05f;
        public FixedCamera(string id, Vector3 direction) : base(id)
        {
            CameraDirection = direction;
            Enabled = true;


        }

        public override void Initialize()
        {
            NearPlane = 1.0f;
            FarPlane = 1000.0f;
            UpVector = Vector3.Up;
            CameraDirection.Normalize();

            Update();

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio,
                NearPlane,
                FarPlane);
        }

        public override void Update()
        {
            CameraDirection = Manager.Owner.Direction;

            Vector3 CameraPosition = new Vector3(
                Manager.Owner.Location.X - (CameraDirection.X * 5.5f),
                Manager.Owner.Location.Y + .5f  - (CameraDirection.Y * 2),
                Manager.Owner.Location.Z - (CameraDirection.Z * 5.5f));

            CurrentTarget = CameraPosition + CameraDirection;

            View = Matrix.CreateLookAt(
                CameraPosition,
                CurrentTarget,
                UpVector);
        }
    }
}
