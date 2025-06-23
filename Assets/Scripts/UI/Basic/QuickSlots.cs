using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




[Serializable]
public class Save_Load_Quick_Slot
{
    public string _name;
    public float mana;
    public float _duration;
    public float _cool_time;
    public float _current_cool_time;
    public float _UsingTime;
    public int _CurrentSlotType;
    public float _skilltype;
    public int control_number;
    public bool _isUsing;
    public string slot_number;
}

[RequireComponent(typeof(SlotDescriptControl))]
public class QuickSlots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public enum Type
    {
        None,
        item,
        skill
    }
    public string _name = null;
    public float mana;
    public GameObject _CooltimeImage;
    public Image _Cool;
    public float _duration;
    public float _cool_time;
    public float _current_cool_time = 0;
    public float _UsingTime = 0f;
    public Type _CurrentSlotType = Type.None;
    public float _skilltype = 0;
    public Image _icon;
    public object control;
    public int control_number;
    public string slot_number;

    private bool _mouseovercheck = false;
    private SlotDescriptControl _descriptControl;
    [SerializeField] GameObject _descrption_window;

    private bool _isUsing = false;


    public Save_Load_Quick_Slot Save_Item()
    {

        Save_Load_Quick_Slot quick = new Save_Load_Quick_Slot
        {
            _name = this._name,
            mana = this.mana,
            _duration = this._duration,
            _cool_time = this._cool_time,
            _current_cool_time = this._current_cool_time,
            _UsingTime = this._UsingTime,
            _CurrentSlotType = (int)this._CurrentSlotType,
            _skilltype = this._skilltype,
            _isUsing = this._isUsing,
            slot_number = this.slot_number
        };


        switch (control)
        {
            case SlotControl:
                quick.control_number = 1;
                break;
            case Skillslots:
                quick.control_number = 2;
                break;
            default:
                quick.control_number = 0;
                break;
        }
        return quick;
    }

    public static QuickSlots Load_Item(Save_Load_Quick_Slot data)
    {

        QuickSlots quick = new QuickSlots
        {

            _name = data._name,
            mana = data.mana,
            _duration = data._duration,
            _cool_time = data._cool_time,
            _current_cool_time = data._current_cool_time,
            _UsingTime = data._UsingTime,
            _CurrentSlotType = (Type)data._CurrentSlotType,
            _skilltype = data._skilltype,
            _isUsing = data._isUsing,
            control_number = data.control_number,
            slot_number = data.slot_number
        };

        return quick;
    }


    public bool GetUsing() => _isUsing;
    public bool SetUsing(bool b) => _isUsing = b;

    [SerializeField] private Image _BackGround;
    [SerializeField] private Transform _DragImage;
    public Text _coolText;


    void Start() => _BackGround = _DragImage.GetComponent<Image>();

    private void Awake()
    {
        _descriptControl = GetComponent<SlotDescriptControl>();
    }

    public void OnPointerEnter(PointerEventData eventData) { if (this._isUsing == false) return; _mouseovercheck = true; _descrption_window.gameObject.SetActive(true); }
    public void OnPointerExit(PointerEventData eventData) { _mouseovercheck = false; _descriptControl._init = false; _descrption_window.gameObject.SetActive(false); }

    private void Update()
    {
        if (_mouseovercheck && this._isUsing)
        _descriptControl.MouseOver(this, _descrption_window);

    }

    public void Clicked()
    {
        if(_current_cool_time != 0) return;
        _DragImage.gameObject.SetActive(true);
        _DragImage.transform.position = Input.mousePosition;
        _BackGround.sprite = _icon.sprite;
    }


    public void Drag()
    {
        if (_isUsing == false || _current_cool_time != 0) return;
        _DragImage.transform.position = Input.mousePosition;
    }

    public void DragEnd()
    {
        if (_isUsing == false || _current_cool_time != 0) return;
        QuickSlots slots = QuickSlotControl._instance.Find_ClosedQuickSlot(_DragImage.transform.position);
        if (slots == null && this._current_cool_time == 0)
        {
            DeleteSlot();
            return;
        }
        if (this == slots || slots._current_cool_time != 0 || this._current_cool_time != 0) return;

        QuickSlotControl._instance.Swap(this, slots);
    }

    public void MouseUp()
    {
        if (_isUsing == false || _current_cool_time != 0) return;
        _DragImage.gameObject.SetActive(false);
    }
    void DeleteSlot()
    {
        this._name = null;
        this._CurrentSlotType = Type.None;
        this.mana = 0;
        this._cool_time = 0;
        this._duration = 0;
        this._skilltype = 0;
        this._icon.sprite = null;
        this._icon.gameObject.SetActive(false);
        this.SetUsing(false);
        this.control = false;
    }
}
