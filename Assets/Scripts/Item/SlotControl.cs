using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[Serializable]
public class Save_Load_Slot
{
    public List<Save_Load_Item> Items;
    public Sprite _default;
    public Image _Icon;

}
[RequireComponent(typeof(SlotDescriptControl))]
[Serializable]
public class SlotControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Stack<ItemControl> _items = new Stack<ItemControl>(); // ��ø�� �� �� �־�� �ϴ� �ڷᱸ���� ����
    public Image _Icon; // ������ ������
    public Sprite _default; // ��ĭ ������
    public TextMeshProUGUI _text;


    private bool _isUsing = false; // ���� �� ĭ�� ����� �ǰ� �ִ���
    private bool _isStackable = false;

    private bool _mouseovercheck = false;

    private SlotDescriptControl _descriptControl;
    
    [SerializeField] private Image _BackGround;
    [SerializeField] private Transform _DragImage;

    [SerializeField] GameObject _descrption_window;

    public Save_Load_Slot SaveSlot()
    {
        return new Save_Load_Slot
        {
            Items = _items.Select(item => item.Save_Item()).ToList(),
            _default = ItemData._instance.DEFAULT,
            _Icon = this._Icon,
        };
    }


    public static SlotControl LoadSlot(Save_Load_Slot data)
    {
        var slot = new SlotControl
        {
            _default = ItemData._instance.DEFAULT,
            _Icon = data._Icon
            
        };

        foreach (var itemData in data.Items)
        {
            slot._items.Push(ItemControl.Load_Item(itemData));
        }

        return slot;
    }

    void Awake()
    {
        _descriptControl = GetComponent<SlotDescriptControl>();
    }
    void Start() => _BackGround = _DragImage.GetComponent<Image>(); 


    public bool CheckingSlots() => _isUsing;
    public bool CheckingStacktables() => _isStackable;
    public bool SetStacktables(bool b) => _isStackable = b;
    public ItemControl GetItem() => _items.Peek();
    public int GetItemCount() => _items.Count;
    public void PushItem(ItemControl itm) => _items.Push(itm);
    public void ItemClear() => _items.Clear();
    public Image GetItemIcon() => _Icon;


    public void OnPointerEnter(PointerEventData eventData) { if (this._isUsing == false) return; _mouseovercheck = true; _descrption_window.gameObject.SetActive(true); }
    public void OnPointerExit(PointerEventData eventData) { _mouseovercheck = false; _descriptControl._init = false; _descrption_window.gameObject.SetActive(false); }
    //���⼭ init�� ���� ������ ���������� ������ ������Ʈ�� �� �ʿ䰡 ������. ������ ������Ʈ�� �ѹ��� ���ֱ� ���ؼ�


    private void Update()
    {
        if (_mouseovercheck && this._isUsing) _descriptControl.MouseOver(this, _descrption_window);
        
    }

    public void ChangeSlot(ItemControl itm)
    {
        _items.Push(itm);
        _isStackable = itm._isStackable;
        UpdateSlot(true, itm._Icon);


    }



    public void UseItem()
    {
        if (!_isUsing) return;  // ����ִ� �Ÿ� �׳� return
        if (Input.GetMouseButtonDown(1))
        {
            Using();
            return;
        }
        
        _DragImage.gameObject.SetActive(true); // �巡�� �̹����� Ȱ��ȭ �����ش�. �̶� �ش� �������� transform�� �������� ������ ������ ��ü�� �̵��� �ȴ�.
        _DragImage.transform.position = Input.mousePosition;
        _BackGround.sprite = _Icon.sprite;
        UpdateSlot(true, _Icon.sprite);
        _text.text = "";

    }
    public void Using()
    {
        if (!_isUsing) return;


        if (_items.Count == 1)
        {
            //�Ѱ��� ��� ��� �� ����!
            _items.Peek().Using();
            _items.Clear(); // ���� ����
            _isStackable = false;
            UpdateSlot(false, _default);
            return;
        }
        ItemControl currentitem =  _items.Pop();
        currentitem.Using();
        UpdateSlot(_isUsing, _Icon.sprite);
    }

    public void QuickUsing()
    {
  
        if (!_isUsing) return;
        if (_items.Count == 1)
        {
            //�Ѱ��� ��� ��� �� ����!
            _items.Peek().Using();
            _items.Clear(); // ���� ����
            _isStackable = false;
            UpdateSlot(false, _default);
            return;
        }
        else
        {
            _items.Pop().Using();
        }
    }
    public void DragItem()
    {
        if (!_isUsing) return;
        _DragImage.transform.position = Input.mousePosition;
    }
    


    public void DragEnd()
    {
        if (!_isUsing) return;

        QuickSlots slots = QuickSlotControl._instance.Find_ClosedQuickSlot(_DragImage.transform.position);
        if (slots == null)
        {
            InventorySystem.instance.Swap(this, _DragImage.transform.position); // ���� �� ���԰� �巡�� �̹��� �����ǰ� ��ü!
        }
        else
        {
            QuickSlotControl._instance.QuickSlotAdd(this, slots);
        }
    }
    public void Up()
    {
        if (!_isUsing) return;
        _DragImage.gameObject.SetActive(false);
        UpdateSlot(true, _Icon.sprite);
    }


    public void UpdateSlot(bool b, Sprite icon)
    {
        this._isUsing = b;
        this._Icon.sprite = icon;
        if (this._isStackable) { _text.text = _items.Count.ToString(); }
        else _text.text = "";
    }
}
