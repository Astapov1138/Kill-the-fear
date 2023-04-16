using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private bool DeathWindowIsActive = false;

    private bool pauseWindowIsNotActive = true;

    public bool deathWindowIsActive
    {
        get { return DeathWindowIsActive; }
        set { DeathWindowIsActive = value; }
    }

    public void Pause()
    {
        pauseWindowIsNotActive = !pauseWindowIsNotActive;
        if (pauseWindowIsNotActive)
            Resume();
        else
        {
            pauseMenu.SetActive(true);
            CursorManager.Instance.SetMenuCursor();
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        pauseWindowIsNotActive = true;
        pauseMenu.SetActive(false);
        CursorManager.Instance.SetScopeCursor();
        Time.timeScale = 1f;
    }

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

    private void Update()
    {
        // ���� �������� ���� - ����� ���� ����� ������ �������
        if (DeathWindowIsActive)
            return;

        // ����� ����� �� ������� escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
