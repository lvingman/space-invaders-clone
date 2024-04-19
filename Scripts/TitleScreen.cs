using Godot;
using System;
using SpaceInvadersClone.Scripts.Global;

public partial class TitleScreen : Node2D
{
	public AudioStreamPlayer TitleTheme { get; set; }
	public Label HighScore { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TitleTheme = (AudioStreamPlayer)GetNode("TitleTheme");
		HighScore = (Label)GetNode("HighScore");
		HighScore.Text = "HIGH SCORE: " + (GlobalFunctions.Instance.LoadHighScore());
		TitleTheme.Play();
		GlobalVariables.Instance.ResetVariables();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			PackedScene mainStage = GD.Load<PackedScene>("res://Scenes/MainStage.tscn");
			GetTree().ChangeSceneToPacked(mainStage);
		}
	}
}
