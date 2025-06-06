using Godot;
using Godot.Collections;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

public partial class ThrowObject : ClickActionResource {
    [Export]
    private float _throwStrengthScale = 1f;

    public override void ClickAction(Array<Node> callArgs) {
        WorldObjectBase worldObject = null!;
        ClickableComponent clickableComponent = null!;

        foreach (var arg in callArgs) {
            if (arg is WorldObjectBase worldObjectBase) worldObject = worldObjectBase;
            if (arg is ClickableComponent clickable) clickableComponent = clickable;
        }

        if (worldObject is null) return;
        if (clickableComponent is null) return;

        worldObject.LinearVelocity = clickableComponent.MouseVelocity * _throwStrengthScale;
    }
}