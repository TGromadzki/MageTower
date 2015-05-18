using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDef
{
    public class ObjectTowerSingle:GameObject
    {
        Model modelTower,modelGround,modelSelected;
        public ObjectTowerSingle()
            : base()
        {
            type = ObjectType.TOWER;
        }
        private float angle = 0f;

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            modelTower = content.Load<Model>("Models\\testing_tower");
            modelGround = content.Load<Model>("Models\\ground_panel_build_grey");
            modelSelected = content.Load<Model>("Models\\testing_tower_green");

            model = modelTower;
        }

        public override void draw()
        {
            Matrix projection = Game1.projectionMatrix;
            Matrix view = Game1.viewMatrix;

            Matrix translateMatrix = Matrix.CreateTranslation(modelPosition);
            Matrix worldMatrix = translateMatrix;

            Matrix eyeTranslateMatrix = Matrix.CreateRotationZ(3 * angle) * Matrix.CreateTranslation(new Vector3(modelPosition.X, modelPosition.Y, modelPosition.Z + 2600.0f));
            Matrix eyeWorldMatrix = eyeTranslateMatrix;

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (mesh.Name == "Tower")
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = worldMatrix;
                        effect.View = view;
                        effect.Projection = projection;

                        effect.EnableDefaultLighting();
                    }
                }
                else
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = eyeWorldMatrix;
                        effect.View = view;
                        effect.Projection = projection;

                        effect.EnableDefaultLighting();
                    }
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
            angle += 0.005f;

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
