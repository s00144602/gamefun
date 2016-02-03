using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Components;
using Microsoft.Xna.Framework;
using GameEngineProgramming.Engine.Base;

namespace Engine
{
    public class GameObject
    {
        public bool Invisible = false;
        public Scene Scene { get; set; }
        public string ID { get; set; }
        public ComponentManager Manager { get; set; }
        public bool Enabled { get; set; }

        public bool InAnimation = false;
        public string CurrentAnimation = "Idle";
        public BoundingBox AABB;

        public event ObjectStringIDHandler OnDestroy;
        public Vector3 Direction { get; set; }

        public Matrix World { get; set; }
        public Vector3 Location { get { return World.Translation; } }
        public Vector3 Scale { get { return World.Scale; } }
        public Quaternion Rotation { get { return World.Rotation; } }
        public string Weapon = "Sword";
        public GameObject()
        {
            Manager = new ComponentManager(this);
            Enabled = true;
            World = Matrix.Identity;
        }

        public GameObject(string id)
        {
            ID = id;
            Manager = new ComponentManager(this);
            Enabled = true;
            World = Matrix.Identity;
        }

        public GameObject(string id, Vector3 location)
        {
            ID = id;
            Manager = new ComponentManager(this);
            Enabled = true;
            World = Matrix.Identity * Matrix.CreateTranslation(location);
        }

        public virtual void Initialize()
        {
            Manager.Initialize();
        }

        public virtual void Update()
        {
            if (Enabled)
            {
                Manager.Update();
            }
        }

        public void Draw(CameraComponent camera)
        {
            if(Invisible != true)
                Manager.Draw(camera);
        }

        public void Destroy()
        {
            if (OnDestroy != null)
                OnDestroy(ID);
        }
    }
}
