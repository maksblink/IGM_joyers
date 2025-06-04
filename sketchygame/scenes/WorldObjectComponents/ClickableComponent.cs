using System.Linq;
using Godot;
using SketchyGame.scenes.WorldObjectComponents.ClickActions;

namespace SketchyGame.scenes.WorldObjectComponents;

/// <summary>
/// Komponent dodający obsługę myszy dla obiektów gry.
/// </summary>
public partial class ClickableComponent : WorldObjectComponentBase {
    [Export]
    private float _holdThreshold = 0.2f;

    [Export]
    private Godot.Collections.Array<ClickActionResource> _onClickActions = [];

    [Export]
    private Godot.Collections.Array<ClickActionResource> _onClickAndHoldActions = [];

    [Export]
    private Godot.Collections.Array<ClickActionResource> _onClickAndDragActions = [];

    private bool _previousState = false;
    private MouseButton _mouseButton = MouseButton.None;
    private Timer _holdTimer = null!;
    private bool _isHolding = false;
    
    private CollisionPolygon2D _collisionPolygon2D = null!;

    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów.
    /// </summary>
    public override void _Ready() {
        _holdTimer = GetNode<Timer>("HoldTimer");
        _holdTimer.WaitTime = _holdThreshold;
        _holdTimer.Stop();
        
        _collisionPolygon2D = GetNode<CollisionPolygon2D>("%ClickCollider");
        
        Owner.Ready += () => {
            var polygon = WorldObject.Polygon.Polygon;
            var offset = WorldObject.GetOffset();

            _collisionPolygon2D.Polygon = polygon.Select(p => p - offset).ToArray();
        };
        
        base._Ready();
    }

    /// <summary>
    /// Funkcja reagująca na wydarzenia wejścia i wywołująca odpowiednie zachowanie dla danego wejścia.
    /// </summary>
    /// <param name="viewport">Nie używane.</param>
    /// <param name="inputEvent">Rodzaj akcji wejścia, np. wciśnięcie przycisku myszy.</param>
    /// <param name="shapeIdx">Nie używane></param>
    private void _onInputEventReceived(Node viewport, InputEvent inputEvent, int shapeIdx) {
        if (inputEvent is not InputEventMouseButton mouseEvent) {
            return;
        }

        if (_holdTimer.IsStopped() && mouseEvent.Pressed) {
            _holdTimer.Start();
        }
        else if (!mouseEvent.Pressed) {
            _holdTimer.Stop();
            _isHolding = false;
        }

        if (_previousState && !mouseEvent.Pressed && !_isHolding) {
            HandleOnClick(mouseEvent.ButtonIndex);
        }

        _previousState = mouseEvent.Pressed;
        _mouseButton = mouseEvent.ButtonIndex;
    }

    /// <summary>
    /// Funkcja obsługująca wydarzenie przeciągania obiektu po ekranie.
    /// </summary>
    /// <param name="viewport">Nie używane.</param>
    /// <param name="inputEvent">Rodzaj akcji wejścia, np. wciśnięcie przycisku myszy.</param>
    /// <param name="shapeIdx">Nie używane></param>
    private void _onInputEventDragged(Node viewport, InputEvent inputEvent, int shapeIdx) {
        if (inputEvent is not InputEventMouseMotion mouseEvent) return;
        if (!_isHolding) return;

        
        WorldObject.Freeze = true;
        WorldObject.Modulate = Colors.Orange;
        WorldObject.GlobalPosition = mouseEvent.GlobalPosition;
    }

    /// <summary>
    /// Funkcja obsługująca pojedyncze wciśnięcie dowolnego przycisku myszy.
    /// </summary>
    /// <param name="button">Indeks przycisku myszy, który został zarejestrowany podczas wystąpienia tego wydarzenia.</param>
    private void HandleOnClick(MouseButton button) {
        foreach (var action in _onClickActions) {
            if (action.MouseButton != button) continue;
            
            action.ClickAction(WorldObject);
        }
        
        WorldObject.Freeze = false;
        WorldObject.Modulate = Colors.White;
    }

    /// <summary>
    /// Funkcja obsługująca pojedyncze wciśnięcie i przytrzymanie dowolnego przycisku myszy.
    /// </summary>
    /// <param name="button">Indeks przycisku myszy, który został zarejestrowany podczas wystąpienia tego wydarzenia.</param>
    private void HandleClickAndHold(MouseButton button) {
        foreach (var action in _onClickAndHoldActions) {
            if (action.MouseButton != button) continue;
            
            action.ClickAction(WorldObject);
        }
    }
    
    private void HandleDragAndDrop() {
        GD.Print("HandleDragAndDrop");
    }
    
    private void _onHoldTimerTimeOut() {
        _isHolding = true;
        HandleClickAndHold(_mouseButton);
    }

    /// <summary>
    /// Funkcja resetująca stan wydarzeń wejścia.
    /// </summary>
    private void _resetMouseState() {
        _previousState = false;
        _mouseButton = MouseButton.None;
        _holdTimer.Stop();
        _isHolding = false;
        WorldObject.Freeze = false;
    }
}