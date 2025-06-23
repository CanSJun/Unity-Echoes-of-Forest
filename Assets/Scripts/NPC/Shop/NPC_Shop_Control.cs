using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Shop_Control : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _parentTransform;

    static NPC_Shop_Control _my;
    public static NPC_Shop_Control _instance { get { return _my; } }
    private void Awake()
    {
        _my = this;
    }
    private void Start()
    {
        UpdateState(UserControl._instance._money);
    }

    [SerializeField] Text _gold;


    public void Create(ItemControl item)
    {
        GameObject obj = Instantiate(_prefab);
        obj.transform.SetParent(_parentTransform);
        obj.GetComponent<Shop_Items_Manager>().Create_Shop_Item(item);

    }

    public void UpdateState(int value) => _gold.text = string.Format("¼ÒÁö °ñµå : {0} Gold", value);
}
