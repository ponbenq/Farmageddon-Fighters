using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Resources;
using ThanaNita.MonoGameTnt;
namespace GameProject
{
    public class ParallaxBackground : Actor
    {
        public ParallaxBackground(string file, Vector2 screenSize, float lv1_speed, float lv2_speed)
        {
            var dir = "Resources/background/";
            var lv1 = dir + file + "_1.png";
            var lv2 = dir + file + "_2.png";

            var first_lv1 = new MovingBackground(lv1, screenSize, new Vector2(0, 0), lv1_speed);
            var second_lv1 = new MovingBackground(lv1, screenSize, new Vector2(first_lv1.GetTextureWidth(), 0), lv1_speed);
            Add(first_lv1);
            Add(second_lv1);
            var first_lv2 = new MovingBackground(lv2, screenSize, new Vector2(0, 0), lv2_speed);
            var second_lv2 = new MovingBackground(lv2, screenSize, new Vector2(first_lv2.GetTextureWidth(), 0), lv2_speed);
            Add(first_lv2);
            Add(second_lv2);
        }
    }
}