using Godot;
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;
using SpaceInvadersClone.Scripts.Global;

public partial class Enemy : CharacterBody2D,  IRecipient<SYSMessages.InvadersAnimation>, IRecipient<SYSMessages.ProjectileHitsAlien>
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

	public async void Receive(SYSMessages.ProjectileHitsAlien message)
	{
		if (message.enemyId == GetRid())
		{
			RemoveFromGroup("Enemies");
			MovementAnim.Play("boom");
			await GlobalFunctions.Instance.Wait(0.2f, this);
			Console.WriteLine($"Alien shot: {Name}");
			Console.WriteLine($"Enemies remaining: {GetTree().GetNodesInGroup("Enemies").Count}");
			QueueFree();
		}
	}

	public async Task Wait(float seconds)
	{
		var timer = GetTree().CreateTimer(seconds);
		await ToSignal(timer, "timeout");
	}

}



