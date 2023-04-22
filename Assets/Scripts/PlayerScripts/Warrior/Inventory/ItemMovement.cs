using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Vector2 offset; // �������� ����� �������� �������� � �������� ����

    private Transform parentTransform;

    private GameObject gunSlot;

    private bool in_inventory_range = true;

    private bool flag_on_start = true;

    private bool is_dragging = false;

    private PointerEventData ItemEventData;

    [SerializeField] private Transform inventoryTransform;

    [SerializeField] private GameObject[] GunSlotIndicators = new GameObject[3];

    private GameObject[] gunSlots = new GameObject[3];

    private GameObject Camera_main;




    private void Start()
    {
        // ������� ��������� ����� ��������� �� ������
        for (int i = 0; i < gunSlots.Count(); i++)
        {
            gunSlots[i] = GameObject.Find($"GunSlot({i + 1})");
        }

        // ������� ������ ������ �� ������
        Camera_main = GameObject.Find("Main Camera");

    }










    public void OnBeginDrag(PointerEventData eventData)
    {

        ItemEventData = eventData;

        // ��������� Transform ������������� �������
        parentTransform = transform.parent;

        // ������������ ������������ ������ � �������� ���������
        transform.SetParent(inventoryTransform);

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


            is_dragging = true;


        }
    }










    public void OnDrag(PointerEventData eventData)
    {

        RaycastResult cursorEvent = eventData.pointerCurrentRaycast;

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

        

        Debug.Log("������� ������ ���������? = " + in_inventory_range);


    }


    
    private void LateUpdate()
    {
        if (is_dragging)
        {
            // �������� ������� �������� �� Canvas
            
            
            Vector2 newPosition = ItemEventData.position - offset;
            (transform as RectTransform).anchoredPosition = newPosition;



        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (eventData.pointerPressRaycast.gameObject != null)
        {
            if (eventData.pointerPressRaycast.gameObject.name == "InventoryRoot")
            {
                in_inventory_range = true;
            }
            else if (eventData.pointerPressRaycast.gameObject.name == "InventoryBackground")
            {
                in_inventory_range = false;

            }
        }
    }












    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;




        /*
         *  ������������ ���������� �� ������� 
        */



        for (int i = 0; i < GunSlotIndicators.Count(); i++)
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
            Camera_main.GetComponent<AmmunitionManager>().PutWeaponToSlot(item, gun, slot, out SuccessAddition);

            if (!SuccessAddition)
            { 
                // ������ ���� ������ �� ���������� � ���� 
            }

            
        }



        /*
         *  ���� ����������� �� �����������, � ������ ���������� � �������� ��������� 
        */


        if (in_inventory_range == true)
        {
            // ������������ �������� �� �������� �������
            transform.position = transform.parent.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;
            Debug.Log("�������� ��������� �� �������� �������");
        }
        else
        { 
        
        }


    }
}