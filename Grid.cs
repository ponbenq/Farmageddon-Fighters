using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class Grid : Actor
    {
        VertexArray array;
        public Grid(Vector2 size, int countX, int countY, Color color)
        {
            for (uint i = 0; i <= countX; i++)
            {
                var start = new Vector2(size.X * i, 0);
                var end = start + new Vector2(0, countY * size.Y);
                Add(RectangleActor.VerticalLine(color, size.X * i, 0, countY * size.Y, 1));
            }

            uint startIndex = (uint)(countX + 1) * 2;
            for (uint i = 0; i <= countY; i++)
            {
                var start = new Vector2(0, size.Y * i);
                var end = start + new Vector2(countX * size.X, 0);
                Add(RectangleActor.HorizontalLine(color, size.Y * i, 0, countX * size.X, 1));
            }
        }
    }
}
