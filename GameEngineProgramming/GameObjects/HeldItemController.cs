using Common;
using Engine;
using Engine.Engines;
using GameEngineProgramming.Engine.Components.Graphics;
using GameEngineProgramming.GameObjects;
using GameEngineProgramming.MazeObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace GameEngineProgramming.GameObjects
{

    public class HeldItemController : Component
    {
        MazePlayer Player;
        AnimationPlayer AnimatedPlayer;
        int BoneToAttach = 14;
        public HeldItemController(string id, MazePlayer player)
           : base(id)
        {
            Player = player;
        }

        public override void Update()
        {

            //Manager.Owner.AABB = new BoundingBox(
            //    new Vector3(Manager.Owner.Location.X - 1f, Manager.Owner.Location.Y - 1, Manager.Owner.Location.Z - 1f),
            //    new Vector3(Manager.Owner.Location.X - 1f, Manager.Owner.Location.Y - 1, Manager.Owner.Location.Z - 1f)
            //    + new Vector3(2, 2, 2));

            if (Player.Manager.HasComponent<SkinnedEffectModel>())
            {
                AnimatedPlayer = (Player.Manager.GetComponent(typeof(SkinnedEffectModel)) as SkinnedEffectModel).Player;
            }
            else
            {
                Destroy();
            }
            if (InputEngine.IsMouseRightClick())
                BoneToAttach++;
            if (BoneToAttach > AnimatedPlayer.worldTransforms.Count() - 1)
                BoneToAttach = 0;


            //Manager.Owner.World = Matrix.CreateTranslation(0, 0, 0);


            //Manager.Owner.World *= Matrix.CreateRotationX(AnimatedPlayer.worldTransforms[BoneToAttach].Rotation.X);
            //Manager.Owner.World *= Matrix.CreateRotationY(AnimatedPlayer.worldTransforms[BoneToAttach].Rotation.Y);
            //Manager.Owner.World *= Matrix.CreateRotationZ(AnimatedPlayer.worldTransforms[BoneToAttach].Rotation.Z);

            //Manager.Owner.World *= Matrix.CreateTranslation(AnimatedPlayer.worldTransforms[BoneToAttach].Translation);


            Manager.Owner.World = AnimatedPlayer.worldTransforms[BoneToAttach];


            if (InputEngine.IsKeyPressed(Keys.M))
            {
                Vector3 lol = Manager.Owner.Scale;
            }
            base.Update();
        }
    }
}