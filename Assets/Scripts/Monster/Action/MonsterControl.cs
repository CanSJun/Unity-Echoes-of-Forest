
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class DropItem
{
    public string item_name;
    public float rate;
}
public class MonsterControl : MonoBehaviour
{

    public enum Character
    {
        None = 0,
        Impetuous, // 선공격
        Normal // 후공격
    }
    public enum State
    {
        None = 0,
        Attack,
        Die,
        Sleep,
        Skill,
    }

    [Header("속도")]
   // public float _movespeed = 1.0f;
    public float _attackspeed;
    public float attackdelay;
    [Header("최대 사거리")]
    public float _distance = 1.0f;
    //[Header("유저 충돌위치")]
    //public float _userdistance = 1.0f;
    [Header("기본 정보")]
    public string _Name = "몬스터";
    public float _level = 1;
    public float _exp = 3;
    public float HP = 100;
    public float _damage = 1;
    public int QuestNum;
    public DropItem[] drop_item;
    [Header("보스 전용")]
    public float delay;
    public float Maxdelay;
    [SerializeField] GameObject Effect1;
    [SerializeField] AudioClip SkillSound;
    //[SerializeField] GameObject Effect2;
    [Header("음향 정보")]
    public AudioClip[] _attacksound;
    public AudioClip _diesound;
    public AudioClip _idlesound;
    [SerializeField] GameObject _BossHpBar;
    [Header("기타")]
    [SerializeField] GameObject _HpBar;
    [SerializeField] float _SeeHpTime = 3.0f;
    [SerializeField] GameObject _Text;
    [SerializeField] GameObject _HitTextPos;
    [SerializeField] GameObject _ExpBall;
    [SerializeField] Image _HP;
    [SerializeField] Text _HP_TEXT;

    [Header("COLLIDER")]
    public Transform _AttackArea;
    [SerializeField] Vector3 _AreaSize = new Vector3(1, 1, 1);



    [SerializeField] private Renderer _render;

    public  GameObject _player;
    public  Vector3 _BackPoint;
    public Quaternion _BackRotation;
    HPSCRIPT _HPScript;
    public bool _isDead = false;
    bool _isCheck = false;
    Animator _Action;
    private NavMeshAgent _Agent;
    private float Bcurrent_HP;

    public State _CurrentState = State.None;
    bool _isSound = false;
    public Character _CurrentCharacter = Character.None;
    void Start()
    {
        _Agent = GetComponent<NavMeshAgent>();
        if (QuestNum == 2)
        {
            Bcurrent_HP = HP;
            delay = Maxdelay;
        }
        else
        {
            _HpBar.GetComponentInChildren<Slider>().maxValue = HP; // 체력을 지정 해준다.
            _HpBar.GetComponentInChildren<Slider>().value = HP; // 체력을 지정 해준다.
            _HPScript = GetComponentInChildren<HPSCRIPT>();
            _HPScript._MAX_HP = HP;
            _HPScript._NOW_HP = HP;
            _HPScript.ChangeHpText(HP);
            _HPScript.ChangeInfoText(_Name + "(Lev" + _level + ")");
        }
        _BackPoint = transform.position; // 내가 돌아가야 할 곳 저장
        _BackRotation = transform.rotation; // 내가 바라봐야 할 곳 저장
        _Action = GetComponent<Animator>();
        attackdelay = 0;
    }

    void Update()
    {
        if (_isDead) return;


        if (QuestNum != 2)
        {
            _SeeHpTime -= Time.deltaTime;
            if (_SeeHpTime < 0)
            {
                _HpBar.SetActive(false);
            }
        }

        if (_CurrentState == State.Skill) return;
        if (attackdelay > 0) { attackdelay -= Time.deltaTime; }
        if (_CurrentState == State.Sleep) return; // 자는 중이면 return;

        if(!_isSound && _CurrentState == State.None)
        {
            _isSound = true;
            SoundManager._instance.PrivatePlaySound(_idlesound, transform, 0.5f);
        }
        else if (_isSound && !_isCheck && _CurrentState == State.None)
        {

            StartCoroutine(SoundCheck()); 
        }
    }

    IEnumerator SoundCheck()
    {
        _isCheck = true;
        yield return new WaitForSecondsRealtime(3);
        _isSound = false;
        _isCheck = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, _distance); // 범위의 시각화
    }
#endif


    public void Attack()
    {
        if (_isDead) return;
        _isSound = true;
       // _Agent.isStopped = true;
        SoundManager._instance.PrivatePlaySound(_attacksound[Random.Range(0, _attacksound.Length)], transform, 0.3f);
        Collider[] Targets = Physics.OverlapBox(_AttackArea.position, _AreaSize, _AttackArea.rotation);
        foreach (Collider target in Targets)
        {

            
            if (target.CompareTag("Player"))
            {
                UserControl user = target.GetComponent<UserControl>();
                Vector3 direction = (target.transform.position - transform.position).normalized;
                float point = Vector3.Dot(target.transform.forward, direction);
                if (point < 0)
                {
                    if (user._isShield == true)
                    {
                        SoundManager._instance.PrivatePlaySound(user._nowShield._UseSound, target.transform, 0.3f);
                        break;
                    }
                }
                user.Hit(_damage);
                break;
            }
        }

    }


    public void CheckHitted(float value)
    {
        if (QuestNum == 2) Boss_Hitted(value);
        else Hitted(value);
    }
    public void Hitted(float damage)
    {
        if (_isDead) return;

        GameObject T = Instantiate(_Text);
        T.transform.position = _HitTextPos.transform.position;
        T.GetComponent<TextControl>()._color =  Color.yellow;
        T.GetComponent<TextControl>()._Size = 5;
        T.GetComponent<TextControl>()._Text = damage.ToString();


        if (!_HpBar.gameObject.activeSelf)
        {
            _HpBar.SetActive(true);
        }
        _SeeHpTime = 7.0f;
        if (HP <= damage)
        {

            gameObject.layer = LayerMask.NameToLayer("DeadBody");

            HP = 0;
            _isDead = true;
            _HPScript.ChangeHpText(HP);
            SpawnItem();
            SoundManager._instance.PrivatePlaySound(_diesound, transform,0.5f);
            while(_exp > 10)
            {
                GameObject _ball = Instantiate(_ExpBall);
                _ball.GetComponent<ExpBallControl>().Set_EXP_Ball(transform, 10);
                _exp -= 10;
            }
            if(_exp > 0)
            {
                GameObject _ball = Instantiate(_ExpBall);
                _ball.GetComponent<ExpBallControl>().Set_EXP_Ball(transform, _exp);
            }

            if(QuestNum > 0)
            {
                if (QuestDoumi._instance.CheckQuest(QuestNum))
                {
                    QuestDoumi._instance.Update_Quest(QuestNum, 1);
                    UserControl._instance.Quest[QuestNum]++;
                    QuestDoumi._instance.Quest_Information_Add();
                }
            }
            _Action.SetTrigger("Death");
        }
        else
        {
            HP -= damage;
            _HPScript.ChangeHpText(HP);
        }
    }


    void UpdateHp() => _HP_TEXT.text = $"{Mathf.FloorToInt(Bcurrent_HP)} / {HP}";
    public void Boss_Hitted(float value)
    {
        if (_isDead) return;

        if (Bcurrent_HP <= value)
        {
            _isDead = true;
            gameObject.layer = LayerMask.NameToLayer("DeadBody");
            SpawnItem();
            _HP.fillAmount =0;
            UpdateHp();
            _Action.ResetTrigger("Jump");
            _Action.ResetTrigger("Attack");
            _Action.SetBool("IsWalk", false);
            _Action.SetBool("IsBack", true);
            SoundManager._instance.PrivatePlaySound(_diesound, transform, 0.5f);
            while (_exp > 10)
            {
                GameObject _ball = Instantiate(_ExpBall);
                _ball.GetComponent<ExpBallControl>().Set_EXP_Ball(transform, 10);
                _exp -= 10;
            }
            if (_exp > 0)
            {
                GameObject _ball = Instantiate(_ExpBall);
                _ball.GetComponent<ExpBallControl>().Set_EXP_Ball(transform, _exp);
            }

            if (QuestNum > 0)
            {
                if (QuestDoumi._instance.CheckQuest(QuestNum))
                {
                    QuestDoumi._instance.Update_Quest(QuestNum, 1);
                    UserControl._instance.Quest[QuestNum]++;
                    QuestDoumi._instance.Quest_Information_Add();
                }
            }
            _BossHpBar.SetActive(false);
            _Action.SetTrigger("Die");
        }
        else
        {
            Bcurrent_HP -= value;
            _HP.fillAmount = Bcurrent_HP / HP;
            UpdateHp();
        }

    }
    void MonsterDie() => Destroy(gameObject);


    void SpawnItem()
    {

        if(drop_item.Length > 0)
        {
            foreach(var x in drop_item)
            {

                if (Random.Range(0, 100) <= x.rate)
                {

                    ItemControl item = ItemDB._instance.GetItem(x.item_name);
                    if(item != null)
                    {

                        GameObject newitem = Instantiate(ItemData._instance.GetPrefeb(item._prefebnum));
                        newitem.transform.position = transform.position;
                        newitem.layer = LayerMask.NameToLayer("Item");
                    }
                }
            }

        }
    }


    // BOSS 전용

    public void Boss_Skill_Delay()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            if(delay <= 0 && _CurrentState != State.Skill)
            {

                _CurrentState = State.Skill;

                //switch(Random.Range(0, 2))
                //{
                //   case 0: // 점프
                //        StartCoroutine(JumpSkill());
                //        break;
                //   case 1: 
                //        break;
                //}
                StartCoroutine(JumpSkill());
                delay = Maxdelay;
            }
        }
    }
    
    IEnumerator JumpSkill()
    {
        
        _Action.SetTrigger("Jump");
        yield return new WaitForSeconds(0.3f);
        while (_Action.GetCurrentAnimatorStateInfo(0).IsName("Jump")) yield return null;
        GameObject eff = Instantiate(Effect1);
        eff.transform.SetParent(transform, false);
        SkillAttack(30, SkillSound, new Vector3(5,5,5));

    }



    void SkillAttack(float damage, AudioClip sound, Vector3 size)
    {
        if (_isDead) return;
        _isSound = true;
        
        Collider[] Targets = Physics.OverlapBox(_AttackArea.position, size, _AttackArea.rotation);
        foreach (Collider target in Targets)
        {

            if (target.CompareTag("Player"))
            {
                SoundManager._instance.PrivatePlaySound(sound, target.transform, 0.5f);
                UserControl user = target.GetComponent<UserControl>();
                Vector3 direction = (target.transform.position - transform.position).normalized;
                float point = Vector3.Dot(target.transform.forward, direction);
                if (point < 0)
                {
                    if (user._isShield == true)
                    {
                        SoundManager._instance.PrivatePlaySound(user._nowShield._UseSound, target.transform, 0.3f);
                        break;
                    }
                }
                user.Hit(damage);
                break;
            }
        }

    }
}
