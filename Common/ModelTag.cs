﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ModelTag
    {
        public List<Vector3> Vertices { get; set; }
        public List <int> Indices { get; set; }

        public ModelTag()
        {
            Vertices = new List<Vector3>();
            Indices = new List<int>();
        }
        public ModelTag(List<Vector3> vertices, List<int> indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}
