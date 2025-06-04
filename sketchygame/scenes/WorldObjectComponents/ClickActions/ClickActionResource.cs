using Godot;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

/// <summary>
/// Klasa przechowująca dane o wydarzeniu dowolnego typu wciśnięcia przycisku myszy. 
/// </summary>
public partial class ClickActionResource : Resource {
    [Export]
    private MouseButton _mouseButton = MouseButton.None;
    
    /// <summary>
    /// Metoda zwracająca przycisk myszy, na który skrypt reaguje.
    /// </summary>
    public MouseButton MouseButton => _mouseButton;
    
    /// <summary>
    /// Definicja zachowania dla wydarzenia wciśnięcia myszy.
    /// </summary>
    /// <param name="args">Dodatkowe argumenty wywołania funkcji</param>
    public virtual void ClickAction(params object[] args) {
        GD.PushWarning("Some clickable action executed. Override this.");
    }
}