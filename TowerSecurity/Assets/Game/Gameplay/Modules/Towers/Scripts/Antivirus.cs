public class Antivirus : BaseTower
{
    protected override void HandleAttackingBehaviour()
    {
        if (aimbot.ContainsTargets())
        {
            if (aimbot.TryGetTargetComponent(out Enemy enemy))
            {
                DealDamage(enemy);
            }
        }
    }
}
