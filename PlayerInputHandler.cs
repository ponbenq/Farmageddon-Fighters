using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class PlayerInputHandler
    {
        private KeyScheme keyScheme;
        private Dictionary<Keys, float> lastPressedKeys;
        private float threshold = 0.3f;
        public PlayerInputHandler(KeyScheme keyScheme)
        {
            this.keyScheme = keyScheme;
            lastPressedKeys = new Dictionary<Keys, float>();
        }
        public bool isJumpPressed(KeyboardInfo keys, Vector2 direction)
        {
            return keys.IsKeyDown(keyScheme.jump) || direction.Y == -1; 
        }
        public bool isAttackPressed(KeyboardInfo keys)
        {
            return keys.IsKeyDown(keyScheme.attack);
        }
        public bool isDoublePressed(Keys keys, float pressedTime)
        {
            if(!lastPressedKeys.ContainsKey(keys))
            {
                lastPressedKeys[keys] = pressedTime;
                return false;
            }
            float lastTime = lastPressedKeys[keys];
            if((pressedTime - lastTime) <= threshold)
            {
                // reset current pressed time
                lastPressedKeys[keys] = 0f;
                return true;
            }
            else
            {
                //if not set same current time
                lastPressedKeys[keys] = pressedTime;
                return false;
            }
        }

    }
}