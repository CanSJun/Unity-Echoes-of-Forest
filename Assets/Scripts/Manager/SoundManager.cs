using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] AudioClip _BMG;

    private AudioSource _Player;

    [SerializeField] AudioClip[] _Effect;

    List<AudioSource> _ltEffectPlayers;

    public enum SOUNDTYPE
    {
        Base_Walk,
        Grass_Walk,
        
        Attack,
        Attack2,
        TargetHit,
        Inventory_OnOff,

        Bear,
        Bear_die,

        One_Hand_Axe,

        NPC,

    }

    static SoundManager _unique;
    public static SoundManager _instance
    {
        get { return _unique; }
    }

    void Awake()
    {
        _unique = this;
        _ltEffectPlayers = new List<AudioSource>();
        _Player = GetComponent<AudioSource>();
    }
    void Start()
    {
        PlayBGM();
    }


    

    void LateUpdate()
    {
        foreach (AudioSource source in _ltEffectPlayers)
        {
            if (source.isPlaying == false)
            {
                _ltEffectPlayers.Remove(source);
                Destroy(source.gameObject);
                break;
            }
        }
        //플레이가 끝난 이펙트는 없엔다
    }


    public bool isPlaying(AudioClip clip)
    {
        foreach(AudioSource source in _ltEffectPlayers)
        {
            if(source.isPlaying && source.clip.name.Equals(clip.name))
            {
                return true;
            }
        }
        return false;
    }
    public void PlayBGM(bool loop = true)
    {
        _Player.clip = _BMG;
        _Player.loop = loop;
        _Player.volume = +MasterControl._Instance.BGM_Volume;
        _Player.Play();
    }


    public void PlaySound( SOUNDTYPE type, float volum = 1.0f, bool loop = false)
    {
        GameObject snd = new GameObject("EffectSound");
        snd.transform.SetParent(transform);
        AudioSource Sound = snd.AddComponent<AudioSource>();
        Sound.clip = _Effect[(int)type];
        Sound.loop = loop;
        Sound.volume = volum + MasterControl._Instance.Effect_Volume;
        Sound.Play();

        _ltEffectPlayers.Add(Sound);
    }

    public void PlaySound_pos(SOUNDTYPE type, Transform pos, float volum = 1.0f, bool loop = false)
    {
        GameObject snd = new GameObject("EffectSound");
        snd.transform.position = pos.position;
        AudioSource Sound = snd.AddComponent<AudioSource>();
        Sound.clip = _Effect[(int)type];
        Sound.loop = loop;
        Sound.volume = volum + MasterControl._Instance.Effect_Volume;

        Sound.rolloffMode = AudioRolloffMode.Linear;
        Sound.spatialBlend = 1f;
        Sound.minDistance = 1f;
        Sound.maxDistance = 10f;
        Sound.Play();

        _ltEffectPlayers.Add(Sound);
    }
    public void PrivatePlaySound(AudioClip clip, Transform pos, float volum = 1.0f, bool loop = false )
    {
        GameObject snd = new GameObject("EffectSound");
        snd.transform.position = pos.position;
        AudioSource Sound = snd.AddComponent<AudioSource>();
        Sound.clip = clip;
        Sound.loop = loop;
        Sound.volume = volum + MasterControl._Instance.Effect_Volume;

        Sound.rolloffMode = AudioRolloffMode.Linear;
        Sound.spatialBlend = 1f;
        Sound.minDistance = 1f;
        Sound.maxDistance = 10f;
        Sound.Play();

        _ltEffectPlayers.Add(Sound);
    }
}