using CommunityToolkit.Mvvm.Messaging;
using Godot;
using System;
using System.Collections.Generic;
using SpaceInvadersClone.Scripts;

public partial class Player : CharacterBody2D, IRecipient<SYSMessages.ProjectileKilled>
{
	public bool playerProyectileActive = false;
	public AudioStreamPlayer ProjectileShotSFX { get; set; }
	public AudioStreamPlayer DeathSFX { get; set; }
	public const float Speed = 300.0f;

	public override void _Ready()
	{
		base._Ready();

		ProjectileShotSFX = (AudioStreamPlayer)GetNode("PlayerSFXs/ProjectileShot");
		DeathSFX = (AudioStreamPlayer)GetNode("PlayerSFXs/Death");

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
	


	private void PlayerFire()
	{
		if (Input.IsActionJustPressed("fire"))
		{
			if (playerProyectileActive == false)
			{
				PackedScene projectile = ResourceLoader.Load("res://Scenes/WorldObjects/Projectiles/PlayerProjectile.tscn") as PackedScene;
				Area2D instance = projectile.Instantiate<Area2D>();
				instance.GlobalPosition = GlobalPosition + new Vector2(0,3);
				GetTree().Root.AddChild(instance);
				ProjectileShotSFX.Play();
				playerProyectileActive = true;

			}
		}
	}

	public void Receive(SYSMessages.ProjectileKilled message)
	{
		playerProyectileActive = false;
	}
	
	//MVVM FUNCTIONS
	public override void _EnterTree()   //Lets to listen messages from IRecipient and the type of message emmited
	{
		base._EnterTree();
		StrongReferenceMessenger.Default.RegisterAll(this);
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		StrongReferenceMessenger.Default.UnregisterAll(this); //Lets to unregister messages (For what idk)
	}

}
