extends SubViewport
class_name SketchPad

## Emitted when canvas was cleared
signal clear_canvas();

## Emitted when undo last action
signal undo_draw(mesh: MeshInstance2D);

## Emitted when redo last action
signal redo_draw(mesh: MeshInstance2D);

## Emitted on start drawing
signal drag_start(start_pos: Vector2);

## Emitted on end drawing
signal drag_end(end_pos: Vector2);

## Any node to hold layers in one place
@export var visible_layers: Node;

## Any node to hold unvisible layers in one place
@export var unvisible_layers: Node;

## Node that creates meshes based on given vector array
@export var mesh_tool: MeshTool;

## Node to export mesh as image
@export var export_tool: Node;

var trail: PackedVector2Array = [];
var dragging: bool = false:
	set(value):
		if value == dragging: return;
		
		if value:
			drag_start.emit(trail[0]);
		else:
			drag_end.emit(trail[-1]);
		
		dragging = value;
		drag_start.emit(trail[0])
	get:
		return dragging

var temp_drawing: MeshInstance2D;

## Clears sketch and draw history
func clear_sketch() -> void:
	for child in visible_layers.get_children():
		child.queue_free();
	
	for child in unvisible_layers.get_children():
		child.queue_free();
	
	clear_canvas.emit();


func undo_action() -> MeshInstance2D:
	if visible_layers.get_children().is_empty():
		push_warning("Nothing to undo.");
		return null;
	
	var mesh: Node = visible_layers.get_children().back();
	if mesh is not MeshInstance2D:
		push_error("Visible layers are empty or are not of type MeshInstance2D.");
		return null;
	
	mesh.reparent(unvisible_layers);
	mesh.visible = false;
	
	undo_draw.emit(mesh);
	return mesh;


func redo_action() -> MeshInstance2D:
	if unvisible_layers.get_children().is_empty():
		push_warning("Nothing to undo.");
		return null;
		
	var mesh: Node = unvisible_layers.get_children().back();
	if mesh is not MeshInstance2D:
		push_error("Unvisible layers are empty or are not of type MeshInstance2D.")
		return null;	

	mesh.reparent(visible_layers);
	mesh.visible = true;
	
	redo_draw.emit(mesh);
	return mesh;


func _ready() -> void:
	temp_drawing = _instantiate_temp_mesh();
	add_child(temp_drawing);


func _unhandled_input(event: InputEvent) -> void:
	if event is not InputEventMouse: return;
	
	var mouse_event := event as InputEventMouse;
	if mouse_event.button_mask == MOUSE_BUTTON_MASK_LEFT:
		trail.push_back(mouse_event.position);
		dragging = true;
		
		mesh_tool.create_mesh(trail, temp_drawing);
	else:
		dragging = false;
		
		if trail.size() <= 0:
			return;
		
		if trail.size() >= 1:
			_parse_draw(trail)
		
		_clear_temp_mesh();
		trail.clear();


func _parse_draw(draw_trail: PackedVector2Array) -> void:
	var mesh_node := mesh_tool.create_mesh(draw_trail);
	visible_layers.add_child(mesh_node);


func _parse_erase(draw_trail: PackedVector2Array) -> void:
	# TODO: Implement method
	pass


func _instantiate_temp_mesh() -> MeshInstance2D:
	var mesh := MeshInstance2D.new();
	mesh.mesh = ArrayMesh.new();
	
	return mesh;


func _clear_temp_mesh() -> void:
	temp_drawing.mesh = ArrayMesh.new();


enum {
	Draw,
	Erase,
}
