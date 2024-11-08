using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : Health
{
    [SerializeField] private List<AudioClip> playerHurtSFXList;
    
    public override void DeductHealth(int damage)
    {
        base.DeductHealth(damage);
        AudioManager.Instance().PlaySFX(playerHurtSFXList[Random.Range(0, playerHurtSFXList.Count)], 0.5f);
        HUDManager.Instance().UpdateHealthBarInfo();
        CameraShake.Instance().ShakeCamera(1f, 0.2f);
    }
}
