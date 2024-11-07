public class EnemyKillable : Killable
{
    public override void Death()
    {
        base.Death();
        GameManager.Instance().AddKillCount();
    }
}
