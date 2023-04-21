using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // �������� ����� �������� �������� � �������� ����

    private Transform parentTransform;

    private GameObject gunSlot;

    private bool flag_on_start = true;


    public void OnBeginDrag(PointerEventData eventData)
    {

        // ��������� Transform ������������� �������
        parentTransform = transform.parent;

        // ������������ ������������ ������ � �������� ���������
        transform.SetParent(GameObject.Find("Inventory").transform);

        // ����������� �������� ����� �������� �������� � �������� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );


        

        // ������ ���������� �� �������� ����

        foreach (GameObject indicator in GameObject.FindGameObjectsWithTag("Indicator"))
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
                    indicator.transform.SetParent(GameObject.Find("Inventory").transform);
                }

            }


        }
    }









    public void OnDrag(PointerEventData eventData)
    {



        // �������� ������� �������� �� Canvas
        Vector2 newPosition = eventData.position - offset;
        (transform as RectTransform).anchoredPosition = newPosition;

        gunSlot = null;


        // ������� �������� ������ �������� �����
        if (eventData.pointerCurrentRaycast.gameObject.tag == "Indicator")
        {
            Indicator ind = eventData.pointerCurrentRaycast.gameObject.GetComponent<Indicator>();
            if (ind != null)
            {
                gunSlot = ind.GetIndicatorParent;
            }

        }
    }










    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject[] indicators = GameObject.FindGameObjectsWithTag("Indicator").ToArray();

        for (int i = 0; i < indicators.Count(); i++)
        {

            Indicator ind = indicators[i].GetComponent<Indicator>();

            if (ind != null)
            {
                if (indicators[i] != null)
                {
                    // ������������ ��������
                    indicators[i].transform.SetParent(GameObject.Find($"GunSlot({i + 1})").transform);
                    
                    // ������������ ��������� �� ������ ��� ������ 
                    indicators[i].transform.SetSiblingIndex(1);

                    // ������� ��������
                    GameObject ind_parent = indicators[i].transform.parent.gameObject;


                    // ��������� ��������
                    ind.RememperParent(ind_parent);


                    // ������������ ��������� �� ������� ������������� �����
                    indicators[i].transform.position = ind_parent.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;

                    // ����������� ���� � ������ �� ����� ���������� ������������ ������ �� �������������
                    flag_on_start = false;
                }
            }


        }

        // ������������ � �������� ������������� ������� ���, ������� ��� ������������ �� ������� (��� �������, ������� �������������)
        transform.SetParent(parentTransform);

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
            GameObject.Find("Main Camera").GetComponent<AmmunitionManager>().PutWeaponToSlot(item, gun, slot, out SuccessAddition);

            if (!SuccessAddition)
            { 
                // ������ ���� ������ �� ���������� � ���� 
            }

            
        }


    }
}