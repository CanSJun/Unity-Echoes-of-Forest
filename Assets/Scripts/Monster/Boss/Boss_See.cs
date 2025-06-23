using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_See : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform Head;

     void Update()
    {
        Vector3 dir = UserControl._instance.transform.position - Head.position;
        Quaternion look = Quaternion.LookRotation(dir);
        Head.rotation = Quaternion.Slerp(Head.rotation, look, Time.deltaTime * 3f);
    }
}
