using Godot;
using System;

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
		Texture2D original = ResourceLoader.Load<Texture2D>("res://assets/object base/Chair.png");
		if (original == null)
		{
			GD.PrintErr("Nie udało się wczytać obrazka.");
			return;
		}

		this._filteredEdgeTexture = EdgeDetector.ApplyEdgeDetection(original);
		GD.Print($"Edge image size: {this._filteredEdgeTexture.GetWidth()} x {this._filteredEdgeTexture.GetHeight()}");
		if (this._filteredEdgeTexture == null)
		{
			GD.PrintErr("EdgeDetector zwrócił null.");
			return;
		}

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
		ResultPolygon.Polygon = polygon;
		ResultPolygon.Color = new Color(1, 0, 0, 0.4f);
	}
}
