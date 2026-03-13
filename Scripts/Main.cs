using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	bool dragging = false;
	Vector2 drag_start = Vector2.Zero;
	Rect2 SelectRect;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (@event.IsAction("LeftClick"))
		{
			if (@event.IsActionPressed("LeftClick"))
			{
				dragging = true;
				drag_start = GetViewport().GetMousePosition();
			}
			else
			{
				if (!dragging)
				{
					Vector2 mousePosition = GetGlobalMousePosition();
					foreach (Char character in GetTree().GetNodesInGroup("PCs"))
					{
					
					}
				}
				dragging = false;
				foreach (Char character in GetTree().GetNodesInGroup("PCs"))
				{
					if (SelectRect.HasPoint(character.GlobalPosition))
					{
						character.selected = true;
					}
					else
					{
						character.selected = false; 
					}
				}
				QueueRedraw();
			}
		}
		if (dragging)
		{
			Vector2 drag_end = GetViewport().GetMousePosition();
			SelectRect = new Rect2(drag_start, drag_end - drag_start).Abs();
			QueueRedraw();
		}
    }

    public override void _Draw()
    {
        base._Draw();
		if (dragging)
		{
			DrawRect(SelectRect, new Color(0, 0.5f, 1, 0.3f), true);
			DrawRect(SelectRect, new Color(0,0.5f,1,1), false);
		}
    }


}
