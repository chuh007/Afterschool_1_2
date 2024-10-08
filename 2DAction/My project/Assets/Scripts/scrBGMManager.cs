using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBGMManager : MonoBehaviour
{
    public bool Fade = false;
    public int BGMIndex = 0;
    [SerializeField] AudioClip[] clip;
    AudioSource audio;
    PlayerInfo playerInfo;
    private void Awake()
    {
        playerInfo = GetComponent<GameManager>().playerInfo;
        audio = GetComponent<AudioSource>();
    }
    public void SetChangeBgm(int Num)
    {
        if (BGMIndex != Num)
        {
            BGMIndex = Num;
            Fade = true;
            StartCoroutine(this.BGMChange());
        }
    }
    public void SetBgmVol(float vol)
    {
        audio.volume = vol;
    }
    IEnumerator BGMChange()
    {
        while (true)
        {
            if (Fade)
            {
                if (audio.volume > 0) { audio.volume -= 0.1f; }
                else
                {
                    if (audio.clip != clip[BGMIndex])
                    {
                        audio.clip = clip[BGMIndex];
                        playerInfo.BGMIndex = BGMIndex;
                        audio.Play();
                    }
                    Fade = false;
                }
            }
            else
            {
                if (audio.volume < playerInfo.BGMVol) { audio.volume += 0.1f; }
                else
                { 
                    audio.volume = playerInfo.BGMVol;
                    break;
                } 
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
