using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;
using static UnityEditor.Progress;


public class InventoryManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    private RectTransform am_UI;

    private RectTransform item_UI;






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

        item_UI = GameObject.Find("ItemsUI").GetComponent<RectTransform>();


        // �������� ��� ��������� �����
        for (int i = 0; i < item_UI.childCount; i++)
        {
            if (item_UI.GetChild(i).GetComponent<ItemSlot>() != null)
            {
                itemSlots.Add(item_UI.GetChild(i).GetComponent<ItemSlot>());
            }
        }
        

        // ����� ���������� 
        GameObject.Find("Inventory").SetActive(false);

    }









    public void PutItem(Item item, GameObject itemObj)
    {
        bool SuccessGunAddition = false;



        /*
         * ���� ������������ ������ �������� ������� 
        */



        if (item.itemType == ItemType.gun)
        {
            foreach (AmmunitionGunSlot slot in am_gun_slots)
            {
                
                PutGunItem(item, itemObj, slot, out SuccessGunAddition);


                if (SuccessGunAddition)
                {
                    // ��������� ��������� ���������
                    SuccessGunAddition = false;

                    return;
                }

            }
        }


        /*
         * ���� ������������ ������� �� �������� ������� ��� ������ 
        */

        if (item.itemType != ItemType.gun && item.itemType != ItemType.secondaty_arms && item.itemType != ItemType.armor)
        {
            foreach (ItemSlot slot in itemSlots)
            {
                bool success = false;

                PutDefaultItem(item, itemObj, slot, out success);


                if (success)
                {
                    // ��������� ��������� ���������
                    success = false;

                    return;
                }
            }
        }


    }








    private void GrabItem(Item item, GameObject TransmittedObject, Slot slot, out bool success)
    {
        
        success = false;


        if (item != null && TransmittedObject != null && slot != null)
        {
            
            // ������� �������� ��������
            Transform item_image_transform = slot.transform.GetChild(1);



            /*
             *  ������������ ����������� �������
            */


            // ������������ ������ ��� child object �������� ��������
            TransmittedObject.transform.SetParent(item_image_transform);

            // ������ ������ ������� �� ����� 
            TransmittedObject.GetComponent<SpriteRenderer>().sprite = null;



            /*
             *  ������������ ����������� ��������, �������� ������� � ����
            */

            item_image_transform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            item_image_transform.GetComponent<Image>().enabled = true;



            /*
             * ���������� ������ �����
            */


            // ������� � ���� ������� � �������������� ��� ������ 
            slot.SetItem(item, TransmittedObject);



            /*
             * �������� ���������� �������� ������ � ���� 
             */


            // ���������� �������� ������ �������
            success = true;
        }
        else 
        {
            success = false;        
        }

    }










    private void PutDefaultItem(Item item, GameObject TransmittedObject, ItemSlot slot, out bool success)
    { 
        success = false;

        if (slot.SlotIsEmpty)
        {
            // ����� ����������� ������� � ���������
            GrabItem(item, TransmittedObject, slot, out success);
        }
        else
        {
            // ���� ��� ����� ������ ���������, ���������� ������ ���������
            success = false;
        }
    }









    private void PutGunItem(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {
                // ����� ����������� ������� � ���������
                GrabItem(item, TransmittedObject, slot, out SuccessGunAddition);

            }
            else
            {
                // ������ ������� �� �������� �������, ������� ��� ������ �������� � ���� ����
                SuccessGunAddition = false;
            }
        }
        else
        {
            // ���� �����. ���������� ������ ���������
            SuccessGunAddition = false;
        }
    }










    private void SetItemToEmptySlot(Item item, GameObject TransmittedObject, Slot slot, out bool ItemAdded)
    {
        ItemAdded = false;

        if (item != null && TransmittedObject != null && slot != null)
        {


            // ������� Transform ��������, � ������� ���� �������� �������
            Transform InputImageTransform = slot.transform.GetChild(1);

            // ������� Transform ��������, ������� �������� ������������� ��������
            Transform currentImageTransform = TransmittedObject.transform.parent;



            /*
             * ������� � ������������ ������, ������� ���������� ������������ ������� 
            */



            // ������������ ������������ ������ �� �������� �����, � ������� �� ������� (��� child ������)
            TransmittedObject.transform.SetParent(InputImageTransform);



            /*
             * ������������ ����������� � ����, � ������� �������� � ������� �
            */


            InputImageTransform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            InputImageTransform.GetComponent<Image>().enabled = true;



            /*
             * ���������� ������ �����, � ������� �������� 
            */



            // ������� � ���� ������� � �������������� ��� ������
            slot.SetItem(item, TransmittedObject);



            /*
             * ���������� ������ �����, �� �������� ���������� ������� 
            */



            // ������ ���� 
            currentImageTransform.parent.gameObject.GetComponent<Slot>().ClearClot();



            /*
             * ���������� �������� �����, �� ������� ���������� �������  
            */



            // ������ ����������� ��������, ������� ����������
            currentImageTransform.GetComponent<Image>().sprite = null;

            // ��������� �������� ��������� �� �����
            currentImageTransform.position = currentImageTransform.parent.gameObject.GetComponent<Slot>().SlotDefaultPosition;

            // ����� �� ����������
            currentImageTransform.GetComponent<Image>().enabled = false;



            /*
             * �������� �������� ��������
            */



            // ���������� �������� ������ �������
            ItemAdded = true;


        }
        else 
        {
            // ��������� �������� ������ �� �������
            ItemAdded = false;
        }

        
    }









    private void SetItemWithReplace(Item item, GameObject TransmittedObject, Slot slot, out bool ItemAdded)
    {
        

        /*
         * ������� ���� � ��������, � ������� ���� �������� ������� 
        */



        // ������� Transform ��������, � ������� ���� �������� �������
        Transform InputImageTransform = slot.transform.GetChild(1);

        // ������� ���� ������� �������� 
        Slot inputSlot = InputImageTransform.parent.GetComponent<Slot>();



        /*
         * ������� ������� ���� � ��������  
        */



        // ������� Transform ��������, ������� �������� ������������� ��������
        Transform currentImageTransform = TransmittedObject.transform.parent;

        // ������� ���� �������� ��������
        Slot currentSlot = currentImageTransform.parent.GetComponent<Slot>();



        /*
         * ����� ������� �������� (� ��������)
        */



        InputImageTransform.transform.SetParent(currentSlot.transform);

        currentImageTransform.transform.SetParent(inputSlot.transform);



        /*
         * ����� ������� �������� (���������)
        */



        // ��������� �������� �������� �� �����
        currentImageTransform.position = currentImageTransform.parent.gameObject.GetComponent<Slot>().SlotDefaultPosition;

        InputImageTransform.position = InputImageTransform.parent.gameObject.GetComponent<Slot>().SlotDefaultPosition;



        /*
         * ���������� ������� ������, � ������� �������� 
        */


        // ���� 1


        // ������� ������, ������� ������ ��������� � CurrentSlot
        GameObject gm_current = currentImageTransform.GetChild(0).transform.gameObject;

        // ������� �������, ������� ������ ��������� � CurrentSlot
        Item item_current = gm_current.GetComponent<FloorItem>().getItem;

        // ������������ �� � ����� ����
        currentSlot.SetItem(item_current, gm_current);


        // ���� 2


        // ������� ������, ������� ������ ��������� � InputSlot
        GameObject gm_input = InputImageTransform.GetChild(0).transform.gameObject;

        // ������� �������, ������� ������ ��������� � CurrentSlot
        Item item_input = gm_input.GetComponent<FloorItem>().getItem;

        // ������������ �� � ����� ���� 
        inputSlot.SetItem(item_input, gm_input);



        /*
         * �������� �������� ��������
        */



        // ������ �������� ������ �������
        ItemAdded = true;


    }










    public void PutItemToSlot(Item item, GameObject TransmittedObject, ItemSlot slot, out bool ItemAdded)
    {
        ItemAdded = false;


        if (item.itemType == ItemType.gun || item.itemType == ItemType.armor || item.itemType == ItemType.secondaty_arms)
        {
            // ������� �� �������� �������, ���������� ������ �� ������� 
            ItemAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {
                // ������������ ������� � ������ ����
                SetItemToEmptySlot(item, TransmittedObject, slot, out ItemAdded);

            }
            else
            {
                // ������������ ������� � ������� ������ ��������� ����
                SetItemWithReplace(item, TransmittedObject, slot, out ItemAdded);
            }
        }


    }









    //          �������, ������� ��������,         ���� �������,          �������� ���������    
    public void PutWeaponToSlot(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, out bool GunIsAdded)
    {


        GunIsAdded = false;


        if (item.itemType != ItemType.gun)
        {
            // ������� �� �������� �������, ���������� ������ �� ������� 
            GunIsAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {

                // ������������ ������ � ������ ����
                SetItemToEmptySlot(item, TransmittedObject, slot, out GunIsAdded);

            }
            else
            {
                // ������������ ������ � ������� ������ ��������� ����
                SetItemWithReplace(item, TransmittedObject, slot, out GunIsAdded);

            }
        }


    }









    public void DropItemFromInventory(Item item, GameObject currentObject, Slot slot, out bool SuccessDrop)
    {
        SuccessDrop = false;

        if (item != null && currentObject != null)
        {



            /*
             * ���������� ���� �������� 
            */



            slot.ClearClot();





            /*
             * ������ �������� �������� � ���������
            */





            // ������� ��������
            Image image_component = currentObject.transform.parent.gameObject.GetComponent<Image>();

            // ������ ��������
            image_component.sprite = null;

            // ����� �������� ����������
            image_component.enabled = false;





            /*
             *  ������ �������� �� ��������� �����
            */

            // ������� ������ ��������
            GameObject image_obj = currentObject.transform.parent.gameObject;

            // ������ �� ��������� ����� 
            image_obj.transform.position = slot.SlotDefaultPosition;





            /*
             * ����������� ������� �� ��������� 
            */





            // �������� ��� �� ������� �������� 
            currentObject.transform.SetParent(null);

            // ������������ ������������ ������� ������� ������
            currentObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;





            /*
             * ������������ ������ �������� 
            */





            // ������� ������ ��������
            Sprite item_floor_image = item.GetFloorIcon;

            // ������������ ������
            currentObject.GetComponent<SpriteRenderer>().sprite = item_floor_image;





            /*
             * �������� ����������
            */




            SuccessDrop = true;





        }
    }


}
