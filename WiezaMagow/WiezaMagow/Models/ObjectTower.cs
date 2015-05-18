using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDef
{
    public class ObjectTower:GameObject
    {
        Model modelTower,modelGround,modelSelected;
        public ObjectTower():base()
        {
            type = ObjectType.TOWER;
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            modelTower = content.Load<Model>("Models\\Tower_AOE_slow");
            modelGround = content.Load<Model>("Models\\ground_panel_build_grey");
            modelSelected = content.Load<Model>("Models\\Tower_AOE_slow_green");

            //modelTower = content.Load<Model>("Models\\Tower_AOE_slow_animated");
            //modelGround = content.Load<Model>("Models\\ground_panel_build_grey");
            //modelSelected = content.Load<Model>("Models\\Tower_single_target");

            model = modelTower;
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

            Vector3 groundPosition = new Vector3(modelPosition.X, modelPosition.Y, modelPosition.Z - 1300.0f);
            translateMatrix = Matrix.CreateTranslation(groundPosition);
            worldMatrix = translateMatrix;

            foreach (ModelMesh mesh in modelGround.Meshes)
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
                model = modelSelected;
            }
            else
            {
                model = modelTower;
            }
        }


    }
}
