using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 플레이어 체력 관리하는 스크립트 */

public class _PlayerHealth : MonoBehaviour
{
    public GameManager GMComponent = null;
    public Slider m_HealthSlider = null;

    /* 장애물 충돌 처리 */
    private void OnCollisionEnter(Collision collision)
    {
        // 돌 트랩과 충돌
        if (collision.collider.CompareTag("Stone"))
        {
            GameManager.PlayerHP -= GameManager.StoneDmg;
            GMComponent.HealthSlider.value = GameManager.PlayerHP;
        }
    }

    /* 장애물과 트리거 처리 */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.PlayerHP -= GameManager.TrapDmg;
            GMComponent.HealthSlider.value = GameManager.PlayerHP;
        }
        // 창 트랩
        if (other.CompareTag("Spear"))
        {
            GameManager.PlayerHP -= GameManager.SpearDmg;
            GMComponent.HealthSlider.value = GameManager.PlayerHP;
        }

        // 추락
        if (other.CompareTag("BackGround"))
        {
            GameManager.PlayerHP -= GameManager.PlayerHP;
            GMComponent.HealthSlider.value = GameManager.PlayerHP;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
