using Godot;
using System;
using SpaceInvadersClone.Scripts.Global;

public partial class BoilerNode : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!HasNode("MainStage"))
		{
			ReloadMainStage();
		}
	}

	private async void ReloadMainStage()
	{
		await GlobalFunctions.Instance.Wait(0.2f, GetTree().Root.GetNode("."));
		PackedScene stage = ResourceLoader.Load("res://Scenes/MainStage.tscn") as PackedScene;
		Node2D instance = stage.Instantiate<Node2D>();
		GetTree().Root.AddChild(instance);
	}
	
}
