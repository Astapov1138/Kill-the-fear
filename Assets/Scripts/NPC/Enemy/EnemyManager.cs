using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //������������ ������ �� �����, ������� ����������� � ������� ������ 

    private static EnemyManager instance;

    public static EnemyManager Instance => instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this.gameObject);


    }

    private HashSet<(int, int)> SetOfDead = new HashSet<(int, int)>();

    public HashSet<(int, int)> SetOfDeadEdit
    {
        get { return SetOfDead; }
        set { SetOfDead = value; }
    }


    // ��������� � �������
    public void AddToDeadList(int id, int sceneId)
    {
        SetOfDead.Add((id, sceneId));
    }


    // ���������� �������� � ��
    public void ToHell()
    {
        foreach (var elem in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = elem.GetComponent<Enemy>();
            if (SetOfDead.Contains((enemy.GetId, enemy.GetSceneId)))
            {
                Destroy(elem);
            }
        }
    }

    // ��� ������
    public void ShowMeTheDead()
    {
        foreach (var elem in SetOfDead)
        {
            Debug.Log(elem);
        }
    }

    public void DestroyReaper()
    {
        Destroy(this.gameObject);
    }

}
