
using Common;
using Engine;
using GameEngineProgramming.Engine.Components.Physics;
using GameEngineProgramming.Engine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Bowling
{
    public class Wall : GameObject
    {
        public Wall(string id, Vector3 location) : base(id, location)
        {
        }

        public override void Initialize()
        {
            //Manager.AddComponent(new BasicEffectModel(ID + "hWall", "Wall"));
            //BoxComponent Wall = new BoxComponent(ID + "hWall", 0);
            //Manager.AddComponent(Wall);
            
            base.Initialize();

        }
    }
}
