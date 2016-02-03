using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using Common;

namespace PipelineExtensions
{
    [ContentProcessor(DisplayName ="Basic Model Processor")]
    public class BasicModelProcessor : ModelProcessor
    {
        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _indices = new List<int>();
        
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ModelContent model = base.Process(input, context);

            FindVertices(input);

            ModelTag tag = new ModelTag()
            {
                Vertices = _vertices,
                Indices = _indices
            };

            model.Tag = tag;

            return model;
        }
        void FindVertices(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                Matrix absoluteTransform = mesh.AbsoluteTransform;
                foreach (GeometryContent geometry in mesh.Geometry)
                {
                    foreach(int index in geometry.Indices)
                    {
                        Vector3 vertex = geometry.Vertices.Positions[index];
                        vertex = Vector3.Transform(vertex, absoluteTransform);
                        _vertices.Add(vertex);
                        _indices.Add(index);
                    }
                        
                }
            }
            foreach(NodeContent child in node.Children)
            {
                FindVertices(child);
            }
        }
    }
}
