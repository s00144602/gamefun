using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Engines;

namespace Engine.Components.Cameras
{
    public class FixedCamera2 : CameraComponent
    {
        public FixedCamera2(string id, Vector3 direction) : base(id)
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
            CurrentTarget = Manager.Owner.Location + CameraDirection;

            View = Matrix.CreateLookAt(
               Manager.Owner.Location,
                CurrentTarget,
                UpVector);
        }
    }
}
