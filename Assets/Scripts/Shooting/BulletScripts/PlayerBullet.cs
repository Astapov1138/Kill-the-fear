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
    
    //������������ �� ������
    private RaycastHit2D WallHit;
    //������������ � ���������
    private RaycastHit2D EnemyHit;

    //����� ������ ���� (������)
    private float deathTime;


    //������� RB2D ������ ���� (������)
    [SerializeField]
    private Rigidbody2D PlayerBulletRB;

    public Rigidbody2D GetPlayerBulletRB => PlayerBulletRB;



    //������� Collider ������ ���� (������)
    [SerializeField]
    private BoxCollider2D PlayerBulletCollider;

    public BoxCollider2D GetPlayerBulletCollider => PlayerBulletCollider;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rangeFinder = player.GetComponentInChildren<RangeFinder>();

        BulletSpeed(PlayerBulletRB);
    }

    void FixedUpdate()
    {

        WallHit = hitTheWall(PlayerBulletRB, PlayerBulletCollider);
        EnemyHit = hitTheEnemy(PlayerBulletRB, PlayerBulletCollider);

        //���� ������������ �� ������
        if (WallHit)
        {
            Debug.Log("Wall hit");
            deathTime = DeathTime(WallHit);
            if (rangeFinder.GetDistToTarget < 0.55f)
            {
                Destroy(gameObject, deathTime);
                
            }
            else { Destroy(gameObject, deathTime); }
            

        }
        else if (EnemyHit)
        {
            deathTime = DeathTime(EnemyHit);

            Enemy enemy = EnemyHit.collider.GetComponent<Enemy>();

            Debug.Log("Enemy hit");

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject, deathTime);
            }

        }
    }

    /*
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(IsKillable);
        if (!IsKillable) return;

        Enemy enemy = collider.GetComponent<Enemy>();
        if ( (enemy != null) && (IsKillable) ) { enemy.TakeDamage(damage); Destroy(gameObject, deathTime); }
    }

    */

}
