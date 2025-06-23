using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    static ItemData _my;
    public static ItemData _instance { get { return _my; } }
    private void Awake()
    {
        _my = this;
    }
    [Header("Á¤º¸")]
    public List<GameObject> PREFAB;
    public List<Sprite> ICONS;
    public Sprite DEFAULT;
    public List<AudioClip> AUDIO;
    public List<AudioClip> EQUIPAUDIO;
    public List<GameObject> Wear;

    public GameObject GetPrefeb(int prefabId) => PREFAB[prefabId];
    public GameObject GetPrefebWear(int prefabId) => Wear[prefabId];
}
