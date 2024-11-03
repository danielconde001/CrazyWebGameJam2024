using UnityEngine;

[RequireComponent(typeof(PlayerAim))]
[RequireComponent(typeof(AudioSource))]
public class PlayerShoot : PlayerControl
{
    private PlayerAim playerAim;

    private void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
    }

    private void Update()
    {
        if (!canControl)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (PlayerManager.Instance().CurrentlyEquippedWeapon == null)
        {
            return;
        }
        
        PlayerManager.Instance().CurrentlyEquippedWeapon.Fire();
    }
}
