using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class Quest
{
    public int id { get; set; }
    public int type { get; set; }
    // 0번은 말만 걸면 되는 것, 1번은 수집 / 몬스터 잡기 등 필요 조건이 있어야하는 경우
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
        this.check = ""; // 완료여부 체크하기 위해서
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

    [Header("속도")]
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
            check = !check; // check가 false라면 true로, true면 false로
            if (_CurrentCourine != null) StopCoroutine(_CurrentCourine); // 현재 코루틴 종료
            _CurrentCourine = StartCoroutine(ONOFF());
        }
    }
    IEnumerator ONOFF()
    {
        Vector2 _pos = check ? _OriginPos : _DstPos;  // check가 true라면 Origin으로 나타나기, 그게 아니라면 뒤로 없어지기

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
            _text.text = "퀘스트가 없습니다.";
        }

    }


    public void Quest_Del(int id)
    {
        var check = quests.SingleOrDefault(q => q.id == id);
        if(check != null) quests.Remove(check); 
    }
    public void Quest_Add(int id, int purpose, string description, int type = 0)
    {
        var check = quests.SingleOrDefault(q => q.id == id); // Quest List에 해당 아이디 퀘스트가 있는지 체크를 한다
        if (check == null) { // 만약에 받지 않은 퀘스트라면 퀘스트를 추가해준다.
            quests.Add(new Quest(id, type, purpose, description));
            Quest_Information_Add();
        }
    }

    public void Update_Quest(int id, int current)
    {
        var check = quests.SingleOrDefault(q => q.id == id); Debug.Log("1");
        if (check != null && check.current < check.purpose) // 현재 퀘스트가 있으며, 목표보다 현재가 낮을 시 
        {
            check.current += current;
            if(check.purpose <= check.current) // 이 경우는 퀘스트를 클리어 한 경우이니 퀘스트 클리어 텍스트 추가
            {
                check.check += "(완료)";
            }
        }
    }

    public bool CheckQuest(int id)  => (quests.SingleOrDefault(q => q.id == id)) != null; // 퀘스트가 있으면 true, 없으면 false;

    

}
