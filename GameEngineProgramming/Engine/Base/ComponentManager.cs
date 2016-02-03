using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class ComponentManager
    {
        private List<Component> Components = new List<Component>();
        public GameObject Owner { get; set; }
        private bool IsInitialized = false;
        public ComponentManager(GameObject owner)
        {
            Owner = owner;
        }

        public void Initialize()
        {
            Components.ForEach(c => c.Initialize());
            IsInitialized = true;
        }

        public void Update()
        {
            for (int i = 0; i < Components.Count; i++)
                if (Components[i].Enabled)
                    Components[i].Update();
        }

        public void Draw(CameraComponent camera)
        {
            foreach (var rcomp in Components.OfType<RenderComponent>())
                rcomp.Draw(camera);
        }

        public bool HasComponent<T>()
        {
            return Components.Any(c => c.GetType() == typeof(T) || c.GetType().IsSubclassOf(typeof(T)));
        }

        #region Add

        public void AddComponent(Component component)
        {
            component.Manager = this;
            if (IsInitialized)
            {
                component.Initialize();
            }
            Components.Add(component);
        }

        #endregion

        #region Remove

        public void RemoveComponent(Component component)
        {
            Components.RemoveAt(Components.IndexOf(component));
        }

        public void RemoveComponent(string id)
        {
            try
            {
                Components.RemoveAt(Components.IndexOf(Components.First(c => c.ID == id)));
            }
            catch
            {

            }
        }

        public void RemoveComponent(int index)
        {
            if (index < Components.Count && index > -1)
                Components.RemoveAt(index);
        }

        #endregion

        #region Get

        public Component GetComponent(string id)
        {
            return Components.FirstOrDefault(c => c.ID == id);
        }

        public Component GetComponent(Type componentType)
        {
            return Components.FirstOrDefault(c => c.GetType() == componentType || c.GetType().IsSubclassOf(componentType));
        }

        public Component[] GetComponents<T>()
        {
            return Components.Where(c => c.GetType() == typeof(T) || c.GetType().IsSubclassOf(typeof(T))).ToArray();
        }

        #endregion
    }
}
