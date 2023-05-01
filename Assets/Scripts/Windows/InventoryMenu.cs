using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InventoryMenu : MonoBehaviour

{

    [SerializeField] private GameObject inventoryWindow;

    [SerializeField] private GameObject faceUI;

    public GameObject GetFaceUI => faceUI;

    public GameObject GetInventoryWindow => inventoryWindow;

    private GameManagerScript gameManagerScript;

    private InventoryManager inventoryManager;

    private PauseMenu pauseMenu;

    private Vector3 beforeOpeningPosition;

    private bool InventoryWindowIsNotActive = true;

    private bool InputIsBlocked = false;

    private bool is_reloading = false;

    public bool set_reloading_status { set { is_reloading = value; }  }

    public bool inventoryWindowIsNotActive => InventoryWindowIsNotActive;


    private bool DeathWindowIsActive = false;

    public bool deathWindowIsActive
    {
        get { return DeathWindowIsActive; }
        set { DeathWindowIsActive = value; }
    }

    private bool PauseWindowIsActive = false;

    public bool pauseWindowIsActive
    {
        get { return PauseWindowIsActive; }
        set { PauseWindowIsActive = value; }
    }


    private void Start()
    {
        gameManagerScript = GetComponent<GameManagerScript>();

        pauseMenu = GetComponent<PauseMenu>();

        inventoryManager = GetComponent<InventoryManager>();
    }

    public void Inventory()
    {
        
        InventoryWindowIsNotActive = !InventoryWindowIsNotActive;
        if (InventoryWindowIsNotActive)
        {
            InventoryClose();
        }
        else
        {

            /*
             * �������� ��������� 
            */

            // �������� ����������� �� R �� ����� ��������� ��������� 
            inventoryManager.set_input_block_status = true;

            // �������� ������� �����������
            inventoryManager.block_current_reload = true;

            gameManagerScript.FreezePlayer();
            CursorManager.Instance.SetMenuCursor();
            inventoryWindow.SetActive(true);

            // �������� ���� �����
            pauseMenu.inventoryWindowIsActive = true;
            

            // ��������� ������� ������� �������
            beforeOpeningPosition = Input.mousePosition;


            // ������ ������� UI
            faceUI.SetActive(false);
        }


    }

    public void InventoryClose()
    {

        // ������ ���������� ����������� ������ �� R �� ����� ��������� ���������
        inventoryManager.set_input_block_status = false;

        // ���������� ������� �����������
        inventoryManager.block_current_reload = false;

        InventoryWindowIsNotActive = true;

        gameManagerScript.UnfreezePlayer();
        CursorManager.Instance.SetScopeCursor();
        inventoryWindow.SetActive(false);

        // ������� ���� �����
        Invoke("ActivatePauseInput", 0.2f);

        InputIsBlocked = true;

        Invoke("TurnOnInventoery", 0.3f);

        // ������� ������� UI
        faceUI.SetActive(true);

        Mouse.current.WarpCursorPosition(beforeOpeningPosition);

        InputState.Change(Mouse.current.position, beforeOpeningPosition);

    }

    // �������� ���� ��� �����
    private void ActivatePauseInput() => pauseMenu.inventoryWindowIsActive = false;

    // �������� ���� ���������
    private void TurnOnInventoery() => InputIsBlocked = false;







    private void Update()
    {
        // ���� �������� ���� - ����� ���� ��������� ������ �������
        if (DeathWindowIsActive || PauseWindowIsActive || InputIsBlocked || is_reloading)
            return;

        // ����� ��������� �� ������� I
        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory();
        }


        if (Input.GetKeyDown(KeyCode.Escape) && !InventoryWindowIsNotActive)
        {
            InventoryClose();
        }

    }
}
