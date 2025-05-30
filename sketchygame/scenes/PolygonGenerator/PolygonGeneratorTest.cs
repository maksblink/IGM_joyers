using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PolygonGeneratorTest : Control
{
	private TextureRect EdgeTexture;
	private Button Generate;
	private Polygon2D ResultPolygon;
	private Texture2D _filteredEdgeTexture;

	public override void _Ready()
	{
		EdgeTexture = GetNode<TextureRect>("%EdgeTexture");
		Generate = GetNode<Button>("%Generate");
		ResultPolygon = GetNode<Polygon2D>("%ResultPolygon");
		
		// Przygotowanie obrazu i wyświetlenie krawędzi
		Texture2D original = ResourceLoader.Load<Texture2D>("res://assets/object base/volleyball.png");

		this._filteredEdgeTexture = EdgeDetector.ApplyEdgeDetection(original);
		EdgeTexture.Texture = this._filteredEdgeTexture;
	}

	private void _onGeneratePressed()
	{
		if (_filteredEdgeTexture == null)
		{
			GD.PrintErr("Brak przefiltrowanego obrazu");
			return;
		}

		Vector2[] polygon = PolygonGenerator.GeneratePolygon(_filteredEdgeTexture, 5f);

		// Pobierz rozmiar kontenera w którym znajduje się Polygon2D
		var container = GetNode<Control>("PolygonContainer");
		Vector2 targetSize = container.GetSize(); // lub container.Size

		// Dopasuj polygon
		Vector2 offset;
		Vector2[] fittedPolygon = FitPolygonToRect(polygon, targetSize, out offset);

		// Ustaw dane w Polygon2D
		ResultPolygon.Polygon = fittedPolygon;
		ResultPolygon.Position = offset;
		ResultPolygon.Color = new Color(1, 0, 0, 0.4f);
	}
	
	private Vector2[] FitPolygonToRect(Vector2[] points, Vector2 targetSize, out Vector2 offset)
	{
		if (points.Length == 0)
		{
			offset = Vector2.Zero;
			return points;
		}

		// 1. Oblicz bounding box
		float minX = points.Min(p => p.X);
		float maxX = points.Max(p => p.X);
		float minY = points.Min(p => p.Y);
		float maxY = points.Max(p => p.Y);

		Vector2 polygonSize = new Vector2(maxX - minX, maxY - minY);
		Vector2 scale = targetSize / polygonSize;

		// 2. Wybierz jednolitą skalę (żeby nie rozciągać)
		float uniformScale = Mathf.Min(scale.X, scale.Y);

		// 3. Przeskaluj i przesuń
		List<Vector2> fitted = new();
		foreach (var p in points)
		{
			Vector2 local = (p - new Vector2(minX, minY)) * uniformScale;
			fitted.Add(local);
		}

		// 4. Oblicz offset do wyśrodkowania
		Vector2 fittedSize = polygonSize * uniformScale;
		offset = (targetSize - fittedSize) / 2.0f;

		return fitted.ToArray();
	}
}
