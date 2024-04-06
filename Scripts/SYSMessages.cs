namespace SpaceInvadersClone.Scripts;
using Godot;

public class SYSMessages
{
    public record InvadersAnimation(bool value);

    public record ProjectileHitsAlien(Rid enemyId, int points);

    public record EnemyTouchesBorder(bool value);

    public record ProjectileHitsDefenseBlock(Rid defenseBlockId, bool isPlayerProjectile);

}