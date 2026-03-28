extends Node2D

var dragging : bool = false
var drag_start: Vector2
var select_rect = Rect2()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.
	z_index = 200

func _unhandled_input(event: InputEvent) -> void:
	if event.is_action("LeftClick"):
		if event.is_action_pressed("LeftClick"):
			dragging = true
			drag_start = get_global_mouse_position()
		else:
			dragging = false
			for character in get_tree().get_nodes_in_group("PCs"):
				if character is Char:
					if select_rect.has_point(character.global_position.abs()):
						character.selected = true
					else:
						character.selected = false
			
		queue_redraw()
	if dragging:
		var drag_end: Vector2 = get_global_mouse_position()
		select_rect = Rect2(drag_start,drag_end - drag_start).abs()
		queue_redraw()
		
func _draw() -> void:
	if dragging:
		draw_rect(select_rect,Color(0,0.5,1,0.3),true)
		draw_rect(select_rect,Color(0,0.5,1,1),false)
