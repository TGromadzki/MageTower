using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDef
{
    public class ObjectEnemy:GameObject
    {
        private int hp;
        private EnemyType enemyType;
        public Direction direction;
        public PathCheckPoint nextCheckPoint;
        private float distanceMoved;
        public int checkPointsCounter;
        public Matrix rotationMatrix;
        public CollisionType collisionWithBullet;
        public bool collision = false;
        public BoundingSphere enemyBoundingSphere;
       
        public ObjectEnemy(EnemyType enemyType):base()
        {
            type = ObjectType.ENEMY;
            this.enemyType = enemyType;
            distanceMoved = 0.0f;
            checkPointsCounter = 1;
            this.enemyBoundingSphere = new BoundingSphere(modelPosition, 500.0f);
        }

        public override void draw()
        {
            Matrix projection = Game1.projectionMatrix;
            Matrix view = Game1.viewMatrix;
            Matrix translateMatrix = Matrix.CreateTranslation(modelPosition);
            Matrix worldMatrix =  Matrix.CreateScale(600.0f, 600.0f, 600.0f) * rotationMatrix * translateMatrix ;

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
            enemyBoundingSphere = new BoundingSphere(modelPosition, 500.0f);
            distanceMoved += 20.0f;
            switch (direction)
            {
                case Direction.DOWN:
                    rotationMatrix = Matrix.CreateRotationZ(-MathHelper.PiOver2);
                    modelPosition += new Vector3(0, -20.0f, 0);
                    if (distanceMoved == GlobalGameConstants.mapGroundPlatesDistance)
                    {
                        positionOnMap.y--;
                        distanceMoved = 0.0f;
                    }
                    break;
                case Direction.LEFT:
                    rotationMatrix = Matrix.CreateRotationZ(-MathHelper.PiOver2);
                    modelPosition += new Vector3(-20.0f, 0, 0);
                    if (distanceMoved == GlobalGameConstants.mapGroundPlatesDistance)
                    {
                        positionOnMap.x--;
                        distanceMoved = 0.0f;
                    }
                    break;
                case Direction.RIGHT:
                    rotationMatrix = Matrix.CreateRotationZ(MathHelper.TwoPi);
                    modelPosition += new Vector3(20.0f, 0, 0);
                    if (distanceMoved == GlobalGameConstants.mapGroundPlatesDistance)
                    {
                        positionOnMap.x++;
                        distanceMoved = 0.0f;
                    }
                    break;
                case Direction.UP:
                    rotationMatrix = Matrix.CreateRotationZ(MathHelper.PiOver2);
                    modelPosition += new Vector3(0, 20.0f, 0);
                    if (distanceMoved == GlobalGameConstants.mapGroundPlatesDistance)
                    {
                        positionOnMap.y++;
                        distanceMoved = 0.0f;
                    }
                    break;
            }            
        }

        public override void loadContent(ContentManager content)
        {
            model = content.Load<Model>("Models\\enemy");
        }

        public bool reachedCheckPoint()
        {
            if (positionOnMap.x == nextCheckPoint.positionOnMap.x && positionOnMap.y == nextCheckPoint.positionOnMap.y)
                return true;
            return false;
        }

        public bool CheckCollision(BoundingSphere sphere, List<GameObject> bulletList)
        {
            foreach (GameObject b in bulletList)
            {
                b.update();
                ObjectBullet ob = (ObjectBullet)b;
                if (ob.bulletSphere.Contains(sphere) != ContainmentType.Disjoint)
                    return true;
            }
                return false;
        }

        public bool CheckCollisionR(BoundingSphere sphere, List<GameObject> bulletListR)
        {
            foreach (GameObject br in bulletListR)
            {
                br.update();
                ObjectBulletR obr = (ObjectBulletR)br;
                if (obr.bulletSphere.Contains(sphere) != ContainmentType.Disjoint)
                    return true;
            }
            return false;
        }

        public bool CheckCollisionD(BoundingSphere sphere, List<GameObject> bulletListD)
        {
            foreach (GameObject bd in bulletListD)
            {
                bd.update();
                ObjectBulletD obd = (ObjectBulletD)bd;
                if (obd.bulletSphere.Contains(sphere) != ContainmentType.Disjoint)
                    return true;
            }
            return false;
        }

        public bool CheckCollisionU(BoundingSphere sphere, List<GameObject> bulletListU)
        {
            foreach (GameObject bu in bulletListU)
            {
                bu.update();
                ObjectBulletU obu = (ObjectBulletU)bu;
                if (obu.bulletSphere.Contains(sphere) != ContainmentType.Disjoint)
                    return true;
            }
            return false;
        }
    }
}
