using Godot;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

public partial class ClickActionResource : Resource {
    [Export]
    private MouseButton _mouseButton = MouseButton.None;
    
    public MouseButton MouseButton => _mouseButton;
    
    public virtual void ClickAction(params object[] args) {
        GD.Print("Some clickable action executed");
    }
}