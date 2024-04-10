using Godot;
using System;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;

//Enemy projectile 3 can be shot


public partial class Projectiles : Area2D
{

	[Export]
	public bool isPlayerFire { get; set; } = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}


	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isPlayerFire)
		{
			GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y - 10);
		}
		else
		{
			GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y + 10);
		}

		if (GlobalPosition.Y < 0)
		{
			DeleteProjectile();
		}
	}

	public void OnAreaEntered(Area2D defenseBlock)
	{
		StrongReferenceMessenger.Default.Send<SYSMessages.ProjectileHitsDefenseBlock>(new(defenseBlock.GetRid(), isPlayerFire));
		DeleteProjectile();
	}
	public void OnBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D alien && alien.Name != "Player")
		{
			if (alien.Name.ToString().Contains("Octopus"))
			{
				StrongReferenceMessenger.Default.Send<SYSMessages.ProjectileHitsAlien>(new(alien.GetRid(), 10));
				DeleteProjectile();
			}
			else if (alien.Name.ToString().Contains("Crab"))
			{
				StrongReferenceMessenger.Default.Send<SYSMessages.ProjectileHitsAlien>(new(alien.GetRid(), 20));
				DeleteProjectile();
			}
			else if (alien.Name.ToString().Contains("Squid"))
			{
				StrongReferenceMessenger.Default.Send<SYSMessages.ProjectileHitsAlien>(new(alien.GetRid(), 30));
				DeleteProjectile();
			}
			
		}
		
	}

	public void DeleteProjectile()
	{
		StrongReferenceMessenger.Default.Send<SYSMessages.ProjectileKilled>(new (true));
		QueueFree();
	}
	
	
}
