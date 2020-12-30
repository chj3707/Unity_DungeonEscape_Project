using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    public Transform TargetObj = null;
    [Header("보간 값")]public float LerpT = 3f;

    
    void Start()
    {

    }
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetObj.position, LerpT * Time.deltaTime);
    }
}
