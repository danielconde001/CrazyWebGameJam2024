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

    public void UpdateBGMVolume()
    {
        BGMVolume = BGMVolumeSlider.value;
        BGMAudioSource.volume = BGMVolume;
    }

    public void UpdateSFXVolume()
    {
        SFXVolume = SFXVolumeSlider.value;
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
        PlaySFX(audioClip, 1.0f);
    }

    public void PlaySFX(AudioClip audioClip, float volumeScale)
    {
        SFXAudioSource.PlayOneShot(audioClip, volumeScale);
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
        BGMVolumeSlider.value = PlayerPrefs.GetFloat("SAVE_BGMVolume", 0.25f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SAVE_SFXVolume", 0.5f);

        UpdateBGMVolume();
        UpdateSFXVolume();
    }
}
