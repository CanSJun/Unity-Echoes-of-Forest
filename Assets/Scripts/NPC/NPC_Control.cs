using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{



    private Animator _Action;
    private GameObject _target; // 유저를 바라보기 위해서
    [SerializeField] private GameObject _spine; // 유저를 바라보기 위해서

    [Header("바라보는 속도")]
    [SerializeField] private float LookSpeed = 5.0f;
    private Quaternion _Origin_Look;
    [Header("ETC")]
    [SerializeField] private GameObject Chat;
    [SerializeField] private int QuestNum;
    [Header("텍스트 표시 위치")]
    [SerializeField] private Transform _textpos;
    private GameObject _EventSystem;
    private EventSystemControl _script;

    private UserControl _User;


    void Start()
    {
        _User = UserControl._instance;
        _Action = GetComponent<Animator>();
        _Origin_Look = _spine.transform.rotation;
    }


    void Update()
    {
        if (_target) {
            User_Action();
            See(); 
        }
        else See_Back();
    }



    void User_Action()
    {
        if (!_target) return;
        if (Input.GetKeyDown(KeyCode.Space) && _User._CurrentState != UserControl.State.Wait)
        {
            _User._Action.SetFloat("X", 0);
            _User._Action.SetFloat("Y", 0);
            _User._Action.Play("Idle,Walk");
            _User._CurrentState = UserControl.State.Wait;

            _User.transform.LookAt(transform.position);
            Chat.GetComponent<ChatControl>()._InputKey = false;
            Chat.SetActive(true);
            
            Chat.GetComponent<ChatControl>().Init(transform.name, "안녕하세요? 무엇을 도와드릴까요?");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _User._CurrentState == UserControl.State.Wait)
        {
            Chat.GetComponent<ChatControl>()._InputKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _User._CurrentState == UserControl.State.Wait)
        {
            _User._CurrentState = UserControl.State.Idle;
            Chat.GetComponent<ChatControl>()._InputKey = false;
            Chat.GetComponent<ChatControl>().Destroy();
        }
    }

    void See()
    {
        if (!_target) return;
        Vector3 diff = _target.transform.position - _spine.transform.position; // 방향

        Quaternion Rotation = Quaternion.LookRotation(diff);
        _spine.transform.rotation = Quaternion.Slerp(_spine.transform.rotation, Rotation, LookSpeed * Time.deltaTime); // 부드럽게 움직이게 
    }

    void See_Back() => _spine.transform.rotation = Quaternion.Slerp(_spine.transform.rotation, _Origin_Look, LookSpeed * Time.deltaTime); // 부드럽게 움직이게 
    

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("UserPoint")) return;
        if (other.CompareTag("UserPoint"))
        {
            _target = other.gameObject;

            if (_EventSystem == null)
            {

                _EventSystem = TotalUiControl._instance.Create(TotalUiControl.Type.EventSystem);
                _EventSystem.transform.SetAsFirstSibling(); // 맨 위로 가서 Ui와 겹치지 않게 하기 위함
                _script = _EventSystem.GetComponent<EventSystemControl>();
                _script.Init(_textpos, "NPC와 대화를 하려면", "[Space]");
            }
            else
            {
                _script.OnOff(true);
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("UserPoint")) return;
        if (_target) { _target = null; }
        if (_EventSystem != null) _script.OnOff(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("UserPoint")) return;
        if (_target) return;
        else _target = other.gameObject;
    }





}
