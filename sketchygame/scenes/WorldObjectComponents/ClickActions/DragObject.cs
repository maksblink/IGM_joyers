using Godot;
using Godot.Collections;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

/// <summary>
/// TODO: Add docs
/// </summary>
public partial class DragObject : ClickActionResource {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callArgs"></param>
    public override void ClickAction(Array<Node> callArgs) {
        WorldObjectBase worldObject = null!;
        
        foreach (var arg in callArgs) {
            if (arg is WorldObjectBase worldObjectBase) worldObject = worldObjectBase;
        }
        
        if (worldObject is null) {
            GD.PrintErr("World object is null");
            return;
        }
        
        var globalMousePosition = worldObject.GetGlobalMousePosition();
        worldObject.Freeze = true;
        worldObject.GlobalPosition = globalMousePosition;
    }
}