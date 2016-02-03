using Engine;
using GameEngineProgramming.Engine.Components.Physics;
using GameEngineProgramming.Engine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.GameObjects
{
    class StaticModelObject : GameObject
    {
        private string asset;
        public StaticModelObject(string id,Vector3 location,string asset) : base(id, location)
        {
            this.asset = asset;
        }
        public override void Initialize()
        {
            Manager.AddComponent(new BasicEffectModel(ID + "staticModel", asset));
            Manager.AddComponent(new StaticMeshComponent(ID + "staticPhys"));
            base.Initialize();
        }
    }
}
