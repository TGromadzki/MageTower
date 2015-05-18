using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDef
{
    public enum ObjectType
    {
        NOT_SPECIFIED, EMPTY, TOWER, TOWERSINGLE, TOWERSINGLE_R, TOWERSINGLE_U, TOWERSINGLE_D, ENEMY, BULLET, BULLET_R, BULLET_D, BULLET_U, PATH, CHECKPOINT, END
    }

    public enum EnemyType
    {
        ST1, ST2, ST3, GOKU
    }

    public enum BulletType
    {
        B1, B2, B3
    }

    public enum CollisionType
    {
        NONE, TARGET, BULLET
    }

}
