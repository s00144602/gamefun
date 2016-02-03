using GameEngineProgramming.Engine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Graphics
{
    public class BasicAnimatedModel: BasicEffectModel
    {
        public BasicAnimatedModel(string id, string asset)
            : base(id, asset)
        {}

        public void UpdateBone(int boneIndex, Matrix transform)
        {
            if (boneIndex <= BoneTransforms.Length)
            {
                Vector3 trans = BoneTransforms[boneIndex].Translation;

                //apply this transform to all the children
                for (int i = boneIndex; i < BoneTransforms.Length; i++)
                {
                    BoneTransforms[i] *= Matrix.CreateTranslation(-trans);
                    BoneTransforms[i] *= transform;
                    BoneTransforms[i] *= Matrix.CreateTranslation(trans);
                }
            }
        }

        public void UpdateBone(string name, Matrix transform)
        {
            int index = -1;

            var bone = Model.Bones.Where(b => b.Name == name).FirstOrDefault();

            if (bone != null)
            {
                index = Model.Bones.IndexOf(bone);

                if (index != -1)
                    UpdateBone(index, transform);
            }
        }
    }
    
}
