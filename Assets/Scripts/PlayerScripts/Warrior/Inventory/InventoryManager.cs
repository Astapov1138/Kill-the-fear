using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;


public class InventoryManager : MonoBehaviour
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









    public void PutItem(Item item, GameObject itemObj)
    {
        bool SuccessGunAddition = false;

        foreach (AmmunitionGunSlot slot in am_gun_slots)
        {
            // ���� ������������ ������ �������� �������
            if (item.itemType == ItemType.gun)
                PutGunItem(item, itemObj, slot, out SuccessGunAddition);

            
            if (SuccessGunAddition)
            {
                // ��������� ��������� ���������
                SuccessGunAddition = false;

                return;
            }
            
        }
    }









    private void PutGunItem(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {

                // ������� �������� � �����, ������� ����� ���������� ������
                Transform gunImageTransform = slot.transform.GetChild(2);




                /*
                 *  ������������ ������ �������������� ����, ������� � �������
                */




                // ������������ ���� ������ ��� child ������ ��������, ������� ��� ����������
                TransmittedObject.transform.SetParent(gunImageTransform);


                TransmittedObject.GetComponent<SpriteRenderer>().sprite = null;



                /*
                 *  ������������ ����������� ������, ������� ������� � ����
                */




                // ������������ �������� � ������, ������� ������� � ����
                gunImageTransform.GetComponent<Image>().sprite = item.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;





                /*
                 * ���������� ������ �����
                */



                // ������� � ���� ������� � �������������� ��� ������ 
                slot.SetItem(item, TransmittedObject);




                /*
                 * �������� ���������� �������� ������ � ���� 
                */ 




                // ���������� �������� ������ �������
                SuccessGunAddition = true;

            }
            else
            {
                // ������ ������� �� �������� �������, ������� ��� ������ �������� � ���� ����
                SuccessGunAddition = false;
            }
        }
        else
        {
            // ���� �����. ���������� ������ �� �������
            SuccessGunAddition = false;
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

                // ������� Transform ��������, � ������� ���� �������� �������
                Transform InputImageTransform = slot.transform.GetChild(2);

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
                currentImageTransform.parent.gameObject.GetComponent<AmmunitionGunSlot>().ClearClot();



                /*
                 * ���������� �������� �����, �� ������� ���������� �������  
                */


                // ������ ����������� ��������, ������� ����������
                currentImageTransform.GetComponent<Image>().sprite = null;

                // ��������� �������� ��������� �� �����
                currentImageTransform.position = currentImageTransform.parent.gameObject.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;

                // ����� �� ����������
                currentImageTransform.GetComponent<Image>().enabled = false;






                /*
                 * �������� �������� ��������
                */



                // ���������� �������� ������ �������
                GunIsAdded = true;

            }
            else
            {





                // ������� Transform ��������, � ������� ���� �������� �������
                Transform InputImageTransform = slot.transform.GetChild(2);

                // ������� Transform ��������, ������� �������� ������������� ��������
                Transform currentImageTransform = TransmittedObject.transform.parent;

                // ������� ���� �������� ��������
                AmmunitionGunSlot currentSlot = currentImageTransform.parent.GetComponent<AmmunitionGunSlot>();

                // ������� ���� ������� �������� 
                AmmunitionGunSlot inputSlot = InputImageTransform.parent.GetComponent<AmmunitionGunSlot>();







                /*
                 * ����� ������� ������� 
                */





                //������� ������ ��������, ������� ��������� �� ������� �����
                GameObject InternalObject = InputImageTransform.GetChild(0).gameObject;

                InternalObject.transform.SetParent(currentImageTransform);

                TransmittedObject.transform.SetParent(InputImageTransform);






                /*
                 * ����� ������� ��������  
                */






                // ������� �������� �������� 
                currentImageTransform.GetComponent<Image>().sprite = currentImageTransform.GetChild(0).GetComponent<FloorItem>().getItem.GetInventoryIcon;

                // ������� ������� ��������
                InputImageTransform.GetComponent<Image>().sprite = InputImageTransform.GetChild(0).GetComponent<FloorItem>().getItem.GetInventoryIcon;

                // ��������� �������� �������� �� �����
                currentImageTransform.position = currentSlot.SlotDefaultPosition;






                /*
                 * ���������� ������� ������, � ������� �������� 
                */






                // ������� ������, ������� ������ ��������� � CurrentSlot
                GameObject gm_current = currentImageTransform.GetChild(0).transform.gameObject;

                // ������� �������, ������� ������ ��������� � CurrentSlot
                Item item_current = gm_current.GetComponent<FloorItem>().getItem;

                // ������������ �� � ����� ����
                currentSlot.SetItem(item_current, gm_current);



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
                GunIsAdded = true;

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
