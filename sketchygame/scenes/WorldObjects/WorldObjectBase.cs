using System.Linq;
using Godot;
using SketchyGame.scenes.Tools.EdgeDetection;
using SketchyGame.scenes.Tools.PolygonGenerator;

namespace SketchyGame.scenes.WorldObjects;

/// <summary>
/// Podstawowa klasa dla obiektów występujących w świecie gry.
/// </summary>
public partial class WorldObjectBase : RigidBody2D {
    [Export]
    private Texture2D _objectTexture = null!;
    
    /// <summary>
    /// Funkcja dostępu do tekstury obiektu.
    /// </summary>
    protected Texture2D GetObjectTexture2D => _objectTexture;
    
    [Export]
    private float _pointDetectDelta = 10f;

    [Export]
    private float _objectWidth;

    /// <summary>
    /// Wielokąt opisujący kształt obiektu.
    /// </summary>
    public Polygon2D Polygon { get; private set; }
    
    private CollisionPolygon2D CollisionShape { get; set; }


    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów.
    /// </summary>
    public override void _Ready() {
        Polygon = GetNode<Polygon2D>("%ObjectSkeleton");
        CollisionShape = GetNode<CollisionPolygon2D>("%ObjectCollisionSkeleton");
        SetupObjectSkeleton();

        base._Ready();
    }

    private void SetupObjectSkeleton() {
        var texture2D = GetObjectTexture2D;
        if (texture2D is null) {
            GD.PrintErr($"{nameof(GetObjectTexture2D)} is null.");
            return;
        }

        var edgeTexture = EdgeDetector.ApplyEdgeDetection(GetObjectTexture2D);
        var vertexArray = PolygonGenerator.GeneratePolygon(edgeTexture, _pointDetectDelta);

        if (Mathf.IsZeroApprox(_objectWidth)) {
            GD.PrintErr($"Set texture width.");
            return;
        }
        
        Polygon.Polygon = GetScaledPolygon(vertexArray);
        Polygon.Texture = texture2D;
        Polygon.TextureScale = GetTextureScale();
        Polygon.Offset = -GetOffset();
        Polygon.TextureOffset = GetOffset();

        CollisionShape.Polygon = GetScaledPolygon(vertexArray).Select(p => p - GetOffset()).ToArray();
    }

    private Vector2[] GetScaledPolygon(Vector2[] points) {
        var textureSize = GetObjectTexture2D.GetSize();
        var scaleRatio = textureSize.Y / textureSize.X;
        var objectScaleX = _objectWidth;
        var objectScaleY = scaleRatio * _objectWidth;

        var scaleVec = new Vector2(objectScaleX, objectScaleY) / textureSize;

        return points.Select(p => p * scaleVec).ToArray();
    }

    private Vector2 GetTextureScale() {
        var textureSize = GetObjectTexture2D.GetSize();
        var textureScaleX = textureSize.X / _objectWidth;
        
        var ratio = textureSize.X / textureSize.Y;
        var textureScaleY = textureSize.Y / _objectWidth * ratio;

        return new Vector2(textureScaleX, textureScaleY);
    }

    /// <summary>
    /// Funkcja obliczająca przesunięcie dla tekstury i kształtu obiektu.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetOffset() {
        var textureSize = GetObjectTexture2D.GetSize();
        var ratio = textureSize.X / textureSize.Y;

        return new Vector2(_objectWidth / 2f, _objectWidth / ratio / 2f);
    }

    private void _onScreenExitedNotifier() {
        QueueFree();
    }
}