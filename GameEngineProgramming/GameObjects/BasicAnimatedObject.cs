using Engine;
using GameEngineProgramming.Engine.Components.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.GameObjects
{
    public class BasicAnimatedObject : GameObject
    {
        private string asset;
        private string start;
        
        public BasicAnimatedObject(string id, Vector3 location, string asset,string startAnimation) : base(id,location)
        {
            start = startAnimation;
            this.asset = asset;
        }
        public override void Initialize()
        {
            Manager.AddComponent(new SkinnedEffectModel(ID + "amodel", asset, start));

            base.Initialize();
        }
    }
}
