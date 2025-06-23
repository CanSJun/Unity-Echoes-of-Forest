using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class Quest
{
    public int id { get; set; }
    public int type { get; set; }
    // 0���� ���� �ɸ� �Ǵ� ��, 1���� ���� / ���� ��� �� �ʿ� ������ �־���ϴ� ���
    public int current { get; set; }
    public int purpose { get; set; }
    public string description { get; set; }

    public string check { get; set; }
    public Quest(int id, int type,  int purpose, string description)
    {
        this.id = id;
        this.type = type;
        this.current = 0;
        this.purpose = purpose;
        this.description = description;
        this.check = ""; // �ϷῩ�� üũ�ϱ� ���ؼ�
    }
    public string GetState()
    {
        if (this.type == 0) return string.Format("{0} {1}", description, check);
        else return string.Format("{0} {1}/{2} {3}", description, current, purpose ,check);
    }
}
public class QuestDoumi : MonoBehaviour
{
    static QuestDoumi _my;
    public static QuestDoumi _instance { get { return _my; } }
    private void Awake()
    {
        _my = this;
    }

    [Header("�ӵ�")]
    [SerializeField] private float _speed;
    [Header("Quest")]
    public Text _text;
    public  List<Quest> quests = new List<Quest>();

    private Vector2 _DstPos = new Vector2(-1155f, 470f);
    private Vector2 _OriginPos = new Vector2(-769f, 470f);
    private RectTransform rect;

    private bool check = false;

    private Coroutine _CurrentCourine;
    void Start() => rect = transform as RectTransform;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            check = !check; // check�� false��� true��, true�� false��
            if (_CurrentCourine != null) StopCoroutine(_CurrentCourine); // ���� �ڷ�ƾ ����
            _CurrentCourine = StartCoroutine(ONOFF());
        }
    }
    IEnumerator ONOFF()
    {
        Vector2 _pos = check ? _OriginPos : _DstPos;  // check�� true��� Origin���� ��Ÿ����, �װ� �ƴ϶�� �ڷ� ��������

            while (Vector2.Distance(rect.anchoredPosition, _pos) > 0.1f)
            {
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, _pos, _speed * Time.deltaTime);
                yield return null;
            }
            rect.anchoredPosition = _pos;
    }

    public void Quest_Information_Add()
    {
       

        if (quests.Count > 0)
        {
            _text.text = "";
            foreach (var quest in quests)
            {
                _text.text += quest.GetState();
            }
        }
        else
        {
            _text.text = "����Ʈ�� �����ϴ�.";
        }

    }


    public void Quest_Del(int id)
    {
        var check = quests.SingleOrDefault(q => q.id == id);
        if(check != null) quests.Remove(check); 
    }
    public void Quest_Add(int id, int purpose, string description, int type = 0)
    {
        var check = quests.SingleOrDefault(q => q.id == id); // Quest List�� �ش� ���̵� ����Ʈ�� �ִ��� üũ�� �Ѵ�
        if (check == null) { // ���࿡ ���� ���� ����Ʈ��� ����Ʈ�� �߰����ش�.
            quests.Add(new Quest(id, type, purpose, description));
            Quest_Information_Add();
        }
    }

    public void Update_Quest(int id, int current)
    {
        var check = quests.SingleOrDefault(q => q.id == id); Debug.Log("1");
        if (check != null && check.current < check.purpose) // ���� ����Ʈ�� ������, ��ǥ���� ���簡 ���� �� 
        {
            check.current += current;
            if(check.purpose <= check.current) // �� ���� ����Ʈ�� Ŭ���� �� ����̴� ����Ʈ Ŭ���� �ؽ�Ʈ �߰�
            {
                check.check += "(�Ϸ�)";
            }
        }
    }

    public bool CheckQuest(int id)  => (quests.SingleOrDefault(q => q.id == id)) != null; // ����Ʈ�� ������ true, ������ false;

    

}
