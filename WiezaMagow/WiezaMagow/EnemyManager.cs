using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDef
{
    class EnemyManager
    {
        ObjectContainer objectContainer;
        ContentManager content;
        Random rnd = new Random();
        int timer;
        int timer2;
        int maxEnemies;
        int i;

        public EnemyManager(ContentManager content, ObjectContainer objectContainer)
        {
            this.content = content;
            this.objectContainer = objectContainer;
            timer = 0;
            timer2 = 0;
            maxEnemies = 0;
            i = 0;
        }

        public void addEnemy()
        {
            GameObject o = objectContainer.firstCheckPoint();
            if (o != null)
            {
                GameObject go = objectContainer.addObject(ObjectType.ENEMY, new Vector3(o.modelPosition.X, o.modelPosition.Y, o.modelPosition.Z + 700.0f), new Position(o.positionOnMap.x, o.positionOnMap.y));
                ObjectEnemy oe = (ObjectEnemy)go;
                oe.nextCheckPoint = (PathCheckPoint)objectContainer.findCheckPoint(1);
                oe.direction = oe.nextCheckPoint.directionToReach;
            }
        }

        public void update()
        {
            
            maxEnemies = rnd.Next(3, 10);
            timer2++;
            if (i < maxEnemies)
            {
                timer++;          
                if (timer > 100)
                {
                    addEnemy();
                    i++;
                    timer = 0;
                }
            }
            if (timer2 > maxEnemies * 500)
            {
                i = 0;
                timer2 = 0;
            }
        }
    }
}
