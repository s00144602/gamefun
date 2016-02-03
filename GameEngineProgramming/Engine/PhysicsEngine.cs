using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.Entities;
using Engine;
using GameEngineProgramming.Engine.Components.Physics;
using GameEngineProgramming.Engine.Engines;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine
{
    public class PhysicsEngine : GameComponent
    {
        private static Space simulationSpace;
        public PhysicsEngine(Game _game) : base(_game)
        {
            simulationSpace = new Space();

            SetSpaceGravity(-9.81f);

            _game.Components.Add(this);
        }
        public void SetSpaceGravity(float gravityAmount)
        {
            simulationSpace.ForceUpdater.Gravity = new BEPUutilities.Vector3(0, gravityAmount, 0);
        }

        public void AddEntity(Entity entity)
        {
            if (entity != null)
                simulationSpace.Add(entity);
        }
        public void RemoveEntity(Entity entity)
        {
            if (entity != null)
                simulationSpace.Remove(entity);
        }
        public static void AddStaticMesh(StaticMesh staticMesh)
        {
            if (staticMesh != null)
                simulationSpace.Add(staticMesh);
        }
        public static void RemoveStaticMesh(StaticMesh staticMesh)
        {
            if (staticMesh != null)
                simulationSpace.Remove(staticMesh);
        }
        public override void Update(GameTime gameTime)
        {
            simulationSpace.Update();
            base.Update(gameTime);
        }
        public static void RayCastIntoSpace(Ray ray, float maximumDistance, bool filterStaticMesh, out PhysicsComponent.GameObjectInfo hitObjectInfo)
        {
            RayCastResult result = new RayCastResult();
            hitObjectInfo = null;

            if (filterStaticMesh)
            {
                simulationSpace.RayCast(new BEPUutilities.Ray(
                                    GameUtilities.MonoVector3ToBepuVector3(ray.Position),
                                    GameUtilities.MonoVector3ToBepuVector3(ray.Direction)),
                                    maximumDistance,
                                    FilterStaticMeshFromRayCast,
                                    out result);
            }
            else
            {
                simulationSpace.RayCast(new BEPUutilities.Ray(
                                    GameUtilities.MonoVector3ToBepuVector3(ray.Position),
                                    GameUtilities.MonoVector3ToBepuVector3(ray.Direction)),
                                    maximumDistance,
                                    out result);
            }
            if (result.HitObject != null)
            {
                if (result.HitObject is EntityCollidable)
                {
                    var collidable = result.HitObject as EntityCollidable;

                    if (collidable.Tag != null)
                    {
                        if (collidable.Tag is PhysicsComponent.GameObjectInfo)
                        {
                            hitObjectInfo = collidable.Tag as PhysicsComponent.GameObjectInfo;
                        }
                    }
                }
            }
        }
        private static bool FilterStaticMeshFromRayCast(BroadPhaseEntry entry)
        {
            if (entry is Collidable)
            {
                if ((entry as Collidable) is StaticMesh)
                    return false;
                else return true;
            }
            return false;
        }
    }
}
