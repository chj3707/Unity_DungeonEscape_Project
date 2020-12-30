using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 이동 발판 관리 스크립트
 * 이동 발판 foward 방향으로 레이를 쏴서 벽과 닿으면 반대 방향으로 이동 하도록 설정 
 * 이동 발판 up 방향으로 레이를 쏴서 플레이어 감지(탑승,하차 다른 오브젝트는 무시) */

public class MovePlatform : MonoBehaviour
{
    [Header("플랫폼 이동 속도")]public float PlatMoveSpeed = 6f;

    public float DelaySec = 3f; // 발판이 멈추어 있을 시간
    protected float m_CurrentSec = 0f;

    void Start()
    {
        m_CurrentSec = DelaySec;
    }
    void Update()
    {
        MoveUpdate();
        RidingUpdate();
    }
    /* 이동 발판 */
    void MoveUpdate()
    {
        RaycastHit hitinfo;
        float temprot = 0f;
        /* 이동 발판의 forward방향 으로 레이를 쏴서 충돌 감지
         * DelaySec값 만큼 기다렸다가 반대로 회전 후 이동 */
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, 1f))
        {
            
            if (hitinfo.collider.tag == "Platform" || hitinfo.collider.tag == "ObstaclePlat")
            {
                PlatMoveSpeed = 0f; // 충돌시 속도 0으로 설정 하여 대기
                m_CurrentSec -= Time.deltaTime;
                /* 대기 시간 종료 */
                if (m_CurrentSec <= 0f)
                {
                    m_CurrentSec = DelaySec; // 주기값 다시 할당
                    PlatMoveSpeed = 3f; // 속도 다시 할당

                    temprot += 180f;
                    transform.Rotate(0f, temprot, 0f); // 반대로 회전
                }
            }
        }

        transform.Translate(0f, 0f, PlatMoveSpeed * Time.deltaTime);
    }
    /* 플레이어 상,하차 설정 */
    void RidingUpdate()
    {
        RaycastHit boxhit;
        // https://dallcom-forever2620.tistory.com/18 다른 오브젝트 무시하고 레이 쏘기
        int layermask = 1 << LayerMask.NameToLayer("Player"); // "Player" 레이어만 검출

        //http://theeye.pe.kr/archives/2764 BoxCast, SphereCast

        /* 플레이어가 점프를 하더라도 이동 발판과 같이 이동하도록 설정
         * LayerMask를 이용해 다른 오브젝트는 무시하고 플레이어만 검출 */
        if (Physics.BoxCast(transform.position + (-transform.up), transform.lossyScale, transform.up, out boxhit, transform.rotation, 5f, layermask))
        {
            // Debug.Log(_PlayerContorl.m_ISriding);
            // Debug.DrawLine(transform.position, hit.point);
            if (boxhit.collider.tag == "Player")
            {
                _PlayerContorl.m_ISriding = true;
                _PlayerContorl.m_MovePlat = this;
            }
        }
        // 발판에 타지 않았을 때
        else
        {
            _PlayerContorl.m_ISriding = false;
            _PlayerContorl.m_MovePlat = null;
        }
    }
}
