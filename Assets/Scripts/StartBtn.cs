using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StratBtn : MonoBehaviour
{
    private void Start()
    {
        LevelManager.Instance.LevelCheck();

    }

    public void StartGame()
    {
        //�Ͻ����� ��ư�� ������ �ʾҴٸ�
        if (PauseBtn.isPaused != true)
        {
            // ���ξ����� �̵�
            //SceneManager.LoadScene("MainScene");


            LevelManager.Instance.OpenCanvas();
        }
    }
}
