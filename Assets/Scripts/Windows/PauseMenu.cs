using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public GameObject GetPauseMenu => pauseMenu;

    private GameObject Face_UI;

    private InventoryMenu inventoryMenu;

    private InventoryManager inventoryManager;

    private GameManagerScript gameManagerScript;

    private Vector3 beforeOpeningPosition;

    private bool DeathWindowIsActive = false;

    private bool pauseWindowIsNotActive = true;

    public bool PauseWindowIsNotActive => pauseWindowIsNotActive;

    private bool is_reloading = false;

    public bool set_reload_status { set { is_reloading = value; } } 





    public bool deathWindowIsActive
    {
        get { return DeathWindowIsActive; }
        set { DeathWindowIsActive = value; }
    }

    private bool InventoryWindowIsActive = false;

    public bool inventoryWindowIsActive
    {
        get { return InventoryWindowIsActive; }
        set { InventoryWindowIsActive = value; }
    }

    public void Pause()
    {
        pauseWindowIsNotActive = !pauseWindowIsNotActive;
        if (pauseWindowIsNotActive)
            Resume();
        else
        {

            /*
             * �������� ���� ����� 
            */


            // �������� ���� ��� ����������� �� R
            inventoryManager.set_input_block_status = true;

            // �������� ����������� ����������� �� R
            inventoryManager.block_current_reload = true;


            // ����������� ������
            gameManagerScript.FreezePlayer();
            CursorManager.Instance.SetMenuCursor();
            pauseMenu.SetActive(true);

            
            // �������� ���� ���������
            inventoryMenu.pauseWindowIsActive = true;

            // ��������� ������� ������� �������
            beforeOpeningPosition = Input.mousePosition;

            Time.timeScale = 0f;

            // �������� ������� UI
            Face_UI.SetActive(false);


        }
    }

    public void Resume()
    {
        // ������� ���� ��� ����������� �� R
        inventoryManager.set_input_block_status = false;

        // ������� ����������� ����������� �� R
        inventoryManager.block_current_reload = false;

        pauseWindowIsNotActive = true;

        // ������������ ������
        gameManagerScript.UnfreezePlayer();
        CursorManager.Instance.SetScopeCursor();
        pauseMenu.SetActive(false);

        // ������� ���� ���������
        inventoryMenu.pauseWindowIsActive = false;


        // ������������ ������
        Mouse.current.WarpCursorPosition(beforeOpeningPosition);

        InputState.Change(Mouse.current.position, beforeOpeningPosition);

        Time.timeScale = 1f;

        Face_UI.SetActive(true);
    }








    private void ActivateInventoryInput() => inventoryMenu.pauseWindowIsActive = false;








    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        //������������ ������
        CursorManager.Instance.SetMenuCursor();
        
        //��������� �� ��� �� ������������ ��� ��������, � ���� ��� �� �����
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
        SceneManager.LoadScene(sceneID);
    }




    private void ActivateFaceUI() => Face_UI.SetActive(true);



    private void Start()
    {
        inventoryMenu = GetComponent<InventoryMenu>();

        gameManagerScript = GetComponent<GameManagerScript>();

        Face_UI = GameObject.Find("FaceUI");

        inventoryManager = GetComponent<InventoryManager>();
    }


    private void Update()
    {
        // ���� �������� ���� - ����� ���� ����� ������ �������
        if (DeathWindowIsActive || InventoryWindowIsActive)
            return;


        // ����� ����� �� ������� escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        


    }
}
