using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDef
{
    class ObjectEnd:GameObject
    {
        public ObjectEnd(): base()
        {
            type = ObjectType.END;
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
