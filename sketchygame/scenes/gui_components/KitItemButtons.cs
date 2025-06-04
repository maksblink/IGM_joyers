using Godot;

namespace SketchyGame.scenes.gui_components;

/// <summary>
/// Skrypt komponentu przybornika.
/// </summary>
public partial class KitItemButtons : Panel {
    /// <summary>
    /// Ustala kolor podświetlenia aktualnie wybranego narzędzia. 
    /// </summary>
    [Export]
    private Color _tintColor = new(0.129f, 0.62f, 0.737f);
    
    private void _onBrushButtonPressed() {
        GetNode<Button>("%btn_Draw").Modulate = _tintColor;
        GetNode<Button>("%btn_Erase").Modulate = Colors.White;
    }
    
    private void _onEraseButtonPressed() {
        GetNode<Button>("%btn_Draw").Modulate = Colors.White;
        GetNode<Button>("%btn_Erase").Modulate = _tintColor;
    }

}