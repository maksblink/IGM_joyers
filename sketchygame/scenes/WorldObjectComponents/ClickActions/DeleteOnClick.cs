using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

public partial class DeleteOnClick : ClickActionResource {
    public override void ClickAction(params object[] args) {
        if (args?[0] is not WorldObjectBase worldObject) {
            GD.Print("Wrong arguments");
            return;
        }
        
        GD.Print($"Queued to delete: ", worldObject);
        worldObject.QueueFree();
    }
}