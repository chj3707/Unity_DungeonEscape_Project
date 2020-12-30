using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 플레이어 컨트롤 관련 스크립트 */

public class _PlayerContorl : MonoBehaviour
{
    [Header("플레이어 이동 속도")] public float PlayerSpeed = 2.0f;
    [Header("회전 속도")] public float TurnSpeed = 10f;
    [Header("점프력")]public float JumpPower = 6f;
    [Header("회전 각도")] public float RotateAngle;
    protected float m_RunValue = 0.75f; 
    protected float m_WalkValue = 0.3f;
    protected Vector3 m_MoveVec = new Vector3();

    protected Camera Cam = null; // 메인 카메라
    public Transform RayStartpos = null; // 레이 발사 시작지점

    protected Animator AniComponent = null; 
    protected Rigidbody RbComponent = null;
    public static MovePlatform m_MovePlat = null; // (MovePlatform 스크립트에서 조절)
   
    protected bool m_ISwalk; // 걷기 확인
    protected bool m_ISjump; // 점프 확인
    protected bool m_ISair;  // 공중에 있는지 확인
    protected bool m_ISsafe; // 플레이어가 시작지점, 도착지점에 있는지 확인
    public static bool m_ISriding; // 발판에 있는지 확인 (MovePlatform 스크립트에서 조절)
    void Start()
    {
        AniComponent = GetComponent<Animator>();
        RbComponent = GetComponent<Rigidbody>();

        Cam = Camera.main;

        m_ISwalk = false;
        m_ISjump = false;
        m_ISair = false;
        m_ISriding = false;

    }

    void Update()
    {
        // 플레이어 사망
        if(Variables.GameState == GAMESTATE.DIE)
        {
            return;
        }

        UpdateControl();

        if(m_ISriding)
        {
            RideProcess(); // 이동발판 탑승
        }

        if(m_ISsafe)
        {
            MoveBlocking(); // 벽 밖으로 나가지 못하도록 설정
        }
    }

    /* 플레이어 컨트롤 */
    void UpdateControl()
    {
        float h = Input.GetAxis("Horizontal"); // A, D
        float v = Input.GetAxis("Vertical"); // W, S

        /* 카메라 방향 (y축 고정) */
        Vector3 camfoward = Vector3.Scale(Cam.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        m_MoveVec = Vector3.Scale(transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        m_MoveVec = camfoward * v + Cam.transform.right * h; // 카메라가 보는 방향을 기준으로 이동벡터 정의

        // 걷기 (LeftShift)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_ISwalk = true;
            AniComponent.SetBool("IsWalk", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || m_MoveVec == Vector3.zero)
        {
            m_ISwalk = false;
            AniComponent.SetBool("IsWalk", false);
        }

        RaycastHit hitinfo;

        /* 빈 오브젝트 에서 바닥으로 레이를 쏴서
         * 플레이어와 바닥간의 거리 확인 */
        if (Physics.Raycast(RayStartpos.transform.position, transform.up * -1, out hitinfo))
        {
            //Debug.Log(hitinfo.distance);
            //Debug.DrawLine(transform.position, hitinfo.point);

            // 착지 모션
            if(hitinfo.distance <= 0.5f && m_ISair)
            {
                AniComponent.SetTrigger("JumpEnd");
                m_ISair = false;
            }
            // 시작지점, 도착지점에 있으면
            if (hitinfo.collider.CompareTag("Platform"))
            {
                m_ISsafe = true;
            }
        }

        // 점프 (Space)
        if (hitinfo.distance <= 0.15f)
        {
            m_ISjump = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_ISjump = true;
                JumpProcess();
            }
        }


        // 이동(키 입력)
        if (m_MoveVec != Vector3.zero) 
        {
            AniComponent.SetBool("IsRun", true);
            RotateAngle = Mathf.Atan2(m_MoveVec.x, m_MoveVec.z) * Mathf.Rad2Deg; // 라디안값 구해서 각도로 변환


            // 삼항 연산자를 사용해서 쉬프트를 클릭한 상태라면 걷는속도, 아니면 뛰는속도로 설정
            transform.position += m_MoveVec * (m_ISwalk ? m_WalkValue * PlayerSpeed : m_RunValue * PlayerSpeed) * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation
                , Quaternion.Euler(0, RotateAngle, 0), TurnSpeed * Time.deltaTime); // 회전 속도 보간
        }
        // 이동 멈춤
        else
        {
            AniComponent.SetBool("IsRun", false);
        }
    }

    /* 점프 */
    void JumpProcess()
    {
        if(!m_ISjump)
        {
            return;
        }
        
        RbComponent.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
      
        AniComponent.SetTrigger("Jumping");
        // Jumping(점프) -> NextAction(활공) -> JumpEnd(착지) 모션
    }

    /* 활공 모션 이벤트 */
    public void NextAction()
    {
        AniComponent.SetTrigger("NextAction");
        m_ISair = true;
    }

    /* 발판에 탑승한 동안(MovePlatform에서 변수 조절) */
    void RideProcess()
    {
        if(m_MovePlat == null)
        {
            return;
        }
        
        Vector3 temppos = transform.position;
        Vector3 platmovevec = m_MovePlat.transform.forward * m_MovePlat.PlatMoveSpeed * Time.deltaTime; // 발판 이동 방향 * 속도 벡터
        temppos += platmovevec; // 플레이어 위치 벡터 갱신
        transform.position = temppos;
    }

    /* 벽 밖으로 나가지 못하도록 설정 */
    void MoveBlocking()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {
            // 플레이어 forward 방향으로 레이를 쏴서 벽이 있으면 가지 못하도록 설정
            if (hit.collider.CompareTag("Wall"))
            {
                transform.position -= m_MoveVec * (m_ISwalk ? m_WalkValue * PlayerSpeed : m_RunValue * PlayerSpeed) * Time.deltaTime;
            }
        }
    }
}
