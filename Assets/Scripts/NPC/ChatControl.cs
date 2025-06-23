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



    [Header("�⺻����")]
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
   
    public  bool _InputKey = false; // Ű �Է� �޾��� �� ��� ä�� ���

    public void Init(string name, string body)
    {
        Unsetting();
        _Name.text = name;
        _TotalBody = body;
      


        gameObject.transform.SetAsLastSibling(); // ������ ���� �� ������
        StartCoroutine(Chat(_Menu));
    }

    public void TalkNpc()
    {

        Unsetting();
            if (_User.Quest[1] == 0)
            {
                _TotalBody = "���� ���� �տ� ���� �־ ���� �ֹε��� ���� �Ҿ��� ���� �ִٳ�. Ȥ�� ������ �� �� �ִ°�?";
                StartCoroutine(Chat(_OkNotMenu));
            }
            else if (_User.Quest[1] < 4)
            {
                _TotalBody = "���� óġ���� ���� ����̱�.. ��ٸ��� �ְڳ�.";
                StartCoroutine(Chat());
            }
            else if (_User.Quest[1] == 4)
            {
                _TotalBody = "���� �ʹ� ���ٳ�! ��� ���п� ���� �ֹε��� �Ƚ��� �ϰڱ�..!";
                _User.Quest[1] = 5;
                QuestDoumi._instance.Quest_Del(1);
                QuestDoumi._instance.Quest_Information_Add();
                StartCoroutine(Chat());
            }
            else if (_User.Quest[1] == 5 && _User.Quest[2] == 0)
            {
                _TotalBody = "�������� �����ٳ�.. �׷��� ���� ���� ���� ��û�� ������ �ִٰ� �ϴ±�... Ȥ�� ������ �� �� �� �ִ°�?";
                StartCoroutine(Chat(_OkNotMenu));
            }
            else if (_User.Quest[1] == 5 && _User.Quest[2] == 1)
            {
            _TotalBody = "���� óġ���� ���� ����̱�.. ��ٸ��� �ְڳ�. ���� ���� ���� �ִٳ�";
            StartCoroutine(Chat());
        }
        else if (_User.Quest[1] == 5 && _User.Quest[2] == 2)
            {
                _TotalBody = "���� �ʹ� ���ٳ�! ��� ���п� ���� �ֹε��� �Ƚ��� �ϰڱ�..!";
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
            _TotalBody = "���� ���� ���ٳ�..!! �� �����ϰ�!";
            QuestDoumi._instance.Quest_Add(1, 3, "�� óġ", 1);
            _User.Quest[1] = 1;
            StartCoroutine(Chat());
        }else if (_User.Quest[1] == 5 && _User.Quest[2] == 0)
        {
            _TotalBody = "���� ���� ���ٳ�..!! �� �����ϰ�! �̹����� ���� ���� �༮�̶� ���� �Ͽ��� �Ұų�";
            QuestDoumi._instance.Quest_Add(2, 1, "���� ��Ŵ�� óġ", 1);
            _User.Quest[2] = 1;
            StartCoroutine(Chat());
        }
    }
    public void NotAccept()
    {
        Unsetting();
        if (_User.Quest[1] == 0)
        {
            _TotalBody = "������ ��Ź�� �ؼ� �̾��ϳ�..";
            StartCoroutine(Chat());
        }
        else if (_User.Quest[1] == 3 && _User.Quest[2] == 0)
        {
            _TotalBody = "�����̶� ������ ������ �ٽ� ���� �ɾ��ְ�..";
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
