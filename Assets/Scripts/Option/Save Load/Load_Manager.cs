using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Load_Manager : MonoBehaviour
{

    public void Load()
    {
        string path = Application.dataPath + "/Save.json";
        if (File.Exists(path))
        {
            string contents = File.ReadAllText(path);

            byte[] bytes = System.Convert.FromBase64String(contents);
            string DecodedJson = System.Text.Encoding.UTF8.GetString(bytes);

            

            MasterControl._Instance.Data = JsonUtility.FromJson<GameData>(DecodedJson);
            MasterControl._Instance.SceneChange("GameScene", MasterControl.GameState.start);


        }
        else
        {
            Debug.Log("저장 된 게임이 없습니다.");
        }
    }
}
