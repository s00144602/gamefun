using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using GameEngineProgramming.Engine.Engines;

namespace Engine
{
    public delegate void ObjectStringIDHandler(string id);

    public static class GameUtilities
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static GameTime Time { get; set; }
        public static ContentManager PersistentContent { get; set; }
        public static ContentManager NonPersistentContent { get; set; }
        public static Random Random { get; set; }
        public static SpriteFont DebugFont { get; set; }

        public static bool IsDevelopmentMode { get { return System.Diagnostics.Debugger.IsAttached; } } 

        public static void UnloadNonPersistentContent()
        {
            NonPersistentContent.Unload();
        }

        public static void SetGraphicsDeviceFor3D()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        public static void SaveObject<T>(object objectToSave, string locationToSave)
        {
            using (FileStream stream = File.Create(locationToSave))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(stream, objectToSave);
            }
        }

        public static T LoadObject<T>(string objectLocation)
        {
            if(File.Exists(objectLocation))
            {
                using (FileStream stream = File.Open(objectLocation, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    return (T)xs.Deserialize(stream);
                }
            }
            else
            {
                return default(T);
            }
        }
        public static Ray CreateRayFromVector2(Vector2 screenPosition)
        {
            Vector3 nearScreenPoint = new Vector3(screenPosition.X, screenPosition.Y, 0);
            Vector3 farScreenPoint = new Vector3(screenPosition.X, screenPosition.Y, 1);

            Vector3 near3DPoint = GraphicsDevice.Viewport.Unproject(
                nearScreenPoint,
                CameraEngine.ActiveCamera.Projection,
                CameraEngine.ActiveCamera.View,
                Matrix.Identity);

            Vector3 far3DPoint = GraphicsDevice.Viewport.Unproject(
                farScreenPoint,
                CameraEngine.ActiveCamera.Projection,
                CameraEngine.ActiveCamera.View,
                Matrix.Identity);

            Vector3 pointerRayDirection = far3DPoint - near3DPoint;

            pointerRayDirection.Normalize();

            return new Ray(near3DPoint, pointerRayDirection);
        }
        public static Matrix BepuMatrixToMonoMatrix(BEPUutilities.Matrix matrix)
        {
            return new Matrix(
                matrix.M11, 
                matrix.M12, 
                matrix.M13, 
                matrix.M14, 
                matrix.M21, 
                matrix.M22, 
                matrix.M23, 
                matrix.M24, 
                matrix.M31, 
                matrix.M32, 
                matrix.M33, 
                matrix.M34, 
                matrix.M41, 
                matrix.M42, 
                matrix.M43, 
                matrix.M44);
        }

        public static void BepuMatrixToMonoMatrix(BEPUutilities.Matrix matrix, out Matrix orignalMatrix)
        {
            orignalMatrix.M11 = matrix.M11;
            orignalMatrix.M12 = matrix.M12;
            orignalMatrix.M13 = matrix.M13;
            orignalMatrix.M14 = matrix.M14;
            orignalMatrix.M21 = matrix.M21;
            orignalMatrix.M22 = matrix.M22;
            orignalMatrix.M23 = matrix.M23;
            orignalMatrix.M24 = matrix.M24;
            orignalMatrix.M31 = matrix.M31;
            orignalMatrix.M32 = matrix.M32;
            orignalMatrix.M33 = matrix.M33;
            orignalMatrix.M34 = matrix.M34;
            orignalMatrix.M41 = matrix.M41;
            orignalMatrix.M42 = matrix.M42;
            orignalMatrix.M43 = matrix.M43;
            orignalMatrix.M44 = matrix.M44;
        }

        public static BEPUutilities.Matrix MonoMatrixToBepuMatrix(Matrix matrix)
        {
            return new BEPUutilities.Matrix(
                matrix.M11,
                matrix.M12,
                matrix.M13,
                matrix.M14,
                matrix.M21,
                matrix.M22,
                matrix.M23,
                matrix.M24,
                matrix.M31,
                matrix.M32,
                matrix.M33,
                matrix.M34,
                matrix.M41,
                matrix.M42,
                matrix.M43,
                matrix.M44);
        }

        public static void MonoMatrixToBepuMatrix(Matrix matrix, out BEPUutilities.Matrix orignalMatrix)
        {
            orignalMatrix.M11 = matrix.M11;
            orignalMatrix.M12 = matrix.M12;
            orignalMatrix.M13 = matrix.M13;
            orignalMatrix.M14 = matrix.M14;
            orignalMatrix.M21 = matrix.M21;
            orignalMatrix.M22 = matrix.M22;
            orignalMatrix.M23 = matrix.M23;
            orignalMatrix.M24 = matrix.M24;
            orignalMatrix.M31 = matrix.M31;
            orignalMatrix.M32 = matrix.M32;
            orignalMatrix.M33 = matrix.M33;
            orignalMatrix.M34 = matrix.M34;
            orignalMatrix.M41 = matrix.M41;
            orignalMatrix.M42 = matrix.M42;
            orignalMatrix.M43 = matrix.M43;
            orignalMatrix.M44 = matrix.M44;
        }

        public static BEPUutilities.Vector3 MonoVector3ToBepuVector3(Vector3 vector)
        {
            return new BEPUutilities.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static void MonoVector3ToBepuVector3(Vector3 vector, out BEPUutilities.Vector3 orignalVector)
        {
            orignalVector.X = vector.X;
            orignalVector.Y = vector.Y;
            orignalVector.Z = vector.Z;
        }

        public static Vector3 BepuVector3ToMonoVector3(BEPUutilities.Vector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        public static void BepuVector3ToMonoVector3(BEPUutilities.Vector3 vector, out Vector3 orignalVector)
        {
            orignalVector.X = vector.X;
            orignalVector.Y = vector.Y;
            orignalVector.Z = vector.Z;
        }
    }

}
