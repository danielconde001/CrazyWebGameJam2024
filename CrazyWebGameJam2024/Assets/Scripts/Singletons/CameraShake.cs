using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance()
    {
        if (instance == null)
        {
            instance = Camera.main.gameObject.AddComponent<CameraShake>();
        }
        return instance;
    }

    private Vector3 defaultPosition = Vector3.zero;
    private Vector3 defaultRotation = Vector3.zero;
    
    private void Awake()
    {
        instance = this;
        defaultPosition = transform.position;
        defaultRotation = transform.localEulerAngles;
    }

    private void OnShake(float pDuration, float pStrength)
    {
        transform.DOShakePosition(pDuration, pStrength).SetUpdate(UpdateType.Normal, true).OnComplete(() => ReturnToDefaultPosition());
        transform.DOShakeRotation(pDuration, pStrength).SetUpdate(UpdateType.Normal, true).OnComplete(() => ReturnToDefaultRotation());
    }
    
    private void ReturnToDefaultPosition()
    {
        DOTween.To(()=> transform.position , x=> transform.position = x, defaultPosition, .1f).SetUpdate(UpdateType.Normal, true);
    }
    
    private void ReturnToDefaultRotation()
    {
        DOTween.To(()=> transform.rotation , x=> transform.rotation = x, defaultRotation, .1f).SetUpdate(UpdateType.Normal, true);
    }
    
    public void ShakeCamera(float pDuration, float pStrength)
    {
        OnShake(pDuration, pStrength);
    }
}
