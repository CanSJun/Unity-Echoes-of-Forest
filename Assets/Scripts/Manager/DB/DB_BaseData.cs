using UnityEngine;




public class DB_BaseData : ScriptableObject
{
    [Header("URL")]
    [SerializeField] public string url;
    [Header("Sheet Name")]
    [SerializeField] public string SheetName;
    [Header("Read")]
    [SerializeField] public int Start;

}

