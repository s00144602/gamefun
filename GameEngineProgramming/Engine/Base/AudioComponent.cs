using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class AudioComponent : Component
    {
        public AudioComponent(string id) : base(id) { }

        public virtual void Play() { }
        public virtual void Pause() { }
        public virtual void Stop() { }
    }
}
