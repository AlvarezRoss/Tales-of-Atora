using Godot;
using System;
using System.IO;
using System.Text.Json;

public partial class PortraitSelection : Control
{
	public ClassAndRace parentScene;
	public TextureRect[] portraits;
	public AnimatedSprite2D[] characterSprites;
	public Panel portraitPanel;
	JsonElement root;
	JsonElement humans;
	JsonElement elfs;

	private const int ELFNUM = 5;
	private const int HUMANNUM = 3;
	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		string CharacterJsonDocString = Godot.FileAccess.GetFileAsString("res://Scripts/Static/characters.json");
		JsonDocument characterJson = JsonDocument.Parse(CharacterJsonDocString);
		root = characterJson.RootElement;
		elfs = root.GetProperty("Elfs");
		humans = root.GetProperty("Humans");

	}

	// This function will be called from ClassAndRace class - Return the player's selected character's aniamted
	// sprite2D from the selected portrait
	public AnimatedSprite2D getCharacterSprite()
	{
	
		return null;
	}

	public void Init()
	{
        parentScene = GetTree().Root.GetNode<ClassAndRace>("CharacterCreatorScene");
        portraitPanel = GetNode<CanvasLayer>("CanvasLayer").GetNode<Panel>("SceneLoadPanel").GetNode<Panel>("PortraitPanel");
        if (parentScene.pickedRace == ClassAndRace.Races.Human)
        {
            portraits = new TextureRect[HUMANNUM];
            characterSprites = new AnimatedSprite2D[HUMANNUM];
        }
        else if (parentScene.pickedRace == ClassAndRace.Races.Elf)
        {
            portraits = new TextureRect[ELFNUM];
            characterSprites = new AnimatedSprite2D[ELFNUM];
        }
    }


}
