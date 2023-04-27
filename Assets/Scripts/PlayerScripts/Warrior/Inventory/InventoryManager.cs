using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;


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









    public void MainInventoryManager(Item item, GameObject itemObj)
    {


        bool SuccessGunAddition = false;



        /*
         * ���� ������������ ������ �������� ������� 
        */


        if (item.itemType == ItemType.gun)
        {
            foreach (AmmunitionGunSlot slot in am_gun_slots)
            {
                
                GrabGunItem(item, itemObj, slot, out SuccessGunAddition);


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

                GrabDefaultItem(item, itemObj, slot, out success);


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










    private void GrabDefaultItem(Item item, GameObject TransmittedObject, ItemSlot slot, out bool success)
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









    private void GrabGunItem(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, out bool SuccessGunAddition)
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
        GameObject gm_current = currentImageTransform.GetChild(0).gameObject;

        // ������� �������, ������� ������ ��������� � CurrentSlot
        Item item_current = gm_current.GetComponent<FloorItem>().getItem;

        // ������������ �� � ����� ����
        currentSlot.SetItem(item_current, gm_current);


        // ���� 2


        // ������� ������, ������� ������ ��������� � InputSlot
        GameObject gm_input = InputImageTransform.GetChild(0).gameObject;

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
            // ���������� ������ �� ������� 
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
            
            // ���������� ����, � ������� ����� �������
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









    public void SetMagToGun(Slot slot, GameObject transmitted_picture, out bool successLoad)
    {

        successLoad = false;

        // ������� ������ � �����
        GameObject gun_in_slot = slot.object_in_slot;

        // ������� ������� � ��������
        GameObject mag_in_pic = transmitted_picture.transform.GetChild(0).gameObject;

        // ������� ���� ��������
        GameObject image_slot = transmitted_picture.transform.parent.gameObject;




        /*
         * �������� �� ��, ���� �� ������� � �������
        */

        if (gun_in_slot.transform.childCount > 0)
        {

            /*
             * ���� ������ �� �������
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {

                // ������������ ������� � ���������� ������
                gun_in_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded)
                {
                    // ������� ������� � �������
                    GameObject mag_in_gun = gun_in_slot.transform.GetChild(0).gameObject;

                    // ������ ������� �� ������ � �������� ���� ���������
                    mag_in_gun.transform.SetParent(transmitted_picture.transform);

                    // ������ ������� �� ��������� ��� �������� ������ ������
                    mag_in_pic.transform.SetParent(gun_in_slot.transform);

                    // ������������ �������� � ���������
                    SetMagInInventoryWithReplace(transmitted_picture, image_slot, mag_in_pic, gun_in_slot, mag_in_gun);

                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // ������������ ������� � ���������� ������
                gun_in_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded)
                {
                    // ������� ������� � �������
                    GameObject mag_in_gun = gun_in_slot.transform.GetChild(0).gameObject;

                    // ������ ������� �� ������ � �������� ���� ���������
                    mag_in_gun.transform.SetParent(transmitted_picture.transform);

                    // ������ ������� �� ��������� ��� �������� ������ ������
                    mag_in_pic.transform.SetParent(gun_in_slot.transform);

                    // ������������ �������� � ���������
                    SetMagInInventoryWithReplace(transmitted_picture, image_slot, mag_in_pic, gun_in_slot, mag_in_gun);

                }

                successLoad = is_loaded;
            }

        }
        else
        {

            /*
             * ���� ������ �� �������
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {
                // ������������ ������� � ���������� ��������� ��������
                gun_in_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded) 
                {
                    // ������������ ������� � ���������
                    SetMagInInventory(transmitted_picture, image_slot, mag_in_pic, gun_in_slot);
                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // ������������ ������� � ���������� ���������
                gun_in_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded)
                {
                    // ������������ ������� � ���������
                    SetMagInInventory(transmitted_picture, image_slot, mag_in_pic, gun_in_slot);
                }
                
                successLoad = is_loaded;

            }

            successLoad = is_loaded;


        }
    }









    private void SetMagInInventory(GameObject transmitted_picture, GameObject image_slot, GameObject mag_in_pic, GameObject gun_in_slot)
    {
        // ������������ ������� ��� �������� ������ ������ 
        mag_in_pic.transform.SetParent(gun_in_slot.transform);

        // ������ �������� ��������
        transmitted_picture.GetComponent<Image>().sprite = null;

        // �������� ��������
        transmitted_picture.GetComponent<Image>().enabled = false;

        // ������ �������� �� ��������� �������
        transmitted_picture.transform.position = image_slot.GetComponent<Slot>().SlotDefaultPosition;

        // ������� �������� ������
        Image gun_image = gun_in_slot.transform.parent.gameObject.GetComponent<Image>();

        // ������ ������ ����������� ������ 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // ������� ����
        image_slot.GetComponent<Slot>().ClearClot();


    }









    private void SetMagInInventoryWithReplace(GameObject transmitted_picture, GameObject image_slot, GameObject mag_in_pic, GameObject gun_in_slot, GameObject mag_in_gun)
    {
        // ������� �������� ������
        Image gun_image = gun_in_slot.transform.parent.gameObject.GetComponent<Image>();

        // ������ ������ ����������� ������ 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // �������� �������� ��������
        transmitted_picture.GetComponent<Image>().sprite = mag_in_gun.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // ������ �������� �� ��������� �������
        transmitted_picture.transform.position = image_slot.GetComponent<Slot>().SlotDefaultPosition;

        // �������� ���� ���������
        image_slot.GetComponent<Slot>().SetItem(mag_in_gun.GetComponent<FloorItem>().getItem, mag_in_gun);
    }


}
