using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 플레이어와 도착 지점의 거리를 계산해서 도착 관리하는 스크립트 */

public class ArriveCheak : MonoBehaviour
{
    public Transform PlayerPos = null; // 플레이어 위치값
    public static bool m_ISarrive; // 도착 확인용
    void Start()
    {
        m_ISarrive = false;
    }

    void Update()
    {
        if (Variables.GameState == GAMESTATE.DIE)
        {
            return;
        }
        
        // 플레이어와 도착 포인트 거리 할당
        float distance = Vector3.Distance(PlayerPos.position, transform.position);
        //Debug.Log(Mathf.Abs(distance));
        
        // 플레이어가 스테이지 클리어지점 근처에 도착
        if (Mathf.Abs(distance) <= 1f)
        {
            m_ISarrive = true;
        }
        else
        {
            m_ISarrive = false;
        }
    }
}
