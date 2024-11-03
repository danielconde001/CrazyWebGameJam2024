using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    private static HUDManager instance;
    public static HUDManager Instance()
    {
        if (instance == null)
        {
            GameObject playerManager = new GameObject("HUDManager");
            instance = playerManager.AddComponent<HUDManager>();
        }
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Image vignetteOverlay;

    public void UseVignette(bool pUseVignette)
    {
        if (pUseVignette)
        {
            vignetteOverlay.DOFade(1f, 1f);
        }
        else
        {
            vignetteOverlay.DOFade(0f, 1f);
        }
    }
}
