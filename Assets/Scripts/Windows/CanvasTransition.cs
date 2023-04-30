using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTransition : MonoBehaviour
{

    private Animator animator;

    private InventoryManager inventoryManager;

    private string sceneName;
    public string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; }
    }

    private void Start()
    {
        animator = GameObject.Find("LevelChanger").GetComponent<Animator>();

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();
    }

    public void StartTransitionFadein()
    {
        animator.Play("Fadein");
    }

    public void StartTransitionFade()
    {
        animator.Play("Fade");
    }

    public void StartTransitionFadeOut()
    {
        animator.Play("FadeOut");
    }

    public void StartDeathTransition()
    {
        animator.Play("DeathFade", -1 ,0f);
    }

    //����� ���������� ������� ���������� (�� 60-� ����� �������� ���������� ���� �����)
    public void OnFadeComplite()
    {
        inventoryManager.LoadInventoryOnFade();
        
        StartTransitionFade();

        StartCoroutine(LoadSceneAsync());
    }

    // ��������� ����� �����������, ����� ToHell �������������� ����� ������ � ����� �����
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        EnemyManager.Instance.ToHell();
    }
}
