using Godot;
using System;

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
	}
	
	
}
