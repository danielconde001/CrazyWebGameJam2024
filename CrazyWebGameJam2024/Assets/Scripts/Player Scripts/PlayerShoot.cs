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
        GameManager.Instance().TimeManipulator.TimeHiccup();
        EventsManager.Instance().PlayerSpotted();
    }

    private bool PlayerHasWeapon()
    {
        if (PlayerManager.Instance() == null) return false;
        
        return PlayerManager.Instance().CurrentlyEquippedWeapon != null;
    }
}
