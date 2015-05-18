using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDef
{
    public class GlobalGameConstants
    {
        public const float nearClip = 100.0f;
        public const float farClip = 1000000.0f;
        public const float viewAngle = 45.0f;

        public const float cameraPositionX = 24000.0f;
        public const float cameraPositionY = 24000.0f;
        public const float cameraHeight = 19000.0f;
               
        public const int mapWidth = 20;
        public const int mapHeight = 15;

        public const float mapGroundPlatesDistance = 1600.0f;
    }

}
