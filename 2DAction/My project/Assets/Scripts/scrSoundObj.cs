using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSoundObj : MonoBehaviour
{
    PlayerInfo playerInfo;
    AudioSource audio;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    public void SetAudioClip(AudioClip clip)
    {
        if (playerInfo == null) playerInfo = GameManager.instance.playerInfo;
        audio.clip = clip;
        audio.volume = playerInfo.SEVol;
        audio.Play();
    }
}
