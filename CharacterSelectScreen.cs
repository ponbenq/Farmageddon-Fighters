using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class CharacterSelectScreen : Actor
    {
        TextureRegion[] tiles;
        TileMap tileMap;
        PlayerSelect player1, player2;
        Vector2 tileSize;
        GameStart gameStart;
        Button startButton;
        string player1Sprite, player2Sprite;
        Boolean player1Selected, player2Selected = false;


        public CharacterSelectScreen(Vector2 screenSize, GameStart gameStart)
        {
            this.gameStart = gameStart;
            tileSize = new Vector2(200, 200);

            PrepareTileSet();
            var tileArray = new int[1, 4] {
                {0, 1, 2, 3}
            };
            tileMap = new TileMap(tileSize, tileArray, CreateTile);
            //tileMap.Origin = tileMap.RawSize / 2;
            //tileMap.Position = screenSize / 2;
            var visual = new Actor();
            visual.Add(tileMap);

            player1 = new PlayerSelect(0) { Position = tileSize / 2 };
            player2 = new PlayerSelect(1) { Position = tileSize / 2 };
            visual.Add(player1);
            visual.Add(player2);
            visual.Origin = visual.RawSize / 2;
            visual.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2);
            Add(visual);

            startButton = new Button("Simvoni.ttf", 50, Color.Brown, "Start", new Vector2(300, 100));
            startButton.Position = new Vector2(screenSize.X / 2.5f, screenSize.Y / 2 + 200);
            startButton.ButtonClicked += GameStart;
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            StepJumpMpvement();

            var keyInfo = GlobalKeyboardInfo.Value;
            if (keyInfo.IsKeyPressed(Keys.Enter))
            {
                player1Sprite = GetSprite(player1);
                player1.Detach();
                player1Selected = true;
            }
            if (keyInfo.IsKeyPressed(Keys.Space))
            {
                player2Sprite = GetSprite(player2);
                player2.Detach();
                player2Selected = true;
            }

            if (player1Selected & player2Selected)
            {
                Add(startButton);
            }
        }

        public void GameStart(GenericButton button)
        {
            startButton.Detach();
            AddAction(new RunAction(() => gameStart(player1Sprite, player2Sprite)));
        }

        public string GetSprite(PlayerSelect player)
        {
            var spritePath = "";
            Vector2i index = tileMap.CalcIndex(player.Position, new Vector2(0, 0));
            int tileCode = tileMap.GetTileCode(index);
            spritePath = Character.GetSpritePath(tileCode);
            return spritePath;
        }
        private void PrepareTileSet()
        {
            var texture = TextureCache.Get("Resources/sprite/characterTiles.png");
            var tiles2d = RegionCutter.Cut(texture, new Vector2(50, 50), countX: 4, countY: 1);
            tiles = RegionSelector.SelectAll(tiles2d);
        }

        private Actor CreateTile(int tileCode)
        {
            var sprite = new SpriteActor(tiles[tileCode]);
            sprite.Origin = sprite.RawSize / 2;
            sprite.Scale = new Vector2(4, 4);
            return sprite;
        }

        private void StepJumpMpvement()
        {
            var keyInfo = GlobalKeyboardInfo.Value;
            if (!keyInfo.IsAnyKeyPressed())
            {
                return;
            }

            var key = keyInfo.GetPressedKeys()[0];
            var player1Direction = DirectionKey.DirectionOf(key);
            if (!IsAllowMove(player1, player1Direction))
            {
                return;
            }
            player1.Position += player1Direction * tileMap.TileSize;

            //Turn w,a,s,d to direction for player2
            var player2Direction = new Vector2();
            if (keyInfo.IsKeyDown(Keys.W))
            {
                player2Direction = new Vector2(0, -1);
            }
            if (keyInfo.IsKeyDown(Keys.S))
            {
                player2Direction = new Vector2(0, 1);
            }
            if (keyInfo.IsKeyDown(Keys.A))
            {
                player2Direction = new Vector2(-1, 0);
            }
            if (keyInfo.IsKeyDown(Keys.D))
            {
                player2Direction = new Vector2(1, 0);
            }
            if (!IsAllowMove(player2, player2Direction))
            {
                return;
            }
            player2.Position += player2Direction * tileMap.TileSize;

        }
        private bool IsAllowMove(PlayerSelect player, Vector2 direction)
        {
            Vector2i index = tileMap.CalcIndex(player.Position, direction);
            return tileMap.IsInside(index);
        }
    }
}
