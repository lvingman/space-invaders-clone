using CommunityToolkit.Mvvm.Messaging;
using Godot;
using System;
using System.Collections.Generic;
using SpaceInvadersClone.Scripts;

public partial class Player : CharacterBody2D
{
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

}
