using Common;
using Engine;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Graphics
{
    public class BasicEffectModel : RenderComponent
    {
        private string modelAsset;
        public Model Model { get; set; }

        public Matrix[] BoneTransforms { get; set; }
        private ModelTag Tag { get; set; }
        private BoundingBox aabb;
        public BoundingBox AABB { get { return aabb; } }
        public BasicEffectModel(string id, string asset)
            : base(id)
        {
            modelAsset = asset;
        }

        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(modelAsset))
            {

                Model = GameUtilities.NonPersistentContent.Load<Model>
                    ("Models/" + modelAsset);

                BoneTransforms = new Matrix[Model.Bones.Count];
                Model.CopyAbsoluteBoneTransformsTo(BoneTransforms);
                if (Model.Tag != null)
                {
                    if (Model.Tag is ModelTag)
                    {
                        Tag = Model.Tag as ModelTag;
                        aabb = BoundingBox.CreateFromPoints(Tag.Vertices);
                        TransformBoundingBox(Manager.Owner.World);
                    }
                }
            }
            base.Initialize();
        }
        public void TransformBoundingBox(Matrix transformMatrix)
        {
            aabb.Min = Vector3.Transform(aabb.Min, transformMatrix);
            aabb.Max = Vector3.Transform(aabb.Max, transformMatrix);
        }
        public override void Update()
        {
            /*if (GameUtilities.IsDevelopmentMode)
                DebugEngine.AddBoundingBox(aabb, Color.Red);
            base.Update();*/
        }
        public override void Draw(CameraComponent camera)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.PreferPerPixelLighting = true;

                    effect.View = camera.View;
                    effect.Projection = camera.Projection;

                    effect.World = BoneTransforms[mesh.ParentBone.Index]
                        * Manager.Owner.World;
                    
                }
                mesh.Draw();
            }
            base.Draw(camera);
        }
    }
}
