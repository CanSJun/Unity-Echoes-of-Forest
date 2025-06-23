using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float run_speed;
    [SerializeField] float frontDistance;

    private Vector3 direction;
    private bool isfrontcheck;
    private int _Imask;
    //Triger 형식으로 구현하는게 더욱 더 효율 적!

    private void Awake()
    {
        _Imask = 1 << LayerMask.NameToLayer("House") | 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Wood");
    }
    void Freeze()
    {
        Vector3 moveDirection = transform.TransformDirection(direction); 
        isfrontcheck = Physics.Raycast(transform.position, moveDirection, frontDistance, _Imask);
        
    }
    void FixedUpdate() => Freeze();
    public void OnKeyUpdate(Animator Action)
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");
        Action.SetFloat("X", direction.x);
        Action.SetFloat("Y", direction.z);
        direction.Normalize();
    }
    public void UserMove(Animator Action, Rigidbody rigidbody)
    {
        bool check = Input.GetKey(KeyCode.LeftShift);
        float currentspeed = check ? run_speed : speed;
        Action.SetBool("IsRun", check);
        if (!isfrontcheck)
        {
            rigidbody.position += rigidbody.transform.TransformDirection(direction * currentspeed * Time.deltaTime);
        }
    }


}
