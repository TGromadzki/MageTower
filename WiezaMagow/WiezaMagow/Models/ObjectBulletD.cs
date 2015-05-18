﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDef
{
    public class ObjectBulletD : GameObject
    {
        private BulletType bullettype;
        public BoundingSphere bulletSphere;

        public ObjectBulletD()
            : base()
        {
            type = ObjectType.BULLET_D;
        }

        public override void loadContent(ContentManager content)
        {
            model = content.Load<Model>("Models\\bullet");
        }

        public override void draw()
        {
            Matrix projection = Game1.projectionMatrix;
            Matrix view = Game1.viewMatrix;
            Matrix translateMatrix = Matrix.CreateTranslation(modelPosition);
            Matrix worldMatrix = Matrix.CreateScale(800.0f, 800.0f, 800.0f) * translateMatrix;

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
            bulletSphere = new BoundingSphere(modelPosition, 100.0f);
            modelPosition += new Vector3(0, -10.0f, -20.0f);
            if (modelPosition.Z == -100.0f)
            {
                modelPosition += new Vector3(0, 2000, 4000);
            }

        }
    }
}