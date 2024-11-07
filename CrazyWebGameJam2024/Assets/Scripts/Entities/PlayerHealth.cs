public class PlayerHealth : Health
{
    public override void DeductHealth(int damage)
    {
        base.DeductHealth(damage);
        HUDManager.Instance().UpdateHealthBarInfo();
        CameraShake.Instance().ShakeCamera(1f, 0.2f);
    }
}
