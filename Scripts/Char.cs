using Godot;
using System;
[GlobalClass]
public partial class Char : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public int speed = 300;
	public Vector2 movementTarget;
	[Export]
	public bool selected; // Bool used to indentify which NPCs are selected and which aren't
	bool moving;
	Area2D clickArea;
	Stats? stats;
	AnimatedSprite2D animatedSprite;
	SpriteFrames frames;
	Directions movementDirection;
	Directions lastDirection;
	public void Init(Stats? stats, SpriteFrames frames)
	{
		// TODO - Default for testing
		this.frames = frames;
		this.stats = stats;
	}
	public override void _Ready()
	{
		clickArea = GetNode<Area2D>("ClickArea");
        clickArea.InputEvent += _on_click_area_input_event;
		Init(null, null);
		animatedSprite = GetNode<AnimatedSprite2D>("CharSprite");
		animatedSprite.SpriteFrames = frames;
    }

	

	public enum Directions
	{
		Idle,
		Up,
		RightUp,
		LeftUp,
		Right,
		Left,
		Down,
		LeftDown,
		RightDown
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		Velocity = Position.DirectionTo(movementTarget) * speed;
		if (Position.DistanceTo(movementTarget) > 20 && moving)
		{
			lastDirection = movementDirection;
			MoveAndSlide();
			
		}
		else
		{
			movementDirection = Directions.Idle;
			moving = false;
			
		}

        Animation();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (@event.IsActionPressed("RightClick") && selected)
		{
			moving = true;
			movementTarget = GetGlobalMousePosition();
            movementDirection = GetDirection(); // We do it here to set the movement direction once
        }
    }

	private void _on_click_area_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event.IsAction("LeftClick"))
		{
			selected = true;
		}
			
	}

	public virtual void Animation()
	{
		if (movementDirection == Directions.Idle) IdleAnimations();

		// TODO - WALKING ANIMATIONS
	}

	private Directions GetDirection()
	{
		if (GlobalPosition.X > movementTarget.X + 40)
		{
			if (GlobalPosition.Y > movementTarget.Y + 40)
			{
				return Directions.LeftUp;
			}
			else if (GlobalPosition.Y < movementTarget.Y - 40)
			{
				return Directions.LeftDown;
			}
			else return Directions.Left;
		}
		else if (GlobalPosition.X < movementTarget.X - 40)
		{
			if (GlobalPosition.Y > movementTarget.Y + 40)
			{
				return Directions.RightUp;
			}
			else if (GlobalPosition.Y < movementTarget.Y - 40)
			{
				return Directions.RightDown;
			}
			else return Directions.Right;
		}
		else if (GlobalPosition.Y > movementTarget.Y) return Directions.Up;
		else if (GlobalPosition.Y < movementTarget.Y) return Directions.Down;

		return Directions.Idle;
	}

	private void IdleAnimations()
	{
		switch(lastDirection)
		{
			case Directions.LeftUp:
				animatedSprite.Play("IdleLeftUp");
				break;
			case Directions.LeftDown:
				animatedSprite.Play("IdleLeftDown");
				break;
			case Directions.Left:
				animatedSprite.Play("IdleLeft");
				break;
			case Directions.RightUp:
				animatedSprite.Play("IdleRightUp");
				break;
			case Directions.Right:
				animatedSprite.Play("IdleRight");
				break;
			case Directions.RightDown:
				animatedSprite.Play("IdleRightDown");
				break;
			case Directions.Up:
				animatedSprite.Play("IdleUp");
				break;
			case Directions.Down:
				animatedSprite.Play("IdleDown");
				break;
			default:
				animatedSprite.Play("IdleDown");
				return;
		}
	}
}
