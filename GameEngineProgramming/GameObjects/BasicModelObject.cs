using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using GameEngineProgramming.Engine.Graphics;
using GameEngineProgramming.Engine.Components.Physics;

namespace GameEngineProgramming.GameObjects
{
    class BasicModelObject : GameObject
    {
        private string asset;
        public BasicModelObject(string id, Vector3 location, string asset)
            : base(id, location)
        {
            this.asset = asset;
        }

        public override void Initialize()
        {
            Manager.AddComponent(new BasicEffectModel(ID + "mod", "cube"));
            Manager.AddComponent(new BoxComponent(ID + "box", 10));
            //Manager.AddComponent(new BasicEffectModel(ID + "model", asset));
            //Manager.AddComponent(new RotateObject(ID + "model", new Vector3(0.03f, 0.01f, 0.1f)));

            //Manager.AddComponent(new RotateObject(ID + "model", new Vector3(0.03f, 0.01f, 0.1f)));
            //Manager.AddComponent(new BobbingObject(ID + "model", 5f, 0.1f));
            base.Initialize();
        }
    }
}
