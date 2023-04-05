using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;
    
    private GameObject player;

    private Player playerParams;

    private Shooting playerShooting;


    public void gameOver()
    {
        gameOverUi.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();
        
        // �������� ������������, �������
        player.GetComponent<WarriorMovement>().enabled = false;

        // �������� ������
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // ������� ��������� ���� � ������ ������ Shooting, ����� ������������ ���� � ���������
        playerShooting.enabled = false;


    }



    public void Restart()
    {
        gameOverUi.SetActive(false);
        player.transform.position = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        //��������� HP ������ � ��������� ���������, �� ������ ������ ��� � ������
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.playerIsDead = false;

        // ������� ������������, �������
        player.GetComponent<WarriorMovement>().enabled = true;

        // ������� ������
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // ������� ��������� ���� � ������ ������ Shooting, ����� ������ �� ������������ ���� � ���������
        playerShooting.enabled = true;


        // ������������ �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
