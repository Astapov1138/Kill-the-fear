using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;
using static UnityEditor.Progress;

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
                PutGunItem(item, gameObj, slot, out SuccessGunAddition);

            
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

    private void PutGunItem(Item item, GameObject gameObj, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {

                // ������� �������� � �����, ������� ����� ���������� ������
                Transform gunImageTransform = slot.transform.GetChild(1);


                // ������������ ������ ������������ ������
                GameObject child = Instantiate(gameObj, slot.transform);


                // ������� ���������� ������ � ���� ��������
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // ��������� ������ ������ � ���� ��������
                slot.GunObj = child;

                // ������������ ���� ������ ��� child ������ ��������, ������� ��� ����������
                child.transform.SetParent(gunImageTransform);

                // ������������ �������� � ���� ���������
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // ������ ���� �� ������
                slot.SlotIsEmpty = false;

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


    //                         ��� �������,     ���� �������,         �������� ���������    
    public void PutWeaponToSlot(GameObject gun, AmmunitionGunSlot slot, out bool GunIsAdded)
    { 
        GunIsAdded = false;

        if (gun.GetComponent<FloorItem>().getItem.itemType != ItemType.gun)
        {
            GunIsAdded = false;
        }
        else 
        {
            if (slot.SlotIsEmpty)
            {


                // ������� �������� � �����, ������� ����� ���������� ������
                Transform gunImageTransform = slot.transform.GetChild(1);

                // ������������ ������ ������������� � ���� ������
                GameObject child = Instantiate(gun, slot.transform);

                // ������� ���������� ������ � ���� ��������
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // ��������� ������ ������ � ���� ��������
                slot.GunObj = child;

                // ������������ ���� ������ ��� child ������ ��������, ������� ��� ����������
                child.transform.SetParent(gunImageTransform);

                // ������������ �������� � ���� ���������
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // ������ ���� �� ������
                slot.SlotIsEmpty = false;





                
                // ������� Transform �������� ������, ������� ��������� � ����
                Transform currentGunImage = gun.transform.parent;

                //������� ���� ������������� ������
                AmmunitionGunSlot currentSlot = currentGunImage.parent.GetComponent<AmmunitionGunSlot>();

                // ������������ �������� �� �������� ���������
                currentGunImage.position = currentSlot.SlotDefaultPosition;
                
                // ������ �������� � ������� 
                currentGunImage.GetComponent<Image>().sprite = null;

                // ����� � ����������
                currentGunImage.GetComponent<Image>().enabled = false;

                //��������� ������������ ������
                Destroy(gun);

                // ������ ���� ������
                currentSlot.SlotIsEmpty = true;

                // ���� �� �������� � ���� �����-���� ������ 
                currentSlot.GunObj = null;

                // ���� �� �������� ScriptableObject
                currentSlot.GunInSlot = null;



                // ���������� �������� ������ �������
                GunIsAdded = true;

            }
            else
            {
                // ������� �������� � �����, ������� ����� ���������� ������
                Transform gunImageTransform = slot.transform.GetChild(1);

                // ������� Transform �������� ������, ������� ��������� � ����
                Transform currentGunImage = gun.transform.parent;

                // �������� �������� ������ ��������
                GameObject InternalObject = Instantiate(gunImageTransform.GetChild(0).gameObject, currentGunImage);

                // ��������� �������� ������ ��������
                Destroy(gunImageTransform.GetChild(0).gameObject);




                // ������������ ������ ������������� � ���� ������
                GameObject child = Instantiate(gun, slot.transform);

                // ��������� �������� ������, ���� �������� � �������
                Destroy(gun);

                // ������� ���������� ������ � ���� ��������
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // ��������� ������ ������ � ���� ��������
                slot.GunObj = child;

                // ������������ ���� ������ ��� child ������ ��������, ������� ��� ����������
                child.transform.SetParent(gunImageTransform);

                // ������������ �������� � ���� ���������
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // ������ ���� �� ������
                slot.SlotIsEmpty = false;







                // ������� ScriptableObject �������
                Item InternalItem = InternalObject.GetComponent<FloorItem>().getItem;

                //������� ���� ������������� ������
                AmmunitionGunSlot currentSlot = currentGunImage.parent.GetComponent<AmmunitionGunSlot>();



                // ������������ �������� �� �������� ���������
                currentGunImage.position = currentSlot.SlotDefaultPosition;

                // ������ �������� ������, ������� ��������� � �����
                currentGunImage.GetComponent<Image>().sprite = InternalItem.GetInventoryIcon;

                // ������������ ScriptableObject � ����
                currentSlot.GunInSlot = InternalItem;

                // ������������ ������ ������ � ����
                currentSlot.GunObj = InternalObject;

                // ���� �� ������
                currentSlot.SlotIsEmpty = false;



                // ������ �������� ������ �������
                GunIsAdded = true;

            }
        }


    }


}
