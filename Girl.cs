using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThanaNita.MonoGameTnt;

namespace Game09
{
    public class Girl : SpriteActor
    {
        AnimationStates states;
        Vector2 V;
        bool Onfloor = false;
        bool isAttacking = false; // ไว้เช็คว่าตีอยู่หรือป่าว
        CollisionObj attackHitbox; // Hitbox สำหรับโจมตี

        public Girl(Vector2 position)
        {
            var size = new Vector2(60, 60);
            Position = position;
            Origin = size / 2;
            Scale = new Vector2(2, 2);

            var texture = TextureCache.Get("Girl.png");
            var regions2d = RegionCutter.Cut(texture, size);
            var selector = new RegionSelector(regions2d);

            // แอนิเมชันสำหรับการยืน, เดินซ้าย, เดินขวา, และโจมตี
            var stay = new Animation(this, 1.0f, selector.Select1by1(0, 4));
            var left = new Animation(this, 1.0f, selector.Select(start: 8, count: 8));
            var right = new Animation(this, 1.0f, selector.Select(start: 16, count: 8));
            var attack = new Animation(this, 0.5f, selector.Select(start: 0, count: 4)); //ใช้แทนไปก่อน

            states = new AnimationStates(new[] { stay, left, right, attack });
            AddAction(states);

            // การชนกัน
            var collisionObj = CollisionObj.CreateWithRect(this, RawRect.CreateAdjusted(0.4f, 1), 1);
            collisionObj.OnCollide = OnCollide;
            collisionObj.DebugDraw = true;
            Add(collisionObj);

            // Hitbox สำหรับโจมตี
            attackHitbox = CollisionObj.CreateWithRect(this, RawRect.CreateAdjusted(0.8f, 0.5f), 1); // สร้าง Hitbox
            attackHitbox.DebugDraw = false;
            Add(attackHitbox);
        }

        private void OnCollide(CollisionObj objB, CollideData data)
        {
            var direction = data.objA.RelativeDirection(data.OverlapRect);

            if (direction.Y > 0)
                Onfloor = true;
            if ((direction.Y > 0 && V.Y > 0) ||
                (direction.Y < 0 && V.Y < 0))
            {
                V.Y = 0;
                Position -= new Vector2(0, data.OverlapRect.Height * direction.Y);
            }

            if ((direction.X > 0 && V.X > 0) ||
                (direction.X < 0 && V.X < 0))
            {
                V.X = 0;
                Position -= new Vector2(data.OverlapRect.Width * direction.X, 0);
            }
        }

        public override void Act(float deltaTime)
        {
            ChangeVy(deltaTime);
            HandleInput();

            if (!isAttacking)
            {
                var direction = DirectionKey.Direction;
                V.X = direction.X * 500;

                if (direction.X > 0)
                    states.Animate(2); // เดินขวา
                else if (direction.X < 0)
                    states.Animate(1); // เดินซ้าย
                else
                    states.Animate(0); // ยืน
            }
            else
            {
                // จัดการการโจมตี
                if (GlobalKeyboardInfo.Value.IsKeyDown(Keys.Left))
                {
                    attackHitbox.Position = Position + new Vector2(-60, 0); // Hitbox ไปทางซ้าย
                    states.Animate(3);
                }
                else if (GlobalKeyboardInfo.Value.IsKeyDown(Keys.Right))
                {
                    attackHitbox.Position = Position + new Vector2(60, 0); // Hitbox ไปทางขวา
                    states.Animate(3);
                }

                attackHitbox.DebugDraw = true;
            }

            base.Act(deltaTime);
            Position += V * deltaTime; // s += v*dt

            Onfloor = false; // รีเซ็ตสถานะบนพื้น
        }

        private void HandleInput()
        {
            var keyInfo = GlobalKeyboardInfo.Value;

            // เมื่อกดปุ่มโจมตี (D1)
            if (keyInfo.IsKeyPressed(Keys.D1) && (keyInfo.IsKeyDown(Keys.Left) || keyInfo.IsKeyDown(Keys.Right)))
            {
                isAttacking = true;
                states.Animate(3); // เปลี่ยนเป็นแอนิเมชันโจมตี
            }
            else
            {
                isAttacking = false; // เมื่อไม่ได้กดโจมตี
                attackHitbox.DebugDraw = false; // ซ่อน hitbox
            }
        }

        private void ChangeVy(float deltaTime)
        {
            // แรงโน้มถ่วง
            var a = new Vector2(0, 1500);
            V.Y += a.Y * deltaTime;

            // กระโดด
            var keyInfo = GlobalKeyboardInfo.Value;
            if (keyInfo.IsKeyPressed(Keys.Space) && Onfloor)
                V.Y = -750;

            // เจ็ต
            if (keyInfo.IsKeyDown(Keys.Tab))
                V.Y = -500;
        }
    }
}
