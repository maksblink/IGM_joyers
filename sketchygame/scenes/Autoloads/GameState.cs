using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.Autoloads;

public partial class GameState : Node {
    public static GameState Instance { get; private set; } = null!;

    [Export]
    private Node _tempStorage = null!;

    public override void _Ready() {
        Instance ??= this;

        base._Ready();
    }

    public void SaveWorld(Node objectContainer) {
        foreach (var child in objectContainer.GetChildren()) {
            if (child is not WorldObjectBase worldObjectBase) continue;

            worldObjectBase.Freeze = true;
            worldObjectBase.Reparent(_tempStorage);
        }
    }

    public void RestoreWorld(Node objectContainer) {
        foreach (var child in _tempStorage.GetChildren()) {
            if (child is not WorldObjectBase worldObjectBase) continue;

            worldObjectBase.Freeze = false;
            worldObjectBase.Reparent(objectContainer);
        }
    }
}