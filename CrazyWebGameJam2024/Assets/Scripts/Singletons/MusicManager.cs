using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance()
    {
        if (instance == null)
        {
            GameObject musicManager = Instantiate(Resources.Load("Prefabs/Singletons/MusicManager", typeof(GameObject)) as GameObject);
            instance = musicManager.GetComponent<MusicManager>();
        }
        return instance;
    }
    
    private AudioSource source;
    
    [SerializeField] private AudioClip[] _audioClips;
    
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic(int pIndex)
    {
        source.clip = _audioClips[pIndex];
        source.Play();
    }
}
