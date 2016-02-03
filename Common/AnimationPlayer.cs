#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace Common
{
    /// <summary>
    /// The animation player is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields


        // Information about the currently playing animation clip.
        AnimationClip currentClipValue;
        string CurrentAnimation;
        string PrevAnimation;
        Dictionary<string, Vector3> Animations;
        TimeSpan currentTimeValue;
        int currentKeyframe;

        // Current animation transform matrices.
        public Matrix[] boneTransforms;
        public Matrix[] worldTransforms;
        public Matrix[] skinTransforms;


        // Backlink to the bind pose and skeleton hierarchy data.
        SkinningData skinningDataValue;
        public bool ReturnValue = false;

        #endregion


        /// <summary>
        /// Constructs a new animation player.
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }


        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip(AnimationClip clip, Dictionary<string, Vector3> _Animations)
        {
            Animations = _Animations;
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;
            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }


        /// <summary>
        /// Advances the current animation position.
        /// </summary>
        public bool Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform, bool InAnimation, string _CurrentAnimation)
        {
            ReturnValue = false;
            if (String.IsNullOrEmpty(CurrentAnimation))
                CurrentAnimation = _CurrentAnimation;

            if (Animations[CurrentAnimation].Z <= Animations[_CurrentAnimation].Z)
                CurrentAnimation = _CurrentAnimation;

            if (CurrentAnimation == PrevAnimation)
            {
                AnimationChange = false;
            }
            else
            {
                AnimationChange = true;
            }
            PrevAnimation = CurrentAnimation;
            /*if (InAnimation)
            {
                UpdateBoneTransforms(time, relativeToCurrentTime);
            }
            else
            {
                if (currentTimeValue.TotalMilliseconds > (currentClipValue.Duration.TotalMilliseconds/2))
                    currentTimeValue += TimeSpan.FromMilliseconds(32);
                else
                {
                    currentTimeValue -= TimeSpan.FromMilliseconds(32);
                }
                if (currentTimeValue.TotalMilliseconds < 0 || currentTimeValue.TotalMilliseconds > currentClipValue.Duration.TotalMilliseconds)
                {
                    currentTimeValue = TimeSpan.FromMilliseconds(0);
                }
                currentKeyframe = 0;

                IList<Keyframe> keyframes = currentClipValue.Keyframes;

                while (currentKeyframe < keyframes.Count)
                {
                    Keyframe keyframe = keyframes[currentKeyframe];

                    // Stop when we've read up to the current time position.
                    if (keyframe.Time > currentTimeValue)
                        break;

                    // Use this keyframe.
                    boneTransforms[keyframe.Bone] = keyframe.Transform;

                    currentKeyframe++;
                }
            }*/
            //currentClipValue = CurrentAnimation;



            UpdateBoneTransforms(time, relativeToCurrentTime);

            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();

            return ReturnValue;
        }
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {

            int StartFrame = (int)Animations[CurrentAnimation].X;
            int StartTime = StartFrame * (1000 / 60);
            int EndFrame = (int)Animations[CurrentAnimation].Y + 1;
            int EndTime = EndFrame * (1000 / 60);

            if (AnimationChange)
            {
                currentKeyframe = StartFrame;
                //time = TimeSpan.FromMilliseconds(StartTime);
                currentTimeValue = TimeSpan.FromMilliseconds(StartTime);
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);

            }

            if (currentClipValue == null)
                throw new InvalidOperationException("AnimationPlayer.Update was called before StartClip");

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                if (currentKeyframe == 22)
                {
                    int i = currentKeyframe;
                }
                time += currentTimeValue;

                // If we reached the end, loop back to the start.
                while (time >= TimeSpan.FromMilliseconds(EndTime))
                {
                    //time -= currentClipValue.Duration;
                    ReturnValue = true;
                    CurrentAnimation = "Idle";
                    time = TimeSpan.FromMilliseconds(StartTime);
                }
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");


            // If the position moved backwards, reset the keyframe index.
            if (time < currentTimeValue)
            {
                currentKeyframe = StartFrame;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            // Read keyframe matrices.
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > currentTimeValue)
                    break;

                // Use this keyframe.
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }

        /// <summary>
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        //public void UpdateWorldTransforms(Matrix rootTransform)
        //{
        //    Matrix RootRotation = Matrix.CreateFromQuaternion(rootTransform.Rotation);
        //    Matrix Scale = Matrix.CreateScale(1, 1, 1);
        //    // Root bone.
        //    worldTransforms[0] = Scale;
        //    worldTransforms[0] *= RootRotation;
        //    worldTransforms[0] *= Matrix.CreateTranslation(boneTransforms[0].Translation + rootTransform.Translation);

        //    // Child bones.
        //    for (int bone = 1; bone < worldTransforms.Length; bone++)
        //    {
        //        int parentBone = skinningDataValue.SkeletonHierarchy[bone];
        //        worldTransforms[bone] = Scale;
        //        worldTransforms[bone] *= Matrix.CreateFromQuaternion(boneTransforms[bone].Rotation + boneTransforms[parentBone].Rotation);
        //        worldTransforms[bone] *= Matrix.CreateTranslation(boneTransforms[bone].Translation + worldTransforms[parentBone].Translation);
        //    }

        //}

        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;
            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }

        }
        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];



            }
        }


        /// <summary>
        /// Gets the current bone transform matrices, relative to their parent bones.
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices, in absolute format.
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices,
        /// relative to the skinning bind pose.
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        /// <summary>
        /// Gets the current play position.
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }

        public bool AnimationChange { get; private set; }
    }
}
