
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ActionHandler))]
[RequireComponent(typeof(Move))]
[RequireComponent(typeof(Hit))]
[RequireComponent(typeof(Rotation))]
public class UserControl : MonoBehaviour
{
    static UserControl _my;
    public static UserControl _instance { get { return _my; } }


    [Header("정보")]
    public int _level = 1;
    public float _HP = 100;
    public float _nowHP = 100;
    public float _MP = 100;
    public float _nowMP = 100;
    public float _EXP = 0;
    public int _def = 0;
    public int _money = 0;
    public int _damage = 1;
    public int _SP = 0;
    [Header("유저 속도 조절")]
    public float _rotationspeed = 240;
    public float _runSpeed = 2.3f;
    public float _walkSpeed = 1.0f;
    public float _attackSpeed = 1.0f;

    [Header("거리")]
    public float _attackDistance = 1.7f;
    public float _frontDistance = 3f;
    [Header("기타")]
    public GameObject _inventory;
    public GameObject _infromation;
    public GameObject _skillbook;
    [Header("Animation에 넘겨줄 값")]
    public bool _isRun = false;
    public bool _isAttack = false;
    [Header("Quest")]
    public int[] Quest; // 현재는 퀘스트 두개
    [Header("COLLIDER")]
    public Transform _HandsArea;
    public Animator _Action;

    [Header("COMBAT")]
    public Transform _RightHand;

    public Transform _LeftHand;

    [Header("ETC")]
    public GameObject _HPBAR;
    public GameObject _MPBAR;
    [SerializeField] GameObject _Text;
    [SerializeField] GameObject _HitTextPos;
     

    public bool _isinventory = false;
    public bool _isstatus = false;
    public bool _isskillbook = false;
    public int _skilltype = 0;
    public bool _isShield = false;
    public bool _isShield_Using = false;
    public ItemControl _nowShield;
    //public bool _isBottomitem = false;
    public ItemControl _isItem = null;



    public bool isdead = false;
    public bool _isCheck;



    [Header("설정")]
    [SerializeField] GameObject Setting_Transform;
    // 새로 구현
    private ActionHandler actionHandler;
    private Move move;
    private Rigidbody _rigid;
    private Hit hit;
    private Rotation rotation;
    private GameObject _EventSystem;
    private EventSystemControl _script;

    public GameObject Die;

    public AudioClip AttackSound;
    public bool _areaskill = false;
    public GameObject _areaprefab;
    public QuickSlots _areaOrigin;

    [Header("LOADING")]
    public GameObject QuickSlots;
    public enum State
    {
        Idle = 0,
        Walk,
        Attack,
        Gathering,
        Jump,
        Roll,
        Die,
        Wait,
        Boss,
        Setting,
    }
    public enum Weapon
    {
        Hands = 0,
        One_Hand_Axe,
        Sword,

    }
    public State _CurrentState = State.Idle; // 기본 상태
    public Weapon _CurrentWeapon = Weapon.Hands;

    void Awake()
    {
        _my = this;
        actionHandler = GetComponent<ActionHandler>();
        move = GetComponent<Move>();
        _rigid = GetComponent<Rigidbody>();
        hit = GetComponent<Hit>();
        rotation = GetComponent<Rotation>();
    }



    public void Init()
    {
        GameData data = MasterControl._Instance.Data;
        if(data != null)
        {
            
            transform.position = new Vector3(data.LocationX, data.LocationY, data.LocationZ);
            transform.rotation = new Quaternion(data.RotationX, data.RotationY, data.RotationZ, data.RotationW);
            _nowHP = data.HP;
            _HP = data.MAXHP;
            _nowMP = data.MP;
            _MP = data.MAXMP;
            _EXP = data.EXP;
            _level = data.Level;
            ExpControl._instance.GetExp(0);
            _def = data.DEF;
            _money = data.MONEY;
            _SP = data.SP;
            for (int i = 0; i < data.QUESTS.Length; i++) { 
                
                Quest[i] = data.QUESTS[i];
               
                
            }

            
            if(Quest[1] > 0 && Quest[1] < 5)QuestDoumi._instance.Quest_Add(1, 3, "곰 처치", 1);
            if(Quest[2] > 0 && Quest[2] < 3) QuestDoumi._instance.Quest_Add(2, 1, "숲의 지킴이 처치", 1);
            for (int i = 0; i < data.QUESTS.Length; i++)
            {
                if (data.QUESTS[i] > 0)QuestDoumi._instance.Update_Quest(i, Quest[i] - 1);
            }
            QuestDoumi._instance.Quest_Information_Add();
            _HPBAR.GetComponent<HPMP_Control>().iniChangeValue(_nowHP/_HP);
            _MPBAR.GetComponent<HPMP_Control>().iniChangeValue(_nowMP/_MP);


        }

        
    }
    void Start()
    {
        _Action = GetComponent<Animator>();
        Init();
        
        _skillbook.gameObject.SetActive(true);
        _skillbook.gameObject.GetComponentInChildren<Skillslots>().InitLearning();
        _skillbook.gameObject.SetActive(false);
    }
    void Update()
    {
        if (_CurrentState == State.Setting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _CurrentState = State.Idle;
                Setting_Transform.gameObject.SetActive(false);
            }
            return;
        }

        if (_CurrentState == State.Die)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                MasterControl._Instance.SceneChange("StartScene", MasterControl.GameState.start);
            }
            return;
        }
        if (_CurrentState == State.Wait || _CurrentState == State.Boss) return;

        if (_areaskill == false)
        {
            if (Input.GetKeyDown(KeyCode.B)) actionHandler.Handle_Gather(ref _CurrentState, _Action, _isItem);

            actionHandler.Handle_Enter(KeyCode.I, ref _isinventory, _inventory);
            actionHandler.Handle_Enter(KeyCode.E, ref _isstatus, _infromation);
            actionHandler.Handle_Enter(KeyCode.K, ref _isskillbook, _skillbook);
            if (Input.GetKeyDown(KeyCode.Escape) && (_isinventory == true || _isstatus == true || _isskillbook == true)) actionHandler.Handle_Escape(ref _isinventory, ref _isstatus, ref _isskillbook, _inventory, _infromation, _skillbook);
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _CurrentState = State.Setting;
                Setting_Transform.gameObject.SetActive(true);
                return;
            }


            if (_CurrentState == State.Gathering) return;
            if (Input.GetButtonDown("Fire1") && _isShield == false) { _CurrentState = State.Attack; actionHandler.Handle_Attack(_isAttack, _isinventory, _isskillbook, _isstatus, ref _skilltype, _CurrentWeapon, _Action, transform); }
            else if (Input.GetButton("Jump")&& _isShield_Using == true)
            {
                _isShield = true;
                _Action.SetTrigger("Shield");
            }
            else if (Input.GetButtonUp("Jump")) { _Action.ResetTrigger("Shield"); _Action.Play("Idle,Walk"); _isShield = false; }


            if (!_Action.GetBool("IsAttack") && _skilltype < 2 && _isShield == false)
            {
                move.OnKeyUpdate(_Action);
                rotation.OnKeyUpdate();
            }
        }
        else
        {
            AreaUpdate();
            move.OnKeyUpdate(_Action);
            rotation.OnKeyUpdate();
            if (Input.GetButtonDown("Fire1"))
            {
                AreaStart();
            }
        }
    }

    void AreaStart()
    {
        _areaskill = false;
        Skill_Manager._instance.UsingAreaSkill_Start(_areaOrigin, _areaprefab.transform);
        _areaOrigin = null;
        Destroy(_areaprefab.gameObject);
        _areaprefab = null;
        _skilltype = 0; 
    }
    void AreaUpdate()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane Ground = new Plane(Vector3.up, Vector3.zero);
        if (Ground.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            Vector3 AreaDistance = worldPosition - transform.position; 
            if (AreaDistance.magnitude > 7f) worldPosition = transform.position + AreaDistance.normalized * 7f; // 방향을 구해 범위 7로 제한을 둠
            _areaprefab.transform.position = worldPosition;
        }
        
    }
    void FixedUpdate()
    {
        if (_CurrentState == State.Wait || _CurrentState == State.Gathering || _CurrentState == State.Boss || _isShield) return;
        if (!_Action.GetBool("IsAttack") && _skilltype < 2) { 
            move.UserMove(_Action, _rigid);
            rotation.UserRoation(_rigid);

        }
        
    }
    void skill_motion_end() { _Action.SetBool("Skill", false); _skilltype = 0; }

    void PickUp()
    {
        if (_isItem == null) return;
        if (InventorySystem.instance._MaxSlots <= 0)
        {
            LogSystem._instance.UpdateLog("인벤토리가 부족합니다.");
            return;
        }
        LogSystem._instance.UpdateLog($"{_isItem._name}을 획득 하셨습니다.");

        InventorySystem.instance.Item_add(_isItem);

        Destroy(_isItem.gameObject);

        _isItem = null;
        UpdateEventSystem(false);
    }

    void Step()
    {
        SoundManager._instance.PlaySound(SoundManager.SOUNDTYPE.Base_Walk, 0.5f);
    }



    void UpdateEventSystem(bool b, string str = null, string str2 = null)
    {

        if(_EventSystem == null)
        {
            _EventSystem = TotalUiControl._instance.Create(TotalUiControl.Type.EventSystem);
            _EventSystem.transform.SetAsFirstSibling(); // 맨 위로 가서 Ui와 겹치지 않게 하기 위함
            _script = _EventSystem.GetComponent<EventSystemControl>();
            _script.Init(transform, str, str2);
        }else if (b == true) _script.Chnage_Text(str, str2);
        _script.OnOff(b);
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.CompareTag("Item"))
        {
            if (_isItem == null)
            {
                _isItem = other.gameObject.GetComponent<ItemControl>();
                 UpdateEventSystem(true, $"{_isItem._name}(을/를) 주으려면", "[B]");
            }
        }
    }
       public void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Item")) {
            if (_isItem == null)
            {
                _isItem = other.gameObject.GetComponent<ItemControl>();
                UpdateEventSystem(true, $"{_isItem._name}(을/를) 주으려면", "[B]");
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Item"))
        {
            _isItem = null;
            UpdateEventSystem(false);
            
        }
    }





    public void Hit(float damage) => hit.Hitted(ref _nowHP, _HP, ref isdead, damage, _HitTextPos.transform);
    


}
