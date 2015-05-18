using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDef
{
    public class PathCheckPoint:GameObject
    {
        public Direction directionToReach;

        public PathCheckPoint(): base()
        {
            type = ObjectType.CHECKPOINT;
        }

        public override void loadContent(ContentManager content)
        {
        }
        
        public override void draw()
        {
        }


        public override void update()
        {

        }
    }
}
