using CommunityToolkit.Mvvm.Messaging;
using Godot;
using System;
using System.Collections.Generic;
using SpaceInvadersClone.Scripts;

public partial class Player : CharacterBody2D
{
	public Timer ReloadTime { get; set; }
	public AudioStreamPlayer ProjectileShotSFX { get; set; }
	public AudioStreamPlayer DeathSFX { get; set; }
	public const float Speed = 300.0f;
	private int invadersLoop = 0;
	public List<AudioStreamPlayer> InvadersMovement = new List<AudioStreamPlayer>();
	
	public override void _Ready()
	{
		base._Ready();
		int i = 1;
		while (GetNode($"EnemyProperties/Movement{i}") != null)
		{
			InvadersMovement.Add((AudioStreamPlayer)GetNode($"EnemyProperties/Movement{i}"));
			i++;
		}
		ProjectileShotSFX = (AudioStreamPlayer)GetNode("PlayerSFXs/ProjectileShot");
		DeathSFX = (AudioStreamPlayer)GetNode("PlayerSFXs/Death");
		ReloadTime = (Timer)GetNode("ReloadTime");

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		//Checking for firing projectiles
		PlayerFire();
	}
	
	private void OnEnemyTimerTimeout()
	{
		StrongReferenceMessenger.Default.Send<SYSMessages.InvadersAnimation>(new(true));
		InvadersMovement[invadersLoop].Play();
		if (invadersLoop == 3)
		{
			invadersLoop = 0;
		}
		else
		{
			invadersLoop++;
		}
	}

	private void PlayerFire()
	{
		if (Input.IsActionJustPressed("fire"))
		{
			if (ReloadTime.TimeLeft == 0)
			{
				PackedScene projectile = ResourceLoader.Load("res://Scenes/WorldObjects/Projectiles/PlayerProjectile.tscn") as PackedScene;
				Area2D instance = projectile.Instantiate<Area2D>();
				instance.GlobalPosition = GlobalPosition + new Vector2(0,3);
				GetTree().Root.GetNode("MainStage").AddChild(instance);
				ProjectileShotSFX.Play();
				ReloadTime.Start();
			}
		}
	}

}
