using System;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class HUDManager : MonoBehaviour
{
    private static HUDManager instance;
    public static HUDManager Instance()
    {
        if (instance == null)
        {
            GameObject hudManager = Instantiate(Resources.Load("Prefabs/Singletons/HUDManager", typeof(GameObject)) as GameObject);
            instance = hudManager.GetComponent<HUDManager>();
        }
        return instance;
    }

    [SerializeField] private GameObject mainCanvasGameObject;
    private MainCanvas mainCanvas;
    
    [SerializeField] private GameObject dialogueSystemObject;
    private DialogueSystem dialogueSystem;
    public DialogueSystem DialogueSystem { get => dialogueSystem; }
    
    [SerializeField] private GameObject gameOverDisplayerObject;
    private GameOverDisplayer gameOverDisplayerDisplayer;
    public GameOverDisplayer GameOverDisplayerDisplayer { get => gameOverDisplayerDisplayer; }
    
    [SerializeField] private GameObject levelCompleteDisplayerObject;
    private LevelCompleteDisplayer levelCompleteDisplayer;
    public LevelCompleteDisplayer LevelCompleteDisplayer { get => levelCompleteDisplayer; }
    
    [SerializeField] private GameObject audioManagerObject;
    private AudioManager audioManager;
    public AudioManager AudioManager { get => audioManager; }
    
    private void Awake()
    {
        instance = this;
        mainCanvas = mainCanvasGameObject.GetComponent<MainCanvas>();
        dialogueSystem = dialogueSystemObject.GetComponent<DialogueSystem>();
        audioManager = audioManagerObject.GetComponent<AudioManager>();
        gameOverDisplayerDisplayer = gameOverDisplayerObject.GetComponent<GameOverDisplayer>();
        levelCompleteDisplayer = levelCompleteDisplayerObject.GetComponent<LevelCompleteDisplayer>();
    }

    private void Start()
    {
        mainCanvas.HealthBar.value 
            = ((float)PlayerManager.Instance().GetPlayerHealth().GetCurrentHealth() / (float)PlayerManager.Instance().GetPlayerHealth().GetMaxHealth());
    }

    public void UseVignette(bool pUseVignette)
    {
        if (pUseVignette)
        {
            mainCanvas.Vignette.DOFade(1f, 1f).SetUpdate(UpdateType.Normal, true);
        }
        else
        {
            mainCanvas.Vignette.DOFade(0f, 1f).SetUpdate(UpdateType.Normal, true);
        }
    }

    public void UpdateAmmoInfo()
    {
        if (PlayerManager.Instance().CurrentlyEquippedWeapon != null)
        {
            int currentAmmo = PlayerManager.Instance().CurrentlyEquippedWeapon.CurrentMagCapacity;
            int maxAmmo = PlayerManager.Instance().CurrentlyEquippedWeapon.MaxMagCapacity;
            mainCanvas.AmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();

            if (currentAmmo <= 0)
            {
                mainCanvas.AmmoText.color = Color.red;
            }
            else
            {
                mainCanvas.AmmoText.color = Color.white;
            }
        }
        else
        {
            mainCanvas.AmmoText.text =  "--";
        }
    }
    
    public void UpdateSlowMoSliderInfo(float currentSliderValue, float maxSliderValue)
    {
        if (currentSliderValue > 0)
        {
            mainCanvas.SlowMoSlider.gameObject.SetActive(true);
        }
        
        mainCanvas.SlowMoSlider.value = (currentSliderValue/maxSliderValue) ;

        if (currentSliderValue <= 0)
        {
            mainCanvas.SlowMoSlider.gameObject.SetActive(false);
        }
    }
    
    public void UpdateHealthBarInfo()
    {
        if (PlayerManager.Instance().CurrentlyEquippedWeapon != null)
        {
            int currentHealth = PlayerManager.Instance().GetPlayerHealth().GetCurrentHealth();
            int maxHealth = PlayerManager.Instance().GetPlayerHealth().GetMaxHealth();

            mainCanvas.HealthBar.value = ((float)currentHealth / (float)maxHealth);
        }
    }

    public void StartCountdownTimer(float seconds)
    {
        mainCanvas.CountdownTimer.gameObject.SetActive(true);
        mainCanvas.CountdownTimer.StartTime(seconds);
    }
}
