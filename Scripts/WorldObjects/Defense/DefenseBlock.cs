using Godot;
using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;

public partial class DefenseBlock : Area2D, IRecipient<SYSMessages.ProjectileHitsDefenseBlock>
{
	// ANIMATION HINT
	// ---
	// TRIANGLE
	// [0], [1], [2] (hit by enemy)
	// [3], [5]
	// [4]
	// (hit by player)
	// ---
	// BLOCK
	// [0], [5],  [6],  [7], [8] (hit by enemies)
	// [1], [9],  [12], [13]
	// [2], [10], [14]
	// [3], [11], 
	// [4]
	// (hit by player)
	//

	private int hitsByPlayer = 0;
	private int hitsByEnemy = 0;
	private List<List<int>> sprites { get; set; }
	public AnimatedSprite2D DefenseBlockAnim { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		AddToGroup("DefenseBlocks");
		DefenseBlockAnim = (AnimatedSprite2D)GetNode("AnimatedSprite2D");
		if (Name.ToString().Contains("Triangle"))
		{
			sprites = new List<List<int>>();
			List<int> row = new List<int>();
			row.Add(0);
			row.Add(1);
			row.Add(2);
			sprites.Add(row);
			
			row = new List<int>();
			row.Add(3);
			row.Add(5);
			sprites.Add(row);
			
			row = new List<int>();
			row.Add(4);
			sprites.Add(row);
		}
		else
		{
			sprites = new List<List<int>>();
			List<int> row = new List<int>();
			row.Add(0);
			row.Add(5);
			row.Add(6);
			row.Add(7);
			row.Add(8);
			sprites.Add(row);
			
			row = new List<int>();
			row.Add(1);
			row.Add(9);
			row.Add(12);
			row.Add(13);
			sprites.Add(row);
			
			row = new List<int>();
			row.Add(2);
			row.Add(10);
			row.Add(14);
			sprites.Add(row);
			
			row = new List<int>();
			row.Add(3);
			row.Add(11);
			sprites.Add(row);
			
			row = new List<int>();
			row.Add(4);
			sprites.Add(row);

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Receive(SYSMessages.ProjectileHitsDefenseBlock message)
	{
		if (message.defenseBlockId == GetRid())
		{
			if (message.isPlayerProjectile)
			{
				hitsByPlayer++;
			}
			else
			{
				hitsByEnemy++;
			}

			try
			{
				DefenseBlockAnim.Frame = sprites[hitsByPlayer][hitsByEnemy];
			}
			catch (Exception e)
			{
				QueueFree();
			}
		}
	}
	
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
	
}
