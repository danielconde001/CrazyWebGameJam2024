using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance()
    {
        if (instance == null)
        {
            GameObject musicManager = Instantiate(Resources.Load("Prefabs/Singletons/AudioManager", typeof(GameObject)) as GameObject);
            instance = musicManager.GetComponent<AudioManager>();
        }
        return instance;
    }
    
    [SerializeField] private AudioSource BGMAudioSource;
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private Slider BGMVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private GameObject soundOptionsPanel;
    [SerializeField] private float BGMTransitionDuration;
    [SerializeField] private List<AudioClip> BGMAudioClips;

    private float BGMVolume;
    private float SFXVolume;

    public void OpenSoundOptions()
    {
        soundOptionsPanel.SetActive(true);
    }

    public void CloseSoundOptions()
    {
        soundOptionsPanel.SetActive(false);
    }

    public void UpdateBGMVolume(float _BGMvolume)
    {
        BGMVolume = _BGMvolume;
        BGMAudioSource.volume = BGMVolume;
    }

    public void UpdateSFXVolume(float _SFXvolume)
    {
        SFXVolume = _SFXvolume;
        SFXAudioSource.volume = SFXVolume;
    }

    public void SaveAudioVolumes()
    {
        PlayerPrefs.SetFloat("SAVE_BGMVolume", BGMVolume);
        PlayerPrefs.SetFloat("SAVE_SFXVolume", SFXVolume);
    }

    public void PlayBGM(int pIndex)
    {
        if(BGMAudioSource.isPlaying == true)
        {
            BGMAudioSource.DOFade(0.0f, BGMTransitionDuration / 2.0f).SetEase(Ease.Linear).OnComplete(()=>{
                BGMAudioSource.Stop();
                BGMAudioSource.clip = BGMAudioClips[pIndex];
                BGMAudioSource.Play();
                BGMAudioSource.DOFade(BGMVolume, BGMTransitionDuration / 2.0f).SetEase(Ease.Linear);
            });
        }
        else
        {
            BGMAudioSource.clip = BGMAudioClips[pIndex];
            BGMAudioSource.Play();
            BGMAudioSource.DOFade(BGMVolume, BGMTransitionDuration / 2.0f).SetEase(Ease.Linear);
        }
    }

    public void PlayRandomBGM()
    {
        int rng = Random.Range(0, BGMAudioClips.Count);

        while(BGMAudioClips[rng] == BGMAudioSource.clip)
        {
            rng = Random.Range(0, BGMAudioClips.Count);
        }

        PlayBGM(rng);
    }

    public void PlaySFX(AudioClip audioClip)
    {
        SFXAudioSource.PlayOneShot(audioClip);
    }
    
    private void Awake()
    {
        instance = this;
        
        DontDestroyOnLoad(this.gameObject);

        CloseSoundOptions();
        GetSavedVolumes();
    }

    private void GetSavedVolumes()
    {
        UpdateBGMVolume(PlayerPrefs.GetFloat("SAVE_BGMVolume", 1.0f));
        UpdateSFXVolume(PlayerPrefs.GetFloat("SAVE_SFXVolume", 1.0f));
    }
}
