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
        { "Resources/sprite/broccoli/broccoli.png",
            "Resources/sprite/eggplant/eggplant.png",
            "Resources/sprite/mushroom/mushroom.png",
            "Resources/sprite/pumpkin/pumpkin.png"
        };

        public static string GetSpritePath(int charIndex)
        {
            return spritePaths[charIndex];
        }
    }
}
