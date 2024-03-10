using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public int enemyType;
	public AnimatedSprite2D MovementAnim { get; set; }
	public override void _Ready()
	{
		base._Ready();
		MovementAnim = (AnimatedSprite2D)GetNode("MovementAnim");
		MovementAnim.Play();
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	private void OnMovementAnimFrameChanged()
	{
		GlobalPosition = new Vector2(GlobalPosition.X + 10, GlobalPosition.Y);
	}
	
}



