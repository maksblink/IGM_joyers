using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

/// <summary>
/// Klasa usuwająca obiekt po wciśnięciu przycisku myszy.
/// </summary>
public partial class DeleteOnClick : ClickActionResource {
    /// <summary>
    /// Definicja zachowania dla usuwania obiektów po wciśnięciu przycisku myszy.
    /// </summary>
    /// <param name="callArgs">Dodatkowe argumenty wywołania funkcji</param>
    public override void ClickAction(Godot.Collections.Array<Node> callArgs) {
        WorldObjectBase worldObject = null!;
        
        foreach (var arg in callArgs) {
            if (arg is WorldObjectBase worldObjectBase) worldObject = worldObjectBase;
        }
        
        worldObject?.QueueFree();
    }
}