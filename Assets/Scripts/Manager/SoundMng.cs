using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMng : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioClip;

    [SerializeField]
    AudioSource _audio;

    [SerializeField]
    AudioClip[] effectClip;

    [SerializeField]
    AudioClip[] tileClip;

    [SerializeField]
    AudioClip[] unitClip;

    [SerializeField]
    AudioSource _effect;
    [SerializeField]
    AudioSource _tile;
    [SerializeField]
    AudioSource _unit;

    float audioVolume = .5f;
    float effectVolume = .5f;

    void Start()
    {
        loginBGM();
    }

    public void changeEffectVolume(float vol)
    {
        effectVolume = vol;
        _effect.volume = vol;
        _unit.volume = vol;
    }
    public void changeAudioVolume(float vol)
    {
        audioVolume = vol;
        _audio.volume = vol;
    }

    public void loginBGM()
    {
        _audio.loop = true;
        //_audio.clip = audioClip[0];
        //_audio.volume = audioVolume;
        //_audio.Play();  // 로고 배경 음악
        StartCoroutine(changeTo(audioClip[0]));
    }

    public void roomBGM()
    {
        _audio.loop = true;
        //_audio.clip = audioClip[1];
        //_audio.volume = audioVolume;
        //_audio.Play();
        StartCoroutine(changeTo(audioClip[1]));
    }
    public void waitBGM()
    {
        _audio.Stop();
        _audio.volume = audioVolume;
        _audio.loop = false;
        _audio.clip = audioClip[2];
        _audio.Play();
    }

    public void uiBTClick()
    {
        _effect.clip = effectClip[0];
        _effect.volume = effectVolume;
        _effect.Play();
    }
    public void tileClick()
    {
        _tile.clip = tileClip[0];
        _tile.volume = effectVolume;
        _tile.Play();
    }

    public void unitClick(UNIT unitCode)
    {
        _unit.clip = unitClip[0];
        _unit.volume = effectVolume;
        _unit.Play();
    }

    IEnumerator changeTo(AudioClip clip)
    {
        while (_audio.volume > 0)
        {
            _audio.volume -= 0.05f;
            yield return new WaitForSeconds(0.02f);
        }
        _audio.Stop();
        _audio.clip = clip;
        _audio.Play();
        while (_audio.volume < audioVolume)
        {
            _audio.volume += 0.05f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
