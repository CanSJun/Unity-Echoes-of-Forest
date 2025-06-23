using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] float speed;
    private float rotationX;
    public void OnKeyUpdate()
    {
        rotationX = Input.GetAxisRaw("Horizontal");
    }
    public void UserRoation(Rigidbody rigidbody)
    {
        rigidbody.rotation = rigidbody.transform.rotation * Quaternion.Euler(0f, rotationX * speed * Time.fixedDeltaTime, 0f);
    }
}
