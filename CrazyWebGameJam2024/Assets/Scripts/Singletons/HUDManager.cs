using UnityEngine;
using DG.Tweening;

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

    private void Awake()
    {
        instance = this;
        mainCanvas = mainCanvasGameObject.GetComponent<MainCanvas>();
    }

    [SerializeField] private GameObject mainCanvasGameObject;
    private MainCanvas mainCanvas;
    
    public void UseVignette(bool pUseVignette)
    {
        if (pUseVignette)
        {
            mainCanvas.Vignette.DOFade(1f, 1f);
        }
        else
        {
            mainCanvas.Vignette.DOFade(0f, 1f);
        }
    }
}
