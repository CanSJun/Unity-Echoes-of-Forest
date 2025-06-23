
using UnityEngine;

public class ExpBallControl : MonoBehaviour
{

    Vector3[] _pos = new Vector3[4];

    GameObject _player;
    [Header("꺾이는 양")]
    public float _Start = 5.0f;
    public float _End = 2.0f;
    [Header("속도")]
    public float _Speed = 1.0f;
    [Header("EXP")]
    public float _exp = 0f;
    private float _CurrentTime;
    private float _MaxTime = 1.3f;
    private float y;
    private float z;
    private float x;



    public void Set_EXP_Ball(Transform StartPos, float value) { 
        _player = GameObject.FindGameObjectWithTag("Player");
        _exp = value;
        x = Random.Range(-2.0f, 3.0f);
        y = Random.Range(0f, 2.0f);
        z = Random.Range(0.8f, 1.2f);

        _pos[0] = StartPos.position; 
        _pos[1] = StartPos.position + (_Start * x * StartPos.right) + (_Start *y * StartPos.up) + (_Start * z * StartPos.forward);
        _pos[2] = _player.transform.position + (_End * x * _player.transform.right) + (_End * y * _player.transform.up) + (_End * z * _player.transform.forward);
        _pos[3] = _player.transform.position;
    }

    void Update()
    {
        if (_CurrentTime > _MaxTime) return;
        if (_exp == 0) return;
        _CurrentTime += _Speed * Time.deltaTime;

        // 지속적으로 플레이어 추적
        _pos[2] = _player.transform.position + (_End * x * _player.transform.right) + (_End * y * _player.transform.up) + (_End * z * _player.transform.forward);
        _pos[3] = _player.transform.position;


        transform.position = new Vector3(BezierCurve(_pos[0].x, _pos[1].x, _pos[2].x, _pos[3].x), BezierCurve(_pos[0].y, _pos[1].y, _pos[2].y, _pos[3].y), BezierCurve(_pos[0].z, _pos[1].z, _pos[2].z, _pos[3].z));

    }
    float BezierCurve(float p0, float p1, float p2, float p3)
    {
        // 베지어 곡선의 공식 B(t)  =  (1 - t)^3 P0 + 3(1-t)^2tP1 + 3(1-t)t^2 P2 + t^3P3
        //                                  시작위치, 시작꺾이는 위치, 도착 꺽이는 위치, 도착위치

        float t = _CurrentTime / _MaxTime;
        return (Mathf.Pow(1 - t, 3) * p0) + (3 * Mathf.Pow(1 - t, 2) * t * p1) + (3 * (1 - t) * Mathf.Pow(t, 2) * p2) + (Mathf.Pow(t, 3) * p3);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            Destroy(gameObject);
        }
    }
}
