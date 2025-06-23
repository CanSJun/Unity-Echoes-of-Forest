using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Information_Manager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string _name;
    public string _description;
    public bool _isUsing = false;
    private bool _mouseovercheck = false;
    private SlotDescriptControl _descriptControl;
    [SerializeField] GameObject _descrption_window;

    void Awake() => _descriptControl = GetComponent<SlotDescriptControl>();
    
    public void OnPointerEnter(PointerEventData eventData) { if (this._isUsing == false) return; _mouseovercheck = true; _descrption_window.gameObject.SetActive(true); }
    public void OnPointerExit(PointerEventData eventData) { _mouseovercheck = false;_descriptControl._init = false; _descrption_window.gameObject.SetActive(false); }


    private void Update()
    {
        if (_mouseovercheck && this._isUsing) _descriptControl.MouseOver(this, _descrption_window);

    }

}
