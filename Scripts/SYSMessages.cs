namespace SpaceInvadersClone.Scripts;
using Godot;

public class SYSMessages
{
    public record InvadersAnimation(bool value);

    public record ProjectileHitsAlien(Rid enemyId, int points);

    public record ProjectileHitsPlayer();

    public record EnemyTouchesBorder(bool value);

    public record ProjectileHitsDefenseBlock(Rid defenseBlockId, bool isPlayerProjectile);

    public record ProjectileKilled(bool isPlayerProjectile);

    public record PlayerDied(Vector2 playerPosition, KinematicCollision2D collision = null);

}