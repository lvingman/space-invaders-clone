using Godot;
using System;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;
using SpaceInvadersClone.Scripts.Global;

public partial class HUD : Control, IRecipient<SYSMessages.ProjectileHitsAlien>
{
	public Label Score { get; set; }
	public Label Round { get; set; }
	
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

	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Score = (Label)GetNode("Score");
		Round = (Label)GetNode("Round");

		Score.Text = "SCORE: \n" + (GlobalVariables.Score);
		Round.Text = "ROUND: \n" + (GlobalVariables.Round);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Receive(SYSMessages.ProjectileHitsAlien message)
	{
		Console.WriteLine("Score up");
		GlobalVariables.Score += message.points;
		Score.Text = "SCORE: \n" + (GlobalVariables.Score);
	}
}
