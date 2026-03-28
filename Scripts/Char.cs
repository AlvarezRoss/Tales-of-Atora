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
	Stats stats;
	AnimatedSprite2D animatedSprite;
	SpriteFrames frames;
	Directions movementDirection;
	Directions lastDirection;
	public void Init(Stats stats, SpriteFrames frames)
	{
		this.frames = frames;
		this.stats = stats;
	}
	public override void _Ready()
	{
		clickArea = GetNode<Area2D>("ClickArea");
        clickArea.InputEvent += _on_click_area_input_event;
		animatedSprite = GetNode<AnimatedSprite2D>("CharSprite");
		animatedSprite.SpriteFrames = frames;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.

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

		if (movementDirection == Directions.LeftUp && animatedSprite.Animation != "WalkingLeftUp")
		{
			animatedSprite.Play("WalkingLeftUp");
		}
		else if (movementDirection == Directions.LeftDown && animatedSprite.Animation != "WalkingLeftDown")
		{
			animatedSprite.Play("WalkingLeftDown");
		}
		else if (movementDirection == Directions.Left && animatedSprite.Animation != "WalkingLeft")
		{
			animatedSprite.Play("WalkingLeft");
		}
		else if (movementDirection == Directions.Right && animatedSprite.Animation != "WalkingRight")
		{
			animatedSprite.Play("WalkingRight");
		}
		else if (movementDirection == Directions.RightDown && animatedSprite.Animation != "WalkingRightDown")
		{
			animatedSprite.Play("WalkingRightDown");
		}
		else if (movementDirection == Directions.RightUp && animatedSprite.Animation != "WalkingRightUp")
		{
			animatedSprite.Play("WalkingRightUp");
		}
		else if (movementDirection == Directions.Up && animatedSprite.Animation != "WalkingUp")
		{
			animatedSprite.Play("WalkingUp");
		}
		else if (movementDirection == Directions.Down && animatedSprite.Animation != "WalkingDown")
		{
			animatedSprite.Play("WalkingDown");
		}
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
