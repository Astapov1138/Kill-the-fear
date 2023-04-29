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
    private float MinFireDist = 0.2f;


    //������� ������� ����� � ����������� ����������� transform  (currentPoint)
    private FirePoint firePoints;

    private Transform firePointTransform;
    public void PlayerShoot() => Shoot();






    protected override void Shoot()
    {

        if ((Time.time - lastShotTime < delayBetweenShots) || (rangeFinder.GetDistToTarget <= MinFireDist) || (current_capacity.get_current_bullet_count <= 0)) { return; }


        lastShotTime = Time.time;

        //�������� ������� CurrentPoint 
        firePoints.UpdateCurrentPoint(ref firePointTransform);

        //bullet = Instantiate(bulletPrefab, firePointTransform.position, firePointTransform.rotation).GetComponent<PlayerBullet>();
        
        GameObject bullet_obj = current_capacity.TakeBullet();
        bullet_obj.transform.position = firePointTransform.position;
        bullet_obj.transform.rotation = firePointTransform.rotation;
        bullet_obj.SetActive(true);

        bullet = bullet_obj.GetComponent<PlayerBullet>();


        playerSounds.PlaySound();
        bullet.damage = damage;
        bullet.bulletSpeed = bulletSpeed;
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - correction.currentAngleDifference;
                for (int i = -8; i < 8; ++i)
                {
                    if (i != 0)
                    {
                        float angle = normalAngle + pelletsSpread * i + Random.Range(-pelletsDeviation, pelletsDeviation); 
                        bullet = Instantiate(bulletPrefab, firePointTransform.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<PlayerBullet>();
                        bullet.damage = damage;
                        bullet.bulletSpeed = bulletSpeed;
                    }
                }
                
                break;
        }
    }







    private Collider2D hammer_range_collider;

    private float last_kick_time;

    public void Kick()
    {
        if (Time.time - last_kick_time < SecondArmWeapon.GetCoolDown) { return; }
            last_kick_time = Time.time;
            hammer_range_collider.enabled = true;
            Invoke("TurnOnCollider", 0.1f);
        
    }

    private void TurnOnCollider() => hammer_range_collider.enabled = false;










    void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePointTransform = firePoints.GetCurrentTransform;

        rangeFinder = GetComponentInChildren<RangeFinder>();
        correction = GetComponent<WarriorMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        bullet = player.GetComponent<PlayerBullet>();

        hammer_range_collider = GameObject.Find("Warrior").transform.GetChild(0).gameObject.GetComponent<Collider2D>();

    }
}
