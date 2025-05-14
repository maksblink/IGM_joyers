using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents;

public partial class WorldObjectComponentBase : Node2D {
    [Export]
    public WorldObjectBase WorldObject { get; private set; } = null!;

    public override void _Ready() {
        var parent = GetParent();

        if (parent is not WorldObjectBase worldObjectBase) return;
        WorldObject = worldObjectBase;
    }
}