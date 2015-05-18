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
        List<GameObject> checkpoint_list;
        List<GameObject> bulletList;
        List<GameObject> bulletListR;
        List<GameObject> bulletListD;
        List<GameObject> bulletListU;

        public ObjectContainer(ContentManager content)
        {
            this.content = content;
            this.list = new List<GameObject>();
            this.checkpoint_list = new List<GameObject>();
            this.bulletList = new List<GameObject>();
            this.bulletListR = new List<GameObject>();
            this.bulletListD = new List<GameObject>();
            this.bulletListU = new List<GameObject>();
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
            foreach (GameObject b in bulletList)
            {
                b.draw();
            }
            foreach (GameObject br in bulletListR)
            {
                br.draw();
            }
            foreach (GameObject bd in bulletListD)
            {
                bd.draw();
            }
            foreach (GameObject bu in bulletListU)
            {
                bu.draw();
            }
        }

        public int update(int cash)
        {
            GameObject toDelete = new ObjectNonSpecified();
            foreach (GameObject o in list)
            {
                o.update();
                if (o.type == ObjectType.ENEMY)
                {
                    ObjectEnemy oe = (ObjectEnemy)o;
                    if (oe.reachedCheckPoint())
                    {
                        GameObject check_if_destination = lastCheckPoint();
                        if (check_if_destination.positionOnMap.x == oe.positionOnMap.x && check_if_destination.positionOnMap.y == oe.positionOnMap.y)
                        {
                            toDelete = oe;
                            cash -= 50;
                        }
                        else
                        {
                            oe.checkPointsCounter++;
                            oe.nextCheckPoint = (PathCheckPoint)findCheckPoint(oe.checkPointsCounter);
                            oe.direction = oe.nextCheckPoint.directionToReach;
                        }
                    }
                    else if (oe.CheckCollision(oe.enemyBoundingSphere, bulletList))
                    {
                        toDelete = oe;
                        cash += 10;
                    }
                    else if (oe.CheckCollisionR(oe.enemyBoundingSphere, bulletListR))
                    {
                        toDelete = oe;
                        cash += 10;
                    }
                    else if (oe.CheckCollisionD(oe.enemyBoundingSphere, bulletListD))
                    {
                        toDelete = oe;
                        cash += 10;
                    }
                    else if (oe.CheckCollisionU(oe.enemyBoundingSphere, bulletListU))
                    {
                        toDelete = oe;
                        cash += 10;
                    }
                }
            }
            if(toDelete.type != ObjectType.NOT_SPECIFIED)
                list.Remove(toDelete);

            return cash;
        }

        public void addBullet(Position position, ObjectType towerType)
        {
            switch (towerType)
            {
                case ObjectType.TOWERSINGLE:
                    GameObject b = new ObjectBullet();
                    b.loadContent(content);
                    b.modelPosition = new Vector3(position.x * 1600, position.y * 1600, 4000);
                    bulletList.Add(b);
                    break;
                case ObjectType.TOWERSINGLE_R:
                    GameObject br = new ObjectBulletR();
                    br.loadContent(content);
                    br.modelPosition = new Vector3(position.x * 1600, position.y * 1600, 4000);
                    bulletListR.Add(br);
                    break;
                case ObjectType.TOWERSINGLE_U:
                    GameObject bu = new ObjectBulletU();
                    bu.loadContent(content);
                    bu.modelPosition = new Vector3(position.x * 1600, position.y * 1600, 4000);
                    bulletListU.Add(bu);
                    break;
                case ObjectType.TOWERSINGLE_D:
                    GameObject bd = new ObjectBulletD();
                    bd.loadContent(content);
                    bd.modelPosition = new Vector3(position.x * 1600, position.y * 1600, 4000);
                    bulletListD.Add(bd);
                    break;
            }
        }

        public GameObject addObject(ObjectType objectType, Vector3 pos, Position positionOnMap)
        {
            switch (objectType)
            {
                case ObjectType.ENEMY:
                    GameObject o = new ObjectEnemy(EnemyType.ST1);
                    o.loadContent(content);
                    o.modelPosition = new Vector3(pos.X, pos.Y, pos.Z);
                    o.positionOnMap = new Position(positionOnMap.x, positionOnMap.y);
                    list.Add(o);
                    return o;
                case ObjectType.CHECKPOINT:
                    GameObject c = new PathCheckPoint();
                    c.loadContent(content);
                    c.modelPosition = pos;
                    c.positionOnMap = positionOnMap;
                    checkpoint_list.Add(c);
                    return c;
            }
            return null;
        }

        public Position firstCheckPointPosition()
        {
            Position position;
            GameObject firstCheckPoint = new PathCheckPoint();

            foreach (GameObject o in checkpoint_list)
            {
                if (o.type == ObjectType.CHECKPOINT)
                {
                    firstCheckPoint = o;
                    break;
                }    
            }
            position = new Position(firstCheckPoint.positionOnMap.x, firstCheckPoint.positionOnMap.y);

            return position;
        }

        public GameObject firstCheckPoint()
        {
            foreach (GameObject o in checkpoint_list)
            {
                if (o.type == ObjectType.CHECKPOINT)
                {
                    return o;
                }
            }
            return null;
        }

        public GameObject findCheckPoint(int i)
        {
            for (int counter = 0; counter < checkpoint_list.Count; counter++)
            {
                if (checkpoint_list[counter].type == ObjectType.CHECKPOINT)
                {
                    if (counter == i)
                        return checkpoint_list[counter];
                }
            }
            return null;
        }

        public GameObject lastCheckPoint()
        {
            GameObject go = new ObjectNonSpecified();
            foreach (GameObject o in checkpoint_list)
            {
                if (o.type == ObjectType.CHECKPOINT)
                    go = o;
            }
            return go;
        }
    }
}
