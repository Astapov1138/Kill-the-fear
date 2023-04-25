using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Gun;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // �������� ����� �������� �������� � �������� ����

    private UnityEngine.Transform parentTransform;

    private GameObject gunSlot;

    private GameObject item_slot;

    private bool in_inventory_range = true;

    private bool flag_on_start = true;

    private bool is_dragging = false;

    private PointerEventData ItemEventData;

    private RectTransform inventoryTransform;

    private GameObject[] GunSlotIndicators = new GameObject[3];

    private GameObject[] gunSlots = new GameObject[3];

    private GameObject Camera_main;

    private RectTransform inventoryRootTransform;

    private RectTransform items_UI_transform;

    private RectTransform canvasTransform;

    private RectTransform parent_transform_of_items_UI;
    
    private Vector2 item_hotspot;


    private void Start()
    {


        // ������� ������ ������ �� ������
        Camera_main = GameObject.Find("Main Camera");

        // ������� RectTransform ����� ���������
        inventoryRootTransform = GameObject.Find("InventoryRoot").GetComponent<RectTransform>();


        /*
         * ������� ��������� ����� 
        */

        for (int i = 0; i < gunSlots.Count(); i++)
        {
            GameObject gunSlotObj = GameObject.Find($"GunSlot({i + 1})");
            gunSlots[i] = gunSlotObj;
            GunSlotIndicators[i] = gunSlotObj.transform.GetChild(1).gameObject;
        }



        /*
         * ������� RectTransform ���������  
        */

        inventoryTransform = GameObject.Find("Inventory").GetComponent<RectTransform>();


        /*
         * ������� Items_UI RectTransform  
        */

        items_UI_transform = GameObject.Find("ItemsUI").GetComponent<RectTransform>();

        /*
         *  ������� Transform �������
        */

        canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();

        /*
         * ������� hotspot ��������������� �������� 
        */

        item_hotspot = new Vector2 (inventoryTransform.position.x - inventoryRootTransform.position.x, inventoryTransform.position.y - inventoryRootTransform.position.y);

    }










    public void OnBeginDrag(PointerEventData eventData)
    {

        ItemEventData = eventData;

        // ��������� Transform ������������� �������
        parentTransform = transform.parent;

        // ��������� Transform ������������� ������� Items_UI
        parent_transform_of_items_UI = items_UI_transform.parent.gameObject.GetComponent<RectTransform>();

        // ������������ ������������ ������ � �������� ���������
        transform.SetParent(inventoryRootTransform);

        // ����������� �������� ����� �������� �������� � �������� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );


        

        // ������ ���������� �� �������� ����

        foreach (GameObject indicator in GunSlotIndicators)
        {

            if (indicator != null)
            {
                // ����� ��� ��� ���������� �������� � �������� ��������� - ���������� ��������� ��������
                Indicator ind = indicator.GetComponent<Indicator>();
                if (ind != null)
                {
                    if (flag_on_start)
                    {
                        // ����� ��� ��� ���������� �������� � �������� ��������� - ���������� ��������� ��������
                        ind.RememperParent(indicator.transform.parent.gameObject);
                    }

                    // ������������� �������� � �������� ���������, ����� ��������� ��������� �� �������� ����
                    indicator.transform.SetParent(inventoryTransform);
                }

            }


        }




        /*
         * Items_UI �� �������� ���� 
        */

        /*
        items_UI_transform.SetParent(canvasTransform);
        */

        /*
         * ������ ��������� ���������� ��������������� �������� ���� ����
        */

        transform.SetSiblingIndex(canvasTransform.childCount - 1);

        // �������� ��������� ���������������� �������, ����� ��� �� ���������� �� ���� 
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        
        is_dragging = true;

        item_slot = null;
    }








    RaycastResult cursorEvent;

    public void OnDrag(PointerEventData eventData)
    {

        cursorEvent = eventData.pointerCurrentRaycast;

        gunSlot = null;

        

        if (cursorEvent.gameObject != null)
        {
            // ���� ������ ����� �� ����������,�� �������� ���� ����� ���������� 
            if (cursorEvent.gameObject.name == "GunSlotIndicator")
            {
                Indicator ind = cursorEvent.gameObject.GetComponent<Indicator>();
                if (ind != null)
                {
                    gunSlot = ind.GetIndicatorParent;
                }

            }


        }



        






        /*
         *  �������� �� ���������� ������� ���� � �������� ����� ���������  
        */



        // ������� ���� �� ������
        Vector3 mousePosition = Input.mousePosition;


        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRootTransform, mousePosition))
        {
            in_inventory_range = true;
        }
        else
        {
            in_inventory_range = false;
        }



    }


    
    private void LateUpdate()
    {
        if (is_dragging)
        {
            // �������� ������� �������� �� Canvas
            
            
            Vector2 newPosition = ItemEventData.position - offset + item_hotspot;
            (transform as RectTransform).anchoredPosition = newPosition;

        }


    }


    private void Update()
    {


        if (is_dragging)
        {
            if (cursorEvent.gameObject != null)
            {

                if (cursorEvent.gameObject.name == "Item_border")
                {
                    // ������� �������� (����)
                    GameObject item_border_parent = cursorEvent.gameObject.transform.parent.gameObject;

                    item_slot = item_border_parent;

                    Debug.Log("������");
                }


            }
        }
    }









    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;



        /*
         *  ������������ ���������� ������� 
        */






        for (int i = 0; i < GunSlotIndicators.Count(); i++)
        {



            if (GunSlotIndicators[i] != null)
            {
                Indicator ind = GunSlotIndicators[i].GetComponent<Indicator>();

                if (ind != null)
                {

                    if (GunSlotIndicators[i] != null)
                    {
                        // ������������ ��������
                        GunSlotIndicators[i].transform.SetParent(gunSlots[i].transform);

                        // ������������ ��������� �� ������ ��� ������ 
                        GunSlotIndicators[i].transform.SetSiblingIndex(1);

                        // ������� ��������
                        GameObject ind_parent = GunSlotIndicators[i].transform.parent.gameObject;


                        // ��������� ��������
                        ind.RememperParent(ind_parent);


                        // ������������ ��������� �� ������� ������������� �����
                        GunSlotIndicators[i].transform.position = ind_parent.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;

                        // ����������� ���� � ������ �� ����� ���������� ������������ ������ �� �������������
                        flag_on_start = false;
                    }
                }
            }




        }





        /*
         * ��������� ItemsUI �������  
        */

        //items_UI_transform.SetParent(parent_transform_of_items_UI);



        // ������������ � �������� ������������� ������� ���, ������� ��� ������������ �� ������� (��� �������, ������� �������������)
        transform.SetParent(parentTransform);






        /*
         *  ���� ��������� ��������� ����� � ������� 
        */






        if (gunSlot != null)
        {

            // ������� ������, ������� �������
            GameObject gun = transform.GetChild(0).gameObject;

            // ������� �������, ������� ������� 
            Item item = gun.GetComponent<FloorItem>().getItem;

            // ������� ����, � ������� ���� �������� ������
            AmmunitionGunSlot slot = gunSlot.GetComponent<AmmunitionGunSlot>();

            bool SuccessAddition;

            // ������� ������ � ����� ����
            Camera_main.GetComponent<InventoryManager>().PutWeaponToSlot(item, gun, slot, out SuccessAddition);

            if (!SuccessAddition)
            {
                // ������ ���� ������ �� ���������� � ���� 

            }


            return;

        }




        if (item_slot != null)
        { 
            Debug.Log($"��� �������, �� ������� �� ������ �������� = {item_slot.name}");
        }


        /*
         * ���� ��������� ���� Item_UI 
        */

        if (item_slot != null)
        {
            Debug.Log("�����������");
            // ������� ������, ������� �������
            GameObject itemObject = transform.GetChild(0).gameObject;

            // ������� �������, ������� ������� 
            Item item = itemObject.GetComponent<FloorItem>().getItem;

            // ������� ����, � ������� ���� �������� ������
            ItemSlot slot = item_slot.GetComponent<ItemSlot>();

            bool SuccessAddition;

            // ������� ������ � ����� ����
            Camera_main.GetComponent<InventoryManager>().PutItemToSlot(item, itemObject, slot, out SuccessAddition);

            if (!SuccessAddition)
            {
                // ������ ���� ������ �� ���������� � ���� 

            }

            Debug.Log("������ ������ ���� ��������� � ����� ����");
            return;
        }



        // �������� ��������� ���������������� �������, ����� ��� �� ���������� �� ���� 
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;


        /*
         *  ���� ����������� �� �����������, � ������ ���������� � �������� ��������� 
        */



        /*

        if (in_inventory_range == true)
        {
            // ������������ �������� �� �������� �������
            transform.position = transform.parent.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;
        }
        else
        {

            // ������� ������������� ������
            GameObject DroppedObj = transform.GetChild(0).gameObject;

            // ������� ������������� �������
            Item item = DroppedObj.GetComponent<FloorItem>().getItem;

            // ������� ���� �� �������� ��������� �������
            GameObject slot = transform.parent.gameObject;

            Slot item_slot = null;

            if (item.itemType == ItemType.gun)
            {
                item_slot = slot.GetComponent<AmmunitionGunSlot>();
            }

            if (DroppedObj != null && item != null && item_slot != null)
            {
                bool drop_is_success;

                Camera_main.GetComponent<InventoryManager>().DropItemFromInventory(item, DroppedObj, item_slot, out drop_is_success);

                if (!drop_is_success)
                {
                    Debug.Log("������� �� ��� ��������");
                }

            }

        }

        */
    }
}