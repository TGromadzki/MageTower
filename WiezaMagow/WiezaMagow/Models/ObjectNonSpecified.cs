using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDef
{
    public class ObjectNonSpecified: GameObject
    {
        public ObjectNonSpecified(): base()
        {
            type = ObjectType.NOT_SPECIFIED;
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
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
