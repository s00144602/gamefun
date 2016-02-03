using BEPUphysics.Entities.Prefabs;
using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Physics
{
    public class BoxComponent : PhysicsComponent
    {
        public BoxComponent(string id,float mass) : base(id, mass) { }
        public override void Initialize()
        {
            var tag = GetModelTagFromOwner();
            if (tag != null)
            {
                BoundingBox aabb =
                    BoundingBox.CreateFromPoints(tag.Vertices);
                Vector3 size = aabb.Max - aabb.Min;
                if (Mass > 0)
                {
                    Entity = new Box(GameUtilities.MonoVector3ToBepuVector3(Manager.Owner.Location),
                        size.X, size.Y, size.Z, Mass);
                }
                else
                {
                    Entity = new Box(
                        GameUtilities.MonoVector3ToBepuVector3(Manager.Owner.Location),
                        size.X, size.Y, size.Z);
                }
                Entity.WorldTransform = GameUtilities.MonoMatrixToBepuMatrix(
                    Manager.Owner.World); 
            }
            else
            {
                Destroy();
            }
            base.Initialize();
        }
    }
}
