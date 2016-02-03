using BEPUphysics.BroadPhaseEntries;
using BEPUutilities;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Physics
{
    class StaticMeshComponent : PhysicsComponent
    {
        public StaticMeshComponent(string id) : base(id, 0) { }

        public override void Initialize()
        {
            var model = GetModelFromOwner();
            if (model != null)
            {
                Vector3[] vertices;
                int[] indicies;
                ModelDataExtractor.GetVerticesAndIndicesFromModel(model, out vertices, out indicies);
                var mesh = new StaticMesh(
                    vertices,
                    indicies,
                    new AffineTransform(
                        GameUtilities.MonoVector3ToBepuVector3(Manager.Owner.Location)));
                mesh.Tag = new PhysicsComponent.GameObjectInfo()
                {
                    ID = Manager.Owner.ID,
                    ObjectType = Manager.Owner.GetType()
                };
                PhysicsEngine.AddStaticMesh(mesh);
            }
            else
            {
                Destroy();
            }
            base.Initialize();
        }

    }
}
