using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRotation))]
public class MiniMap_Manager : MonoBehaviour
{
    private UserControl player;
    private CameraRotation camerarotation;
    void Awake()
    {
        camerarotation = GetComponent<CameraRotation>();
    }
    void Start()
    {
        player = UserControl._instance;
    }

    void Update()
    {
        CameraMove();
        if(player._CurrentState == UserControl.State.Idle)
        camerarotation.Mini_Rotation();
    }

    void CameraMove()
    {
        Vector3 pos = player.transform.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
