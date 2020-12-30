using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 창 트랩의 창 속도와 발사 주기, 충돌 처리하는 스크립트 */
public class SpearCollision : MonoBehaviour
{
    public float m_SpearSpeed = 5f; // 창 속도

    public float DestroySec = 2f;
    protected float m_CurrentSec = 0f;

    // 플레이어와 트리거 처리 순간 오브젝트 삭제
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    void Start()
    {
        m_CurrentSec = DestroySec;
    }

    void Update()
    {
        transform.position += transform.up * m_SpearSpeed * Time.deltaTime; // 창 이동 방향 * 이동 속도

        m_CurrentSec -= Time.deltaTime; 
        if(m_CurrentSec <= 0f) // DestroySec 주기값 마다 오브젝트 삭제
        {
            m_CurrentSec = DestroySec;
            GameObject.Destroy(this.gameObject);
        }
    }
}
