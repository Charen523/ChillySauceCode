using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StratBtn : MonoBehaviour
{
    public void StartGame()
    {
        // 메인씬으로 이동
        SceneManager.LoadScene("MainScene");

    }
}
