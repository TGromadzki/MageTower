using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace TowerDef
{
    class Map
    {
        ObjectContainer objectContainer;		
        GameObject[,] map;
        public int width;
        public int height;

        public GameObject selectedObject;
        ContentManager content;

        public Map(int width, int height, ContentManager content, ObjectContainer objectContainer)
        {
            this.width = width;
            this.height = height;
            this.content = content;
            this.objectContainer = objectContainer;
        }

        // Load map from file
        private Dictionary<int, List<Char>> loadMapFromFile()
        {
            StreamReader r = File.OpenText(content.RootDirectory + "//map.txt");
            Dictionary<int, List<char>> table = new Dictionary<int, List<char>>();
            int i = 0, j = 0;
            string line = null;

            List<char> list = new List<char>();
            while ((line = r.ReadLine()) != null)
            {
                foreach (Char c in line)
                {
                    list.Add(c);
                }
                i++;
            }
            table.Add(i, list);

            r.Close();
            return table;
        }

        public void loadContent()
        {
            Dictionary<int, List<char>> list = loadMapFromFile();

            height = list.Keys.First();
            width = list[list.Keys.First()].Count / height;

            map = new GameObject[height, width];

            List<char> line = list.First().Value;

            int charCounter = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    switch (line[charCounter])
                    {
                        case 't': //empty field
                            GameObject t = new ObjectEmptyField();
                            t.loadContent(content);
                            t.modelPosition = new Vector3(i * GlobalGameConstants.mapGroundPlatesDistance, j * GlobalGameConstants.mapGroundPlatesDistance, 0);
                            t.positionOnMap = new Position(i, j);
                            map[i, j] = t;
                            break;
                        case 'x': //path
                            GameObject x = new ObjectPath();
                            x.loadContent(content);
                            x.modelPosition = new Vector3(i * GlobalGameConstants.mapGroundPlatesDistance, j * GlobalGameConstants.mapGroundPlatesDistance, -800);
                            x.positionOnMap = new Position(i, j);
                            map[i, j] = x;
                            break;
                        case '-': //not a map
                            GameObject o = new ObjectNonSpecified();
                            o.loadContent(content);
                            o.modelPosition = new Vector3(i * GlobalGameConstants.mapGroundPlatesDistance, j * GlobalGameConstants.mapGroundPlatesDistance, 0);
                            o.positionOnMap = new Position(i, j);
                            map[i, j] = o;
                            break;
                        case 's': //first selected object
                            GameObject s = new ObjectPath();
                            s.loadContent(content);
                            s.modelPosition = new Vector3(i * GlobalGameConstants.mapGroundPlatesDistance, j * GlobalGameConstants.mapGroundPlatesDistance, -800);
                            s.positionOnMap = new Position(i, j);
                            map[i, j] = s;
                            selectedObject = map[i, j];
                            objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(s.modelPosition.X, s.modelPosition.Y, s.modelPosition.Z), new Position(s.positionOnMap.x, s.positionOnMap.y));
                            break;
                        case 'e': //PathEnd
                            GameObject e = new ObjectEnd();
                            e.loadContent(content);
                            e.modelPosition = new Vector3(i * GlobalGameConstants.mapGroundPlatesDistance, j * GlobalGameConstants.mapGroundPlatesDistance, 0);
                            e.positionOnMap = new Position(i, j);
                            map[i, j] = e;
                            break;
                    }
                    charCounter++;
                }
            }

            checkPath();
        }

        // Put a tower
        public void putFieldType(Position position, ObjectType fieldType)
        {
            if (checkIfEmpty(position))
            {
                switch (fieldType)
                {
                    case ObjectType.TOWERSINGLE:
                        GameObject ts = new ObjectTowerSingle();
                        ts.loadContent(content);
                        ts.modelPosition = selectedObject.modelPosition;
                        ts.modelPosition = new Vector3(ts.modelPosition.X, ts.modelPosition.Y, 1300.0f);
                        ts.positionOnMap = position;
                        map[position.x, position.y] = ts;
                        break;
                    case ObjectType.TOWERSINGLE_R:
                        GameObject tsr = new ObjectTowerSingle();
                        tsr.loadContent(content);
                        tsr.modelPosition = selectedObject.modelPosition;
                        tsr.modelPosition = new Vector3(tsr.modelPosition.X, tsr.modelPosition.Y, 1300.0f);
                        tsr.positionOnMap = position;
                        map[position.x, position.y] = tsr;
                        break;
                    case ObjectType.TOWERSINGLE_U:
                        GameObject tsu = new ObjectTowerSingle();
                        tsu.loadContent(content);
                        tsu.modelPosition = selectedObject.modelPosition;
                        tsu.modelPosition = new Vector3(tsu.modelPosition.X, tsu.modelPosition.Y, 1300.0f);
                        tsu.positionOnMap = position;
                        map[position.x, position.y] = tsu;
                        break;
                    case ObjectType.TOWERSINGLE_D:
                        GameObject tsd = new ObjectTowerSingle();
                        tsd.loadContent(content);
                        tsd.modelPosition = selectedObject.modelPosition;
                        tsd.modelPosition = new Vector3(tsd.modelPosition.X, tsd.modelPosition.Y, 1300.0f);
                        tsd.positionOnMap = position;
                        map[position.x, position.y] = tsd;
                        break;
                }
            }

        }

        private Boolean checkIfEmpty(Position position)
        {
            if (map[position.x, position.y].type == ObjectType.EMPTY)
            {
                return true;
            }
            return false;
        }

        public void draw(float aspectRatio)
        {
            foreach (GameObject f in map)
            {
                f.draw();
            }
        }

        public void update(Position selectedItem)
        {
            selectedObject.isSelected = false;

            map[selectedItem.x, selectedItem.y].isSelected = true;
            selectedObject = map[selectedItem.x, selectedItem.y];

            foreach (GameObject g in map)
                g.update();
        }

        // Method to check if player can move "cursor" to desired field
        public bool isMovePossibe(int x, int y)
        {
            try
            {
                if (map[x, y].type == ObjectType.NOT_SPECIFIED)
                    return false;
                GameObject o = map[x, y];
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private ObjectType FieldType(int x, int y)
        {
            return map[x, y].type;
        }

        // Method to check enemy's possible movement path
        private void checkDirection(int x, int y, Direction dir, ref Direction lastDirection, ref Position lastPositionChecked, ref bool isOutOfMap)
        {
            switch (dir)
            {
                case Direction.DOWN:
                    if (y - 1 >= 0)
                    {
                        if (FieldType(x, y - 1) == ObjectType.PATH && FieldType(x + 1, y) == ObjectType.PATH)
                        {
                            lastDirection = Direction.DOWN;
                            lastPositionChecked = new Position(x, y - 1);
                            isOutOfMap = false;
                            PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                            pcp.directionToReach = lastDirection;
                        }
                        else if (FieldType(x, y - 1) == ObjectType.PATH)
                            checkDirection(x, y - 1, dir, ref lastDirection, ref lastPositionChecked, ref isOutOfMap);
                        else
                        {
                            lastDirection = Direction.DOWN;
                            lastPositionChecked = new Position(x, y - 1);
                            isOutOfMap = false;
                            PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x,y));
                            pcp.directionToReach = lastDirection;
                        }
                    }
                    else
                    {
                        lastDirection = Direction.DOWN;
                        lastPositionChecked = new Position(x, y);
                        isOutOfMap = true;
                        PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                        pcp.directionToReach = lastDirection;
                    }
                    break;
                case Direction.LEFT:
                    if (x - 1 >= 0)
                    {
                        if (FieldType(x - 1, y) == ObjectType.PATH)
                            checkDirection(x - 1, y, dir, ref lastDirection, ref lastPositionChecked, ref isOutOfMap);
                        else
                        {
                            lastDirection = Direction.LEFT;
                            lastPositionChecked = new Position(x - 1, y);
                            isOutOfMap = false;
                            PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                            pcp.directionToReach = lastDirection;
                        }
                    }
                    else
                    {
                        lastDirection = Direction.LEFT;
                        lastPositionChecked = new Position(x, y);
                        isOutOfMap = true;
                        PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                        pcp.directionToReach = lastDirection;
                    }
                    break;
                case Direction.RIGHT:
                    if (x + 1 < height)
                    {
                        if (FieldType(x + 1, y) == ObjectType.PATH)
                            checkDirection(x + 1, y, dir, ref lastDirection, ref lastPositionChecked, ref isOutOfMap);
                        else
                        {
                            lastDirection = Direction.RIGHT;
                            lastPositionChecked = new Position(x + 1, y);
                            isOutOfMap = false;
                            PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                            pcp.directionToReach = lastDirection;
                        }
                    }
                    else
                    {
                        lastDirection = Direction.RIGHT;
                        lastPositionChecked = new Position(x, y);
                        isOutOfMap = true;
                        PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                        pcp.directionToReach = lastDirection;
                    }
                    break;
                case Direction.UP:
                    if (y + 1 < width)
                    {
                        if (FieldType(x, y + 1) == ObjectType.PATH && FieldType(x + 1, y) == ObjectType.PATH)
                        {
                            lastDirection = Direction.UP;
                            lastPositionChecked = new Position(x, y + 1);
                            isOutOfMap = false;
                            PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                            pcp.directionToReach = lastDirection;
                        }
                        else if (FieldType(x, y + 1) == ObjectType.PATH)
                            checkDirection(x, y + 1, dir, ref lastDirection, ref lastPositionChecked, ref isOutOfMap);
                        else
                        {
                            lastDirection = Direction.UP;
                            lastPositionChecked = new Position(x, y + 1);
                            isOutOfMap = false;
                            PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                            pcp.directionToReach = lastDirection;
                        }
                    }
                    else
                    {
                        lastDirection = Direction.UP;
                        lastPositionChecked = new Position(x, y);
                        isOutOfMap = true;
                        PathCheckPoint pcp = (PathCheckPoint)objectContainer.addObject(ObjectType.CHECKPOINT, new Vector3(x + GlobalGameConstants.mapGroundPlatesDistance, y * GlobalGameConstants.mapGroundPlatesDistance, -800), new Position(x, y));
                        pcp.directionToReach = lastDirection;
                    }
                    break;
            }
        }

        // Method to switch enemy's movement direction
        private void switchDirection(ref Direction dir, int x, int y, Direction lastDirection)
        {
            int choosed_value;
            switch (lastDirection)
            {
                case Direction.NONE:
                    // all directions
                    if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(4);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                        else if (choosed_value == 3)
                            dir = Direction.UP;
                        else if (choosed_value == 4)
                            dir = Direction.DOWN;
                    }
                    // without DOWN
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                        else if (choosed_value == 3)
                            dir = Direction.UP;
                    }
                    // without UP
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                        else if (choosed_value == 3)
                            dir = Direction.DOWN;
                    }
                    // without LEFT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                        else if (choosed_value == 3)
                            dir = Direction.DOWN;
                    }
                    // without RIGHT
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                        else if (choosed_value == 3)
                            dir = Direction.DOWN;
                    }
                    // RIGHT & LEFT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                    }
                    // RIGHT & DOWN
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.DOWN;
                    }
                    // RIGHT & UP
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }
                    // LEFT & DOWN
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.DOWN;
                    }
                    // LEFT & UP
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }
                    // DOWN & UP
                    else if (y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.DOWN;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }
                    // RIGHT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH)
                        dir = Direction.RIGHT;
                    // LEFT
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH)
                        dir = Direction.LEFT;
                    // UP
                    else if (y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                        dir = Direction.UP;
                    // DOWN
                    else if (y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                        dir = Direction.DOWN;
                    break;

                case Direction.DOWN:
                    // all possible directions (without UP)
                    /*if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                        else if (choosed_value == 3)
                            dir = Direction.DOWN;
                    }
                    // RIGHT & LEFT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                    }*/
                    // RIGHT & DOWN
                    if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        /*choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.DOWN;*/
                        dir = Direction.RIGHT;
                    }
                    // LEFT & DOWN
                    /*else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.DOWN;
                    }*/
                    // RIGHT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH)
                        dir = Direction.RIGHT;
                    // LEFT
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH)
                        dir = Direction.LEFT;
                    // DOWN
                    else if (y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                        dir = Direction.DOWN;
                    break;

                case Direction.LEFT:
                    // all possible directions (without RIGHT)
                    if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                        else if (choosed_value == 3)
                            dir = Direction.DOWN;
                    }
                    // LEFT & DOWN
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.DOWN;
                    }
                    // LEFT & UP
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }
                    // DOWN & UP
                    else if (y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.DOWN;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }
                    // LEFT
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH)
                        dir = Direction.LEFT;
                    // UP
                    else if (y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                        dir = Direction.UP;
                    // DOWN
                    else if (y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                        dir = Direction.DOWN;
                    break;

                case Direction.RIGHT:
                    // all possible directions (without LEFT)
                    if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                        else if (choosed_value == 3)
                            dir = Direction.DOWN;
                    }
                    // RIGHT & DOWN
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.DOWN;
                    }
                    // RIGHT & UP
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }
                    // DOWN & UP
                    else if (y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.DOWN;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }
                    // RIGHT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH)
                        dir = Direction.RIGHT;
                    // UP
                    else if (y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                        dir = Direction.UP;
                    // DOWN
                    else if (y - 1 >= 0 && FieldType(x, y - 1) == ObjectType.PATH)
                        dir = Direction.DOWN;
                    break;

                case Direction.UP:
                    // all possible directions (without DOWN)
                    /*if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(3);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                        else if (choosed_value == 3)
                            dir = Direction.UP;
                    }                    
                    // RIGHT & LEFT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.LEFT;
                    }*/
                    // RIGHT & UP
                    if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        /*choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.RIGHT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;*/
                        dir = Direction.RIGHT;
                    }
                    // LEFT & UP
                    /*else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH && y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                    {
                        choosed_value = ERABE(2);
                        if (choosed_value == 1)
                            dir = Direction.LEFT;
                        else if (choosed_value == 2)
                            dir = Direction.UP;
                    }*/
                    // RIGHT
                    else if (x + 1 < height && FieldType(x + 1, y) == ObjectType.PATH)
                        dir = Direction.RIGHT;
                    // LEFT
                    else if (x - 1 >= 0 && FieldType(x - 1, y) == ObjectType.PATH)
                        dir = Direction.LEFT;
                    // UP
                    else if (y + 1 < width && FieldType(x, y + 1) == ObjectType.PATH)
                        dir = Direction.UP;
                    break;
            }
        }

        public int ERABE(int x)
        {
            Random random = new Random();
            int random_value = random.Next(1, x + 1);
            return (random_value);
        }

        //Method to check enemy's current path
        public void checkPath()
        {
            Direction dir = Direction.NONE, lastDirection = Direction.NONE;
            int x = objectContainer.firstCheckPointPosition().x;
            int y = objectContainer.firstCheckPointPosition().y;
            Position lastPositionChecked = new Position(x, y);
            bool isOutOfMap = false;

            switchDirection(ref dir, x, y, lastDirection);
            lastDirection = dir;


            while (map[lastPositionChecked.x, lastPositionChecked.y].type != ObjectType.END)
            {
                checkDirection(x, y, dir, ref lastDirection, ref lastPositionChecked, ref isOutOfMap);

                if (isOutOfMap)
                {
                    x = lastPositionChecked.x;
                    y = lastPositionChecked.y;
                }
                else
                {
                    switch (lastDirection)
                    {
                        case Direction.DOWN:
                            x = lastPositionChecked.x;
                            y = lastPositionChecked.y + 1;
                            break;
                        case Direction.LEFT:
                            x = lastPositionChecked.x + 1;
                            y = lastPositionChecked.y;
                            break;
                        case Direction.RIGHT:
                            x = lastPositionChecked.x - 1;
                            y = lastPositionChecked.y;
                            break;
                        case Direction.UP:
                            x = lastPositionChecked.x;
                            y = lastPositionChecked.y - 1;
                            break;
                    }
                }
                switchDirection(ref dir, x, y, lastDirection);
            }
        }

        /*public bool reachedEnd(int x, int y)
        {
            if (map[x, y].type == ObjectType.END)
                return true;
            return false;
        }*/
    }
}
