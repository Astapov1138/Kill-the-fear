using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    //140 �������� �� �������
    private const float WarriorRotateCorrection = 140f;

    //���������� ���������
    [SerializeField]
    private float WarriorSpeed = 1.0f;
    [SerializeField]
    private Rigidbody2D Warrior;
    [SerializeField]
    private Camera cam;

    //�������
    Vector2 MovementDirection;
    Vector2 MousePosition;

    void Update()
    {
        //����������� ��������
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");
        //������� �������
        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        //������� ���������
        Vector2 LookDirection = MousePosition - Warrior.position;
        Warrior.rotation = (Mathf.Atan2(LookDirection.x, LookDirection.y) * -Mathf.Rad2Deg) + WarriorRotateCorrection;
        Warrior.velocity = MovementDirection * WarriorSpeed;

    }
}
