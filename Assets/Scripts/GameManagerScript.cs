using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;
    
    private GameObject player;

    private GameObject Face_UI;

    private Player playerParams;

    private Shooting playerShooting;

    private PauseMenu pauseMenu;

    private InventoryMenu inventoryMenu;

    private InventoryManager inventoryManager;

    private int StartSceneIndex;

    private Vector3 SpawnPointPosition;

    private EnemyManager enemyReaper;

    private CanvasTransition transition;

    private Gun gun;


    public void gameOver()
    {
        // ���� ����� ���� - �������� ���� ����� 
        if (!pauseMenu.PauseWindowIsNotActive)
        {
            pauseMenu.Resume();
        }


        // ���� ����� ���� - �������� ���� ���������
        if (!inventoryMenu.inventoryWindowIsNotActive)
        {
            inventoryMenu.InventoryClose();
        }

        // �������� ������� UI
        Face_UI.SetActive(false);


        //������������ ������
        CursorManager.Instance.SetMenuCursor();

        gameOverUi.SetActive(true);

        // ����������� ������
        FreezePlayer();

        // �������� �������� ���� �����������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = false;
            }
        }

        // �������� ���� ��� �����
        pauseMenu.deathWindowIsActive = true;

        // �������� ���� ��� ���������
        inventoryMenu.deathWindowIsActive = true;

        

    }


    public void FreezePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();

        enemyReaper = GameObject.Find("EnemyReaper").GetComponent<EnemyManager>();
        gun = player.GetComponent<PlayerGun>();

        // �������� ������������, �������
        player.GetComponent<WarriorMovement>().enabled = false;

        // �������� ������
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // ���� �� ����� ���� ������ (��� ���� �������) ����� � ������� ��������� - � ��� ��������
        gun.TriggerIsPulled = false;

        // ������� ��������� ���� � ������ ������ Shooting, ����� ������������ ���� � ���������
        playerShooting.enabled = false;
    }

    public void UnfreezePlayer()
    {
        // ������� ������������, �������
        player.GetComponent<WarriorMovement>().enabled = true;

        // ������� ������
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // ������� ��������� ���� � ������ ������ Shooting, ����� ������ �� ������������ ���� � ���������
        playerShooting.enabled = true;
    }


    private void Start()
    {

        pauseMenu = GetComponent<PauseMenu>();

        inventoryMenu = GetComponent<InventoryMenu>();

        // ������� �����, � ������� �� ���������� �����������
        StartSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ������� ������� ����� ������ ������
        SpawnPointPosition = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        // ������� ����� �������� ��������
        transition = GameObject.Find("LevelChanger").GetComponent<CanvasTransition>();

        Face_UI = GameObject.Find("FaceUI");

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();
    }



    public void Restart()
    {

        //������������ ������
        CursorManager.Instance.SetScopeCursor();

        // ������������ �����
        SceneManager.LoadScene(StartSceneIndex);

        // ������������ ���������
        inventoryManager.ResetInventory();

        //������� ����������
        transition.StartDeathTransition();

        gameOverUi.SetActive(false);

        // ��������� ������ �� ��������� �������
        player.transform.position = SpawnPointPosition;

        //��������� HP ������ � ��������� ���������, �� ������ ������ ��� � ������
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.playerIsDead = false;

        UnfreezePlayer();

        // ������� �������� ���� �����������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = true;
            }
        }

        // �������� ���� ��� �����
        pauseMenu.deathWindowIsActive = false;

        // �������� ���� ��� ���������
        inventoryMenu.deathWindowIsActive = false;

        // ������� ������� UI
        Face_UI.SetActive(true);

        // ������� ������ ������ � �����
        enemyReaper.SetOfDeadEdit.Clear();

    }






    public void Home(int sceneID)
    {
        gameOverUi.SetActive(false);
        SceneManager.LoadScene(sceneID);

        //������������ ������
        CursorManager.Instance.SetMenuCursor();

        //��������� �� ��� �� ������������ ��� ��������, � ���� ��� �� �����
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
    }
}
