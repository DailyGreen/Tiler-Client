using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMng : MonoBehaviour
{
    //=========================
    // 0 : 로그인 배경음
    // 1 : 방 배경음
    // 2 : 게임 준비 배경음
    // 3 : 패배 배경음
    // 4 : 승리 배경음
    [SerializeField]
    AudioClip[] audioClip;

    [SerializeField]
    AudioClip[] ingameClip;

    [SerializeField]
    AudioSource _audio;

    //=========================
    // 0 : UI 버튼 클릭
    // 1 : 새 행동 리스트 효과음
    // 2 : 새 채팅 메세지 효과음
    [SerializeField]
    AudioClip[] effectClip;

    [SerializeField]
    AudioClip[] tileClip;

    [SerializeField]
    AudioClip builtClip;

    [SerializeField]
    AudioClip[] unitClip;

    [SerializeField]
    AudioSource _effect;
    [SerializeField]
    AudioSource _tile;
    [SerializeField]
    AudioSource _unit;

    public float audioVolume = .5f;
    public float effectVolume = .5f;

    void Start()
    {
        loginBGM();
    }

    public void changeEffectVolume(float vol)
    {
        effectVolume = vol;
        _effect.volume = vol;
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
    
    public void ingameBGM()
    {
        StartCoroutine(PlayInGameBGM());
    }

    IEnumerator PlayInGameBGM()
    {
        for (int i = 0; i < ingameClip.Length; i++)
        {
            _audio.Stop();
            _audio.clip = ingameClip[i];
            _audio.volume = audioVolume;
            _audio.Play();
            yield return new WaitForSeconds(ingameClip[i].length - 3f); // 3초 전에 소리 조금씩 줄임
            _audio.volume = audioVolume / 3;
            yield return new WaitForSeconds(1); // 3초 전에 소리 조금씩 줄임
            _audio.volume = audioVolume / 5;
            yield return new WaitForSeconds(1); // 3초 전에 소리 조금씩 줄임
            _audio.volume = 0;
            yield return new WaitForSeconds(1); // 3초 전에 소리 조금씩 줄임
            _audio.volume = audioVolume;
        }
        yield return null;
        StartCoroutine(PlayInGameBGM());
    }

    public void loseBGM()
    {
        StopCoroutine(PlayInGameBGM());
        _audio.clip = audioClip[3];
        _audio.volume = audioVolume;
        _audio.Play();
    }

    public void winBGM()
    {
        StopCoroutine(PlayInGameBGM());
        _audio.clip = audioClip[4];
        _audio.volume = audioVolume;
        _audio.Play();
    }

    public void waitBGM()
    {
        //_audio.Stop();
        //_audio.volume = audioVolume;
        //_audio.loop = false;
        //_audio.clip = audioClip[2];
        //_audio.Play();
        ingameBGM();
    }

    public void uiBTClick()
    {
        _effect.clip = effectClip[0];
        _effect.volume = effectVolume;
        _effect.Play();
    }

    public void newActMsg()
    {
        _effect.clip = effectClip[1];
        _effect.volume = effectVolume;
        _effect.Play();
    }

    public void newChatMsg()
    {
        _effect.clip = effectClip[2];
        _effect.volume = effectVolume;
        _effect.Play();
    }

    public void tileClick()
    {
        _tile.clip = tileClip[0];
        _tile.volume = effectVolume;
        _tile.Play();
    }

    public void builtClick()
    {
        _tile.clip = builtClip;
        _tile.volume = effectVolume;
        _tile.Play();
    }

    public void unitClick(int unitCode)
    {
        _unit.clip = unitClip[(unitCode - (int)UNIT.FOREST_WORKER) % 6];
        _unit.volume = effectVolume;
        _unit.Play();
    }

    public void myTurnEffect()
    {
        _effect.clip = effectClip[3];
        _effect.volume = effectVolume;
        _effect.Play();
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
