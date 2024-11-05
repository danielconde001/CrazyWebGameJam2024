using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private Image vignette;
    
    public Image Vignette
    {
        get => vignette;
    }

    [SerializeField] private TextMeshProUGUI ammoText;
    public TextMeshProUGUI AmmoText { get => ammoText; }
    
    [SerializeField] private Slider slowMoSlider;
    public Slider SlowMoSlider { get => slowMoSlider; }
}
