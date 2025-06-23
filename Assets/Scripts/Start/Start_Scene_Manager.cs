using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Scene_Manager : MonoBehaviour
{




    public void OnStart() => MasterControl._Instance.SceneChange("GameScene", MasterControl.GameState.start);
    
}
