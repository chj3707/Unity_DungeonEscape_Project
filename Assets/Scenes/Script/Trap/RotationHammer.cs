using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 망치 손잡이의 하위로 망치를 두고 손잡이 회전
 * 망치 트랩의 회전을 관리할 스크립트 */

public class RotationHammer : MonoBehaviour
{
    protected float HammerRotSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        HammerRotSpeed = Random.Range(80f, 160f);
        transform.Rotate(HammerRotSpeed * Time.deltaTime, 0f, 0f);
    }
}
