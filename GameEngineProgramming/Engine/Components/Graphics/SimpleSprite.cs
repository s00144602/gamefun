using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.Engine.Components.Graphics
{
    public class SimpleSprite : RenderComponent
    {
        private string _asset;
        public Texture2D Texture { get; set; }
        private SpriteBatch batch;
        private BasicEffect effect;
        Vector2 origin;
        public bool IsBillboard { get; set; }
        public SimpleSprite (string id, string asset) : base(id)
        {
            _asset = asset;
        }
        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(_asset))
            {
                Texture = GameUtilities.NonPersistentContent.Load<Texture2D>("Texture\\" + _asset);
                origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

                batch = new SpriteBatch(GameUtilities.GraphicsDevice);

                effect = new BasicEffect(GameUtilities.GraphicsDevice);
                effect.TextureEnabled = true;
            }

            base.Initialize();
        }
        public override void Draw(CameraComponent camera)
        {
            if (IsBillboard)
            {
                effect.World = Matrix.CreateConstrainedBillboard(Manager.Owner.World.Translation, camera.Manager.Owner.World.Translation, Vector3.Down, null, null);
            }
            else
            {
                effect.World = Matrix.CreateScale(1, 1, 1) * Matrix.CreateTranslation(Manager.Owner.World.Translation);
            }
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, effect);
            batch.Draw(Texture, Vector2.Zero, null, Color.White, 0, origin, 0.01f, SpriteEffects.None, 0);
            batch.End();
            base.Draw(camera);
            GameUtilities.SetGraphicsDeviceFor3D();
        }
    }
}
