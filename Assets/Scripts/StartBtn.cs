using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StratBtn : MonoBehaviour
{
    public void StartGame()
    {
        //�Ͻ����� ��ư�� ������ �ʾҴٸ�
        if (PauseBtn.isPaused != true)
        { 
            // ���ξ����� �̵�
            SceneManager.LoadScene("MainScene");
        }
    }
}
