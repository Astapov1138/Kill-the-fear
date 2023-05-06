using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class mag : bullets_capacity
{


    private void Start()
    {
        // ������� ������� ��������
        Mag mag_item = GetComponent<FloorItem>().getItem as Mag;
        capacity = mag_item.GetCapacity;

        // ������������� ����
        bullets = new Stack<GameObject>();

        // �������� ������ ������ 
        if (bulletPrefab != null)
        {
            for (int i = 0; i < capacity; i++)
            {
                GameObject bulletInstance = Instantiate(bulletPrefab, transform);
                bulletInstance.SetActive(false);
                bullets.Push(bulletInstance);
                
            }


        }


        // ������������� ������� ���������� ������
        current_bullet_count = bullets.Count;
    }


}
