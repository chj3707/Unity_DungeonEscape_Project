using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Pivot(카메라의 중심)을 기준으로 메인 카메라 위치, 회전 관리하는 스크립트 */

public class CamRotation : MonoBehaviour
{
    public static float PivotToCamera = 5f; // 카메라 까지의 거리
    public float RotSpeed = 5f; // 회전 속도
    
    float mx = 0f;
    float my = 0f;

    [Header("카메라 각도(상,하) 범위")]
    public float MinAngle = -50f;
    public float MaxAngle = 50f;

    void Start()
    {
    }

    void Update()
    {
        // 플레이어 사망
        if(Variables.GameState == GAMESTATE.DIE)
        {
            return;
        }

        /* 스크린 공간에서 이동하는 마우스값(-1 ~ 1) * 회전 속도 만큼 변수에 저장 */
        mx += -Input.GetAxis("Mouse Y") * RotSpeed; // 상, 하 (반전) 회전값
        mx = Mathf.Clamp(mx, MinAngle, MaxAngle); // 마우스 상,하 각도 범위 지정
        my += Input.GetAxis("Mouse X") * RotSpeed; // 좌, 우 회전값

        /* 메인 카메라를 오브젝트(Pivot) 하위로 두어서 같이 회전 */
        Quaternion pivotrot = Quaternion.Euler(mx, my, 0f);
        transform.rotation = pivotrot;

        /* 메인 카메라 위치 설정하는 벡터 (플레이어 뒤쪽에 위치)
         * 플레이어와 같은 방향을 바라보는 오브젝트의
         * 방향(플레이어 뒤쪽) * 카메라 까지의 거리 + 오브젝트 위치 */
        Vector3 maincampos = (-transform.forward * PivotToCamera) + transform.position;

        Camera.main.transform.position = maincampos; // 메인 카메라 위치
    }
}
