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

    private RectTransform parent_transform_of_ammunitionUI;

    private RectTransform ammunitionUI_transform;
    
    private Vector2 item_hotspot;

    //bool target_on_slot;


    private void Start()
    {


        // ������� ������ ������ �� ������
        Camera_main = GameObject.Find("Main Camera");

        // ������� RectTransform ����� ���������
        inventoryRootTransform = GameObject.Find("InventoryRoot").GetComponent<RectTransform>();

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
         * ������� RectTransform AmmunitionUI 
        */

        ammunitionUI_transform = GameObject.Find("AmmunitionUI").GetComponent<RectTransform>();


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

        // ��������� Transform ������������� ������� AmmunitionUI
        parent_transform_of_ammunitionUI = ammunitionUI_transform.parent.gameObject.GetComponent<RectTransform>();

        // ������������ ������������ ������ � �������� ���������
        transform.SetParent(inventoryRootTransform);

        // ����������� �������� ����� �������� �������� � �������� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );



        /*
         * ������ ��������� ���������� ��������������� �������� ���� ����
        */

        transform.SetSiblingIndex(canvasTransform.childCount - 1);

        // �������� Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = false;


        
        is_dragging = true;

        item_slot = null;

        gunSlot = null;

        //target_on_slot = true;

    }








    RaycastResult cursorEvent;


    public void OnDrag(PointerEventData eventData)
    {




        cursorEvent = eventData.pointerCurrentRaycast;

        GameObject TargetObject = cursorEvent.gameObject;



        if (TargetObject != null)
        {
            // ���� ������ ����� �� ����������,�� �������� ���� ����� ���������� 
            if (TargetObject.name == "Gun_border")
            {
                // ������� �������� (����)
                GameObject gun_border_parent = TargetObject.transform.parent.gameObject;

                gunSlot = gun_border_parent;

            }
            else
                gunSlot = null;

            if (TargetObject.name == "Item_border")
            {
                // ������� �������� (����)
                GameObject item_border_parent = TargetObject.transform.parent.gameObject;

                item_slot = item_border_parent;


            }
            else
                item_slot = null;

            if (TargetObject.name == "GunImage" && TargetObject != transform.gameObject)
            {
                GameObject gun_border_parent = TargetObject.transform.parent.gameObject;
                gunSlot = gun_border_parent;
            }

            if (TargetObject.name == "ItemImage" && TargetObject != transform.gameObject)
            {
                GameObject item_border_parent = TargetObject.transform.parent.gameObject;
                item_slot = item_border_parent;
            }




            if (item_slot != null)
                Debug.Log($"��������� {cursorEvent.gameObject.name}, ��� ���� Item slot = {item_slot.name}");
            else
                Debug.Log($"��������� {cursorEvent.gameObject.name}, ��� ���� Item slot = Null");


        }
        else 
        {
            gunSlot = null;
            item_slot = null;
        }


        /*
         *  �������� �� ���������� ������� ���� � �������� ����� ���������  
        */

        // ������� ���� �� ������
        Vector3 mousePosition = Input.mousePosition;

        // �������� �� ���������� ������� ���� � �������� ����� ��������� 
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(inventoryRootTransform, mousePosition);

        //bool is_slot = false;

        //GameObject current_object_on_cursor = eventData.pointerCurrentRaycast.gameObject;


        if (contains)
        {
            in_inventory_range = true;
            //target_on_slot = false;
        }
        else if (!contains)
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












    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;


        // ������� Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = true;


        /*
         * ��������� ItemsUI �������  
        */

        //items_UI_transform.SetParent(parent_transform_of_items_UI);



        // ������������ � �������� ������������� ������� ���, ������� ��� ������������ �� ������� (��� �������, ������� �������������)
        transform.SetParent(parentTransform);



        /*
         *  ���� ��������� ��������� ���� 
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

                // ������������ �������� �� �������� �������
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

            }


            return;

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
                // ������������ �������� �� �������� �������
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
                Debug.Log("���-�� ����� �� �����");
            }


            return;
        }



        /*
         *  ���� ����������� �� �����������, � ������ ���������� � �������� ��������� 
        */



        

        if (in_inventory_range == true)
        {
            // ������������ �������� �� �������� �������
            transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
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

                    // ������������ �������� �� �������� �������
                    transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
                }

            }
            else 
            {
                // ������������ �������� �� �������� �������
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
            }

        }

        
    }
}