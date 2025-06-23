using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class MasterControl : MonoBehaviour
{

    static MasterControl _my;
    public static MasterControl _Instance
    {
        get { return _my; }


    }
    public enum GameState
    {
        none = 0,
        start,
        loading,
        setting,
        end
    }


    [Header("¼³Á¤")]
    public float Sensitive_X;
    public float Sensitive_Y;
    public float BGM_Volume;
    public float Effect_Volume;

    [SerializeField] private skill_data_class skillDB;
    [SerializeField] private Bundle_item_data_Class BundleDB;
    [SerializeField] private Equip_item_data_Class EquipDB;

    public GameObject _obj;
    public Text _Progress;
    public Text _Tip;
    public string[] tip_text;

    GameState _nowState = GameState.none;
    AsyncOperation _loadProc;

    public GameData Data;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _my = this;
        SceneChange("StartScene", GameState.start);
    }

    public skill_data_class GETSKILLDB() => skillDB;
    void Start()
    {

        skillDB = Resources.Load<skill_data_class>("DB/skill_data");
        BundleDB = Resources.Load<Bundle_item_data_Class>("DB/Bundle_item_Reader");
        EquipDB = Resources.Load<Equip_item_data_Class>("DB/Equip_item_Reader");

    }
    public void SceneChange(string SceneName, GameState type)
    {
        _obj.gameObject.SetActive(true);
        _Progress.text = "0 %";
        _nowState = type;
        _Tip.text = tip_text[Random.Range(0, tip_text.Length)];
        _loadProc = SceneManager.LoadSceneAsync(SceneName);
        
    }

    void Update()
    {
        if (_loadProc != null)
        {
            _Progress.text = string.Format("{0:0} %", _loadProc.progress * 100);
            if (!_loadProc.isDone)
            {
                _nowState = GameState.loading;
            }
            else
            {
                _nowState = GameState.end;
                _loadProc = null;
                _obj.gameObject.SetActive(false);
            }
        }
    }
}
