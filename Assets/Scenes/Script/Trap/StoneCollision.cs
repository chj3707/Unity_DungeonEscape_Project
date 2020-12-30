using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 돌 오브젝트 충돌 처리 */

public class StoneCollision : MonoBehaviour
{
    // 충돌 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 콜라이더의 태그가 플레이어나 플랫폼이면 오브젝트 삭제
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Platform"))
        {
            GameObject.Destroy(this.gameObject); // 게임 오브젝트 삭제
            StoneSpawner.CopyObjList.Remove(this.gameObject); // 오브젝트 리스트에서 삭제
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
