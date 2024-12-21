using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class PlayerInputHandler
    {
        private KeyScheme keyScheme;
        private Dictionary<Keys, float> lastPressedKeys;
        private float threshold = 0.2f;
        public PlayerInputHandler(KeyScheme keyScheme)
        {
            this.keyScheme = keyScheme;
            lastPressedKeys = new Dictionary<Keys, float>();
        }
        public Vector2 getDirection(KeyboardInfo key)
        {
            var direction = Vector2.Zero;
            if(key.IsKeyDown(keyScheme.up)) direction.Y -= 1;
            if(key.IsKeyDown(keyScheme.down)) direction.Y += 1;
            if(key.IsKeyDown(keyScheme.right)) direction.X += 1;
            if(key.IsKeyDown(keyScheme.left)) direction.X -= 1;

            return direction;
        }
        public bool isJumpPressed(KeyboardInfo keys, Vector2 direction)
        {
            return keys.IsKeyPressed(keyScheme.jump)|| direction.Y == -1; 
        }
        public bool isAttackPressed(KeyboardInfo keys)
        {
            return keys.IsKeyPressed(keyScheme.attack);
        }
        public bool isKickPressed(KeyboardInfo keys)
        {
            return keys.IsKeyPressed(keyScheme.kick);
        }
        public bool isBlockingPressed(KeyboardInfo keys)
        {
            return keys.IsKeyDown(keyScheme.down);
        }
        public bool isDoublePressed(Keys key, float pressedTime)
        {
            var isPressed = GlobalKeyboardInfo.Value.IsKeyPressed(key);
            if(isPressed)
            {
                if (!lastPressedKeys.ContainsKey(key))
                {
                    lastPressedKeys[key] = pressedTime;
                    return false;
                }
                float lastTime = lastPressedKeys[key];
                if ((pressedTime - lastTime) <= threshold)
                {
                    lastPressedKeys.Remove(key);
                    return true;
                }
                lastPressedKeys[key] = pressedTime;
            }

            return false;
        }

    }
}