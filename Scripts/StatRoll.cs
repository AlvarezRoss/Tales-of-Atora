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

	public override void _Ready()
	{
		canvas = GetNode<CanvasLayer>("CanvasLayer");
		rollForStatsButton = canvas.GetNode<Button>("RollForStats");
		statGrid = canvas.GetNode<GridContainer>("StatGridContainer");
		rollForStatsButton.ButtonDown += RollForStats;

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

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RollForStats()
	{
		for (int i = 0; i < labels.Length; i++)
		{
			labels[i].Text = randomNumGen.Next(4,13).ToString();
		}
		EnableButtons();
	}

	public void PlusTwoButtonPress()
	{
		if (plusTwoPressed)
		{
			for (int i = 0; i<plusTwoButtons.Length;i++)
			{
				if (plusTwoButtons[i].ButtonPressed == true) UpdateScores(i, true, false);
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
		if (plusTwo)
		{
			Label label = statGrid.GetChild<Panel>(buttonIndex + 3).GetChild<Label>(0);
			int currentValue = label.Text.ToInt();
			if (buttonPress)
			{
                newValue = currentValue + 2;
            }
			else
			{
				newValue = currentValue - 2;
			}
			label.Text = newValue.ToString();
		}
	}
}
