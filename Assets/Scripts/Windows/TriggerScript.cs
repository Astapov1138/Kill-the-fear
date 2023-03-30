using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class TriggerScript : MonoBehaviour
{
    // ��� ����� �� ������� ����� ����������
    [SerializeField] private string sceneName;

    // ��������� ������� ��������� � �������-��������� 
    private Vector3 playerPosition;

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition = collision.transform.position;

            Transition transition = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transition>();

            await transition.StartTransition();

            SceneManager.LoadScene(sceneName);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = playerPosition;

        }
    }
}
