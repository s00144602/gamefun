using Engine;
using GameEngineProgramming.Engine.Base;
using GameEngineProgramming.Engine.Components.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Input
{
    public class ObjectSelectionHandler : ScriptComponent
    {
        public GameObject SelectedObject { get; set; }
        public ObjectSelectionHandler(string id) : base(id)
        {

        }

        public override void Initialize()
        {
            if (Manager.HasComponent<RayCaster>())
                (Manager.GetComponent(typeof(RayCaster)) as RayCaster).GameObjectSelected += OnSelection;


            base.Initialize();
        }
        public virtual void OnSelection(PhysicsComponent.GameObjectInfo info)
        {
            if (Manager.Owner.Scene.HasObject(info.ID))
            {
                if (SelectedObject != null)
                    SelectedObject.OnDestroy -= SelectedObject_OnDestroy;
                SelectedObject = Manager.Owner.Scene.GetObject(info.ID);
                SelectedObject.OnDestroy += SelectedObject_OnDestroy;
            }
        }

        private void SelectedObject_OnDestroy(string id)
        {
            SelectedObject = null;
        }

    }
}
