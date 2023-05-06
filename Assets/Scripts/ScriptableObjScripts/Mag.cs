using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : Item
{
    public enum MagType { RifleMag, PistolMag }


    /*
     * ������� �������� 
    */

    protected int capacity;

    public int GetCapacity => capacity;


    /*
     * ��� �������� 
    */

    protected MagType magType;

    public MagType GetMagType => magType;


}
