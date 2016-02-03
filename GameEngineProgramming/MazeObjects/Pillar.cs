
using Common;
using Engine;
using GameEngineProgramming.Engine.Components.Physics;
using GameEngineProgramming.Engine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.MazeObjects
{
    public class Pillar : GameObject
    {
        public Pillar(string id, Vector3 location) : base(id, location)
        {
        }

        public override void Initialize()
        {
            Manager.AddComponent(new BasicEffectModel(ID + "pillar", "Pillar"));
            BoxComponent Pillar = new BoxComponent(ID + "pillar", 0);
            Manager.AddComponent(Pillar);

            base.Initialize();

        }
    }
}
