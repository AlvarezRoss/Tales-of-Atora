using Godot;
using System;

public partial class StatRoll : Control
{
	// Called when the node enters the scene tree for the first time.
	Button rollForStatsButton;
	GridContainer statGrid;
	CanvasLayer canvas;
	Stats characterStats;
	const int AbilitySocoreNum = 6;

	/*Ability Scores Lables*/
	Label strLabel;
	Label wisLabel;
	Label intLabel;
	Label conLabel;
	Label dexLabel;
	Label chrLabel;

	Label[] labels;

	Random randomNumGen; // to be used in random number generation

	/*Abillity core bonus VBoxes*/
	VBoxContainer plusTwoContainer;
	VBoxContainer plusOneContainer;
	/*Ability socre bonus buttons*/
	Button[] plusTwoButtons;
	Button[] plusOneButtons;
	/*These booleans will be used in the button press function for the ability bonuses
	 The idea is to create a flag so we know if we need to disable to enable all other buttons*/
	bool plusTwoPressed = false;
	bool plusOnePressed = false;

	Panel characterPanel; //Panel to be used to display the character options
	Node2D[] characterSprites; // character modles will be added as Node then we access the aniamted sprite with the GetChild function
	Button characterNextButton;
	int spriteIndex;

	Button saveButton;

	public override void _Ready()
	{
		canvas = GetNode<CanvasLayer>("CanvasLayer");
		rollForStatsButton = canvas.GetNode<Button>("RollForStats");
		statGrid = canvas.GetNode<GridContainer>("StatGridContainer");
		rollForStatsButton.ButtonDown += RollForStats;

		characterPanel = canvas.GetNode<Panel>("CharacterPanel");
		characterNextButton = canvas.GetNode<Button>("CharNextButton");
		characterNextButton.ButtonDown += IncreaseSpriteIndex;

		saveButton = canvas.GetNode<Button>("SaveButton");
		saveButton.ButtonDown += SaveButtonClick;

		/*Gets pointer to Ability Score Labels*/
		strLabel = statGrid.GetNode<Panel>("StrValuePanel").GetNode<Label>("StrValueLabel");
		wisLabel = statGrid.GetNode<Panel>("WisValuePanel").GetNode<Label>("WisValueLabel");
		intLabel = statGrid.GetNode<Panel>("IntValuePanel").GetNode<Label>("IntValueLabel");
		dexLabel = statGrid.GetNode<Panel>("DexValuePanel").GetNode<Label>("DexValueLabel");
		conLabel = statGrid.GetNode<Panel>("ConstValuePanel").GetNode<Label>("ConstValueLabel");
		chrLabel = statGrid.GetNode<Panel>("ChrValuePanel").GetNode<Label>("ChrValueLabel");

		labels = new Label[6];

		labels[0] = strLabel;
		labels[1] = wisLabel;
		labels[2] = intLabel;
		labels[3] = conLabel;
		labels[4] = dexLabel;
		labels[5] = chrLabel;
		
		randomNumGen = new Random();

		//Gets poiner to ability socre bonus and adds them into an heap allocated array

		plusTwoContainer = canvas.GetNode<VBoxContainer>("+2VBoxContainer");
		plusTwoButtons = new Button[AbilitySocoreNum];
		for (int i = 0; i<AbilitySocoreNum; i++)
		{
			Button button = plusTwoContainer.GetChild<Button>(i);
			if (button != null)
			{
				plusTwoButtons[i] = button;
				button.Pressed += PlusTwoButtonPress;
				button.Disabled = true;
			}
			else break;
		}
		plusOneContainer = canvas.GetNode<VBoxContainer>("+1VBoxContainer");
		plusOneButtons = new Button[AbilitySocoreNum];
		for (int i = 0; i<AbilitySocoreNum;i++)
		{
			Button button = plusOneContainer.GetChild<Button>(i);
			if (button != null)
			{
				plusOneButtons[i] = button;
				button.Pressed += PlusOneButtonPress;
				button.Disabled = true;
			}
			else break;
		}

		////// Loads Animated Sprites

		PackedScene femHealerScene = ResourceLoader.Load<PackedScene>("res://Scenes/CharacterSprites/female_healer.tscn");
		PackedScene femMageScene = ResourceLoader.Load<PackedScene>("res://Scenes/CharacterSprites/female_mage.tscn");
		PackedScene maleRogueScene = ResourceLoader.Load<PackedScene>("res://Scenes/CharacterSprites/male_rogue.tscn");

		Node2D femHealerNode = femHealerScene.Instantiate<Node2D>();
		Node2D femMageNode = femMageScene.Instantiate<Node2D>();
		Node2D maleRogueNode = maleRogueScene.Instantiate<Node2D>();

		characterSprites = new Node2D[3];
		characterSprites[0] = femHealerNode;
		characterSprites[1] = femMageNode;
		characterSprites[2] = maleRogueNode;
		spriteIndex = 0;

		LoadCharacterSprite(spriteIndex);

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RollForStats()
	{
		for (int i = 0; i < labels.Length; i++)
		{
			labels[i].Text = randomNumGen.Next(4,16).ToString();
		}
		EnableButtons();
	}

	public void PlusTwoButtonPress()
	{
		if (plusTwoPressed)
		{
			for (int i = 0; i<plusTwoButtons.Length;i++)
			{
				if (plusTwoButtons[i].Disabled == false) UpdateScores(i, true, false);
				plusTwoButtons[i].Disabled = false;
			}
			plusTwoPressed = false;
		}
		else
		{
            Button buttonPressed;
            for (int i = 0; i < plusTwoButtons.Length; i++)
            {
                if (plusTwoButtons[i].ButtonPressed)
				{
                    buttonPressed = plusTwoButtons[i];
					UpdateScores(i, true, true);
					continue;
                }
				plusTwoButtons[i].Disabled = true;
            }
			plusTwoPressed = true;
        }
	}
	public void PlusOneButtonPress()
	{
        if (plusOnePressed)
        {
            for (int i = 0; i < plusOneButtons.Length; i++)
            {
				if (plusOneButtons[i].Disabled == false) UpdateScores(i, false, false);
                plusOneButtons[i].Disabled = false;
            }
            plusOnePressed = false;
        }
        else
        {
            Button buttonPressed;
            for (int i = 0; i < plusOneButtons.Length; i++)
            {
                if (plusOneButtons[i].ButtonPressed)
                {
                    buttonPressed = plusOneButtons[i];
					UpdateScores(i, false, true);
                    continue;
                }
                plusOneButtons[i].Disabled = true;
            }
            plusOnePressed = true;
        }
    }

	private void EnableButtons()
	{
		for(int i = 0; i < plusTwoButtons.Length; i++)
		{
			plusTwoButtons[i].Disabled = false;
			plusOneButtons[i].Disabled = false;
		}
	}

	private void UpdateScores(int buttonIndex, bool plusTwo, bool buttonPress)
	{
		int newValue;
		Label label;
		int currentValue;
		int bonus = 0;
		if (plusTwo) bonus = 2;
		else if (!plusTwo) bonus = 1;
		
		
			label = statGrid.GetChild<Panel>(buttonIndex * 2 + 1).GetChild<Label>(0);
			currentValue = label.Text.ToInt();
			if (buttonPress)
			{
                newValue = currentValue + bonus;
            }
			else
			{
				newValue = currentValue - bonus;
			}
			label.Text = newValue.ToString();
		
	}

	private void LoadCharacterSprite(int spriteNum)
	{
		foreach (Node2D child in characterPanel.GetChildren())
		{
			characterPanel.RemoveChild(child);
			child.Hide();
		}
		characterPanel.AddChild(characterSprites[spriteNum]);
        characterSprites[spriteNum].Position = characterPanel.Size / 2;
		characterSprites[spriteNum].Show();
    }

	private void IncreaseSpriteIndex()
	{
		spriteIndex++;
		if (spriteIndex > 2) spriteIndex = 0; 
		LoadCharacterSprite(spriteIndex);
	}

	private void SaveButtonClick()
	{
		PackedScene mainLevel = ResourceLoader.Load<PackedScene>("res://Scenes/main.tscn");
		PackedScene characterScene = ResourceLoader.Load<PackedScene>("res://Scenes/charTest.tscn");
		AnimatedSprite2D characterSprite = (AnimatedSprite2D)characterSprites[spriteIndex];
		characterStats = new Stats(labels[0].Text.ToInt(), labels[1].Text.ToInt(), labels[2].Text.ToInt(), labels[3].Text.ToInt(), labels[4].Text.ToInt(), labels[5].Text.ToInt());
        characterPanel.RemoveChild(characterSprite); // removes the sprite from its parent so it can be loaded to the main scene
		Char newCharacter = characterScene.Instantiate<Char>();
		newCharacter.Init(characterStats, characterSprite.SpriteFrames);
		Main mainLevelInsrance = (Main)mainLevel.Instantiate();
		mainLevelInsrance.AddChild(newCharacter);
		mainLevelInsrance.AddPlayer(newCharacter);
		GetTree().ChangeSceneToNode(mainLevelInsrance);
	}
}
