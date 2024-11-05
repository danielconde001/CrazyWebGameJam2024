public class PlayerKillable : Killable
{
    private bool isKilled = false;
    public override void Death()
    {
        isKilled = true;
        base.Death();
    }

    public bool IsKilled()
    {
        return isKilled;
    }
}
