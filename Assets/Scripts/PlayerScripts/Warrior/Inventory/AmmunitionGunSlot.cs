using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static Gun;

public class AmmunitionGunSlot : MonoBehaviour
{
    private bool IsEmpty = true;

    public bool SlotIsEmpty
    { 
        get { return IsEmpty; }
        set { IsEmpty = value; }
    }

    private GameObject child;

    private Item gun;

    public Item GunInSlot;


    

    /*
    public void AddItem(Item item, out bool SuccessAddition)
    {
        if (IsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {
                // ������� ���������� ������ � ���� ��������
                gun = item;

                // ���������� �������� ������ �������
                SuccessAddition = true;

                // ������ ���� �� ������
                IsEmpty = false;

                // ������� �������� � �����, ������� ����� ���������� ������
                Transform gunImageTransform = transform.GetChild(1);


                // ������������ ������ ������������ ������
                child = Instantiate(gun.ScriptableGameObject, transform);

                // ������������ ���� ������ ��� child ������ ��������, ������� ��� ����������
                child.transform.SetParent(gunImageTransform);

                // ������������ �������� � ���� ���������
                SetImage(gunImageTransform);

            }
            else
            {
                // ��� �� ������, ���������� ������ �� �������
                SuccessAddition = false;
            }
        }
        else
        {
            // ���� �����. ���������� ������ �� �������
            SuccessAddition = false;
        }
    }


    public void SetImage(Transform imageObject)
    {
        imageObject.GetComponent<Image>().sprite = gun.GetInventoryIcon;

        imageObject.GetComponent<Image>().enabled = true;
    }

    */
}
