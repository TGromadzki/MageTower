using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDef
{
    public class ObjectEmptyField : GameObject
    {
        private Model modelSelected;
        private Model modelEmpty;

        public ObjectEmptyField():base()
        {
            type = ObjectType.EMPTY;
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            modelEmpty = content.Load<Model>("Models\\ground_panel_build_grey");
            modelSelected = content.Load<Model>("Models\\ground_panel_build_green");

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
