using Engine;
using Engine.Components.Cameras;
using GameEngineProgramming.Engine.Components.Graphics;
using GameEngineProgramming.Engine.Components.Input;
using GameEngineProgramming.Engine.Graphics;
using GameEngineProgramming.GameObjects;
using GameEngineProgramming.MazeObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.MazeObjects
{
    public class Sword : GameObject
    {
        private string asset;
        private string start;
        public MazePlayer Player;

        public Sword(string id, Vector3 location, string asset, MazePlayer player) : base(id, location)
        {
            this.asset = asset;
            Player = player;
        }
        public override void Initialize()
        {
            Manager.AddComponent(new BasicEffectModel(ID + "amodel", asset));
            Manager.AddComponent(new HeldItemController("Sword", Player,true));
            Manager.AddComponent(new FixedCamera("cam", Direction));

            base.Initialize();
        }
    }
}
