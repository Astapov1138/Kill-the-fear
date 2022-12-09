using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //������� � LookDir ������ � �������     
    private float AngleDifference;
    public float angleDifference => AngleDifference;

    //LookDirection �������
    private float StartEnemyDir;
    //LookDirection ������                            
    private float StartGunDir;

    //��� ������           
    [SerializeField]
    private Transform EnemyGunAxis;

    //��� �������         
    [SerializeField]
    private Transform EnemyAxis;

    void Start()
    {
        StartEnemyDir = EnemyAxis.rotation.z * Mathf.Rad2Deg;
        StartGunDir = EnemyGunAxis.rotation.z * Mathf.Rad2Deg;
        AngleDifference = StartEnemyDir + StartGunDir;
    }

}

