extends Control

var stat_roll_scene: PackedScene = ResourceLoader.load("res://Scenes/stat_roll.tscn")
var newCharacterButton: Button = null
var quitButton: Button = null
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	newCharacterButton = get_node("CanvasLayer/NewChar/Button")
	quitButton = get_node("CanvasLayer/Quit/Button")


func _on_button_pressed() -> void:
	get_tree().change_scene_to_packed(stat_roll_scene)


func _on_quit_button_pressed() -> void:
	get_tree().quit()
