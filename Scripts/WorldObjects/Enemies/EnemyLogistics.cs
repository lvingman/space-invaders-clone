using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging;

namespace SpaceInvadersClone.Scripts.WorldObjects.Enemies;

using Godot;


public partial class EnemyLogistics : Node2D,  IRecipient<SYSMessages.ProjectileHitsAlien>
{
    private int invadersLoop = 0;
    public List<AudioStreamPlayer> InvadersMovement = new List<AudioStreamPlayer>();
    public Timer EnemyTimer { get; set; }
    private static bool stageCleared;

    
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

    
    
    public override void _Ready()
    {
        base._Ready();
        int i = 1;
        while (GetNode($"Movement{i}") != null)
        {
            InvadersMovement.Add((AudioStreamPlayer)GetNode($"Movement{i}"));
            i++;
        }
        EnemyTimer = (Timer)GetNode("EnemyTimer");
        stageCleared = false;
    }
    
    private void OnEnemyTimerTimeout()
    {
        StrongReferenceMessenger.Default.Send<SYSMessages.InvadersAnimation>(new(true));
        InvadersMovement[invadersLoop].Play();
        if (invadersLoop == 3)
        {
            invadersLoop = 0;
        }
        else
        {
            invadersLoop++;
        }
    }

    //Manage enemies killed
    private void StageCleared()
    {
        if (stageCleared == false)
        {
            if (GetTree().GetNodesInGroup("Enemies").Count == 1)
            {
                Console.WriteLine("STAGE CLEARED");
                EnemyTimer.Stop();
                stageCleared = true;
            }
        }
    }

    public void Receive(SYSMessages.ProjectileHitsAlien message)
    {
        EnemyTimer.WaitTime -= 0.03;
        StageCleared();
    }
}