using Godot;
using System;

public partial class ClassAndRace : Control
{
	PackedScene statRollScene;
	Panel workPanel;
	Button statRollButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		statRollButton = GetNode<Sprite2D>("StatsSprite").GetNode<Button>("StatsButton");
		statRollButton.ButtonDown += LoadStatRollScene;
		workPanel = GetNode<TextureRect>("WorkTexture").GetNode<Panel>("SceneLoadPanel");

		statRollScene = ResourceLoader.Load<PackedScene>("res://Scenes/stat_roll.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadStatRollScene()
	{
		StatRoll instance = statRollScene.Instantiate<StatRoll>();
		workPanel.AddChild(instance);
	}
}
