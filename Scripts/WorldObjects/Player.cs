using CommunityToolkit.Mvvm.Messaging;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceInvadersClone.Scripts;
using SpaceInvadersClone.Scripts.Global;

public partial class Player : CharacterBody2D, IRecipient<SYSMessages.ProjectileKilled>, IRecipient<SYSMessages.ProjectileHitsPlayer>
{
	public bool playerProyectileActive = false;
	public AnimatedSprite2D PlayerAnims { get; set; }
	public AudioStreamPlayer ProjectileShotSFX { get; set; }
	public AudioStreamPlayer DeathSFX { get; set; }
	public const float Speed = 300.0f;

	public override void _Ready()
	{
		base._Ready();

		PlayerAnims = (AnimatedSprite2D)GetNode("AnimatedSprite2D");
		ProjectileShotSFX = (AudioStreamPlayer)GetNode("PlayerSFXs/ProjectileShot");
		DeathSFX = (AudioStreamPlayer)GetNode("PlayerSFXs/Death");

	}

	public override void _PhysicsProcess(double delta)
	{
		if (PlayerAnims.Animation != "death")
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

			var collision = MoveAndCollide(velocity * (float)delta);
			
			//Checking for firing projectiles
			PlayerFire();
			
			//Check if enemy touches player
			if (collision != null)
			{
				if (collision.GetCollider() is Enemy enemy)
				{
					PlayerDeath();
				}
			}
			
			if (Input.IsActionJustPressed("reset"))
			{
				PlayerDeath();
			}
			
		}
		
	}

	private async void PlayerDeath()
	{
		PlayerAnims.Play("death");
		DeathSFX.Play();
		await GlobalFunctions.Instance.Wait(1.5f,this);
		StrongReferenceMessenger.Default.Send<SYSMessages.PlayerDied>(new(GlobalPosition));
		QueueFree();
	}

	private void PlayerFire()
	{
		if (Input.IsActionJustPressed("fire"))
		{
			if (playerProyectileActive == false)
			{
				PackedScene projectile = ResourceLoader.Load("res://Scenes/WorldObjects/Projectiles/PlayerProjectile.tscn") as PackedScene;
				Projectiles instance = projectile.Instantiate<Projectiles>();
				instance.GlobalPosition = GlobalPosition + new Vector2(0,3);
				instance.isPlayerFire = true;
				GetTree().Root.AddChild(instance);
				ProjectileShotSFX.Play();
				playerProyectileActive = true;

			}
		}
	}

	public void Receive(SYSMessages.ProjectileKilled message)
	{
		if (message.isPlayerProjectile)
		{
			playerProyectileActive = false;
		}
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

	public void Receive(SYSMessages.ProjectileHitsPlayer message)
	{
		PlayerDeath();
	}
}
