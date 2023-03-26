using UnityEngine;

public class Shooting : MonoBehaviour
{
    //����� ������ 
    [SerializeField]
    private PlayerGun playerGun;

    [SerializeField]
    private PlayerGunSounds playerSounds;

    private FirePoint firePoints;

    void Awake()
    {
        firePoints = GetComponent<FirePoint>();

        
        playerGun.ChangeGun(1);

        firePoints.ChoosePoint(1);

        playerSounds.ChangePlayerSound(1);

    }

    void Update()
    {
        //����� ������
        if (Input.GetKey("1"))      { playerGun.ChangeGun(1); playerSounds.ChangePlayerSound(1); firePoints.ChoosePoint(1); }
        else if (Input.GetKey("2")) { playerGun.ChangeGun(2); playerSounds.ChangePlayerSound(2); firePoints.ChoosePoint(2); }
        else if (Input.GetKey("3")) { playerGun.ChangeGun(3); playerSounds.ChangePlayerSound(3); firePoints.ChoosePoint(3); }
        else if (Input.GetKey("4")) { playerGun.ChangeGun(4); playerSounds.ChangePlayerSound(2); firePoints.ChoosePoint(4); }
        if (Input.GetButtonDown("Fire1")) { playerGun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { playerGun.PullTheTrigger(); }


        if (playerGun.GetIsTriggered())
        {
            //�������
            if ((playerGun.GetShootMode() == Gun.ShootMode.auto)) { playerGun.PlayerShoot(); }
        }
        
    }
}
