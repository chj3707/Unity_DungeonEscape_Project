using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 창 발사 주기, 복사된 오브젝트(창) 설정 */
public class ShootSpear: MonoBehaviour
{
    public Transform SpearShootPos = null; // 창 발사 지점
    public GameObject Spear = null;

    public float DelaySec = 1f; // 발사 주기
    protected float m_CurrentSec = 0f;

    void Start()
    {
        m_CurrentSec = DelaySec;
        Spear.SetActive(false);
    }

    void Update()
    {

        m_CurrentSec -= Time.deltaTime;
        if(m_CurrentSec <= 0f)
        {
            m_CurrentSec = DelaySec; // 발사 주기값 다시 할당
            GameObject copyobj = GameObject.Instantiate(Spear); // 오브젝트 복사
            copyobj.SetActive(true); // 오브젝트 활성화
            copyobj.tag = "Spear"; // 태그 설정
            copyobj.transform.parent = SpearShootPos; // 창 발사 지점 하위로 설정
            copyobj.transform.Translate(SpearShootPos.position); 
            copyobj.transform.Rotate(SpearShootPos.rotation.eulerAngles);
        }
    }
}
