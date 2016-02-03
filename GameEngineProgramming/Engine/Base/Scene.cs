using Engine;
using GameEngineProgramming.Engine.Components.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Base
{
    public class Scene
    {
        public string ID { get; set; }
        public List<GameObject> pool = new List<GameObject>();
        public List<GameObject> Pool { get { return pool; } }

        protected GameEngine Engine;
        private bool isInitialized = false;

        public Scene(string id, GameEngine engine)
        {
            ID = id;
            this.Engine = engine;
        }

        public void AddObject(GameObject newGameObject)
        {
            if (isInitialized)
            {
                newGameObject.Initialize();
                FilterPhysicsComponents(newGameObject);
            }
            newGameObject.Scene = this;
            newGameObject.OnDestroy += NewGameObject_OnDestroy;
            pool.Add(newGameObject);
        }
        public bool HasObject(string id)
        {
            return pool.Any(go => go.ID == id);
        }
        private void NewGameObject_OnDestroy(string id)
        {
            RemoveObject(id);
        }

        public int GetObjectIndex(string id)
        {
            return pool.FindIndex(ls => ls.ID == id);
        }

        public GameObject GetObject(string id)
        {
            int index = GetObjectIndex(id);
            if (index > -1)
                return pool[index];
            else return null;
        }
        public virtual void Initialize()
        {
            for (int i=0; i < pool.Count; i++)
            {
                pool[i].Initialize();
                FilterPhysicsComponents(pool[i]);
            }

            isInitialized = true;
        }

        public virtual void HandleInput()
        {

        }
        
        public void Update()
        {
            HandleInput();

            foreach (GameObject gameObject in pool)
            {
                gameObject.Update();
            }
        }

        public void RemoveObject(string id)
        {
            pool.RemoveAt(GetObjectIndex(id));
        }

        public virtual void Draw3D(CameraComponent camera)
        {
            foreach (GameObject gameObject in pool)
                gameObject.Draw(camera);
        }

        public virtual void DrawUI()
        {

        }
        
        private PhysicsComponent[] GetPhysicsComponentFromObject(GameObject GoObject)
        {
            if (GoObject.Manager.HasComponent<PhysicsComponent>())
            {
                var comps = GoObject.Manager.GetComponents<PhysicsComponent>();
                return Array.ConvertAll(comps, item => (PhysicsComponent)item);
            }
            return null;
        }
        private void FilterPhysicsComponents(GameObject gameObject)
        {
            var physComponents = GetPhysicsComponentFromObject(gameObject);
            if (physComponents != null)
                foreach (var c in physComponents)
                    Engine.Physics.AddEntity(c.Entity);
        }
    }
}
