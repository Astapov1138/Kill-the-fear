using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FireDirection : MonoBehaviour
{
    //��� ��������� ����������� ���� ������� AngleDifference 
    [SerializeField]
    WarriorMovement wm;
    
    //�����
    private GameObject player;
    //����� ������ 
    private PlayerGun gun;

    //����������� ��������
    private Vector2 fireDirection;

    //����������� ��������, �������� ��������, ����� ��������� � ������ ��������
    public Vector2 GetFireDir => fireDirection;


    //������� ������� ����� � ����������� ����������� transform  (currentPoint)
    private FirePoint firePoints;

    private GameObject firePoint;

    private Transform firePointAxis;


    private void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePoint = firePoints.GetCurrentPoint;
        firePointAxis = firePoint.transform;

        player = GameObject.FindGameObjectWithTag("Player"); 

    }

    void Update()
    {
        
        //���� ������� ����� ������� � �������� 
        float RotateAngle = wm.angleDifference;
        
        //�������
        Quaternion q = Quaternion.AngleAxis(-RotateAngle, Vector3.forward);

        //����������� ������� ���
        Vector3 StartDirection = firePointAxis.right;
        

        //��� ��� ���������� ���� ���� fireDir
        fireDirection = q * StartDirection;
    }
}
