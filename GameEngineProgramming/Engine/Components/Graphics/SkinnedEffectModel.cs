using Common;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Graphics
{
    public class SkinnedEffectModel : RenderComponent
    {
        private string asset;
        private string startingAnimation;
        public Dictionary<string, Vector3> Animations;

        public Model Model { get; set; }
        public SkinningData Data { get; set; }
        public AnimationPlayer Player { get; set; }
        private Matrix[] currentBoneTransforms;

        public SkinnedEffectModel(string id, string asset, string startingAnimation) : base(id)
        {
            this.asset = asset;
            this.startingAnimation = startingAnimation;
        }
        public SkinnedEffectModel(string id, string asset, string startingAnimation, Dictionary<string, Vector3> _Animations) : base(id)
        {
            Animations = _Animations;
            this.asset = asset;
            this.startingAnimation = startingAnimation;
        }
        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(asset))
            {
                Model = GameUtilities.NonPersistentContent.Load<Model>("Animated Models/" + asset);

                ApplySkinnedEffect();
                if (Model.Tag != null)
                {
                    if (Model.Tag is SkinningData)
                    {
                        Data = Model.Tag as SkinningData;
                        Player = new AnimationPlayer(Data);
                        PlayAnimation(startingAnimation);
                    }
                }
            }
            base.Initialize();
        }
        private void ApplySkinnedEffect()
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    SkinnedEffect seffect = new SkinnedEffect(GameUtilities.GraphicsDevice);
                    BasicEffect originalEffect = part.Effect as BasicEffect;

                    seffect.Texture = originalEffect.Texture;
                    seffect.SpecularColor = originalEffect.SpecularColor;
                    seffect.DiffuseColor = originalEffect.DiffuseColor;

                    part.Effect = seffect;
                }
            }
        }
        public void PlayAnimation(string name)
        {
            if (Player != null)
            {
                if (Data.AnimationClips.ContainsKey(name))
                    Player.StartClip(Data.AnimationClips[name], Animations);
            }
        }

        public override void Update()
        {

            if (Player.Update(GameUtilities.Time.ElapsedGameTime, true, Manager.Owner.World, Manager.Owner.InAnimation, Manager.Owner.CurrentAnimation))
            {
                Manager.Owner.CurrentAnimation = "Idle";
            }
            base.Update();
        }
        public override void Draw(CameraComponent camera)
        {
            if (Player != null)
            {
                currentBoneTransforms = Player.GetSkinTransforms();

                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (SkinnedEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.View = camera.View;
                        effect.Projection = camera.Projection;
                        effect.SetBoneTransforms(currentBoneTransforms);
                    }
                    mesh.Draw();
                }
            }
            base.Draw(camera);
        }
        /*public void SplitAnimations()
        {
            if (Data.AnimationClips.ContainsKey("Take 001"))
            {
                for (int i = 0; i < AnimationName.Count; i++)
                {
                    AnimationClip AnimationToBeAdded = Data.AnimationClips["Take 001"];

                    for (int j = AnimationToBeAdded.Keyframes.Count-1; j >= 0; j--)
                    {
                        if (j < AnimationFrames[i].X || j > AnimationFrames[i].Y)
                        {
                            AnimationToBeAdded.Keyframes.RemoveAt(j);
                        }
                    }
                    int AnimationDuration = AnimationToBeAdded.Keyframes.Count / 3 * (1000 / 60);
                    AnimationToBeAdded = new AnimationClip(TimeSpan.FromMilliseconds(AnimationDuration), AnimationToBeAdded.Keyframes);
                    Data.AnimationClips.Add(AnimationName[i],AnimationToBeAdded);
                }
            }
        }*/
    }
}
