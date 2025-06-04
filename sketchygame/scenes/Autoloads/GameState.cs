using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.Autoloads;

/// <summary>
/// Klasa typu Singleton — używana do zapisywania i odzyskiwania stanu świata gry.
/// </summary>
public partial class GameState : Node {
    /// <summary>
    /// Instancja klasy.
    /// </summary>
    /// <returns>Globalny Singleton tej klasy.</returns>
    public static GameState Instance { get; private set; } = null!;

    /// <summary>
    /// Węzeł używany do tymczasowego przechowywania wszystkich obiektów ze świata gry.
    /// </summary>
    [Export]
    private Node _tempStorage = null!;

    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów. Tworzy globalny Singleton tej klasy.
    /// </summary>
    public override void _Ready() {
        Instance ??= this;

        base._Ready();
    }

    /// <summary>
    /// Zapisuje wszystkie obiekty ze świata gry w _tempStorage.
    /// </summary>
    /// <param name="objectContainer">Węzeł (kontener) ze wszystkimi obiektami gry.</param>
    public void SaveWorld(Node objectContainer) {
        foreach (var child in objectContainer.GetChildren()) {
            if (child is not WorldObjectBase worldObjectBase) continue;

            worldObjectBase.Freeze = true;
            worldObjectBase.Reparent(_tempStorage);
        }
    }

    /// <summary>
    /// Przywraca wszystkie obiekty z _tempStorage do kontenera obiektów w świecie gry.
    /// </summary>
    /// <param name="objectContainer">Węzeł (kontener) ze wszystkimi obiektami gry.</param>
    public void RestoreWorld(Node objectContainer) {
        foreach (var child in _tempStorage.GetChildren()) {
            if (child is not WorldObjectBase worldObjectBase) continue;

            worldObjectBase.Freeze = false;
            worldObjectBase.Reparent(objectContainer);
        }
    }
}