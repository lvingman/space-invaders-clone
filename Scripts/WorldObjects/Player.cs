using CommunityToolkit.Mvvm.Messaging;
using Godot;
using System;
using SpaceInvadersClone.Scripts;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;


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
		StrongReferenceMessenger.Default.
			Send<SYSMessages.InvadersAnimation>(new(true));
	}

}
