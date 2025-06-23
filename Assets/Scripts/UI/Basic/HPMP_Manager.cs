using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPMP_Manager : MonoBehaviour
{
    private Rect imagepos;
    private RawImage _obj;
    [Header("Flow¼Óµµ")]
    [SerializeField] float _VerticalSpeed = 0f;
    [SerializeField] float _HorizentalSpeed = 0f;



    void Start() { 
        _obj = GetComponent<RawImage>();
        imagepos = _obj.uvRect;
    }

    // Update is called once per frame
    void Update()
    {

            imagepos.y -= _VerticalSpeed * Time.deltaTime;
            imagepos.x -= _HorizentalSpeed * Time.deltaTime;
            _obj.uvRect = imagepos;


    }
}
