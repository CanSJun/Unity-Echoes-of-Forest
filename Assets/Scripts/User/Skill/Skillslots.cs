
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(SlotDescriptControl))]

public class Skillslots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image slot;
    [SerializeField] Text text;

    [SerializeField] private Image _BackGround;
    [SerializeField] private Transform _DragImage;

    public SkillControl skillControl;

    private string leveltext;
    private int current_level, max_level;

    private bool _mouseovercheck = false;
    private SlotDescriptControl _descriptControl;
    [SerializeField] GameObject _descrption_window;
    public bool checkLoad = false;
 
    void Awake()
    {
        _descriptControl = GetComponent<SlotDescriptControl>();
        
    }
    void Start()
    {

        skillControl = SkillControl._instance;
        _BackGround = _DragImage.GetComponent<Image>();
    
    }



    public void OnPointerEnter(PointerEventData eventData) { _mouseovercheck = true; _descrption_window.gameObject.SetActive(true); }
    public void OnPointerExit(PointerEventData eventData) { _mouseovercheck = false; _descriptControl._init = false; _descrption_window.gameObject.SetActive(false); }

    public Skill GetSkill()
    {
        return SkillControl._instance.GetSkill(transform.name);
    }
    private void Update()
    {
        if (_mouseovercheck) _descriptControl.MouseOver(this, _descrption_window);

    }
    void outputText(string text)
    {
        LogSystem._instance.UpdateLog(text);
    }
    void Learning(int i)
    {
        if (i == 0) outputText($"{transform.name}을 배우셨습니다");
        else outputText($"{transform.name}의 레벨이 상승하였습니다.");
        UserControl._instance._SP -= 1;
        skillControl.UpdateSkill(transform.name, out leveltext, true);
        SkillUI._instance.UpdateSp();
        text.text = leveltext;
    }


    
    void Loading_skill(Skillslots slot, string name)
    {
        int current_level = 0;
        int max_level = 0;

            if (SkillControl._instance.UpdateSkill(name, out leveltext) == false) // 안배웠을 경우
            {
                slot.slot.color = Color.white;
                SkillControl._instance.UpdateSkill(name, out leveltext, true);
                slot.text.text = leveltext;
            }
            else
            {
                if (SkillControl._instance.GetSkillLevel(name, out current_level, out max_level) == true)
                {
                    if (current_level < max_level)
                    {
                    slot.slot.color = Color.white;
                    SkillControl._instance.UpdateSkill(name, out leveltext, true);
                    slot.text.text = leveltext;
                    }
                }
            
        }
    }
    public void InitLearning()
    {

        GameData data = MasterControl._Instance.Data;
        if (data != null)
        {
            string[] skill_name = { "블리자드", "방어력 상승", "포효", "회전베기", "라이트닝" };

            for (int i = 0; i < skill_name.Length; i++)
            {
                if (data.SKILL[i] != 0)
                {

                    GameObject skill = GameObject.Find(skill_name[i]);
                    if (skill != null)
                    {
                        Skillslots skillSlot = skill.GetComponent<Skillslots>();

                        if (skillSlot != null)
                        {
                            if (skillSlot.checkLoad == false)
                            {
                                skillSlot.checkLoad = true;
                                Loading_skill(skillSlot, skill.transform.name);
                            }
                        }
                    }
                }
            }
        }



    }
    public void Clicked()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (UserControl._instance._SP <= 0)
            {
                outputText("SP 포인트가 부족합니다.");
                return;
            }
            else
            {
                if (skillControl.UpdateSkill(transform.name, out leveltext) == false) // 안배웠을 경우
                {

                    slot.color = Color.white;
                    Learning(0);
                }
                else
                {
                    if (skillControl.GetSkillLevel(transform.name, out current_level, out max_level) == true)
                    {
                        if (current_level >= max_level)
                        {
                            outputText("더 이상 올릴 수 없습니다.");
                        }
                        else
                        {
                            Learning(1);
                        }
                    }
                }

            }
        }
        else if (Input.GetMouseButtonDown(0))
        { // QuickSlot에 옮기기 위해서 좌클릭
            if (skillControl.UpdateSkill(transform.name, out leveltext) == true) // 배웠을 경우만 이동이 되도록!
            {
                _DragImage.gameObject.SetActive(true);
                _DragImage.transform.position = Input.mousePosition;
                _BackGround.sprite = slot.sprite;
            }
        }
    }

    public void Drag()
    {
        if (skillControl.UpdateSkill(transform.name, out leveltext) == false) return;
        _DragImage.transform.position = Input.mousePosition;
    }

    public void DragEnd()
    {
        if (skillControl.UpdateSkill(transform.name, out leveltext) == false) return;
        QuickSlots slots = QuickSlotControl._instance.Find_ClosedQuickSlot(_DragImage.transform.position);
        if(slots == null)
        {
            Debug.Log("SkillSlots.cs - DragEnd - Slots not found");
            return;
        }
        
        QuickSlotControl._instance.QuickSlotAdd(this, slots);
    }

    public void MouseUp()
    {
        if (skillControl.UpdateSkill(transform.name, out leveltext) == false) return;
        _DragImage.gameObject.SetActive(false);
    }

}
