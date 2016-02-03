using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Graphics
{
    public class BillboardSpriteFont : RenderComponent
    {
        string _asset;
        SpriteBatch batch;
        SpriteFont sfont;
        BasicEffect effect;
        Vector2 origin;
        public string Text { get; set; }
        public BillboardSpriteFont(string id, string asset, string text) : base(id)
        {
            _asset = asset;
            Text = text;
        }
    }
}
