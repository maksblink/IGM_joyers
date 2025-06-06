using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents;

/// <summary>
/// Bazowa klasa dla komponentów występujących jako dodatkowe funkcjonalności obiektów gry.
/// </summary>
public partial class WorldObjectComponentBase : Node2D {
    /// <summary>
    /// Obiekt, w którym występuje dana instancja komponentu.
    /// </summary>
    [Export]
    protected WorldObjectBase Parent { get; private set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    [Export]
    protected Godot.Collections.Array<Node> CallArguments { get; private set; } = [];

    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów.
    /// </summary>
    public override void _Ready() {
        var parent = GetParent();

        if (parent is not WorldObjectBase worldObjectBase) return;
        Parent = worldObjectBase;
        CallArguments.Add(Parent);
    }
}