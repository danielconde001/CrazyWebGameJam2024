using UnityEngine;

public class PlayerKillable : Killable
{
    [SerializeField] private AudioClip playerDeathSFX;

    private bool isKilled = false;
    public override void Death()
    {
        isKilled = true;
        base.Death();
        AudioManager.Instance().PlaySFX(playerDeathSFX, 0.5f);
        GameManager.Instance().TimeManipulator.NormalizeTime();
        GameManager.Instance().GameOver();
    }

    public bool IsKilled()
    {
        return isKilled;
    }
}
