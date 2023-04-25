using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    /*
     *  ���� ������ ???
    */


    protected bool IsEmpty = true;

    public bool SlotIsEmpty
    { 
        get { return IsEmpty; }
        set { IsEmpty = value; }
    }



    /*
     * ������� � �����  
    */


    protected Item item;

    public Item item_in_slot
    {
        get { return item; }
        set { item = value; }
    }



    /*
     * ������ � ����� 
    */



    protected GameObject internal_object;

    public GameObject object_in_slot
    {
        get { return internal_object; }
        set { internal_object = value; }
    }




    /*
     * ��������� ������� �����
    */


    private Vector3 defaultPosition;

    public Vector3 SlotDefaultPosition => defaultPosition;






    /*
     * 
     * ������ 
     * �����  
     * 
    */






    public void SetItem(Item item, GameObject gunObj)
    {
        this.item = item;

        internal_object = gunObj;

        IsEmpty = false;
    }

    public void ClearClot()
    {
        item = null;

        internal_object = null;

        IsEmpty = true;
    }







    private void Start()
    {
        /*
        try
        {
            defaultPosition = transform.GetChild(2).transform.position;
            Debug.Log($"�������� ��������� ������� � �������� 2, ���������� ����� = {defaultPosition}");
        }
        catch (UnityException)
        {
            defaultPosition = transform.GetChild(1).transform.position;
            Debug.Log($"�������� ��������� ������� � �������� 1, ���������� ����� = {defaultPosition}");
        }

        */





        if (transform.gameObject.tag == "ItemSlot")
        {

            defaultPosition = transform.localPosition;
            Debug.Log($"�������� ��������� ������� � �������� 1, ���������� ����� = {defaultPosition}");
        }
        else if (transform.gameObject.tag == "GunSlot")
        {
            defaultPosition = transform.GetChild(2).position;
            Debug.Log($"�������� ��������� ������� � �������� 2, ���������� ����� = {defaultPosition}");
        }


    }
}
