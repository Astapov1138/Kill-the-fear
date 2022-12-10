using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //�������
    private GameObject player;
    //��������� �� ������� �������
    private RangeFinder rangeFinder;
    //Player bullet is hit
    private RaycastHit2D PlayerHit;
    //����� ������ ���� (������)
    private float deathTime;


    //������� RB2D ������ ���� (������)
    [SerializeField]
    private Rigidbody2D PlayerBulletRB;
    //������� Collider ������ ���� (������)
    [SerializeField]
    private BoxCollider2D PlayerBulletCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rangeFinder = player.GetComponent<RangeFinder>();

        BulletSpeed(PlayerBulletRB);
    }

    void Update()
    {
        PlayerHit = hitTheWall(PlayerBulletRB, PlayerBulletCollider);
        deathTime = DeathTime(PlayerHit);

        //���� ������������ �� ������ ��� ������ ��������
        if (PlayerHit)
        {

            if (rangeFinder.GetDistToTarget < 0.55f)
            {
                Destroy(gameObject, deathTime);
            }
            else { Destroy(gameObject, deathTime); }


        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null) { enemy.TakeDamage(damage); Destroy(gameObject, deathTime); }
    }

}
