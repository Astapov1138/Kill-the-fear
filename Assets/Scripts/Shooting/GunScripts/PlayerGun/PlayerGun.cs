using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    //��� ����� ��������
    [SerializeField]
    private PlayerGunSounds playerSounds;

    //��������� � ������� ������
    private RangeFinder rangeFinder;

    //��������� ���� (��� ������ ���� PlayerBullet)

    [SerializeField]
    private GameObject bulletPrefab;

    public GameObject GetBulletPrefab => bulletPrefab;

    //��������� ��� ��������� (������� � LookDirection �������)
    private WarriorMovement correction;

    //����� � ������ ����
    private GameObject player;
    private PlayerBullet bullet;



    //����������� ��������� ��� ��������
    private float MinFireDist = 0.3f;


    //������� ������� ����� � ����������� ����������� transform  (currentPoint)
    private FirePoint firePoints;

    private GameObject firePoint;

    private Transform firePointAxis;


    public void PlayerShoot() => Shoot();




    protected override void Shoot()
    {
        if ((Time.time - lastShotTime < delayBetweenShots) || (rangeFinder.GetDistToTarget <= MinFireDist) ) { return; }
        lastShotTime = Time.time;
        bullet = Instantiate(bulletPrefab, firePointAxis.position, firePointAxis.rotation).GetComponent<PlayerBullet>();
        playerSounds.PlaySound();
        bullet.damage = damage;
        bullet.bulletSpeed = bulletSpeed;
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - correction.angleDifference;
                for (int i = -4; i < 4; ++i)
                {
                    if (i != 0)
                    {
                        float angle = normalAngle + pelletsSpread * i + Random.Range(-pelletsDeviation, pelletsDeviation);
                        bullet = Instantiate(bulletPrefab, firePointAxis.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<PlayerBullet>();
                        bullet.damage = damage;
                        bullet.bulletSpeed = bulletSpeed;
                    }
                }
                
                break;
        }
    }



    void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePoint = firePoints.GetCurrentPoint;
        firePointAxis = firePoint.transform;

        rangeFinder = GetComponentInChildren<RangeFinder>();
        correction = GetComponent<WarriorMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        bullet = player.GetComponent<PlayerBullet>();
    }
}
