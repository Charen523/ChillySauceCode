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
        //일시정지 버튼이 눌리지 않았다면
        if (PauseBtn.isPaused != true)
        {
            // 메인씬으로 이동
            //SceneManager.LoadScene("MainScene");


            LevelManager.Instance.OpenCanvas();
        }
    }
}
