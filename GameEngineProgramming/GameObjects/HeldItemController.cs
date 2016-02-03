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
        int BoneToAttach = 18;
        public bool Active;
        public HeldItemController(string id, MazePlayer player, bool active)
           : base(id)
        {
            Player = player;
            Active = active;
        }
        public override void Initialize()
        {
            if (Player.Manager.HasComponent<SkinnedEffectModel>())
            {
                AnimatedPlayer = (Player.Manager.GetComponent(typeof(SkinnedEffectModel)) as SkinnedEffectModel).Player;
            }
            else
            {
                Destroy();
            }
            base.Initialize();
        }
        public override void Update()
        {

            if(ID == Player.Weapon)
            {
                Manager.Owner.World = AnimatedPlayer.worldTransforms[BoneToAttach];
                Manager.Owner.Invisible = false;
            }
            else
            {
                Manager.Owner.Invisible = true;
            }
            //if (InputEngine.IsMouseRightClick())
            //    BoneToAttach++;
            //if (BoneToAttach > AnimatedPlayer.worldTransforms.Count() - 1)
            //    BoneToAttach = 0;




            //if (InputEngine.IsKeyPressed(Keys.M))
            //{
            //    Vector3 lol = Manager.Owner.Scale;
            //}
            base.Update();
        }
    }
}