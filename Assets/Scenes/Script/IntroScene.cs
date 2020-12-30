using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene: MonoBehaviour
{

    void Awake()
    {
        Variables.GameState = GAMESTATE.INTRO; // 게임 상태 인트로 상태로 설정
    }
    void Start()
    {
        
    }

    void Update()
    {
        // 씬 전환 키 "F"
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("Stage1_Scene"); // 스테이지1 씬 로드
        }
    }
}
