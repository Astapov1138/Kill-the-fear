using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // MainCamera - ��� ������������ ������ �� �����. ���� ������ ��� ���
    // ��� �� ����� ������������ ��� �������� �� ������ �����

    private static CameraManager instance;

    public static CameraManager Instance => instance;

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

    public void DestroyCamera()
    {
        Destroy(this.gameObject);
    }
}
