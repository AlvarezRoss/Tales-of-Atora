using Godot;
using System;
[GlobalClass]
public partial class ClassAndRace : Control
{
	Panel workPanel;
	Button statRollButton;
	Button raceChoiceButton;
	Button appearanceButton;
	public PackedScene[] sprites; // Used to load all the predefined characters and their portraits
	public int[] statValues; // Used to pass to stat class constructor - will be accesed and modified from StatScene.cs
    Stats characterStats;

    PackedScene statRollScene;
    StatScene statRollSceneInstance;
	CanvasLayer statRollCanvas;

	PackedScene raceChoiceScene;
	CanvasLayer raceChoiceCanvas;
	Control raceChoiceInstance;

	PackedScene appearanceScene;
	CanvasLayer appearanceCanvas;
	Control appearanceInstance;

	CanvasLayer[] scenesCanvas;
	// To be modified from the race and class menu
	[Export]
	public Races pickedRace;
	[Export]
	public CharacterClass pickedClass;
	public enum Races
	{
		Human,
		Elf
	};

	public enum CharacterClass
	{
		Fighter,
		Wizard,
		Rogue
	};

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		statRollButton = GetNode<Sprite2D>("StatsSprite").GetNode<Button>("StatsButton");
		statRollButton.ButtonDown += ShowStatRollScene;

		raceChoiceButton = GetNode<Sprite2D>("RaceSprite").GetNode<Button>("RaceButton");
		raceChoiceButton.ButtonDown += ShowRaceChoiceScene;

		appearanceButton = GetNode<Sprite2D>("AppearanceSprite").GetNode<Button>("AppearanceButton");
		appearanceButton.ButtonDown += ShowAppereanceScene;

		workPanel = GetNode<TextureRect>("WorkTexture").GetNode<Panel>("SceneLoadPanel");

        sprites = new PackedScene[10];
        scenesCanvas = new CanvasLayer[3];
		statValues = new int[6];

        LoadCharacterCreatorScenes();
		
		
		
    }

	public void ShowStatRollScene()
	{
		HideScenes();
		statRollCanvas.Show();
	}

    public void ShowRaceChoiceScene()
    {
		HideScenes();
		raceChoiceCanvas.Show();
    }

	public void ShowAppereanceScene()
	{
		HideScenes();
		PortraitSelection scene = workPanel.GetNode<PortraitSelection>("PortraitSelection");
		scene.Init();
		appearanceCanvas.Show();
	}

	private void HideScenes()
	{
		for(int i = 0; i < scenesCanvas.Length; i++)
		{
			scenesCanvas[i].Hide();
		}
	}
    public void LoadCharacterCreatorScenes()
	{
        statRollScene = ResourceLoader.Load<PackedScene>("res://Scenes/StatScene.tscn");
        raceChoiceScene = ResourceLoader.Load<PackedScene>("res://Scenes/RaceChoice.tscn");
		appearanceScene = ResourceLoader.Load<PackedScene>("res://Scenes/portrait_selection.tscn");
		AddUISceneToPanel(statRollScene, ref statRollCanvas);
		AddUISceneToPanel(raceChoiceScene, ref raceChoiceCanvas);
		AddUISceneToPanel(appearanceScene, ref appearanceCanvas);

    }
	
	private void AddUISceneToPanel(PackedScene scene,ref CanvasLayer canvas)
	{
		if (scene == null) return;

		Control instance = scene.Instantiate<Control>();
		canvas = instance.GetChild<CanvasLayer>(0);
        workPanel.AddChild(instance);
        Panel panel = canvas.GetChild<Panel>(0);
		panel.GlobalPosition = workPanel.GlobalPosition;

		for (int i = 0; i< scenesCanvas.Length; i++)
		{
			if (scenesCanvas[i] == null && canvas != null)
			{
				scenesCanvas[i] = canvas;
				scenesCanvas[i].Hide();
				return;
			}
		}

	}

	
}
