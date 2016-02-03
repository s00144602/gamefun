using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Base
{
    public class ScriptComponent : Component
    {
        public event ObjectStringIDHandler OnComplete;

        public ScriptComponent(string id)
        : base(id)
        {

        }

        public virtual bool HasCompleted()
        {
            return false;
        }

        public override void Update()
        {
            if (HasCompleted())
                if (OnComplete != null)
                    OnComplete(this.ID);
            base.Update();
        }
    }
}
