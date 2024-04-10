using Godot;
using System;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;
using SpaceInvadersClone.Scripts.Global;

public partial class HUD : Control, IRecipient<SYSMessages.ProjectileHitsAlien>
{
	public Label Score { get; set; }
	public Label Round { get; set; }
	public Label HighScore { get; set; }
	
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
		HighScore = (Label)GetNode("HighScore");
		
		
		
		Score.Text = "SCORE \n" + (GlobalVariables.Instance.Score);
		Round.Text = "ROUND \n" + (GlobalVariables.Instance.Round);
		HighScore.Text = "HIGH SCORE \n" + (GlobalFunctions.Instance.LoadHighScore());


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateHighScore();
	}

	
	private void UpdateHighScore()
	{
		if (GlobalVariables.Instance.Score > GlobalVariables.Instance.HighScore)
		{
			GlobalVariables.Instance.HighScore = GlobalVariables.Instance.Score;
			GlobalFunctions.Instance.SaveHighScore();
			HighScore.Text = "HIGH SCORE \n" + (GlobalVariables.Instance.HighScore);
		}
	}
	
	public void Receive(SYSMessages.ProjectileHitsAlien message)
	{
		Console.WriteLine("Score up");
		GlobalVariables.Instance.Score += message.points;
		Score.Text = "SCORE \n" + (GlobalVariables.Instance.Score);
	}
}
