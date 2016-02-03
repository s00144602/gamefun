using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Engine
{
    public class CameraComponent : Component
    {
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        public Vector3 CurrentTarget {get; set; }
        public Vector3 CameraDirection { get; set; }
        public Vector3 UpVector { get; set; }

        public CameraComponent(string id) : base(id) { }
    }
}
