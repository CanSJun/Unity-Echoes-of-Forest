using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    static SkillData _my;
    public static SkillData _instance { get { return _my; } }

    private void Awake()
    {
        _my = this;
    }

    [Header("Á¤º¸")]
    public List<Sprite> ICONS;
    public List<GameObject> PreFab;
    public List<AudioClip> Sound;
    public List<GameObject> Effect_PreFab;
    



}
