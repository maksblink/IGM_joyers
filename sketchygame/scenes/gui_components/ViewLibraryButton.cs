using Godot;

namespace SketchyGame.scenes.gui_components;

/// <summary>
/// Skrypt przycisku odpowiedzialnego za załadowanie widoku `LibraryView`.
/// </summary>
public partial class ViewLibraryButton : ButtonNoFocus {
    /// <summary>
    /// Sygnał wysyłający informację o wciśnięciu PPM.
    /// </summary>
    [Signal]
    public delegate void ViewLibraryButtonPressedEventHandler();

    /// <summary>
    /// Reaguje na sygnał `pressed`.
    /// </summary>
    protected override void _on_pressed() {
        EmitSignalViewLibraryButtonPressed();
        
        base._on_pressed();
    }
}