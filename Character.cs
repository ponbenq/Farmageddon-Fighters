using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class Character
    {
        private static List<string> spritePaths = new List<string> 
        {
            "Resources/sprite/mushroom/mushroom.png",
            "Resources/sprite/broccoli/broccoli.png",
            "Resources/sprite/pumpkin/pumpkin.png",
            "Resources/sprite/eggplant/eggplant.png",
            "Resources/sprite/garlic/garlic.png"
        };

        public static string GetSpritePath(int charIndex)
        {
            return spritePaths[charIndex];
        }

        public static int GetCharacterIndex(string spritePath)
        {
            return spritePaths.IndexOf(spritePath);
        }
    }
}
