using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using SpaceInvadersClone.Scripts.Global;

namespace SpaceInvadersClone.Scripts.WorldObjects.Enemies;

using Godot;


public partial class EnemyLogistics : Node2D,  IRecipient<SYSMessages.ProjectileHitsAlien>, IRecipient<SYSMessages.EnemyTouchesBorder>, IRecipient<SYSMessages.PlayerDied>,IRecipient<SYSMessages.ProjectileKilled>
{
    private static Random rand = new Random();
    private bool enemyProjectileActive = false;
    private int invadersLoop = 0;
    public ColorRect Loading { get; set; }
    public Timer ReloadStageTimer { get; set; }
    
    public List<AudioStreamPlayer> InvadersMovement = new List<AudioStreamPlayer>();

    public AudioStreamPlayer InvaderKilled { get; set; }
    
    public Timer EnemyTimer { get; set; }

    private static bool stageCleared = false;
    private List<Enemy> Aliens = new List<Enemy>();
    private bool goingRight = true;
    private bool goingDown = false;
    

    
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

        Loading = (ColorRect)GetNode("Loading");
        InvaderKilled = (AudioStreamPlayer)GetNode("InvaderKilled");
        EnemyTimer = (Timer)GetNode("EnemyTimer");

        ReloadStageTimer = (Timer)GetNode("ReloadStageTimer");
        stageCleared = false;
        
        var nodes = GetTree().GetNodesInGroup("Enemies");
        foreach (var node in nodes)
        {
            if (node is Enemy enemy)
            {
                Aliens.Add(enemy);
                enemy.GlobalPosition = new Vector2(enemy.GlobalPosition.X, enemy.GlobalPosition.Y + 20 * GlobalVariables.Instance.Round);
            }
        }
       
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (ReloadStageTimer.TimeLeft > 0 && ReloadStageTimer.TimeLeft < 0.1f)
        {
            Loading.Visible = true;
        }
        EnemyFire();
    }

    private void OnReloadStageTimerTimeout()
    {
        GetTree().ReloadCurrentScene();
    }
    
    //Enemy Timers
    /// <summary>
    /// Makes the enemies move
    /// </summary>
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
        EnemyMovement();
    }

    /// <summary>
    /// Makes the enemies shoot at the player
    /// </summary>
    private void EnemyFire()
    {
        if (Aliens.Any())
        {
            if (!enemyProjectileActive)
            {
                int r = rand.Next(Aliens.Count - 1);
                PackedScene projectile = ResourceLoader.Load("res://Scenes/WorldObjects/Projectiles/EnemyProjectile1.tscn") as PackedScene;
                Projectiles instance = projectile.Instantiate<Projectiles>();
                instance.GlobalPosition = Aliens[r].GlobalPosition;
                instance.isPlayerFire = false;
                GetTree().Root.AddChild(instance);
                enemyProjectileActive = true;
            }
            
        }

    }
    
    private void EnemyMovement()
    {
        if (goingDown)
        {
            
            foreach (var alien in Aliens)
            {
                try
                {
                    alien.GlobalPosition = new Vector2(alien.GlobalPosition.X, alien.GlobalPosition.Y + 20);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            goingDown = false;
        }
        else if (goingRight)
        {
            foreach (var alien in Aliens)
            {
                try
                {
                    alien.GlobalPosition = new Vector2(alien.GlobalPosition.X + 10, alien.GlobalPosition.Y);
                    if (GetViewportRect().Size.X - alien.GlobalPosition.X < 50)
                    {
                        StrongReferenceMessenger.Default.Send<SYSMessages.EnemyTouchesBorder>(new(false));
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
        else if (!goingRight)
        {
            foreach (var alien in Aliens)
            {
                try
                {
                    alien.GlobalPosition = new Vector2(alien.GlobalPosition.X - 10, alien.GlobalPosition.Y);
                    if (alien.GlobalPosition.X < 50)
                    {
                        StrongReferenceMessenger.Default.Send<SYSMessages.EnemyTouchesBorder>(new(true));
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }

    
    //Manage enemies killed
    private async void StageCleared()
    {
        if (stageCleared == false)
        {
            if (GetTree().GetNodesInGroup("Enemies").Count == 0)
            {
                Console.WriteLine("STAGE CLEARED");
                EnemyTimer.Stop();
                GlobalVariables.Instance.Round++;
                stageCleared = true;
                ReloadStageTimer.Start();
              

            }
        }
    }

    public void Receive(SYSMessages.ProjectileHitsAlien message)
    {
        //Registers the sounds playing in a list in order to remove them properly when they start playing over each other

        InvaderKilled.Play();
        EnemyTimer.WaitTime -= 0.0155;
        int alienToDelete = 0;

        
        foreach (Enemy alien in Aliens)
        {
            if (alien.GetRid() == message.enemyId)
            {
                break;
            }
            else
            {
                alienToDelete++;
            }
        }
        Aliens.RemoveAt(alienToDelete);
        StageCleared();
        
    }
    
    public void Receive(SYSMessages.EnemyTouchesBorder message)
    {
        goingDown = true;
        goingRight = message.value;
    }


    public async void Receive(SYSMessages.PlayerDied message)
    {
        await GlobalFunctions.Instance.Wait(1,this);
        var scene = GD.Load<PackedScene>("res://Scenes/WorldObjects/Player.tscn");
        CharacterBody2D instance = (CharacterBody2D)scene.Instantiate();
        instance.GlobalPosition = message.playerPosition;
        GetTree().Root.GetNode("MainStage").AddChild(instance);
        GlobalVariables.Instance.Lives--;
    }

    public void Receive(SYSMessages.ProjectileKilled message)
    {
        if (!message.isPlayerProjectile)
        {
           enemyProjectileActive = false;
        }
    }
}