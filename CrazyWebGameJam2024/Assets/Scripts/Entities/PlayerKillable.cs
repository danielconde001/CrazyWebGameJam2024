public class PlayerKillable : Killable
{
    private bool isKilled = false;
    public override void Death()
    {
        isKilled = true;
        base.Death();
        GameManager.Instance().TimeManipulator.NormalizeTime();
        GameManager.Instance().GameOver();
    }

    public bool IsKilled()
    {
        return isKilled;
    }
}
