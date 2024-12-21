using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class MoveBackground : Actor
    {
        public MoveBackground(string file, Vector2 screenSize)
        {
            var lv1 = file + "_1.png";
            var lv2 = file + "_2.png";
           
            var first_lv1 = new Background(lv1, screenSize, new Vector2(0, 0), 50f);
            var second_lv1 = new Background(lv1, screenSize, new Vector2(first_lv1.GetTextureWidth(), 0), 50f);
            Add(first_lv1);
            Add(second_lv1);

            var first_lv2 = new Background(lv2, screenSize, new Vector2(0, 0), 100f);
            var second_lv2 = new Background(lv2, screenSize, new Vector2(first_lv2.GetTextureWidth(), 0), 100f);
            Add(first_lv2);
            Add(second_lv2);
        }
    }
}