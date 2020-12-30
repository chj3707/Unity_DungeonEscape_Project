using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Scene(Stage), UI, 게임 상태 관리하는 스크립트 */

public enum GAMESTATE
{
    INTRO = 0,
    PLAY,
    NEXTSTAGE,
    CLEAR,
    DIE
}

// 정적 클래스 
public static class Variables
{
    public static GAMESTATE GameState; // 게임 상태
    public static List<float> ClearTime = new List<float>(); // 엔드씬 출력용도
}

public class GameManager : MonoBehaviour
{
    public Slider HealthSlider = null; // 체력바
    public Text TimerTxT = null; // 경과 시간 보여줄 텍스트
    public GameObject NextStageTxT = null; // 스테이지 안내 텍스트
    public GameObject RestartBtn = null; // 다시하기 버튼
    public GameObject ExitBtn = null; // 종료 버튼

    public static int PlayerHP; // 플레이어 체력
    public static int SpearDmg = 5; // 창 트랩 데미지
    public static int TrapDmg = 10; // 장애물 데미지
    public static int StoneDmg = 15; // 돌 트랩 데미지 
    protected readonly int StageCount = 3; // 스테이지 개수
    public float ElapsedTime; // 경과 시간

    public Transform Player = null; // 플레이어
    

    void Awake()
    {
        // 인트로 씬에서 넘어 왔거나, 다시하기, 다음 스테이지 초기화
        if (Variables.GameState == GAMESTATE.INTRO)
        {
            Time.timeScale = 1f; // pause 해제 (다시하기)
            Cursor.visible = false;  // 커서 안보이도록 함
            Cursor.lockState = CursorLockMode.Locked; // 커서 고정

            PlayerHP = 100; // 체력 설정
            HealthSlider.value = PlayerHP; // 체력바
            ElapsedTime = 0f; // 경과 시간

            RestartBtn.SetActive(false); // 재시작 버튼
            ExitBtn.SetActive(false); // 종료 버튼
            NextStageTxT.SetActive(false); // 스테이지 안내 텍스트

            // 플레이어 위치 초기화
            Player.transform.position = new Vector3(0f, 0.2f, 3f);

            // 게임 시작
            Variables.GameState = GAMESTATE.PLAY;
        }
    }

    void Start()
    {
       
    }


    void Update()
    {
        switch (Variables.GameState)
        {
                // 다음 스테이지, 사망 초기화
            case GAMESTATE.INTRO:
                Awake();
                break;
                // 게임 진행중
            case GAMESTATE.PLAY:
                PlayProcess();
                break;
                // 다음 스테이지(Scene)으로 넘어가는 부분 관리
            case GAMESTATE.NEXTSTAGE:
                NextStageProcess();
                break;
                // 게임 클리어(마지막 스테이지에서 NEXTSTAGE)
            case GAMESTATE.CLEAR:
                ClearProcess();
                break;
                // 플레이어 사망
            case GAMESTATE.DIE:
                DieProcess();
                break;
        }
    }

    void PlayProcess()
    {
        TimerTxT.text = string.Format("경과 시간 : {0}", Mathf.Round((ElapsedTime += Time.deltaTime) * 10) * 0.1f); // 경과 시간
        // 플레이어 스테이지 클리어 지점에 도착
        if (ArriveCheak.m_ISarrive) 
        {
            Variables.GameState = GAMESTATE.NEXTSTAGE;
        }
        
        if (HealthSlider.value <= 0)
        {
            Variables.GameState = GAMESTATE.DIE;
            return;
        }
    }

    void NextStageProcess()
    {
        // 스테이지 클리어 지점에서 멀어지면 리턴
        if(!ArriveCheak.m_ISarrive)
        {
            NextStageTxT.SetActive(false);
            Variables.GameState = GAMESTATE.PLAY;
            return;
        }
        
        // 스테이지 클리어 지점 근처에 오면
        NextStageTxT.SetActive(true); // 스테이지 안내 메시지

        //https://ansohxxn.github.io/unitydocs/unityengine-scenemanagement/ SceneManager
        // SceneManager.GetActiveScene().buildIndex 현재 활성화 되어있는 씬의 인덱스 리턴
        if (Input.GetKeyDown(KeyCode.F))
        {
            Variables.ClearTime.Add(ElapsedTime); // 클리어 시간 저장

            // 마지막 스테이지에서 도착 하면 게임 클리어
            if (SceneManager.GetActiveScene().buildIndex == StageCount)
            {
                Variables.GameState = GAMESTATE.CLEAR;
                return;
            }
            Variables.GameState = GAMESTATE.INTRO;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // 현재 씬의 인덱스 +1 (다음 씬) 로드
        }
    }

    void ClearProcess()
    {
        Time.timeScale = 0f; // pause
        Cursor.visible = true;   //마우스 커서 보이도록 함
        Cursor.lockState = CursorLockMode.None;   //마우스 커서 고정해제

        SceneManager.LoadScene("End Scene"); // 엔드 씬 로드
    }

    void DieProcess()
    {
        Time.timeScale = 0f; // pause
        Cursor.visible = true;   //마우스 커서 보이도록 함
        Cursor.lockState = CursorLockMode.None;   //마우스 커서 고정해제

        RestartBtn.SetActive(true);
        ExitBtn.SetActive(true);
    }

    // 다시하기 버튼
    public void _On_RestartBtn()
    {
        Variables.GameState = GAMESTATE.INTRO;
    }
    // 종료 버튼
    public void _On_ExitBtn()
    {
        //https://ssscool.tistory.com/306
        Application.Quit(); // 종료
    }
}
