using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(PlayerAim))]
[RequireComponent(typeof(AudioSource))]
public class PlayerShoot : PlayerControl
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private AudioClip gunshotClip;
    private PlayerAim playerAim;
    private PlayerTimeManipulation playerTimeManipulation;
    private AudioSource audioSource;

    private void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
        playerTimeManipulation = GetComponent<PlayerTimeManipulation>();
        audioSource = GetComponent<AudioSource>();
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
        if (GameManager.Instance().CurrentlyEquippedWeapon == null)
        {
            return;
        }

        if (GameManager.Instance().CurrentlyEquippedWeapon.CurrentMagCapacity <= 0)
        {
            // Play empty gun click sound
            return;
        }
        
        GameManager.Instance().CurrentlyEquippedWeapon.Fire();
        
        playerTimeManipulation.TimeHiccup();
        ShowMuzzle();

        BulletBehaviour spawnedBullet
            = Instantiate(bullet, playerAim.GetCenterTransform().position, Quaternion.identity).GetComponent<BulletBehaviour>();

        Vector2 newBulletDir = playerAim.GetAimDirection().normalized;
        newBulletDir = Vector2.ClampMagnitude(newBulletDir, 1);

        spawnedBullet.SetBulletDirection(newBulletDir);
    }

    private async void ShowMuzzle()
    {
        muzzle.SetActive(true);
        audioSource.PlayOneShot(gunshotClip, 0.1f);
        await Task.Delay(100);
        muzzle.SetActive(false);
    }
}
