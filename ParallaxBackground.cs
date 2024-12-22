using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Dynamic;
using System.Resources;
using ThanaNita.MonoGameTnt;
namespace GameProject
{
    public class ParallaxBackground : Actor
    {
        public Vector2 player1Position, player2Position;
        private MovingBackground first_lv1, second_lv1, first_lv2, second_lv2;
        public ParallaxBackground(string file, Vector2 screenSize, float lv1_speed, float lv2_speed, bool isGame)
        {
            var dir = "Resources/background/";
            var lv0 = dir + file + "_0.png";
            var lv1 = dir + file + "_1.png";
            var lv2 = dir + file + "_2.png";


            first_lv1 = new MovingBackground(lv1, screenSize, new Vector2(0, 0), lv1_speed, isGame);
            second_lv1 = new MovingBackground(lv1, screenSize, new Vector2(first_lv1.GetTextureWidth(), 0), lv1_speed, isGame);
            Add(first_lv1);
            Add(second_lv1);
            first_lv2 = new MovingBackground(lv2, screenSize, new Vector2(0, 0), lv2_speed, isGame);
            second_lv2 = new MovingBackground(lv2, screenSize, new Vector2(first_lv2.GetTextureWidth(), 0), lv2_speed, isGame);
            Add(first_lv2);
            Add(second_lv2);


            if (isGame)
            {
                // init
                var texture = new TextureRegion(TextureCache.Get(lv0));
                first_lv1 = new MovingBackground(lv1, screenSize, new Vector2(0, 0), lv1_speed, isGame);
                first_lv2 = new MovingBackground(lv2, screenSize, new Vector2(0, 0), lv2_speed, isGame);
                Add(new SpriteActor(texture) { Scale = new Vector2(4, 4) });
                Add(first_lv1);
                Add(first_lv2);

                //Fence
                var fenceRegion = new TextureRegion(TextureCache.Get("Resources/ground/fence.png"));
                var fence = new SpriteActor(fenceRegion);
                fence.Position = new Vector2(0, screenSize.Y - 330);
                fence.Scale = new Vector2(0.3f, 0.3f);
                Add(fence);
            }
        }

        public void getPlayerPosition(Vector2 player1Position, Vector2 player2Position)
        {
            this.player1Position = player1Position;
            this.player2Position = player2Position;
        }
        public void sentPosition()
        {
            first_lv1.getPosition(player1Position, player2Position);
            first_lv2.getPosition(player1Position, player2Position);
        }
        public void getLastPos(Vector2 player1, Vector2 player2)
        {
            first_lv1.getLastPosition(player1, player2);
            first_lv2.getLastPosition(player1, player2);
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            sentPosition();
        }
    }
}