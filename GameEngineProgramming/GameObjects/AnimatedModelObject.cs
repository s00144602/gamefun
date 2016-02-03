using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using GameEngineProgramming.Engine.Graphics;
using GameEngineProgramming.Engine.Components.Graphics;

namespace GameEngineProgramming.GameObjects
{
    class AnimatedModelObject : GameObject
    {
        public AnimatedModelObject(string id, Vector3 location)
            : base(id, location)
        {

        }

        public override void Initialize()
        {
            Manager.AddComponent(new BasicAnimatedModel(ID + "ani", "door"));
            base.Initialize();
        }
    }
}
