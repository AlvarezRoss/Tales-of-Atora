using Godot;
using System;

public partial class Char : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public int speed = 300;
	public Vector2 movementTarget;
	public bool selected; // Bool used to indentify which NPCs are selected and which aren't
	bool moving;
	Area2D clickArea;
	Stats stats;

	Char(Stats stats) // Constructor
	{
		this.stats = stats; 
	}
	public override void _Ready()
	{
		clickArea = GetNode<Area2D>("ClickArea");
		clickArea.InputEvent += _on_click_area_input_event;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		Velocity = Position.DirectionTo(movementTarget) * speed;
		if (Position.DistanceTo(movementTarget) > 10 && moving)
		{
			MoveAndSlide();
		}
		else
		{
			moving = false;
		}
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (@event.IsActionPressed("RightClick") && selected)
		{
			moving = true;
			movementTarget = GetViewport().GetMousePosition();
		}
    }

	private void _on_click_area_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event.IsAction("LeftClick"))
		{
			selected = true;
		}
			
	}
}
