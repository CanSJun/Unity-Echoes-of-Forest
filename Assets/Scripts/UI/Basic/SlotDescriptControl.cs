using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class SlotDescriptControl : MonoBehaviour
{


    [SerializeField] Text _toptext;
    [SerializeField] Text _bodytext;

    private GameObject _window;
    private RectTransform _Rwindow;
    public bool _init = false;
    private Vector2 mousePos;
    private Vector2 calPos;

    float minX;
    float maxX;
    float minY;
    float maxY;

    private bool sizecalcheck = false;
    public void MouseOver(object script, GameObject window)
    {
        _window = window;
         _Rwindow = _window.transform as RectTransform;

        UpdateDescriptionPosition(); // 위치 업데이트
        if (_init) return;

        if (script is SlotControl slot) InformationUpdate(slot.GetItem()._name, slot.GetItem()._description);
        else if(script is Skillslots skillslot)
        {
            Skill skill = skillslot.GetSkill();
            if (skill != null)
            {
                InformationUpdate(skill.skill_name, skill.description);
            }
        }else if(script is Information_Manager information)
        {
            if(information._isUsing)InformationUpdate(information._name, information._description);
        }
        else if(script is QuickSlots quickslot)
        {
            if (quickslot.control is SlotControl item_slot)
            {
                InformationUpdate(item_slot.GetItem()._name, item_slot.GetItem()._description);
            }else if(quickslot.control is Skillslots skill_slot)
            {
                Skill skill = skill_slot.GetSkill();
                if (skill != null)
                {
                    InformationUpdate(skill.skill_name, skill.description);
                }
            }
        }
    }


    void InformationUpdate(string Toptext, string BodyText)
    {
        _toptext.text = Toptext;
        _bodytext.text = BodyText;
    }


  
    void UpdateDescriptionPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _Rwindow.parent as RectTransform,
                Input.mousePosition,
                null,
                out mousePos);
        calPos = mousePos - _Rwindow.sizeDelta / 2;
        CheckTransform(); 
        _Rwindow.anchoredPosition = calPos;
    }


    void CheckTransform()
    {

        RectTransform canvas = _Rwindow.parent as RectTransform;
        minX = -canvas.rect.width / 2 + _Rwindow.sizeDelta.x / 2;
        maxX = canvas.rect.width / 2 - _Rwindow.sizeDelta.x / 2;
        minY = -canvas.rect.height / 2 + _Rwindow.sizeDelta.y / 2;
        maxY = canvas.rect.height / 2 - _Rwindow.sizeDelta.y / 2;

        if (calPos.x < minX)
        {
            calPos.x = minX;
        }
        else if (calPos.x > maxX)
        {
            calPos.x = maxX;
        }

        if (calPos.y < minY)
        {
            calPos.y = minY;
        }
        else if (calPos.y > maxY)
        {
            calPos.y = maxY;
        }

    }
}
