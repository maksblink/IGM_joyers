using Godot;

namespace SketchyGame.scenes.WorldObjectComponents;

public partial class ClickableComponent : WorldObjectComponentBase {
    private void _onClickDetectionZoneInputEvent(Node viewport, InputEvent inputEvent, int shapeIdx) {
        if (inputEvent is not InputEventMouse eventMouse) return;

        if (eventMouse.ButtonMask == MouseButtonMask.Left) {
            WorldObject?.ApplyForce(Vector2.Right * 50f);
        }
    }
}