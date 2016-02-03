using Common;
using Engine;
using GameEngineProgramming.Engine.Components.Physics;
using GameEngineProgramming.Engine.Graphics;
using GameEngineProgramming.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.MazeObjects
{
    public class FloorTile : GameObject
    {
        public bool Opened = false;
        public Vector2 GridPosition;
        public List<FloorTile> FloorTiles;
        public List<HorizWall> HorizWalls;
        public List<VertWall> VertWalls;
        Random rnd;
        bool UpChecked = false;
        bool DownChecked = false;
        bool LeftChecked = false;
        bool RightChecked = false;

        public bool UpPossible = false;
        public bool DownPossible = false;
        public bool LeftPossible = false;
        public bool RightPossible = false;

        public int Width;
        public int Depth;

        public bool Revealed = false; // For minimap
        public bool CurrentCell = false; // For minimap

        int NumberOfTiles;
        public string FloorType;
        public FloorTile(string id, Vector3 location, Vector2 _GridPosition, List<FloorTile> _FloorTiles, List<HorizWall> _HorizWalls, List<VertWall> _VertWalls, Random _rnd, string _FloorType) : base(id, location)
        {
            
            FloorType = _FloorType;
            if (FloorType == "Cell")
            {
                GridPosition = _GridPosition;
                FloorTiles = _FloorTiles;
                HorizWalls = _HorizWalls;
                VertWalls = _VertWalls;
                NumberOfTiles = FloorTiles.Count;
                rnd = _rnd;
            }
        }

        public override void Initialize()
        {
            if (FloorType == "Cell")
            {
                Width = 5;
                Depth = 5;
                Manager.AddComponent(new BasicEffectModel(ID + "floor", "FloorTile"));
                BoxComponent FloorTile = new BoxComponent(ID + "Box", 0);
                Manager.AddComponent(FloorTile);
            }
            else if (FloorType == "HorizFloor")
            {
                UpPossible = true;
                DownPossible = true;
                Width = 5;
                Depth = 1;
                Manager.AddComponent(new BasicEffectModel(ID + "floor", "HorizWallReplace"));
                BoxComponent FloorTile = new BoxComponent(ID + "Box", 0);
                Manager.AddComponent(FloorTile);
            }
            else if (FloorType == "VertFloor")
            {
                LeftPossible = true;
                RightPossible = true;
                Width = 1;
                Depth = 5;
                Manager.AddComponent(new BasicEffectModel(ID + "floor", "VertWallReplace"));
                BoxComponent FloorTile = new BoxComponent(ID + "Box", 0);
                Manager.AddComponent(FloorTile);
            }

            base.Initialize();
        }

        public bool OpenWall()
        {
            if (Opened == true)
            {
                int chooseDirection;
                chooseDirection = rnd.Next(0, 4);
                if (chooseDirection == 0 && UpChecked == false)
                {
                    UpChecked = true;

                    return CheckUp();
                }

                else if (chooseDirection == 1 && DownChecked == false)
                {
                    DownChecked = true;
                    return CheckDown();
                }
                else if (chooseDirection == 2 && LeftChecked == false)
                {
                    LeftChecked = true;
                    return CheckLeft();
                }

                else if (chooseDirection == 3 && RightChecked == false)
                {
                    RightChecked = true;
                    return CheckRight();
                }
            }
            return false;
        }
        public bool CheckUp()
        {
            Vector2 FloorTileToCheck = new Vector2(GridPosition.X, GridPosition.Y - 1);
            Vector2 WallToRemove = new Vector2(GridPosition.X, GridPosition.Y);

            for (int i = 0; i < FloorTiles.Count; i++)
            {
                if (FloorTiles[i].GridPosition == FloorTileToCheck)
                {
                    if (FloorTiles[i].Opened == false)
                    {
                        FloorTiles[i].Opened = true;
                        FloorTiles[i].DownPossible = true;
                        for (int j = 0; j < HorizWalls.Count; j++)
                        {
                            if (HorizWalls[j].GridPosition == WallToRemove)
                            {
                                UpPossible = true;
                                HorizWalls[j].ReplaceFloor();
                                HorizWalls.RemoveAt(j);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool CheckDown()
        {
            Vector2 FloorTileToCheck = new Vector2(GridPosition.X, GridPosition.Y + 1);
            Vector2 WallToRemove = new Vector2(GridPosition.X, GridPosition.Y + 1);

            for (int i = 0; i < FloorTiles.Count; i++)
            {
                if (FloorTiles[i].GridPosition == FloorTileToCheck)
                {
                    if (FloorTiles[i].Opened == false)
                    {
                        FloorTiles[i].UpPossible = true;
                        FloorTiles[i].Opened = true;

                        for (int j = 0; j < HorizWalls.Count; j++)
                        {
                            if (HorizWalls[j].GridPosition == WallToRemove)
                            {
                                DownPossible = true;
                                HorizWalls[j].ReplaceFloor();
                                HorizWalls.RemoveAt(j);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool CheckLeft()
        {
            Vector2 FloorTileToCheck = new Vector2(GridPosition.X - 1, GridPosition.Y);
            Vector2 WallToRemove = new Vector2(GridPosition.X, GridPosition.Y);

            for (int i = 0; i < FloorTiles.Count; i++)
            {
                if (FloorTiles[i].GridPosition == FloorTileToCheck)
                {
                    if (FloorTiles[i].Opened == false)
                    {
                        FloorTiles[i].RightPossible = true;
                        FloorTiles[i].Opened = true;

                        for (int j = 0; j < VertWalls.Count; j++)
                        {
                            if (VertWalls[j].GridPosition == WallToRemove)
                            {
                                LeftPossible = true;
                                VertWalls[j].ReplaceFloor();
                                VertWalls.RemoveAt(j);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool CheckRight()
        {
            Vector2 FloorTileToCheck = new Vector2(GridPosition.X + 1, GridPosition.Y);
            Vector2 WallToRemove = new Vector2(GridPosition.X + 1, GridPosition.Y);

            for (int i = 0; i < FloorTiles.Count; i++)
            {
                if (FloorTiles[i].GridPosition == FloorTileToCheck)
                {
                    if (FloorTiles[i].Opened == false)
                    {
                        FloorTiles[i].LeftPossible = true;
                        FloorTiles[i].Opened = true;

                        for (int j = 0; j < VertWalls.Count; j++)
                        {
                            if (VertWalls[j].GridPosition == WallToRemove)
                            {
                                RightPossible = true;
                                VertWalls[j].ReplaceFloor();
                                VertWalls.RemoveAt(j);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

    }
}
