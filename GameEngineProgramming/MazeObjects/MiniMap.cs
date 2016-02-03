using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngineProgramming.MazeObjects
{
    public class MiniMap
    {
        public List<FloorTile> FloorTiles;
        public Texture2D Revealed;
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        private int Scale = 2;
        Rectangle DestinationRect;
        private float Transparency = .6f;
        private Vector2 Origin;
        public MiniMap(List<FloorTile> _FloorTiles, Texture2D _Revealed, Vector2 _EndPosition)
        {
            FloorTiles = _FloorTiles;
            Revealed = _Revealed;
            StartPosition = new Vector2(10,10);
            EndPosition = new Vector2((_EndPosition.X) * (6* Scale), (_EndPosition.Y) * (6 * Scale));
            Origin = new Vector2(StartPosition.X + (EndPosition.X / 2), StartPosition.Y + (EndPosition.Y / 2));
        }
        public void Draw(SpriteBatch sp)
        {
            //sp.Draw(Revealed, new Rectangle((int)StartPosition.X, (int)StartPosition.Y,(int)EndPosition.X,(int)EndPosition.Y), Color.Black * Transparency);
            foreach (FloorTile FloorTile in FloorTiles)
            {

                Vector2 PositionRect = new Vector2((FloorTile.Location.X * Scale) + StartPosition.X, (FloorTile.Location.Z * Scale) + StartPosition.Y);
                DestinationRect = new Rectangle((int)PositionRect.X, (int)PositionRect.Y, (int)FloorTile.Width * Scale, (int)FloorTile.Depth * Scale);

                if (FloorTile.Revealed)
                {
                    sp.Draw(Revealed, DestinationRect, Color.White * Transparency);
                }
                if (FloorTile.CurrentCell)
                {
                    sp.Draw(Revealed, DestinationRect, Color.Red * Transparency);
                }
            }
        }
    }
}
