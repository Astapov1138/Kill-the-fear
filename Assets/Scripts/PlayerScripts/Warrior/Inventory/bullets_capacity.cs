using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullets_capacity : MonoBehaviour
{

    // ������� ��������
    protected int capacity;

    // ������� ���������� ������ � ��������
    public int current_bullet_count;

    // ���� ��������
    protected Stack<GameObject> bullets;

    // ������ ���� 
    [SerializeField] protected GameObject bulletPrefab;




    public int get_current_bullet_count => current_bullet_count;

    public int get_capacity => capacity;







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
