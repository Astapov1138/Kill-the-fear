using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;

public class AmmunitionManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    private RectTransform am_UI;



    private void Start()
    {

        am_UI = GameObject.Find("AmmunitionUI").GetComponent<RectTransform>();

        // �������� ��� ����� ��� �������������� ������
        for (int i = 0; i < am_UI.childCount; i++)
        {
            if (am_UI.GetChild(i).GetComponent<AmmunitionGunSlot>() != null)
            {
                am_gun_slots.Add(am_UI.GetChild(i).GetComponent<AmmunitionGunSlot>());
            }
        }

        GameObject.Find("Inventory").SetActive(false);

    }


    private bool SuccessGunAddition = false;

    private bool SuccessAdd = false;
    public void PutItem(Item item, GameObject gameObj)
    {
        

        foreach (AmmunitionGunSlot slot in am_gun_slots)
        {
            // �������� �� �������� � �� ������ 
            if (item.itemType == ItemType.gun)
                PutGunItem(item, slot, out SuccessGunAddition);

            
            if (SuccessGunAddition)
            {
                // ��������� ��������� ���������
                SuccessGunAddition = false;

                // ��������� ������ �� �����, ���� ���������� ������ ������� 
                Destroy(gameObj);

                Debug.Log("������ ���������");

                return;
            }
            

            // ���� ������� ��������� - ��������� ������� ������, ������ ��������� ��������
            if (SuccessAdd)
            {
                SuccessAdd = false;

                return;

            }
        }
    }

    private void PutGunItem(Item item, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {

                // ������ ���� �� ������
                slot.SlotIsEmpty = false;

                // ������� �������� � �����, ������� ����� ���������� ������
                Transform gunImageTransform = slot.transform.GetChild(1);



                // ������������ ������ ������������ ������
                GameObject child = Instantiate(item.ScriptableGameObject, slot.transform);




                // ������� ���������� ������ � ���� ��������
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // ������������ ���� ������ ��� child ������ ��������, ������� ��� ����������
                child.transform.SetParent(gunImageTransform);

                // ������������ �������� � ���� ���������
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // ���������� �������� ������ �������
                SuccessGunAddition = true;

            }
            else
            {
                // ��� �� ������, ���������� ������ �� �������
                SuccessGunAddition = false;
            }
        }
        else
        {
            // ���� �����. ���������� ������ �� �������
            SuccessGunAddition = false;
        }
    }


}
