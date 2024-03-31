using Godot;
using System;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;

public partial class Enemy : CharacterBody2D,  IRecipient<SYSMessages.InvadersAnimation>, IRecipient<SYSMessages.ProjectileHitsAlien>, IRecipient<SYSMessages.EnemyTouchesBorder>
{
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



	public static bool goingRight = true;
	public const float Speed = 300.0f;
	
	[Export]
	public static float animSpeed = 0.5f;
	public int enemyType;
	public AnimatedSprite2D MovementAnim { get; set; }
	public override void _Ready()
	{
		base._Ready();
		AddToGroup("Enemies");
		MovementAnim = (AnimatedSprite2D)GetNode("MovementAnim");
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	private void OnMovementAnimFrameChanged()
	{
		if (goingRight)
		{
			if (GetViewportRect().Size.X - GlobalPosition.X < 50)
			{
				StrongReferenceMessenger.Default.Send<SYSMessages.EnemyTouchesBorder>(new(false));
				return;
			}
			GlobalPosition = new Vector2(GlobalPosition.X + 10, GlobalPosition.Y);
			

		}
		else if (!goingRight)
		{
			if (GlobalPosition.X < 50)
			{
				StrongReferenceMessenger.Default.Send<SYSMessages.EnemyTouchesBorder>(new(true));
				return;
			}
			GlobalPosition = new Vector2(GlobalPosition.X - 10, GlobalPosition.Y);
		}
	}

	
	


	public void Receive(SYSMessages.InvadersAnimation message)
	{
		if (MovementAnim.Frame == 1)
		{
			// Reset to the first frame to loop the animation
			MovementAnim.Frame = 0;
			Console.WriteLine("Animation 0");
			return;
		}
		
		MovementAnim.Frame += 1;
		Console.WriteLine("Animation 1");

		// Check if we reached the end of the animation
	
	}

	public void Receive(SYSMessages.ProjectileHitsAlien message)
	{
		if (message.enemyId == GetRid())
		{
			Console.WriteLine($"Alien shot: {Name}");
			Console.WriteLine($"Enemies remaining: {GetTree().GetNodesInGroup("Enemies").Count}");
			QueueFree();
		}
	}

	public void Receive(SYSMessages.EnemyTouchesBorder message)
	{
		GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y + 10);
		goingRight = message.value;
	}
}



