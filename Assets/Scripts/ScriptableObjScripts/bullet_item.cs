using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_item : Item
{


    /*
     * ��� ���� 
    */


    public enum bullet_type { rifleBullet, pistol, pellet }

    protected bullet_type type_of_bullet;



    /*
     * �������� ���� 
    */

    protected float bullet_speed;

    public float GetBulletSpeed => bullet_speed;



}
