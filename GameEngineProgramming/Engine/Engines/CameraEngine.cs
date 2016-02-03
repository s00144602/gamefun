using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace GameEngineProgramming.Engine.Engines
{
    public sealed class CameraEngine : GameComponent
    {
        private Dictionary<string, CameraComponent> cameras;
        private static CameraComponent activeCamera;
        private string activeCameraID;

        public static CameraComponent ActiveCamera
        {
            get { return activeCamera; }
        }

        public CameraEngine(Game _game)
            :base(_game)
        {
            cameras = new Dictionary<string, CameraComponent>();

            _game.Components.Add(this);
        }

        public void SetActiveCamera(string ID)
        {
            if (cameras.ContainsKey(ID))
            {
                if (activeCamera != cameras[ID])
                {
                    activeCamera = cameras[ID];
                    activeCameraID = ID;
                }
            }
        }

        public void AddCamera(CameraComponent camera)
        {
            if (!cameras.ContainsKey(camera.ID))
            {
                cameras.Add(camera.ID, camera);
                if (cameras.Count == 1)
                    activeCamera = camera;
            }
        }

        public void Clear()
        {
            cameras.Clear();
        }

        public List<string> GetCurrentCameras()
        {
            return cameras.Keys.ToList();
        }

        public void RemoveCamera(string id)
        {
            if (cameras.ContainsKey(id))
            {
                cameras.Remove(id);
                if (activeCameraID == id)
                {
                    activeCamera = null;
                    activeCameraID = null;
                }
            }
        }
    }
}
