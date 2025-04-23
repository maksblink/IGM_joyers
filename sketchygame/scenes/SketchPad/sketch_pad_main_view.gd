extends SketchPad



func _on_undo_btn_pressed() -> void:
	undo_action()

func _on_redo_btn_pressed() -> void:
	redo_action()

func _on_clear_btn_pressed() -> void:
	clear_sketch()
