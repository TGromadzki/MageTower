using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDef
{
    public abstract class GameObject
    {
        public ObjectType type { get; set; }
        public Model model { get; set; }
        public Vector3 modelPosition { get; set; }
        public bool isSelected { get; set; }
        public Position positionOnMap { get; set; }
        public BoundingSphere boundingSphere { get; set; }

        public GameObject()
        {
            model = null;
            modelPosition = Vector3.Zero;
            type = ObjectType.NOT_SPECIFIED;
            isSelected=false;
        }

        public abstract void loadContent(ContentManager content);

        public abstract void draw();

        public abstract void update();
    }
}
