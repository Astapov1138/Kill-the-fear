using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    //140 �������� �� �������
    private const float WarriorRotateCorrection = 140f;

    //���� �����������
    private sbyte DirectionSignX = 0;
    private sbyte DirectionSignY = 0;

    //�������
    private const double Brakes = 0.2;
    private const sbyte MaxSpeed = 1;

    //���������� ���������
    [SerializeField]
    private float WarriorSpeed = 1f;
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



        //��������� (�������) ������ �� ��� �
        if ( Mathf.Abs(Warrior.velocity.x) < MaxSpeed) Warrior.AddForce(new Vector2(MovementDirection.x, 0) * WarriorSpeed, ForceMode2D.Impulse);
        else Warrior.velocity = new Vector2(MaxSpeed * DirectionSignX, Warrior.velocity.y);

        //��������� (�������) ������ �� ��� Y
        if ( Mathf.Abs(Warrior.velocity.y) < MaxSpeed) Warrior.AddForce(new Vector2(0, MovementDirection.y) * WarriorSpeed, ForceMode2D.Impulse);
        else Warrior.velocity = new Vector2(Warrior.velocity.x, MaxSpeed * DirectionSignY);


        //�������� ��������� (�������) �� � ���� �� ������ ������
        if (Warrior.velocity.x > 0) this.DirectionSignX = 1;

        else if (Warrior.velocity.x < 0) this.DirectionSignX = -1;

        if ((MovementDirection.x == 0) & ( (Warrior.velocity.x != 0) ))
        {
            Warrior.AddForce(new Vector2( DirectionSignX * (-1), 0) * WarriorSpeed, ForceMode2D.Impulse);
            if (Mathf.Abs(Warrior.velocity.x) <= Brakes) Warrior.velocity = new Vector2(0, Warrior.velocity.y);
        }



        //�������� ��������� (�������) �� Y ���� �� ������ ������
        if (Warrior.velocity.y > 0) this.DirectionSignY = 1;

        else if (Warrior.velocity.y < 0) this.DirectionSignY = -1;

        if ((MovementDirection.y == 0) & ((Warrior.velocity.y != 0)))
        {
            Warrior.AddForce(new Vector2(0, DirectionSignY * (-1)) * WarriorSpeed, ForceMode2D.Impulse);
            if (Mathf.Abs(Warrior.velocity.y) <= Brakes) Warrior.velocity = new Vector2(Warrior.velocity.x, 0);
        }

    }
}
