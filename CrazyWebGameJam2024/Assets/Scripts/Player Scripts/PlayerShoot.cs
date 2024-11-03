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

        if (PlayerHasWeapon() == false)
        {
            return;
        }
        
        else if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        PlayerManager.Instance().CurrentlyEquippedWeapon.Fire();
    }

    private bool PlayerHasWeapon()
    {
        return PlayerManager.Instance().CurrentlyEquippedWeapon != null;
    }
}
