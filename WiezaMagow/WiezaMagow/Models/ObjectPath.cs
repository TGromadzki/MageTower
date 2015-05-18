using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDef
{
    public class ObjectPath : GameObject
    {
        private Model modelSelected;
        private Model modelEmpty;

        public ObjectPath():base()
        {
            type = ObjectType.PATH;
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            modelEmpty = content.Load<Model>("Models\\ground_panel_path");
            modelSelected = content.Load<Model>("Models\\ground_panel_path_green");

            model = modelEmpty;
        }
        
        public override void draw()
        {
            Matrix projection = Game1.projectionMatrix;
            Matrix view = Game1.viewMatrix;

            Matrix translateMatrix = Matrix.CreateTranslation(modelPosition);
            Matrix worldMatrix = translateMatrix;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }


        public override void update()
        {
            if (isSelected)
            {
                Console.WriteLine("Selected: " + modelPosition.X + ";" + modelPosition.Y);
                model = modelSelected;
            }
            else
            {
                model = modelEmpty;
            }
        }
    }
}
