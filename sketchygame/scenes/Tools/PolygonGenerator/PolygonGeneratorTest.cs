using System.Collections.Generic;
using System.Linq;
using Godot;
using SketchyGame.scenes.Tools.EdgeDetection;

namespace SketchyGame.scenes.Tools.PolygonGenerator;

public partial class PolygonGeneratorTest : Control {
	private TextureRect _edgeTexture;
	private Button _generate;
	private Polygon2D _resultPolygon;
	private Texture2D _filteredEdgeTexture;
	private OptionButton _edgeSelector;
	private readonly List<string> _imagePaths = [];

	[Export]
	private float _delta = 5f;

	public override void _Ready() {
		_edgeTexture = GetNode<TextureRect>("%EdgeTexture");
		_generate = GetNode<Button>("%Generate");
		_resultPolygon = GetNode<Polygon2D>("%ResultPolygon");
		_edgeSelector = GetNode<OptionButton>("%EdgeSelector");
		
		LoadImages();
		_onEdgeSelected(0);
	}

	private void _onGeneratePressed() {
		var polygon = PolygonGenerator.GeneratePolygon(_filteredEdgeTexture, _delta);
		_resultPolygon.Polygon = polygon;
		_resultPolygon.Color = new Color(1, 0, 0, 0.4f);

		// Oblicz bounding box
		Rect2 bounds = GetPolygonBounds(polygon);

		// Pobierz rozmiar kontenera, w którym rysujesz
		var polygonNode = GetNode<Node2D>("PolygonContainer/Polygon");
		Vector2 containerSize = ((Control)polygonNode.GetParent()).Size;
		
		Vector2 padding = new Vector2(20, 20);
		Vector2 effectiveSize = containerSize - padding * 2;

		// Oblicz skalę (zachowaj proporcje)
		float scale = Mathf.Min(effectiveSize.X / bounds.Size.X, effectiveSize.Y / bounds.Size.Y);

		// Ustaw skalę i pozycję tak, aby polygon był wycentrowany
		polygonNode.Scale = new Vector2(scale, scale);
		polygonNode.Position = padding + (effectiveSize - bounds.Size * scale) / 2 - bounds.Position * scale;

		// Rysuj punkty po przeskalowaniu
		var drawer = GetNode<PolygonDrawer>("PolygonContainer/Polygon");
		drawer.Points = polygon;
		drawer.QueueRedraw();
	}
	
	private Rect2 GetPolygonBounds(Vector2[] points)
	{
		if (points.Length == 0)
			return new Rect2();

		float minX = points.Min(p => p.X);
		float maxX = points.Max(p => p.X);
		float minY = points.Min(p => p.Y);
		float maxY = points.Max(p => p.Y);

		return new Rect2(minX, minY, maxX - minX, maxY - minY);
	}
	
	private void LoadImages() {
		const string path = "res://assets/object_base";

		foreach (var fileName in ResourceLoader.ListDirectory(path).Where(file => file.EndsWith(".png"))) {
			_imagePaths.Add(path + "/" + fileName);
			_edgeSelector.AddItem(fileName);
		}
	}

	private void _onEdgeSelected(int index) {
		var path = _imagePaths[index];
		var image = ResourceLoader.Load<Texture2D>(path);

		this._filteredEdgeTexture = EdgeDetector.ApplyEdgeDetection(image);
		_edgeTexture.Texture = this._filteredEdgeTexture;
	}
}
