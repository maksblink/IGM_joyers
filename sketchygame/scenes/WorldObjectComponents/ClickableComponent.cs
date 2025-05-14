using Godot;
using SketchyGame.scenes.WorldObjectComponents.ClickActions;

namespace SketchyGame.scenes.WorldObjectComponents;

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

    public override void _Ready() {
        _holdTimer = GetNode<Timer>("HoldTimer");
        _holdTimer.WaitTime = _holdThreshold;
        _holdTimer.Stop();

        base._Ready();
    }

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

    private void HandleOnClick(MouseButton button) {
        GD.Print("HandleOnClick");
        foreach (var action in _onClickActions) {
            if (action.MouseButton != button) continue;
            
            action.ClickAction(WorldObject);
        }
    }

    private void HandleClickAndHold(MouseButton button) {
        GD.Print("HandleClickAndHold");
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

    private void _resetMouseState() {
        _previousState = false;
        _mouseButton = MouseButton.None;
        _holdTimer.Stop();
        _isHolding = false;
    }
}