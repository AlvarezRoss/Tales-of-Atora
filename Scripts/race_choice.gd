extends Control

var human_button: Button
var elf_button: Button
var character_creator_menu : ClassAndRace
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.
	human_button = get_node("CanvasLayer/SceneLoadPanel/HumanButtonSprite/Button")
	elf_button = get_node("CanvasLayer/SceneLoadPanel/ElfButtonSprite/Button")
	character_creator_menu = get_tree().root.get_node("CharacterCreatorScene")
	
	


func _on_human_select_button_down() -> void:
	character_creator_menu.pickedRace = 0


func _on_elf_select_button_pressed() -> void:
	character_creator_menu.pickedRace = 1
