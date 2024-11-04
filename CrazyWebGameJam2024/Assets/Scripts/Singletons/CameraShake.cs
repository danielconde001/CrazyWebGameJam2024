using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance()
    {
        if (instance == null)
        {
            GameObject cs = new GameObject("CameraShake");
            instance = cs.AddComponent<CameraShake>();
        }
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnShake(float pDuration, float pStrength)
    {
        transform.DOShakePosition(pDuration, pStrength);
        transform.DOShakeRotation(pDuration, pStrength);
    }

    public void ShakeCamera(float pDuration, float pStrength)
    {
        OnShake(pDuration, pStrength);
    }
}
