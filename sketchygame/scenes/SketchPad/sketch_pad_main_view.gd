extends SketchPad

func _on_undo_btn_pressed() -> void:
	undo_action()


func _on_redo_btn_pressed() -> void:
	redo_action()


func _on_clear_btn_pressed() -> void:
	clear_sketch()


func _on_export_image_to_bmp() -> void:
	var _export_path = export_tool.ExportAsBitMap(visible_layers, size)
	

func _on_btn_draw_pressed() -> void:
	action = SketchAction.Draw


func _on_btn_erase_pressed() -> void:
	action = SketchAction.Erase