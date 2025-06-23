using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChatControl : MonoBehaviour
{
    static ChatControl _my;
    public static ChatControl _instance { get { return _my; } }

    void Awake() { _my = this;  _User = UserControl._instance; }



    [Header("기본정보")]
    [SerializeField] private Text _Name;
    [SerializeField] private Text _Body;
    [SerializeField] private float _Speed;

    [Header("Menu")]
    [SerializeField] private GameObject _Menu;
    [SerializeField] private GameObject _OkNotMenu;
    [SerializeField] private GameObject _NpcShop;
    private UserControl _User;
    private string _TotalBody;

    private bool _DestroyCheck;
   
    public  bool _InputKey = false; // 키 입력 받았을 시 모든 채팅 출력

    public void Init(string name, string body)
    {
        Unsetting();
        _Name.text = name;
        _TotalBody = body;
      


        gameObject.transform.SetAsLastSibling(); // 가림이 생길 수 있으니
        StartCoroutine(Chat(_Menu));
    }

    public void TalkNpc()
    {

        Unsetting();
            if (_User.Quest[1] == 0)
            {
                _TotalBody = "요즘 마을 앞에 곰이 있어서 마을 주민들이 많이 불안을 떨고 있다네. 혹시 도움을 줄 수 있는가?";
                StartCoroutine(Chat(_OkNotMenu));
            }
            else if (_User.Quest[1] < 4)
            {
                _TotalBody = "아직 처치하지 않은 모양이군.. 기다리고 있겠네.";
                StartCoroutine(Chat());
            }
            else if (_User.Quest[1] == 4)
            {
                _TotalBody = "오오 너무 고맙다네! 당신 덕분에 마을 주민들이 안심을 하겠군..!";
                _User.Quest[1] = 5;
                QuestDoumi._instance.Quest_Del(1);
                QuestDoumi._instance.Quest_Information_Add();
                StartCoroutine(Chat());
            }
            else if (_User.Quest[1] == 5 && _User.Quest[2] == 0)
            {
                _TotalBody = "이전에는 고마웠다네.. 그런데 숲의 깊은 곳에 엄청난 괴물이 있다고 하는군... 혹시 도움을 또 줄 수 있는가?";
                StartCoroutine(Chat(_OkNotMenu));
            }
            else if (_User.Quest[1] == 5 && _User.Quest[2] == 1)
            {
            _TotalBody = "아직 처치하지 않은 모양이군.. 기다리고 있겠네. 숲속 깊은 곳에 있다네";
            StartCoroutine(Chat());
        }
        else if (_User.Quest[1] == 5 && _User.Quest[2] == 2)
            {
                _TotalBody = "오오 너무 고맙다네! 당신 덕분에 마을 주민들이 안심을 하겠군..!";
                _User.Quest[2] = 3;
                QuestDoumi._instance.Quest_Del(2);
                QuestDoumi._instance.Quest_Information_Add();
                StartCoroutine(Chat());

                
            }

    }


    public void Accept()
    {
        Unsetting();
        if (_User.Quest[1] == 0)
        {
            _TotalBody = "오오 정말 고맙다네..!! 몸 조심하게!";
            QuestDoumi._instance.Quest_Add(1, 3, "곰 처치", 1);
            _User.Quest[1] = 1;
            StartCoroutine(Chat());
        }else if (_User.Quest[1] == 5 && _User.Quest[2] == 0)
        {
            _TotalBody = "오오 정말 고맙다네..!! 몸 조심하게! 이번에는 정말 강한 녀석이라 조심 하여야 할거네";
            QuestDoumi._instance.Quest_Add(2, 1, "숲의 지킴이 처치", 1);
            _User.Quest[2] = 1;
            StartCoroutine(Chat());
        }
    }
    public void NotAccept()
    {
        Unsetting();
        if (_User.Quest[1] == 0)
        {
            _TotalBody = "무리한 부탁을 해서 미안하네..";
            StartCoroutine(Chat());
        }
        else if (_User.Quest[1] == 3 && _User.Quest[2] == 0)
        {
            _TotalBody = "다음이라도 생각이 있으면 다시 말을 걸어주게..";
            StartCoroutine(Chat());
        }

      }
    IEnumerator Chat(GameObject menu)
    {
        foreach (char c in _TotalBody)
        {
            if (_InputKey)
            {
                _Body.text = _TotalBody;
                menu.SetActive(true);
                yield break;
            }
            _Body.text += c;
            yield return new WaitForSeconds(_Speed);
        }
        _InputKey = true;
        menu.SetActive(true);
    }
    IEnumerator Chat()
    {
        foreach (char c in _TotalBody)
        {
            if (_InputKey)
            {
                _Body.text = _TotalBody;
                _DestroyCheck = true;
                if (_User.Quest[1] == 5 && _User.Quest[2] == 3) MasterControl._Instance.SceneChange("EndingScene", MasterControl.GameState.start);
                yield break;
            }
            _Body.text += c;
            yield return new WaitForSeconds(_Speed);
        }
        _InputKey = true;
        _DestroyCheck = true;

        if(_User.Quest[1] == 5 && _User.Quest[2] == 3) MasterControl._Instance.SceneChange("EndingScene", MasterControl.GameState.start);
    }

    void Update()
    {
        if (_DestroyCheck)
        {
            if (Input.anyKeyDown)
            {
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        _User._CurrentState = UserControl.State.Idle;
        _Body.text = "";
        _TotalBody = "";
        _InputKey = false;
        _DestroyCheck = false;
        _Menu.SetActive(false);
        _OkNotMenu.SetActive(false);
        gameObject.SetActive(false);
        _NpcShop.SetActive(false);
    }

    void Unsetting()
    {
        _Body.text = "";
        _TotalBody = "";
        _InputKey = false;
        _DestroyCheck = false;
        _Menu.SetActive(false);
        _OkNotMenu.SetActive(false);
        _NpcShop.SetActive(false);
    }


    public void NPC_SHOP()
    {

        if(_NpcShop.gameObject.activeSelf == true) return;
        Destroy();
        _User._CurrentState = UserControl.State.Wait;
        _NpcShop.gameObject.SetActive(true);

    }
}
