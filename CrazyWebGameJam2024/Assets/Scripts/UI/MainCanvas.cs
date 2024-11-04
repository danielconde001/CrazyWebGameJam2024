using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private Image vignette;
    
    public Image Vignette
    {
        get => vignette;
    }
}
