using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;


public class InventoryManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    private List<SecondArmSlot> second_arm_slots = new List<SecondArmSlot>();

    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    private RectTransform am_UI;

    private RectTransform item_UI;

    private Gun gun;








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
            else if (am_UI.GetChild(i).GetComponent<SecondArmSlot>() != null)
            {
                second_arm_slots.Add(am_UI.GetChild(i).GetComponent<SecondArmSlot>());
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

        // �������� ��������� ��������
        second_arm_slots[0].transform.GetChild(1).GetComponent<Image>().enabled = false;

        // �������� ��������� (������� ������� ������)
        Invoke("EndInventoryLoad", 0.1f);

        // ������� ������ ������
        gun = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

    }








    private void EndInventoryLoad()
    { 
        GameObject.Find("Inventory").SetActive(false);
        second_arm_slots[0].transform.GetChild(1).GetComponent<Image>().enabled = true;
    }








    private bool is_reloading = false;

    private void Update() 
    {
        if (Input.GetKey(KeyCode.R) && !is_reloading)
        {
            // ������� ���� �������� ������
            GameObject current_gun_slot = gun.GetCurrentSlot();

            // ������� ������ � ����� 
            GameObject gun_in_slot = current_gun_slot.GetComponent<Slot>().object_in_slot;

            // ������� ����� �����������
            float reload_time = gun.get_current_reload_time;

            Gun.Guns gun_type = gun.GetGunType();

            // ��� ���� � �������� ���������
            GameObject mag_slot = null;

            if (gun_type == Gun.Guns.hammer || gun_type == Gun.Guns.none) { return; }

            if (gun_type == Gun.Guns.shotgun) { mag_slot = SearchSlotWithBulletStack(); }
            else { mag_slot = SearchSlotWithMag(gun_type); }

            if (mag_slot != null && gun_in_slot != null) 
            {
                if (gun_type == Gun.Guns.shotgun)
                {
                }
                else
                {
                    Debug.Log($"��������� ������� ����� ������ = {mag_slot.GetComponent<Slot>().SlotDefaultPosition}");
                    StartCoroutine(ReloadGun(current_gun_slot, mag_slot, reload_time));
                }
            }

        }
    }









    private GameObject SearchSlotWithMag(Gun.Guns gun_type)
    {
        foreach (Slot slot in itemSlots)
        {

            if (gun_type == Gun.Guns.assaultRifle)
            {
                // ������� ������� � �����
                RifleMag mag_in_slot = slot.item_in_slot as RifleMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    return slot.gameObject;
            }
            else
            {
                // ������� ������� � �����
                PistolMag mag_in_slot = slot.item_in_slot as PistolMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    return slot.gameObject;
            }
        }
        return null;
    }








    private GameObject SearchSlotWithBulletStack()
    {
        foreach (Slot slot in itemSlots)
        {
            // ������� ������� � �����
            BulletStack internal_bullet_stack = slot.item_in_slot as BulletStack;
            if (internal_bullet_stack != null && slot.object_in_slot.transform.childCount > 0)
                return slot.gameObject;
        }
        return null;
    }








    IEnumerator ReloadGun(GameObject gun_slot, GameObject mag_slot, float reload_time)
    {
        is_reloading = true;
        
        yield return new WaitForSeconds(reload_time);

        bool flag = false;
        
        SetMagToGun(gun_slot, mag_slot, out flag);

        Debug.Log($"������ ������ ������, = {flag}");
        
        is_reloading = false;
    }










    public void MainInventoryManager(Item item, GameObject itemObj)
    {




        /*
         * ���� ������������ ������ �������� ������� 
        */


        if (item.itemType == ItemType.gun)
        {
            foreach (AmmunitionGunSlot slot in am_gun_slots)
            {

                bool SuccessGunAddition = false;

                GrabGunItem(item, itemObj, slot, out SuccessGunAddition);


                if (SuccessGunAddition) return;

            }
        }



        /*
         * ���� ������������ ������� �� �������� ������� ��� ������ 
        */


        if (item.itemType != ItemType.gun && item.itemType != ItemType.secondaty_arms && item.itemType != ItemType.armor && item.itemType != ItemType.edged_weapon) 
        {
            foreach (ItemSlot slot in itemSlots)
            {
                bool success = false;

                GrabDefaultItem(item, itemObj, slot, out success);


                if (success) return;
            }
        }




        if (item.itemType == ItemType.edged_weapon)
        {
            GrabEdgedWeapon(item, itemObj);
        }


    }









    private void GrabEdgedWeapon(Item item, GameObject itemObj)
    {

        Slot slot = second_arm_slots[0];

        if (slot.SlotIsEmpty)
        {

            // ������� �������� ��������
            Transform image = slot.transform.GetChild(1);




            /*
             *  ���������� ����������� �������
            */

            // ������ ������ ������� �� ����� 
            itemObj.GetComponent<SpriteRenderer>().sprite = null;

            // �������� ��������� �������� �� �����, ����� ��� ������ ���� ��������� ������
            itemObj.GetComponent<Collider2D>().enabled = false;




            /*
             *  ������������ ����������� ��������, �������� ������� � ����
            */

            image.GetComponent<Image>().sprite = item.GetInventoryIcon;

            image.GetComponent<Image>().enabled = true;




            /*
             * ���������� ������ �����
            */

            // ������� � ���� ������� � �������������� ��� ������ 
            slot.SetItem(item, itemObj);


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
             *  ���������� ����������� �������
            */


            // ������ ������ ������� �� ����� 
            TransmittedObject.GetComponent<SpriteRenderer>().sprite = null;

            // �������� ��������� �������� �� �����, ����� ��� ������ ���� ��������� ������
            TransmittedObject.GetComponent<Collider2D>().enabled = false;




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










    private void SetItemToEmptySlot(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;

        if (item != null && TransmittedObject != null && slot != null)
        {


            // ������� Transform ��������, � ������� ���� �������� �������
            Transform InputImageTransform = slot.transform.GetChild(1);

            // ������� Transform ��������, ������� �������� ������������� ��������
            Transform currentImageTransform = current_slot.transform.GetChild(1);




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
            current_slot.ClearClot();




            /*
             * ���������� �������� �����, �� ������� ���������� �������  
            */


            // ������ ����������� ��������, ������� ����������
            currentImageTransform.GetComponent<Image>().sprite = null;

            // ��������� �������� ��������� �� �����
            currentImageTransform.position = current_slot.SlotDefaultPosition;

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









    private void SetItemWithReplace(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        

        /*
         * ������� �������� ������
        */


        // ������� Transform ��������, � ������� ���� �������� �������
        Transform InputImageTransform = slot.transform.GetChild(1);

        // ������� Transform ��������, ������� �������� ������������� ��������
        Transform currentImageTransform = current_slot.transform.GetChild(1);




        /*
         * ����� ������� �������� (� ��������)
        */


        InputImageTransform.SetParent(current_slot.transform);

        currentImageTransform.SetParent(slot.transform);




        /*
         * ����� ������� �������� (���������)
        */


        // ��������� �������� �������� �� �����
        currentImageTransform.position = slot.SlotDefaultPosition;

        InputImageTransform.position = current_slot.SlotDefaultPosition;




        /*
         * ���������� ������� ������, � ������� �������� 
        */


        // ������� ������, ������� ������ ��������� � CurrentSlot
        GameObject current_obj = current_slot.object_in_slot;

        // ������� �������, ������� ������ ��������� � CurrentSlot
        Item item_current = current_slot.item_in_slot;

        // ������� ������, ������� ������ ��������� � InputSlot
        GameObject input_obj = slot.object_in_slot;

        // ������� �������, ������� ������ ��������� � CurrentSlot
        Item item_input = slot.item_in_slot;

        // ������������ �� � ����� ����
        slot.SetItem(item_current, current_obj);

        // ������������ �� � ����� ���� 
        current_slot.SetItem(item_input, input_obj);




        /*
         * �������� �������� ��������
        */


        // ������ �������� ������ �������
        ItemAdded = true;


    }










    public void PutItemToSlot(Item item, GameObject TransmittedObject, ItemSlot slot, ItemSlot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;


        if (item.itemType == ItemType.gun || item.itemType == ItemType.armor || item.itemType == ItemType.secondaty_arms || item.itemType == ItemType.edged_weapon)
        {
            // ���������� ������ �� ������� 
            ItemAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {
                // ������������ ������� � ������ ����
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out ItemAdded);

            }
            else
            {
                // ������������ ������� � ������� ������ ��������� ����
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out ItemAdded);
            }
        }


    }









    //          �������, ������� ��������,         ���� �������,          �������� ���������    
    public void PutWeaponToSlot(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, AmmunitionGunSlot current_slot, out bool GunIsAdded)
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
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out GunIsAdded);

            }
            else
            {
                // ������������ ������ � ������� ������ ��������� ����
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out GunIsAdded);

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
            Image image_component = slot.transform.GetChild(1).gameObject.GetComponent<Image>();

            // ������ ��������
            image_component.sprite = null;

            // ����� �������� ����������
            image_component.enabled = false;




            /*
             *  ������ �������� �� ��������� �����
            */


            // ������� ������ ��������
            GameObject image_obj = slot.transform.GetChild(1).gameObject;

            // ������ �� ��������� ����� 
            image_obj.transform.position = slot.SlotDefaultPosition;




            /*
             * ������� ������� ����, ����� �������� ������ �������� ������ 
            */

            gun.ChangeGun(gun.get_current_slot); 



            /*
             * ����������� ������� �� ��������� 
            */


            // ������������ ������������ ������� ������� ������
            currentObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;




            /*
             * ������������ ������ �������� 
            */


            // ������� ������ ��������
            Sprite item_floor_image = item.GetFloorIcon;

            // ������������ ������
            currentObject.GetComponent<SpriteRenderer>().sprite = item_floor_image;

            // ������� ��������� �������, ����� ��� ����� ���� ���������
            currentObject.GetComponent<Collider2D>().enabled = true;




            /*
             * �������� ����������
            */

            SuccessDrop = true;


        }
    }









    public void SetMagToGun(GameObject input_slot, GameObject current_slot, out bool successLoad)
    {

        successLoad = false;

        // ������� ������ �������� �����
        Slot input_slot_data = input_slot.GetComponent<Slot>();

        // ������� ������ �� ������� �����
        GameObject gun_in_input_slot = input_slot_data.object_in_slot;

        // ������� ������ �������� �����
        Slot current_slot_data = current_slot.GetComponent<Slot>();

        // ������� ������� � �������� �����
        GameObject mag_in_current_slot = current_slot_data.object_in_slot;




        /*
         * �������� �� ��, ���� �� ������� � �������
        */

        if (gun_in_input_slot.transform.childCount > 0)
        {

            /*
             * ���� ������ �������
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {

                // ������������ ������� � ���������� ������
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // ������� ������� � �������
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // ������ ������� � ��������� �������� � �������� �������
                    dropped_gun_mag.transform.SetParent(null);

                    // ������ ������� �� ��������� ��� �������� ������ ������
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // ������������ �������� � ���������
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    // �������� ������ �� ������
                    gun.ChangeGun(gun.get_current_slot);

                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // ������������ ������� � ���������� ������
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // ������� ������� � �������
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // ������ ������� � ��������� �������� � �������� �������
                    dropped_gun_mag.transform.SetParent(null);

                    // ������ ������� �� ��������� ��� �������� ������ ������
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // ������������ �������� � ���������
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    // �������� ������ �� ������
                    gun.ChangeGun(gun.get_current_slot);

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

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {
                // ������������ ������� � ���������� ��������� ��������
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                // �������� ������ �� ������
                gun.ChangeGun(gun.get_current_slot);

                if (is_loaded) 
                {
                    // ������������ ������� � ���������
                    SetMagInInventory(input_slot, current_slot);
                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // ������������ ������� � ���������� ���������
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                // �������� ������ �� ������
                gun.ChangeGun(gun.get_current_slot);

                if (is_loaded)
                {
                    // ������������ ������� � ���������
                    SetMagInInventory(input_slot, current_slot);
                }
                
                successLoad = is_loaded;

            }

            successLoad = is_loaded;


        }
    }









    private void SetMagInInventory(GameObject input_slot, GameObject current_slot)
    {

        // ������� ������� � ������� ��������
        GameObject mag_in_pic = current_slot.GetComponent<Slot>().object_in_slot;

        // ������� ������ �� ������� �����
        GameObject gun_in_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // ������� �������� ������
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // ������� �������� �������� �����
        Image transmitted_picture = current_slot.transform.GetChild(1).gameObject.GetComponent<Image>();




        // ������������ ������� ��� �������� ������ ������ 
        mag_in_pic.transform.SetParent(gun_in_slot.transform);

        // ������ �������� ��������
        transmitted_picture.GetComponent<Image>().sprite = null;

        // �������� ��������
        transmitted_picture.GetComponent<Image>().enabled = false;

        // ������ �������� �� ��������� �������
        transmitted_picture.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;

        // ������ ������ ����������� ������ 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // ������� ����
        current_slot.GetComponent<Slot>().ClearClot();


    }









    private void SetMagInInventoryWithReplace(GameObject input_slot, GameObject current_slot, GameObject dropped_gun_mag)
    {
        // ������� �������� ������
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // ������� ������ � �����
        GameObject gun_in_input_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // ������� �������� �������� �����
        GameObject current_image = current_slot.transform.GetChild(1).gameObject;




        // ������ ������ ����������� ������ 
        gun_image.sprite = gun_in_input_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // �������� �������� ��������
        current_image.GetComponent<Image>().sprite = dropped_gun_mag.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // ������ �������� �� ��������� �������
        current_image.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;
        Debug.Log(current_image.transform.position);

        // �������� ���� ���������
        current_slot.GetComponent<Slot>().SetItem(dropped_gun_mag.GetComponent<FloorItem>().getItem, dropped_gun_mag);

    }









    public void ResetInventory()
    {
        foreach (Slot slot in am_gun_slots)
        {
            ResetSlot(slot);
        }

        foreach (Slot slot in second_arm_slots)
        {
            ResetSlot(slot);
        }

        foreach (Slot slot in itemSlots)
        {
            ResetSlot(slot);
        }
    }









    private void ResetSlot(Slot slot)
    {
        // �������� ����
        slot.ClearClot();

        // ������� ��������
        slot.transform.GetChild(1).GetComponent<Image>().sprite = null;

        // ������ �������� ����������
        slot.transform.GetChild(1).GetComponent<Image>().enabled = false;
    }



}
