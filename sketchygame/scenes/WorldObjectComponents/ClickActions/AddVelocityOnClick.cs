using System;
using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

/// <summary>
/// Klasa przykładająca losowy impuls siły do obiektu.
/// </summary>
public partial class AddVelocityOnClick : ClickActionResource {
    [Export]
    private float _velocityAdded = 100f;
    
    /// <summary>
    /// Definicja zachowania dla nadania impulsu
    /// </summary>
    /// <param name="callArgs">Dodatkowe argumenty wywołania funkcji</param>
    public override void ClickAction(Godot.Collections.Array<Node> callArgs) {
        WorldObjectBase worldObject = null!;
        
        foreach (var arg in callArgs) {
            if (arg is WorldObjectBase worldObjectBase) worldObject = worldObjectBase;
        }
        
        if (worldObject is null) return;

        var randX = Random.Shared.NextSingle() * 2f - 1f;
        var vec = new Vector2(randX, -1f).Normalized();
        
        worldObject.ApplyImpulse(vec * _velocityAdded);
    }
}