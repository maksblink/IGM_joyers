using Godot;

namespace SketchyGame.scenes.gui_components;

public partial class ViewLibraryButton : ButtonNoFocus {
    [Signal]
    public delegate void ViewLibraryButtonPressedEventHandler();
    
    public override void _on_pressed() {
        EmitSignalViewLibraryButtonPressed();
        
        base._on_pressed();
    }
}