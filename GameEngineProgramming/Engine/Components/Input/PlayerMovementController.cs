using Engine;
using Engine.Engines;
using GameEngineProgramming.MazeObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Input
{
    public class PlayerMovementController : Component
    {
        public float MovementSpeed { get; set; }
        MouseState CurMouseState = Mouse.GetState();
        MouseState CentredMouse = new MouseState();
        Matrix Next;
        List<FloorTile> FloorTiles;
        public float UpLimit;
        public float DownLimit;
        public float LeftLimit;
        public float RightLimit;
        FloorTile DetectedFloorTile;
        SpriteFont font;
        SpriteBatch batch = new SpriteBatch(GameUtilities.GraphicsDevice);

        public PlayerMovementController(string id)
            : base(id)
        {
            MovementSpeed = 0.05f;
        }

        public PlayerMovementController(string id, float hSpeed, List<FloorTile> _FloorTiles)
           : base(id)
        {
            MovementSpeed = hSpeed;
            CentredMouse = Mouse.GetState();
            FloorTiles = _FloorTiles;
            font = GameUtilities.NonPersistentContent.Load<SpriteFont>("debug");


        }
        public void DetectPath()
        {
            Vector2 _PlayerLocation = new Vector2(Manager.Owner.Location.X, Manager.Owner.Location.Z);
            
            foreach (FloorTile FloorTile in FloorTiles)
            {
                Rectangle CheckRectangle = new Rectangle((int)FloorTile.Location.X - 1, (int)FloorTile.Location.Z - 1, FloorTile.Width + 2, FloorTile.Depth + 2);

                if (CheckRectangle.Contains(new Vector2(_PlayerLocation.X - .2f, _PlayerLocation.Y - .2f))
                    || CheckRectangle.Contains(new Vector2(_PlayerLocation.X - .2f, _PlayerLocation.Y + .2f))
                    || CheckRectangle.Contains(new Vector2(_PlayerLocation.X + .2f, _PlayerLocation.Y - .2f))
                    || CheckRectangle.Contains(new Vector2(_PlayerLocation.X + .2f, _PlayerLocation.Y + .2f))
                    )
                {
                    DetectedFloorTile = FloorTile;
                    DetectedFloorTile.Revealed = true;
                }
                else
                {
                    string lol = FloorTile.FloorType;
                }
            }
            if (DetectedFloorTile != null)
            {
                if (DetectedFloorTile.UpPossible == false)
                {
                    UpLimit = DetectedFloorTile.Location.Z + 1.5f;
                }
                if (DetectedFloorTile.DownPossible == false)
                {
                    DownLimit = DetectedFloorTile.Location.Z + 3.5f;

                }
                if (DetectedFloorTile.LeftPossible == false)
                {
                    LeftLimit = DetectedFloorTile.Location.X + 1.5f;

                }
                if (DetectedFloorTile.RightPossible == false)
                {
                    RightLimit = DetectedFloorTile.Location.X + 3.5f;

                }

            }
            else
            {
                DetectedFloorTile = null;
            }
        }
        public override void Update()
        {
            if (FloorTiles != null)
            {
                foreach (FloorTile Floortile in FloorTiles)
                {
                    Floortile.CurrentCell = false;
                }
            }
            
            if (DetectedFloorTile != null)
            {
                DetectedFloorTile.Revealed = true;
                DetectedFloorTile.CurrentCell = true;
                UpLimit = DetectedFloorTile.Location.Z - 100;
                DownLimit = DetectedFloorTile.Location.Z + 100;
                LeftLimit = DetectedFloorTile.Location.X - 100;
                RightLimit = DetectedFloorTile.Location.X + 100;
            }

            DetectPath();
            #region MouseRotation
            CurMouseState = Mouse.GetState();
            if (CurMouseState != CentredMouse)
            {
                int RotateSpeed = CentredMouse.X - CurMouseState.X;
                if (RotateSpeed > 100)
                {
                    RotateSpeed = 100;
                }
                if (RotateSpeed < -100)
                {
                    RotateSpeed = -100;
                }
                Manager.Owner.Direction = Vector3.Transform(Manager.Owner.Direction, Matrix.CreateRotationY(MathHelper.ToRadians((RotateSpeed) / 8)));

                Vector3 Trans = Manager.Owner.Location;
                Manager.Owner.World *= Matrix.CreateTranslation(-Trans); // Spin around 0,0,0
                Manager.Owner.World *= Matrix.CreateRotationY(MathHelper.ToRadians((RotateSpeed) / 8));
                Manager.Owner.World *= Matrix.CreateTranslation(Trans); // Put back to normal position

                /*Manager.Owner.Direction = Vector3.Transform(Manager.Owner.Direction,
                    (Matrix.CreateRotationX
                    (MathHelper.ToRadians((CentredMouse.Y - CurMouseState.Y) * Manager.Owner.Direction.Z))) * 

                    (Matrix.CreateRotationZ
                    (MathHelper.ToRadians((CentredMouse.Y - CurMouseState.Y) * Manager.Owner.Direction.X))));

                 */

                Vector3 NewDirection;
                NewDirection = Vector3.Transform(Manager.Owner.Direction, (Matrix.CreateRotationX(MathHelper.ToRadians((CentredMouse.Y - CurMouseState.Y) * -Manager.Owner.Direction.Z) / 4)));
                NewDirection = Vector3.Transform(NewDirection, (Matrix.CreateRotationZ(MathHelper.ToRadians((CentredMouse.Y - CurMouseState.Y) * Manager.Owner.Direction.X) / 4)));

                if (NewDirection.Y > -.8 && NewDirection.Y < .8)
                {
                    Manager.Owner.Direction = NewDirection;
                }
                //Manager.Owner.Direction = Vector3.Transform(Manager.Owner.Direction, (Matrix.CreateRotationX(MathHelper.ToRadians((CentredMouse.Y - CurMouseState.Y) * -Manager.Owner.Direction.Z))));
                //Manager.Owner.Direction = Vector3.Transform(Manager.Owner.Direction, (Matrix.CreateRotationZ(MathHelper.ToRadians((CentredMouse.Y - CurMouseState.Y) * Manager.Owner.Direction.X))));
            }
            #endregion
            #region WASDmovement
            if (InputEngine.IsKeyHeld(Keys.W))
            {
                Next = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(Manager.Owner.Direction.X, 0, Manager.Owner.Direction.Z) * MovementSpeed);
                Vector3 Difference = Next.Translation - Manager.Owner.World.Translation;
                float DirectionX = Manager.Owner.Direction.X;
                float DirectionZ = Manager.Owner.Direction.Z;

                if (Difference.X < 0)
                {
                    if (Next.Translation.X < LeftLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.X > 0)
                {
                    if (Next.Translation.X > RightLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.Z < 0)
                {
                    if (Next.Translation.Z < UpLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                if (Difference.Z > 0)
                {
                    if (Next.Translation.Z > DownLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                Manager.Owner.World = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(DirectionX, 0, DirectionZ) * MovementSpeed);

                Manager.Owner.CurrentAnimation = "Walk";
            }


            if (InputEngine.IsKeyHeld(Keys.I))
            {
                Vector2 lol = new Vector2(DetectedFloorTile.Location.Z, DetectedFloorTile.World.Translation.Z);
                string loler = DetectedFloorTile.FloorType;
            }

            if (InputEngine.IsKeyHeld(Keys.S))
            {


                // Manager.Owner.World *= Matrix.CreateTranslation(new Vector3(-Manager.Owner.Direction.X, 0, -Manager.Owner.Direction.Z) * MovementSpeed);

                Next = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(-Manager.Owner.Direction.X, 0, -Manager.Owner.Direction.Z) * MovementSpeed);
                Vector3 Difference = Next.Translation - Manager.Owner.World.Translation;
                float DirectionX = -Manager.Owner.Direction.X;
                float DirectionZ = -Manager.Owner.Direction.Z;

                if (Difference.X < 0)
                {
                    if (Next.Translation.X < LeftLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.X > 0)
                {
                    if (Next.Translation.X > RightLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.Z < 0)
                {
                    if (Next.Translation.Z < UpLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                if (Difference.Z > 0)
                {
                    if (Next.Translation.Z > DownLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                Manager.Owner.World = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(DirectionX, 0, DirectionZ) * MovementSpeed);


            }





            if (InputEngine.IsKeyHeld(Keys.A))
            {
                //Manager.Owner.World *= Matrix.CreateTranslation(new Vector3(Manager.Owner.Direction.Z, 0, -Manager.Owner.Direction.X) * (MovementSpeed*1.5f));

                Next = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(Manager.Owner.Direction.Z, 0, -Manager.Owner.Direction.X) * (MovementSpeed * 1.5f));
                Vector3 Difference = Next.Translation - Manager.Owner.World.Translation;
                float DirectionX = Manager.Owner.Direction.Z;
                float DirectionZ = -Manager.Owner.Direction.X;

                if (Difference.X < 0)
                {
                    if (Next.Translation.X < LeftLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.X > 0)
                {
                    if (Next.Translation.X > RightLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.Z < 0)
                {
                    if (Next.Translation.Z < UpLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                if (Difference.Z > 0)
                {
                    if (Next.Translation.Z > DownLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                Manager.Owner.World = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(DirectionX, 0, DirectionZ) * MovementSpeed);


            }
            if (InputEngine.IsKeyHeld(Keys.D))
            {
                //Manager.Owner.World *= Matrix.CreateTranslation(new Vector3(-Manager.Owner.Direction.Z, 0, Manager.Owner.Direction.X) * (MovementSpeed*1.5f));

                Next = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(-Manager.Owner.Direction.Z, 0, Manager.Owner.Direction.X) * (MovementSpeed * 1.5f));
                Vector3 Difference = Next.Translation - Manager.Owner.World.Translation;
                float DirectionX = -Manager.Owner.Direction.Z;
                float DirectionZ = Manager.Owner.Direction.X;

                if (Difference.X < 0)
                {
                    if (Next.Translation.X < LeftLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.X > 0)
                {
                    if (Next.Translation.X > RightLimit)
                    {
                        DirectionX = 0;
                    }
                }
                if (Difference.Z < 0)
                {
                    if (Next.Translation.Z < UpLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                if (Difference.Z > 0)
                {
                    if (Next.Translation.Z > DownLimit)
                    {
                        DirectionZ = 0;
                    }
                }
                Manager.Owner.World = Manager.Owner.World * Matrix.CreateTranslation(new Vector3(DirectionX, 0, DirectionZ) * MovementSpeed);

            }
            #endregion

            if (InputEngine.IsKeyHeld(Keys.P))
            {
                CurMouseState = Mouse.GetState();
            }

            if (InputEngine.IsKeyHeld(Keys.Up))
                Manager.Owner.World *= Matrix.CreateTranslation(new Vector3(0, MovementSpeed, 0));
            if (InputEngine.IsKeyHeld(Keys.Down))
                Manager.Owner.World *= Matrix.CreateTranslation(new Vector3(0, -MovementSpeed, 0));



            Mouse.SetPosition(CentredMouse.X, CentredMouse.Y);
            base.Update();

        }

    }
}
