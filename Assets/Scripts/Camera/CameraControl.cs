using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


[RequireComponent(typeof(CameraZoom))]
[RequireComponent(typeof(CameraRotation))]
public class CameraControl : MonoBehaviour
{
    public Transform _player;


    private CameraZoom cameraZoom;
    private CameraRotation camerarotation;
    private UserControl _user;
    private void Awake()
    {
        cameraZoom = GetComponent<CameraZoom>();
       camerarotation = GetComponent<CameraRotation>();
    }
    private Vector3 Center;
    void Start() {
        //Center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        _user = UserControl._instance;

     }
    bool CheckMove() => !_user._isinventory && !_user._isskillbook && _user._CurrentState != UserControl.State.Wait && _user._CurrentState != UserControl.State.Boss;

    void Update() {
        if (CheckMove())
        {
            if(Input.GetButton("Fire2"))camerarotation.Rotation();
            else if(_user._CurrentState == UserControl.State.Idle)camerarotation.Auto_Rotation();
            cameraZoom.Zoom(_player);
        }
    }
}
