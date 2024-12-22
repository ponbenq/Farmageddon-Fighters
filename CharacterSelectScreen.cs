using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GameProject
{
    public class CharacterSelectScreen : Actor
    {
        TextureRegion[] tiles;
        TileMap tileMap;
        PlayerSelect player1, player2;
        Vector2 tileSize;
        GameStart gameStart;
        ImageButton startButton ,stageButton;
        string player1Sprite, player2Sprite;
        Boolean player1Selected, player2Selected, stageSelected = false;
        SoundEffect select, move;
        string stage;
        Text selectedStageText;

        public CharacterSelectScreen(Vector2 screenSize, GameStart gameStart)
        {

            //Background
            var file = "bgmain";
            Add(new ParallaxBackground(file, screenSize, 20f, 50f, false));

            //Stage select
            var stageText = new Text("Resources/Fonts/ZFTERMIN__.ttf", 70, Color.White, "Stage select");
            stageText.Origin = stageText.RawSize / 2;
            stageText.Effect = FontStashSharp.FontSystemEffect.Stroked;
            stageText.EffectAmount = 3;
            stageText.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 - 450);
            this.Add(stageText);

            // Stage selection buttons
            AddStageButtons(screenSize);

            selectedStageText = new Text("Resources/Fonts/ZFTERMIN__.ttf", 80, Color.SteelBlue, "");
            selectedStageText.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 - 70);
            this.Add(selectedStageText);



            //Fighter select
            var fighterText = new Text("Resources/Fonts/ZFTERMIN__.ttf", 70, Color.White, "Fighter select");
            fighterText.Origin = fighterText.RawSize / 2;
            fighterText.Effect = FontStashSharp.FontSystemEffect.Stroked;
            fighterText.EffectAmount = 2;
            fighterText.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 50);
            this.Add(fighterText);

            this.gameStart = gameStart;
            tileSize = new Vector2(224, 224);

            PrepareTileSet();
            var tileArray = new int[1, 5] {
                {0, 1, 2, 3, 4}
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
            visual.Position = new Vector2(screenSize.X / 2 -560, screenSize.Y / 2 + 100);
            Add(visual);

            //Start button
            var btnRegion = new TextureRegion(TextureCache.Get("Resources/img/btn.png"));
            startButton = new ImageButton(btnRegion);
            startButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 400);
            startButton.Origin = btnRegion.Size / 2;
            startButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65, Color.DimGray, "Start");
            startButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            startButton.ButtonClicked += GameStart;

            select = SoundEffect.FromFile("Resources/soundeffect/select.wav");
            move = SoundEffect.FromFile("Resources/soundeffect/move.wav");
        }
        private void AddStageButtons(Vector2 screenSize)
        {
            Vector2[] offsets = { new Vector2(-666, -250), new Vector2(-222, -250), new Vector2(222, -250), new Vector2(666, -250) };
            RectF[] sprites = { new RectF(0, 0, 444, 250), new RectF(444, 0, 444, 250), new RectF(888, 0, 444, 250), new RectF(1332, 0, 444, 250) };

            for (int i = 0; i < 4; i++)
            {
                var stageRegion = new TextureRegion(TextureCache.Get("Resources/sprite/stage_frame.png"), sprites[i]);
                var stageButton = new ImageButton(stageRegion)
                {
                    Position = new Vector2(screenSize.X / 2 + offsets[i].X, screenSize.Y / 2 + offsets[i].Y),
                    Origin = stageRegion.Size / 2
                };
                stageButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 80, Color.SlateGray, (i + 1).ToString());
                stageButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
                stageButton.ButtonClicked += (button) => SelectStage((ImageButton)button);
                Add(stageButton);
            }
        }
        private void SelectStage(ImageButton button)
        {
            var stageNumber = button.Str;
            stage = $"stage{stageNumber}";
            stageSelected = true;
            selectedStageText.Str = $"STAGE {stageNumber} !!";
            selectedStageText.Origin = selectedStageText.RawSize / 2;
            
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
                select.Play();
            }
            if (keyInfo.IsKeyPressed(Keys.Space))
            {
                player2Sprite = GetSprite(player2);
                player2.Detach();
                player2Selected = true;
                select.Play();
            }

            if (player1Selected & player2Selected & stageSelected)
            {
                Add(startButton);
            }
        }

        public void GameStart(GenericButton button)
        {
            startButton.Detach();
            AddAction(new RunAction(() => gameStart(player1Sprite, player2Sprite, stage)));
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
            var texture = TextureCache.Get("Resources/sprite/char_frame.png");
            var tiles2d = RegionCutter.Cut(texture, new Vector2(56, 56), countX: 5, countY: 1);
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
            move.Play();
        }
        private bool IsAllowMove(PlayerSelect player, Vector2 direction)
        {
            Vector2i index = tileMap.CalcIndex(player.Position, direction);
            return tileMap.IsInside(index);
        }
    }
}
