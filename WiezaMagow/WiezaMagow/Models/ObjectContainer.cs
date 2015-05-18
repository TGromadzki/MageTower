using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDef
{
    public class ObjectContainer
    {
        ContentManager content;
        List<GameObject> list;

        public ObjectContainer(ContentManager content)
        {
            this.content = content;
            list = new List<GameObject>();
        }

        public void loadContent()
        {

        }

        public void draw()
        {
            foreach (GameObject o in list)
            {
                o.draw();
            }
        }

        public void update()
        {
            foreach (GameObject o in list)
            {
                o.update();
            }
        }

        public static void addObject(ObjectType objectType, ContentManager content2, Vector3 pos)
        {
            switch (objectType)
            {
                case ObjectType.ENEMY:
                    GameObject o = new ObjectEnemy(EnemyType.ST1);
                    o.loadContent(content2);
                    o.modelPosition = pos;
                    break;
            }
        }
    }
}
