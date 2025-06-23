using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MasterControl;

public class StartSceneManager : MonoBehaviour
{


    public void OnStart()
    {
        _Instance.Data = null; // ������ ���ؼ� null�� �־� �� ������ ����.
        _Instance.SceneChange("GameScene",GameState.start);
   
    }

    public void OnLoad()
    {
        _Instance.SceneChange("GameScene", GameState.start);
        UserControl._instance.Init();
        InventorySystem.instance.Init();

    }

    public void OnSetting() => _Instance.SceneChange("SettingScene",GameState.setting);



    public void OnExit() => Application.Quit();
}
