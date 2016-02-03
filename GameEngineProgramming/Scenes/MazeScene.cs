using CrappyBird;
using Engine;
using GameEngineProgramming.Bowling;
using GameEngineProgramming.Engine.Base;
using GameEngineProgramming.GameObjects;
using GameEngineProgramming.MazeObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Scenes
{
    public class MazeScene : Scene
    {
        SpriteBatch batch;
        SpriteFont bigFont;
        MazePlayer player;


        public int HorizWallWidth = 5;
        public int HorizWallDepth = 1;

        public int VertWallWidth = 1;
        public int VertWallDepth = 5;

        public int MazeWidth = 23;
        public int MazeHeight = 17;

        List<FloorTile> FloorTiles = new List<FloorTile>();
        List<HorizWall> HorizWalls= new List<HorizWall>();
        List<VertWall> VertWalls = new List<VertWall>();
        MiniMap miniMap;
        Texture2D MiniMapRevealed;
        Random rnd = new Random();
        //Axe PlayersAxe;
        public MazeScene(string id, GameEngine engine) : base(id, engine) { }

        #region CreateMazeBase
        public void CreateCells()
        {
            int CellPositionX = VertWallWidth;
            int CellPositionZ = HorizWallDepth;
            int CellColumns = MazeWidth;
            int CellRows = MazeHeight;
            for (int i = 0; i < CellRows; i++)
            {
                for (int j = 0; j < CellColumns; j++)
                {
                    FloorTiles.Add(new FloorTile("floorTile" + i.ToString() + j.ToString(), new Vector3(CellPositionX, 0, CellPositionZ), new Vector2(j, i),FloorTiles,HorizWalls,VertWalls,rnd,"Cell"));
                    CellPositionX += (HorizWallWidth + VertWallWidth);
                }
                CellPositionX = VertWallWidth;
                CellPositionZ += VertWallDepth + HorizWallDepth;
            }
        }
        public void CreateVertWalls()
        {
            int VertWallsPositionX = 0;
            int VertWallsPositionZ = HorizWallDepth;
            int VertWallsColumns = MazeWidth+1;
            int VertWallsRows = MazeHeight;
            int VertWallsDistance = VertWallWidth + HorizWallWidth;
            for (int i = 0; i < VertWallsRows; i++)
            {
                for (int j = 0; j < VertWallsColumns; j++)
                {

                    VertWalls.Add(new VertWall("vWall" + i.ToString() + j.ToString(), new Vector3(VertWallsPositionX, 0, VertWallsPositionZ),new Vector2(j, i),FloorTiles));

                    VertWallsPositionX += VertWallsDistance;

                }
                VertWallsPositionX = 0;
                VertWallsPositionZ += HorizWallDepth + VertWallDepth;
            }
        }
        public void CreateHorizWalls()
        {
            int HorizWallsPositionX = VertWallWidth;
            int HorizWallsPositionZ = 0;
            int HorizWallsColumns = MazeWidth;
            int HorizWallsRows = MazeHeight + 1;
            int HorizWallsDistance = HorizWallWidth + VertWallWidth;
            for (int i = 0; i < HorizWallsRows; i++)
            {
                for (int j = 0; j < HorizWallsColumns; j++)
                {

                    HorizWalls.Add(new HorizWall("hWall" + i.ToString() + j.ToString(), new Vector3(HorizWallsPositionX, 0, HorizWallsPositionZ),new Vector2(j,i),FloorTiles));

                    HorizWallsPositionX += HorizWallsDistance;

                }
                HorizWallsPositionX = VertWallWidth;
                HorizWallsPositionZ += HorizWallDepth + VertWallDepth;
            }
        }
        public void CreatePillars()
        {
            int PillarPositionX = 0;
            int PillarPositionZ = 0;
            int PillarColumns = MazeWidth + 1;
            int PillarRows = MazeHeight + 1;
            int PillarDistance = HorizWallWidth + VertWallWidth;
            for (int i = 0; i < PillarRows; i++)
            {
                for (int j = 0; j < PillarColumns; j++)
                {

                    AddObject(new Pillar("pillar" + i.ToString() + j.ToString(), new Vector3(PillarPositionX, 0, PillarPositionZ)));

                    PillarPositionX += PillarDistance;

                }
                PillarPositionX = 0;
                PillarPositionZ += HorizWallDepth + VertWallDepth;
            }
        }
        #endregion CreateMazeBase 

        public override void Initialize()
        {

            Vector3 WallDimentions = new Vector3(5, 4, 1);
            

            batch = new SpriteBatch(GameUtilities.GraphicsDevice);
            bigFont = GameUtilities.NonPersistentContent.Load<SpriteFont>("bigFont");

            MiniMapRevealed = GameUtilities.NonPersistentContent.Load<Texture2D>("Textures/MiniMapRevealed");


            miniMap = new MiniMap(FloorTiles, MiniMapRevealed,new Vector2(MazeWidth, MazeHeight));

            //player = new BowlingPlayer("player0", new Vector3(0, 5, 40));
            //AddObject(new BasicModelObject("cube", new Vector3(-4, 4, 0), "cube"));
            //AddObject(new Wall("wall", new Vector3(1, 0, 0),new Vector3(0,MathHelper.ToRadians(90),0)));

            CreateCells();
            CreateHorizWalls();
            CreateVertWalls();
            /* for (int i=0; i<Pool.Count;i++)
             {
                 if (Pool[i].GetType() == typeof(FloorTile))
                 {
                     FloorTiles.Add(Pool[i] as FloorTile);
                 }

                 if (Pool[i].GetType() == typeof(HorizWall))
                 {
                     HorizWalls.Add(Pool[i] as HorizWall);
                 }

                 if (Pool[i].GetType() == typeof(VertWall))
                 {
                     VertWalls.Add(Pool[i] as VertWall);
                 }
             }*/
            CreatePillars();
            int ClosedCells = FloorTiles.Count;

            FloorTiles[FloorTiles.Count / 2].Opened = true;
            ClosedCells--;

            while (ClosedCells > 0)
            {
                for (int i = 0; i < FloorTiles.Count; i++)
                {
                    if (FloorTiles[i].FloorType == "Cell")
                        if (FloorTiles[i].OpenWall())
                        {
                            ClosedCells--;
                        }
                }
            }
            
            foreach(FloorTile cell in FloorTiles)
            {
                pool.Add(cell);
            }
            foreach (HorizWall hWall in HorizWalls)
            {
                pool.Add(hWall);
            }
            foreach (VertWall vWall in VertWalls)
            {
                pool.Add(vWall);
            }
            //AddObject(new BasicAnimatedObject("player0", new Vector3(60, 2, 0), "wagging_tail", "Take 001"));

            //pool.Add(new MazePlayer("mazePlayer", new Vector3(0, 18,6)));
            //player = new Week5Player("player0", new Vector3(39, 2, 39), new Vector3(0, 0, -1), FloorTiles);
            player = new MazePlayer("player0", new Vector3(0, 0, 0), "Test", "Take 001",new Vector3(0, 0, 1), FloorTiles);

            AddObject(player);
            //PlayersAxe = new Axe("axe", new Vector3(0, 0, 0), "Sword", player);
            //AddObject(PlayersAxe);
            base.Initialize();
            Engine.Cameras.AddCamera((CameraComponent)player.Manager.GetComponent("cam"));
        }
        public override void HandleInput()
        {

            base.HandleInput();
        }
        public override void DrawUI()
        {
            batch.Begin();
            //batch.DrawString(bigFont, player.Location.ToString(), Vector2.Zero, Color.Red);
            miniMap.Draw(batch);
            batch.End();
            base.DrawUI();
        }
    }
}

