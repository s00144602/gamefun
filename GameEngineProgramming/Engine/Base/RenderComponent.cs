using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class RenderComponent : Component
    {
        public RenderComponent(string id) : base(id) { }

        public virtual void Draw(CameraComponent camera) { }
    }
}
