using Godot;
using System;

public partial class ClassAndRace : Control
{
	Panel workPanel;
	Button statRollButton;

    Stats characterStats;

    PackedScene statRollScene;
    StatScene statRollSceneInstance;
	CanvasLayer statRollCanvas;
	


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		statRollButton = GetNode<Sprite2D>("StatsSprite").GetNode<Button>("StatsButton");
		statRollButton.ButtonDown += LoadStatRollScene;
		workPanel = GetNode<TextureRect>("WorkTexture").GetNode<Panel>("SceneLoadPanel");
		LoadCharacterCreatorScenes();
		
		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadStatRollScene()
	{
		statRollCanvas.Show();
	}

	public void LoadCharacterCreatorScenes()
	{
        statRollScene = ResourceLoader.Load<PackedScene>("res://Scenes/StatScene.tscn");
		statRollSceneInstance = statRollScene.Instantiate<StatScene>();
		statRollCanvas = statRollSceneInstance.GetChild<CanvasLayer>(0);
		workPanel.AddChild(statRollSceneInstance);
		Panel statPanel = statRollCanvas.GetChild<Panel>(0);
		statPanel.GlobalPosition = workPanel.GlobalPosition;
		statRollCanvas.Hide();
		

    }
}
