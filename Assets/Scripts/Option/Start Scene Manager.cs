using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MasterControl;

public class StartSceneManager : MonoBehaviour
{


    public void OnStart()
    {
        _Instance.Data = null; // 만약을 위해서 null을 넣어 새 시작을 해줌.
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
