using BEPUphysics.Entities;
using Common;
using Engine;
using GameEngineProgramming.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Physics
{
    public class PhysicsComponent : Component
    {
        public float Mass { get; set; }
        private Entity _entity;
        public Entity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }

        public class GameObjectInfo
        {
            public string ID { get; set; }
            public Type ObjectType { get; set; }
        }
        public PhysicsComponent(string id, float mass) : base(id)
        {
            Mass = mass;
        }
        public override void Initialize()
        {
            if(Entity != null)
            {
                var info = new GameObjectInfo()
                {
                    ID = Manager.Owner.ID,
                    ObjectType = Manager.Owner.GetType()
                };
                Entity.Tag = info;
                Entity.CollisionInformation.Tag = info;
            }
            base.Initialize();
        }
        public override void Update()
        {
            if (Entity != null)
                Manager.Owner.World =
                    GameUtilities.BepuMatrixToMonoMatrix(Entity.WorldTransform);
            base.Update();
        }
        public Model GetModelFromOwner()
        {
            if (Manager.HasComponent<BasicEffectModel>())
            {
                var modelComponent =
                    (BasicEffectModel)Manager.GetComponent(
                        typeof(BasicEffectModel));
                if (modelComponent != null)
                {
                    if (modelComponent.Model != null)
                        return modelComponent.Model;
                    else return null;
                }
                else return null;
            }
            else return null;
        }
        public ModelTag GetModelTagFromOwner()
        {
            var model = GetModelFromOwner();
            if(model != null)
            {
                if (model.Tag != null)
                {
                    return model.Tag as ModelTag;
                }
            }
            return null;
        }
    }
}
