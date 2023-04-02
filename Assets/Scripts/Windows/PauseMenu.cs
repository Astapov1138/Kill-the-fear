using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        PlayerManager.Instance.gameObject.SetActive(false);
        CameraManager.Instance.gameObject.SetActive(false);
        CanvasManager.Instance.gameObject.SetActive(false);
        EnemyManager.Instance.gameObject.SetActive(false);
        PauseManager.Instance.gameObject.SetActive(false);
        EventManager.Instance.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneID);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
