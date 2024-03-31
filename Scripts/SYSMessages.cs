namespace SpaceInvadersClone.Scripts;
using Godot;

public class SYSMessages
{
    public record InvadersAnimation(bool value);

    public record ProjectileHitsAlien(Rid enemyId);

    public record EnemyTouchesBorder(bool value);
}