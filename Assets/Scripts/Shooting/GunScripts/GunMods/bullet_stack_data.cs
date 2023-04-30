using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_stack_data : MonoBehaviour
{

    // ������� ����� � ���������
    private int capacity;

    // ������� ���������� ������ � �����
    private int current_bullet_count;

    // ���� ��������
    private Stack<GameObject> bullets;

    // ������ ���� 
    [SerializeField] private GameObject bulletPrefab;




    public int get_current_bullet_count => current_bullet_count;

    public int get_capacity => capacity;



    private void Start()
    {
        // ������� ������� ����� � ��������� 
        BulletStack bullets_stack_item = GetComponent<FloorItem>().getItem as BulletStack;
        capacity = bullets_stack_item.GetStackCapacity;

        // ������������� ����
        bullets = new Stack<GameObject>();

        // �������� ���� ������ 
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


    public GameObject TakeBullet()
    {
        if (current_bullet_count > 0)
        {
            GameObject takenBullet = bullets.Pop();

            current_bullet_count--;

            return takenBullet;
        }
        else
        {

            return null;
        }
    }


    public void LoadBullet(GameObject bullet, out GameObject load_result)
    {
        // ���� ���������� ������� �� ������� - �� ����� ���������� ������������ ����
        load_result = bullet;

        if (current_bullet_count < capacity)
        {
            bullets.Push(bullet);

            current_bullet_count++;

            // ���� ���������� ������ �������, �� ������ �� ����� null
            load_result = null;
        }
    }




}
