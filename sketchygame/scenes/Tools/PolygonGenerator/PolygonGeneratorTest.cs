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

    [Export]
    private float _delta = 5f;

    public override void _Ready() {
        _edgeTexture = GetNode<TextureRect>("%EdgeTexture");
        _generate = GetNode<Button>("%Generate");
        _resultPolygon = GetNode<Polygon2D>("%ResultPolygon");

        // Przygotowanie obrazu i wyświetlenie krawędzi
        Texture2D original = ResourceLoader.Load<Texture2D>("res://assets/object base/plant.png");

        this._filteredEdgeTexture = EdgeDetector.ApplyEdgeDetection(original);
        _edgeTexture.Texture = this._filteredEdgeTexture;
    }

    private void _onGeneratePressed() {
        if (_filteredEdgeTexture == null) {
            GD.PrintErr("Brak przefiltrowanego obrazu");
            return;
        }

        var polygon = global::SketchyGame.scenes.Tools.PolygonGenerator.PolygonGenerator.GeneratePolygon(_filteredEdgeTexture, _delta);

        // Pobierz rozmiar kontenera w którym znajduje się Polygon2D
        var container = GetNode<Control>("PolygonContainer");
        var targetSize = container.GetSize(); // lub container.Size

        // Dopasuj polygon
        var fittedPolygon = FitPolygonToRect(polygon, targetSize, out var offset);

        // Ustaw dane w Polygon2D
        _resultPolygon.Polygon = polygon;
        _resultPolygon.Position = offset;
        _resultPolygon.Color = new Color(1, 0, 0, 0.4f);

        QueueRedraw();
    }

    public override void _Draw() {
        foreach (var point in _resultPolygon.Polygon) {
            DrawCircle(point, 1f, Colors.Blue);
        }

        base._Draw();
    }

    private Vector2[] FitPolygonToRect(Vector2[] points, Vector2 targetSize, out Vector2 offset) {
        if (points.Length == 0) {
            offset = Vector2.Zero;
            return points;
        }

        // 1. Oblicz bounding box
        var minX = points.Min(p => p.X);
        var maxX = points.Max(p => p.X);
        var minY = points.Min(p => p.Y);
        var maxY = points.Max(p => p.Y);

        var polygonSize = new Vector2(maxX - minX, maxY - minY);
        var scale = targetSize / polygonSize;

        // 2. Wybierz jednolitą skalę (żeby nie rozciągać)
        var uniformScale = Mathf.Min(scale.X, scale.Y);

        // 3. Przeskaluj i przesuń
        List<Vector2> fitted = new();
        foreach (var p in points) {
            Vector2 local = (p - new Vector2(minX, minY)) * uniformScale;
            fitted.Add(local);
        }

        // 4. Oblicz offset do wyśrodkowania
        var fittedSize = polygonSize * uniformScale;
        offset = (targetSize - fittedSize) / 2.0f;

        return fitted.ToArray();
    }
}