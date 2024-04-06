using Godot;
using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts;

public partial class Defense : Node2D
{
	//ATTRIBUTES
	private List<DefenseBlock> DefenseBlocks = new List<DefenseBlock>();
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var nodes = GetTree().GetNodesInGroup("DefenseBlocks");
		foreach (var node in nodes)
		{
			if (node is DefenseBlock defenseBlock)
			{
				DefenseBlocks.Add(defenseBlock);
			}
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
