using Engine;
using Engine.Components.Cameras;
using GameEngineProgramming.Engine.Components.Graphics;
using GameEngineProgramming.Engine.Components.Input;
using GameEngineProgramming.Engine.Components.Physics;
using GameEngineProgramming.Engine.Graphics;
using GameEngineProgramming.MazeObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.GameObjects
{
    public class MazePlayer : GameObject
    {
        private string asset;
        private string start;
        public List<FloorTile> FloorTiles;
        //public List<Enemy> Enemies;
        //public MazeExit ExitLocation;

        public MazePlayer(string id, Vector3 location, string asset, string startAnimation, Vector3 _Direction, List<FloorTile> _FloorTiles) : base(id, location)
        {
            start = startAnimation;
            this.asset = asset;
            Direction = _Direction;
            FloorTiles = _FloorTiles;
            //Enemies = enemies;
            //ExitLocation = exitLocation;
        }



        public override void Initialize()
        {
            Dictionary<string, Vector3> Animations = new Dictionary<string, Vector3>();
            Animations.Add("Walk", new Vector3(1, 119, 0));
            //Animations.Add("Jump", new Vector2(100, 180));
            Animations.Add("Hit", new Vector3(200, 260, 1));
            Animations.Add("Idle", new Vector3(0, 0, 0));
            Manager.AddComponent(new FixedCamera("cam", Direction));
            //Manager.AddComponent(new BasicEffectModel(ID + "Jim", asset));
            //Manager.AddComponent(new BasicEffectModel(ID + "hWall", "HorizHedge"));

            //BoxComponent Wall = new BoxComponent(ID + "box", 0);

            // Manager.AddComponent(Wall);
            Manager.AddComponent(new PlayerMovementController("move", .15f, FloorTiles));
            Manager.AddComponent(new SkinnedEffectModel(ID + "amodel", asset, start, Animations));

            base.Initialize();

        }
    }
}