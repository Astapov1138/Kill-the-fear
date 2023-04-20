using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // �������� ����� �������� �������� � �������� ����

    private Transform parentTransform;

    private GameObject gunSlot;

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
    }

    public void OnDrag(PointerEventData eventData)
    {

        // �������� ������� �������� �� Canvas
        Vector2 newPosition = eventData.position - offset;
        (transform as RectTransform).anchoredPosition = newPosition;

        gunSlot = null;

        // ������� �������� ������ �������� �����
        if (eventData.pointerCurrentRaycast.gameObject.tag == "GunSlot")
        {
            gunSlot = eventData.pointerCurrentRaycast.gameObject;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (gunSlot != null)
        { 
            /*

            //������� ������, ������� �������
            GameObject gun = transform.GetChild(0).gameObject;

            // ������� ����, � ������� ���� �������� ������
            AmmunitionGunSlot slot = gunSlot.GetComponent<AmmunitionGunSlot>();

            bool SuccessAddition;

            // ������� ������ � ����� ����
            GameObject.Find("Main Camera").GetComponent<AmmunitionManager>().PutWeaponToSlot(gun, slot, out SuccessAddition);

            */
        }

        // ������������ � �������� ������������� ������� ���, ������� ��� ������������ �� �������
        transform.SetParent(parentTransform);
    }
}