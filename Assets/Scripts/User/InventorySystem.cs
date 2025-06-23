using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    static InventorySystem _my;
    public static InventorySystem instance {  get { return _my; } }
    [Header("UI")]
    public List<GameObject> _Slots;
    public GameObject _SlotImage;
    public float _StartX = -367f;
    public float _StartY = -369f;
    public float _SlotSize = 100f;
    public float _SlotGap = 100f;
    public float _SlotX = 7f;
    public float _SlotY = 5f;

    [Header("����")]
    public float _MaxSlots;
    [Header("������ ���� ��ġ")]
    [SerializeField] GameObject _DropArea;

    [Header("ETC")]
    [SerializeField] private InputField _Input;
    [SerializeField] private GameObject _Check;
    [SerializeField] private GameObject _HighText;

    void Awake()
    {
        _my = this;
        _MaxSlots = _SlotX * _SlotY;
        for (int y = 0; y < _SlotY; y++)
        {
            for(int x = 0; x < _SlotX; x++)
            {
                GameObject slot = Instantiate(_SlotImage);
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                RectTransform item = slot.transform.GetChild(0).GetComponent<RectTransform>();
                slot.name = $"slot_{y}_{x}";
                slot.transform.SetParent(transform);
                slotRect.localPosition = new Vector3(_StartX + (_SlotX * x) + (_SlotGap * (x + 1)), -(_StartY + (_SlotY * y) + (_SlotGap * (y + 1))), 0);
                slotRect.localScale = Vector3.one;
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _SlotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _SlotSize);
                slot.gameObject.SetActive(true);
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _SlotSize - _SlotSize * 0.3f); 
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _SlotSize - _SlotSize * 0.3f);
                _Slots.Add(slot);
            }
        }
        Invoke("Init", 0.1f);
    }

    public void Init()
    {
        GameData data = MasterControl._Instance.Data;
        if (data != null)
        {

            for (int i = 0; i < 36; i++)
            {
                if (data.SLOTS[i] == null || data.SLOTS[i].Items.Count < 1) { continue; }
                SlotControl slot = SlotControl.LoadSlot(data.SLOTS[i]);
                _Slots[i].GetComponent<SlotControl>()._items = slot._items;
                _Slots[i].GetComponent<SlotControl>()._Icon.sprite = ItemData._instance.ICONS[slot._items.Peek().icon_number];
                _Slots[i].GetComponent<SlotControl>()._default = slot._default;
                _Slots[i].GetComponent<SlotControl>().UpdateSlot(true, _Slots[i].GetComponent<SlotControl>()._Icon.sprite);
            }
        }
    }

    public SlotControl GetSlot(int i) {
        if (_Slots[i].GetComponent<SlotControl>()._items.Count < 1) return null;
        return _Slots[i].GetComponent<SlotControl>();
    }
    public void Item_add(ItemControl item)
    {
        if (_MaxSlots <= 0) { LogSystem._instance.UpdateLog("�κ��丮�� �����մϴ�."); return; }
        // ���� �ش� �������� �� �κ��丮�� �ִ��� ���� üũ!
        for(int i = 0; i < _Slots.Count; i++)
        {
            SlotControl slot = _Slots[i].GetComponent<SlotControl>();
            if (slot.CheckingSlots() == false) continue; // ����ϰ� ���� �ʴٸ� ���� ����ϰ� ���� �������� ���°Ŵ� continue �����ش�
            if(slot.GetItem()._item_type == item._item_type) // ���࿡ ���ٸ�? ���� ������!
            {
                slot.ChangeSlot(item);
                return;
            }
        }
        // ���� �Դٸ� ���� ��ĭ�� ã��!
        for(int i = 0; i < _Slots.Count;i++) {
            SlotControl slot = _Slots[i].GetComponent<SlotControl>();
            if (slot.CheckingSlots() == true) continue; // ������̸� �н�!
            slot.ChangeSlot(item);
            _MaxSlots--; // ���� �����ֱ�!
            return;
        }
    }

    public SlotControl Find_Closed_Slot(Vector3 pos)
    {
        float Closed = 10000f;
        int index = -1;
        for(int i = 0; i < _Slots.Count;i++)
        {
            Vector2 NowSlotPos = _Slots[i].transform.GetChild(1).position;
            float diff = Vector2.Distance(NowSlotPos, pos);
            if(diff < Closed) { Closed = diff; index = i; }
        }
        if(Closed > _SlotSize) return null; // SlotsSize�� ������ ����ٸ�? �ʹ� �ִ�! return!
        return _Slots[index].GetComponent<SlotControl>();

    }


    private ItemControl _dropitem;
    private SlotControl _CurrentSlot;
    public void Swap(SlotControl _Before, Vector3 _After)
    {
        SlotControl _GetPosition = Find_Closed_Slot(_After); // ���� ���콺 ��ġ���� ���� ����� ������ �����´�.

        if(_GetPosition == null)
        {

                RectTransform inventory = transform.GetComponent<RectTransform>();
                if (inventory == null) return;
                if (!RectTransformUtility.RectangleContainsScreenPoint(inventory, Input.mousePosition, null))
                {
                    _dropitem = _Before.GetItem();
                    
                    if (_dropitem._isStackable)
                    {
                    _Input.text = "";
                    _CurrentSlot = _Before;
                    _Check.SetActive(true);
                    }
                    else
                    {
                        GameObject newitem = Instantiate(ItemData._instance.GetPrefeb(_dropitem._prefebnum));
                         
                        
                        newitem.transform.position = _DropArea.transform.position;
                        newitem.layer = LayerMask.NameToLayer("Item");
                        ItemControl current = newitem.GetComponent<ItemControl>();    
                        ItemControl newitemcontrol = ItemDB._instance.GetItem(current._name);
                        if (newitemcontrol != null) current.CopyValue(newitemcontrol);    
                    
                        

                        LogSystem._instance.UpdateLog($"{_Before.GetItem()._name}�� 1�� �����̽��ϴ�");
                        
                        DeleteSlot(_Before);
                        _dropitem = null;
                    }
                    

                }
            return;
        }
        if(_Before == _GetPosition )
        {
            _Before.UpdateSlot(true, _Before.GetItemIcon().sprite); // �ش� ��ġ �ٽ� ���!
            return;
        }

        if (!_GetPosition.CheckingSlots()) // ������� �ƴ϶��?
        {
            Change(_GetPosition, _Before);
        }
        else
        {
            int Count = _Before.GetItemCount();
            ItemControl _item = _Before.GetItem();
            Stack<ItemControl> temp = new Stack<ItemControl>();
            for (int i = 0; i < Count; i++) temp.Push(_item); // ������ ���� �����۵��̴� �׳� Push��!
            _Before.ItemClear(); // �� �����ֱ�
            
            //���� ��ĭ���ٰ� Ÿ�� ������ �������� �Ű��ش�!
            Change(_Before, _GetPosition);

            //���� Ÿ�� �������� �� �����۵��� �־��ش�.
            Count = temp.Count;
            _item = temp.Peek();

            for (int i = 0; i < Count; i++) _GetPosition.PushItem(_item);
            _GetPosition.SetStacktables(_item._isStackable);
            _GetPosition.UpdateSlot(true, temp.Peek()._Icon);
        }

    }

    void Change(SlotControl TargetSlot, SlotControl BeforeSlot)
    {
        int Count = BeforeSlot.GetItemCount();
        ItemControl _item = BeforeSlot.GetItem();

        for (int i = 0; i < Count; i++) { if (TargetSlot != null) TargetSlot.PushItem(_item); }

        if (TargetSlot != null) { 
            TargetSlot.SetStacktables(_item._isStackable);
            TargetSlot.UpdateSlot(true, BeforeSlot.GetItemIcon().sprite);
            
        }
        DeleteSlot(BeforeSlot);
    }






    public void OkButton()
    {
        int Count = int.Parse(_Input.text);
        if (_Input.text is null || Count <= 0)
        {
            GameObject obj = Instantiate(_HighText);
            obj.GetComponentInChildren<TopTextControl>().Change_Text($"����� �Է��� ���ּ���.");
            return;
        }
        else
        {
            if(Count > _CurrentSlot._items.Count)
            {
                GameObject obj = Instantiate(_HighText);
                obj.GetComponentInChildren<TopTextControl>().Change_Text($"���� �������� �����ϴ�.");
                return;
            }
            _Check.SetActive(false);
            
            int CurrentCount = _CurrentSlot._items.Count;
            string item_name = _CurrentSlot.GetItem()._name;
            for (int i = 0; i < Count; i++)
            {
                GameObject newitem = Instantiate(ItemData._instance.GetPrefeb(_dropitem._prefebnum));
                newitem.transform.position = _DropArea.transform.position;
                newitem.layer = LayerMask.NameToLayer("Item");
                if (CurrentCount > 0)
                {
                    _CurrentSlot._items.Pop();
                    CurrentCount--;
                }
            }
            if (CurrentCount == 0) DeleteSlot(_CurrentSlot);
            else _CurrentSlot.UpdateSlot(true, _CurrentSlot.GetItemIcon().sprite);

            LogSystem._instance.UpdateLog($"{item_name}�� {Count}�� �����̽��ϴ�");

            _dropitem = null;
            _CurrentSlot = null;
        }
    }


    void DeleteSlot(SlotControl slot)
    {
        slot.ItemClear();
        slot.SetStacktables(false);
        slot.UpdateSlot(false, slot._default);
    }
    public void NoButton() {
        _Check.SetActive(false);
        _dropitem = null;
        _CurrentSlot = null;
    }
}
