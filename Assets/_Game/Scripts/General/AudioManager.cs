using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using System;

public enum SoundEffect
{
    HOVERBUTTON,
    CONFIRMBUTTON,
    CLICK,
    MENUTRACK,
    TRACK1,
    FIREBALL,
    SWORDSWING,
    BOW,
    ATTACK_NECRO,
    ATTACK_SKELETON,
    ATTACK_GOBLIN,
    ATTACK_ENEMY_ARCHER,
    DEATH_ARCHER,
    DEATH_MAGE,
    DEATH_WARRIOR,
    DEATH_SKELETON,
    DEATH_GOBLIN,
    DEATH_NECRO,
    DEATH_ENEMY_ARCHER,
    WINGAME,
    WINEFFECT,
    LEVELUP,
    GAMEOVER
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    Dictionary<SoundEffect, Sound> SoundEffects;

    [SerializeField] float masterVolume = .5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SoundEffects = new Dictionary<SoundEffect, Sound>();
        List<AudioClip> allSoundClips = Resources.LoadAll("SFX/").Cast<AudioClip>().ToList();
        foreach (var soundName in Enum.GetNames(typeof(SoundEffect)))
        {
            AudioClip[] soundClips = allSoundClips.Where(clip => clip.name.Contains(soundName)).ToArray();
            SoundEffect effect = (SoundEffect)Enum.Parse(typeof(SoundEffect), soundName);
            SoundEffects.Add(effect, new Sound());
            SoundEffects[effect].name = soundName;
            SoundEffects[effect].clips = soundClips;
            SoundEffects[effect].source = gameObject.AddComponent<AudioSource>();
            SoundEffects[effect].source.volume = masterVolume;
        }
    }

    public void ChangeVolume(float v)
    {
        masterVolume = v;
        foreach (var effect in SoundEffects)
        {
            effect.Value.source.volume = v;
            effect.Value.volume = v;
        }
    }

    public float GetVolume()
    {
        return masterVolume;
    }

    public void Play(SoundEffect effect, bool loop = false)
    {
        SoundEffects[effect].source.clip = SoundEffects[effect].clips[UnityEngine.Random.Range(0, SoundEffects[effect].clips.Length)];
        SoundEffects[effect].source.loop = loop;
        SoundEffects[effect].source.volume = masterVolume;
        SoundEffects[effect].source.Play();
    }
    public void Pause(SoundEffect effect)
    {
        SoundEffects[effect].source.volume = Mathf.Lerp(SoundEffects[effect].source.volume, 0, 1f * Time.deltaTime);
        SoundEffects[effect].source.Pause();
    }
    public void Stop(SoundEffect effect)
    {
        SoundEffects[effect].source.volume = Mathf.Lerp(SoundEffects[effect].source.volume, 0, 1f * Time.deltaTime);
        SoundEffects[effect].source.Stop();
    }
}


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}