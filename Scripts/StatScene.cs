using Godot;
using System;

public partial class StatScene : Control
{
    ClassAndRace characterCreatorScene;
    Button rollForStatsButton;
    GridContainer statGrid;
    CanvasLayer canvas;
    Stats characterStats;
    const int AbilitySocoreNum = 6;
    Panel sceneLoadPanel;

    /*Ability Scores Lables*/
    Label strLabel;
    Label wisLabel;
    Label intLabel;
    Label conLabel;
    Label dexLabel;
    Label chrLabel;

    public Label[] labels;

    public enum abilityScoreNames
    {
        str,
        dex,
        wis,
        con,
        intel,
        charis
    };
    Random randomNumGen; // to be used in random number generation

    /*Abillity core bonus VBoxes*/
    VBoxContainer plusTwoContainer;
    VBoxContainer plusOneContainer;
    /*Ability score bonus buttons*/
    Button[] plusTwoButtons;
    Button[] plusOneButtons;
    Action buttonPress;
    /*These booleans will be used in the button press function for the ability bonuses
	 The idea is to create a flag so we know if we need to disable to enable all other buttons*/
    bool plusTwoPressed = false;
    bool plusOnePressed = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        canvas = GetNode<CanvasLayer>("CanvasLayer");
        sceneLoadPanel = canvas.GetNode<Panel>("SceneLoadPanel");
        rollForStatsButton = sceneLoadPanel.GetNode<Button>("RollForStats");
        statGrid = sceneLoadPanel.GetNode<GridContainer>("StatGridContainer");
        rollForStatsButton.ButtonDown += RollForStats;
        characterCreatorScene = GetTree().Root.GetNode<ClassAndRace>("CharacterCreatorScene");
        

        /*Gets pointer to Ability Score Labels*/
        strLabel = statGrid.GetNode<Panel>("StrValuePanel").GetNode<Label>("StrValueLabel");
        wisLabel = statGrid.GetNode<Panel>("WisValuePanel").GetNode<Label>("WisValueLabel");
        intLabel = statGrid.GetNode<Panel>("IntValuePanel").GetNode<Label>("IntValueLabel");
        dexLabel = statGrid.GetNode<Panel>("DexValuePanel").GetNode<Label>("DexValueLabel");
        conLabel = statGrid.GetNode<Panel>("ConstValuePanel").GetNode<Label>("ConstValueLabel");
        chrLabel = statGrid.GetNode<Panel>("ChrValuePanel").GetNode<Label>("ChrValueLabel");

        labels = new Label[6];
        // DO NOT CHANGE THIS ORDER UNLESS YOU ARE 200% SURE
        labels[(int)abilityScoreNames.str] = strLabel;
        labels[(int)abilityScoreNames.dex] = dexLabel;
        labels[(int)abilityScoreNames.wis] = wisLabel;
        labels[(int)abilityScoreNames.con] = conLabel;
        labels[(int)abilityScoreNames.intel] = intLabel;
        labels[(int)abilityScoreNames.charis] = chrLabel;

        randomNumGen = new Random();

        //Gets poiner to ability socre bonus and adds them into an heap allocated array

        plusTwoContainer = sceneLoadPanel.GetNode<VBoxContainer>("+2VBoxContainer");
        plusTwoButtons = new Button[AbilitySocoreNum];
        for (int i = 0; i < AbilitySocoreNum; i++)
        {
            Button button = plusTwoContainer.GetChild<Button>(i);
            if (button != null)
            {
                plusTwoButtons[i] = button;
                button.Pressed += () => ButtonToggle(button.ButtonPressed,button);
                button.Disabled = true;
            }
            else break;
        }
        plusOneContainer = sceneLoadPanel.GetNode<VBoxContainer>("+1VBoxContainer");
        plusOneButtons = new Button[AbilitySocoreNum];
        for (int i = 0; i < AbilitySocoreNum; i++)
        {
            Button button = plusOneContainer.GetChild<Button>(i);
            if (button != null)
            {
                plusOneButtons[i] = button;
                //button.Pressed += PlusOneButtonPress;
                button.Pressed += () => ButtonToggle(button.ButtonPressed, button);
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
            labels[i].Text = randomNumGen.Next(4, 16).ToString();
            characterCreatorScene.statValues[i] = labels[i].Text.ToInt(); // Updates parent scene stat values array
        }
        ResetButtons(); // Resets buttons before renabling them
        EnableButtons();
    }

    private void EnableButtons()
    {
        for (int i = 0; i < plusTwoButtons.Length; i++)
        {
            plusTwoButtons[i].Disabled = false;
            plusOneButtons[i].Disabled = false;
        }
    }

    public void ButtonToggle(bool toggled, Button button)
    {
        VBoxContainer buttonParent = button.GetParent<VBoxContainer>();
        int bonus = 0;
        int buttonIndex = 255;
        if (buttonParent == null) return;

        if (buttonParent == plusOneContainer)
        {
            bonus = 1;
            buttonIndex = getButtonIndex(button, plusOneButtons);
            UpdateButtonStatus(toggled, button, plusOneButtons);
        }
        else if (buttonParent == plusTwoContainer)
        {
            bonus = 2;
            buttonIndex = getButtonIndex(button, plusTwoButtons);
            UpdateButtonStatus(toggled,button, plusTwoButtons);
        }
        
        if (buttonIndex == 255) return;
        UpdateScore(toggled, buttonIndex, bonus);

    }

    private int getButtonIndex(Button button, Button[] buttonArray)
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (button == buttonArray[i]) return i;
        }

        return 255;
    }
    private void UpdateScore(bool toggled,int buttonIndex, int bonus)
    {
        int previousScore = 0;
        int newScore = 0;
        
        previousScore = labels[buttonIndex].Text.ToInt();
        
        if (toggled) newScore = previousScore + bonus;
        else newScore = previousScore - bonus;
        
        labels[buttonIndex].Text = newScore.ToString();
        characterCreatorScene.statValues[buttonIndex] = newScore;
    }
    private void UpdateButtonStatus(bool toggled, Button buttonPressed, Button[]buttonArray)
    {
        for (int i = 0; i < buttonArray.Length;i++)
        {
            if (buttonPressed != buttonArray[i]) buttonArray[i].Disabled = toggled? true:false;
        }
    }

    private void ResetButtons() // Used in the roll stats function
    {
        for (int i = 0; i < AbilitySocoreNum; i++)
        {
            plusOneButtons[i].ButtonPressed = false;
            plusTwoButtons[i].ButtonPressed = false;
        }
    }
}
