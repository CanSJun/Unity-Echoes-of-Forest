using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtonManager : MonoBehaviour
{
    [SerializeField] Transform _transform;
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _Setting_transform;


    public void OnSetting()
    {
        _transform.gameObject.SetActive(false);
        _Setting_transform.gameObject.SetActive(true);
        
    }
    public void OnResume() { UserControl._instance._CurrentState = UserControl.State.Idle; _parent.gameObject.SetActive(false); }
    public void OnExit() => Application.Quit();

}
