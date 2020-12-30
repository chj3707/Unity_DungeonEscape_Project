using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public Text[] StageClearTime = null;
    public Button ReStartBtn = null;
    public Button ExitBtn = null;

    void Start()
    {
        for (int i = 0; i < StageClearTime.Length; i++)
        {
            // 클리어 타임 출력 (소수점 1자리 까지)
            StageClearTime[i].text = string.Format("Stage{0} Clear Time : {1}초", i + 1, Mathf.Round(Variables.ClearTime[i] * 10) * 0.1);
        }
    }

    void Update()
    {
        
    }

    // 다시하기 버튼
    public void _On_RestartClick()
    {
        Variables.GameState = GAMESTATE.INTRO;
        Variables.ClearTime.Clear(); // 리스트 초기화
        SceneManager.LoadScene("Stage1_Scene");
    }

    // 종료 버튼
    public void _On_ExitClick()
    {
        Application.Quit();
    }
}
