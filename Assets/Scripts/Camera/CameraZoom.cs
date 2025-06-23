using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("ZOOM")]
    public float _MinDistance = 2f;
    public float _CameraDistance = 5f;
    public float _ZoomSpeed = 5f;
    private float _CurrentDistance;

    public void Zoom(Transform player)
    {
        int Imask = 1 << LayerMask.NameToLayer("House") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Wood");

        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, -transform.forward, out hit, _CameraDistance, Imask))
        {
            _CurrentDistance = Mathf.Clamp(hit.distance, _MinDistance, _CameraDistance);
        }
        else
        {
            _CurrentDistance = Mathf.Lerp(_CurrentDistance, _CameraDistance, _ZoomSpeed * Time.deltaTime);
        }
        transform.position = player.position - (transform.forward * _CurrentDistance);

    }
}
