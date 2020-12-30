using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 스포너에서 일정 주기마다 일정 확률로 돌 생성 */

public class StoneSpawner : MonoBehaviour
{
    public GameObject m_Stone = null;
    public Transform m_StoneSpawner = null;
    public static List<GameObject> CopyObjList = null; // 돌 오브젝트 삭제를 관리할 리스트

    [Header("돌 스폰 확률")]public int SpawnPercentage = 60;

    public float DelaySec = 1.5f; // 생성 주기
    protected float m_CurrentSec = 0f;
    void Start()
    {
        CopyObjList = new List<GameObject>();

        m_Stone.SetActive(false); // 오브젝트 비활성화
        m_CurrentSec = DelaySec;
    }
    void Update()
    {
        // 플레이어 사망 상태
        if (Variables.GameState == GAMESTATE.DIE)
        {
            foreach(var item in CopyObjList)
            {
                GameObject.Destroy(item); // 리스트에 저장된 돌 오브젝트 전부 삭제
            }
            CopyObjList.Clear(); // 리스트 초기화
            return;
        }

        // 생성 주기마다 돌 생성 
        m_CurrentSec -= Time.deltaTime;
        if(m_CurrentSec <= 0f)
        {
            m_CurrentSec = DelaySec;
            SpawnUpdate();
        }   
    }

    void SpawnUpdate()
    {
        int randval = Random.Range(0, 101); // 0 ~ 100 사이의 랜덤값
        float randrotval = Random.Range(0, 361); // 0 ~ 360 사이의 랜덤값
        Vector3 randrotvec = new Vector3(randrotval, randrotval, randrotval); // 회전에 사용할 랜덤 벡터

        // 스폰 확률에 따라 생성
        if (randval <= SpawnPercentage) 
        {
            GameObject copyobj = GameObject.Instantiate(m_Stone); // 게임 오브젝트 복사
            copyobj.tag = "Stone"; // 태그 설정
            copyobj.transform.parent = m_StoneSpawner; // 스포너 하위로 설정
            copyobj.transform.Translate(m_StoneSpawner.position); // 생성 위치 설정
            copyobj.transform.Rotate(randrotvec); // 오브젝트 회전 설정

            CopyObjList.Add(copyobj); // 오브젝트 리스트에 복사된 오브젝트 추가
            foreach (var item in CopyObjList)
            {
                item.SetActive(true); // 리스트에 추가된 오브젝트 활성화
            }
        }
    }
}
