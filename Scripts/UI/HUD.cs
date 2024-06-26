using Godot;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;
using SpaceInvadersClone.Scripts.Global;

public partial class HUD : Control, IRecipient<SYSMessages.ProjectileHitsAlien>
{
	public Label Score { get; set; }
	public Label Round { get; set; }
	public Label Pause { get; set; }
	public Label HighScore { get; set; }
	public List<TextureRect> Lives = new List<TextureRect>();
	public ColorRect GameOverScreen { get; set; }
	public AudioStreamPlayer GameOverTheme { get; set; }
	public AudioStreamPlayer PauseSFX { get; set; }
	private bool gameOver = false;
	
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

	private void PauseGame()
	{
		if (GetTree().Paused == false)
		{
			PauseSFX.Play();
		}
		GetTree().Paused = !GetTree().Paused;
		Pause.Visible = !Pause.Visible;
		
	}

	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Score = (Label)GetNode("Score");
		Round = (Label)GetNode("Round");
		Pause = (Label)GetNode("Invisible/Pause");
		HighScore = (Label)GetNode("HighScore");
		PauseSFX = (AudioStreamPlayer)GetNode("PauseSFX");
		GameOverScreen = (ColorRect)GetNode("GameOverScreen");
		GameOverTheme = (AudioStreamPlayer)GetNode("GameOverScreen/GameOverTheme");

		TextureRect originalLife = GetNode<TextureRect>("Life"); // Make sure the node path is correct
		if (originalLife == null)
		{
			GD.Print("Original life node not found!");
			return;
		}
		
		Lives.Add(originalLife);

		for (int i = 1; i < 3; i++) // Start from 1 since the first life is already added
		{
			TextureRect duplicatedLife = (TextureRect)originalLife.Duplicate();
			AddChild(duplicatedLife); // Adding to the same parent as the script's node
			duplicatedLife.GlobalPosition = new Vector2(Lives[i - 1].GlobalPosition.X + 40, Lives[i - 1].GlobalPosition.Y);
			Lives.Add(duplicatedLife);
		}
		

		Score.Text = "SCORE \n" + (GlobalVariables.Instance.Score);
		Round.Text = "ROUND \n" + (GlobalVariables.Instance.Round);
		HighScore.Text = "HIGH SCORE \n" + (GlobalFunctions.Instance.LoadHighScore());


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateHighScore();
		RemoveLives();
		if (Input.IsActionJustPressed("pause"))
		{
			PauseGame();
		}

		if (gameOver)
		{
			GameOver();
		}
		if (GlobalVariables.Instance.Lives < 0)
		{
			GameOverScreen.Visible = true;
			gameOver = true;
		}


	}

	private void RemoveLives()
	{
		if (Lives.Count > GlobalVariables.Instance.Lives && GlobalVariables.Instance.Lives >= 0)
		{
			Lives[Lives.Count - 1].QueueFree();
			Lives.RemoveAt(Lives.Count - 1);
		}
	}

	private void GameOver()
	{
		GetTree().Paused = true;
		GameOverTheme.Play();
		while (GameOverTheme.Playing)
		{
			//Does nothing
		}

		{
			GetTree().Paused = false;
			PackedScene titleScreen = GD.Load<PackedScene>("res://Scenes/TitleScreen.tscn");
			GetTree().ChangeSceneToPacked(titleScreen);
		}
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
