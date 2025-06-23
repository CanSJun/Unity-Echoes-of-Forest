using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("Rotation")]
    public float _RotationSensitivity = 300f; // 기본 Default!
    public float _RotationSpeed = 100f; // 기본 Default!
    public float _CameraDistance = 5f;
    public float _minRotation = -65f;
    public float _maxRotation = 65f;

    private float x_rotation;
    private float y_rotation;
    private float z_rotation;

    private void Start()
    {
        x_rotation = transform.localEulerAngles.y;
        y_rotation = transform.localEulerAngles.x;
        z_rotation = transform.localEulerAngles.z;
    }
    public void Rotation()
    {

        float X = _RotationSensitivity + MasterControl._Instance.Sensitive_X;
        float Y = _RotationSensitivity + MasterControl._Instance.Sensitive_Y;
        x_rotation += Input.GetAxisRaw("Mouse X") * X * Time.deltaTime;
        y_rotation -= Input.GetAxisRaw("Mouse Y") * Y * Time.deltaTime;
        y_rotation = Mathf.Clamp(y_rotation, _minRotation, _maxRotation);
        transform.localEulerAngles = new Vector3(y_rotation, x_rotation, z_rotation);
    }
    public void Auto_Rotation()
    {
        x_rotation += Input.GetAxisRaw("Horizontal") * _RotationSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(y_rotation, x_rotation, z_rotation);
    }

    public void Mini_Rotation()
    {
        z_rotation += Input.GetAxisRaw("Mouse X") * _RotationSensitivity * Time.deltaTime;
        transform.localEulerAngles = new Vector3(90, 180, z_rotation);
    }
}
