
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
    public class VertWall : GameObject
    {
        public Vector2 GridPosition;
        public bool Destroyed = false;
        public List<FloorTile> FloorTiles;
        public VertWall(string id, Vector3 location, Vector2 _GridPosition, List<FloorTile> _FloorTiles) : base(id, location)
        {
            GridPosition = _GridPosition;
            FloorTiles = _FloorTiles;
        }

        public override void Initialize()
        {
            //Manager.AddComponent(new BasicEffectModel(ID + "vWall", "VertHedge"));

            Manager.AddComponent(new BasicEffectModel(ID + "vWall", "VertHedge"));

            BoxComponent Wall = new BoxComponent(ID + "box", 0);
            Manager.AddComponent(Wall);

            base.Initialize();

        }
        public void ReplaceFloor()
        {
            FloorTiles.Add(new FloorTile("ID", Location, new Vector2(-2, -2), FloorTiles, null, null, null, "VertFloor"));
        }
    }
}
