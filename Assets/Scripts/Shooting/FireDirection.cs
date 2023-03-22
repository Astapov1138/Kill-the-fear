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
    //������ ����� ������ ��������
    private Transform firePoint;

    //����������� ��������
    private Vector2 fireDirection;

    //����������� ��������, �������� ��������, ����� ��������� � ������ ��������
    public Vector2 GetFireDir => fireDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        gun = player.GetComponent<PlayerGun>();
        firePoint = gun.GetComponent<Transform>();
    }

    void Update()
    {
        
        //���� ������� ����� ������� � �������� 
        float RotateAngle = wm.angleDifference;
        
        //�������
        Quaternion q = Quaternion.AngleAxis(-RotateAngle, Vector3.forward);

        //����������� ������� ���
        Vector3 StartDirection = firePoint.right;
        

        //��� ��� ���������� ���� ���� fireDir
        fireDirection = q * StartDirection;
    }
}
