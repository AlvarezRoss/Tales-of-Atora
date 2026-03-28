using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	PackedScene initialMap;
	TileMapLayer initialMapInstance;
	Marker2D spawnPoint;

    Char player;
	public override void _Ready()
	{
		initialMap = ResourceLoader.Load<PackedScene>("res://Scenes/map1.tscn");
		initialMapInstance = initialMap.Instantiate<TileMapLayer>();
		this.AddChild(initialMapInstance);
		spawnPoint = initialMapInstance.GetNode<Marker2D>("SpawnPoint");
		player.GlobalPosition = spawnPoint.GlobalPosition;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddPlayer(Char player)
	{
		this.player = player;
		player.ZIndex = 20;
	}

}
