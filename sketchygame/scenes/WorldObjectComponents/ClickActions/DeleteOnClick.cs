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
    /// <param name="args">Dodatkowe argumenty wywołania funkcji</param>
    public override void ClickAction(params object[] args) {
        if (args?[0] is not WorldObjectBase worldObject) {
            GD.PrintErr("Wrong arguments.");
            return;
        }
        
        worldObject.QueueFree();
    }
}