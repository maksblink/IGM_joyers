extends Node
class_name MeshTool

func create_mesh(vertices: PackedVector2Array, mesh_node: MeshInstance2D = null) -> MeshInstance2D:
	if mesh_node == null:
		mesh_node = MeshInstance2D.new();
	var indices: PackedInt32Array = [];
	
	indices = range(vertices.size());
	
	var surface_arr := [];
	surface_arr.resize(Mesh.ARRAY_MAX);
	surface_arr[Mesh.ARRAY_VERTEX] = vertices;
	surface_arr[Mesh.ARRAY_INDEX] = indices;
	
	var mesh := ArrayMesh.new();
	
	if vertices.size() <= 1:
		mesh.add_surface_from_arrays(Mesh.PRIMITIVE_POINTS, 
		surface_arr);
	else:
		mesh.add_surface_from_arrays(Mesh.PRIMITIVE_LINE_STRIP,
		surface_arr);
	
	mesh_node.mesh = mesh;
	return mesh_node;
	
	
func delete_mesh(point: Vector2, mesh_nodes: Array) ->  MeshInstance2D:
	if mesh_nodes.is_empty():
		return null;
		
	var crossed_mesh: MeshInstance2D = null;
	
	for mesh in mesh_nodes:
		# Gets vertex array from surface=0
		var vertices: PackedVector2Array = mesh.mesh.surface_get_arrays(0)[Mesh.ARRAY_VERTEX];
		
		if vertices.is_empty():
			continue;
		
		if vertices.has(point):
			crossed_mesh = mesh;
	
	return crossed_mesh;
