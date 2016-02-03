using Engine;
using Engine.Engines;
using GameEngineProgramming.Engine.Components.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Input
{
    public delegate void GameObjectInfoDelegate(PhysicsComponent.GameObjectInfo info);
    public class RayCaster : Component
    {
        public event GameObjectInfoDelegate GameObjectSelected;

        public RayCaster(string id) : base(id)
        {

        }
        public override void Update()
        {
            if (InputEngine.IsMouseLeftClick())
            {
                PhysicsComponent.GameObjectInfo result = null;

                PhysicsEngine.RayCastIntoSpace(
                    GameUtilities.CreateRayFromVector2(InputEngine.MousePosition),
                    1000,
                    true,
                    out result);

                if (result != null)
                {
                    if (GameObjectSelected != null)
                        GameObjectSelected(result);
                }
            }
            base.Update();
        }
    }
}
