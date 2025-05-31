using Godot;

namespace SketchyGame.scenes.gui_components;

public partial class SaveItemComponent : Button {
    public string SaveName { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        var description = GetNode<Label>("%Description");
        description.Text = Tr("SAVE_ITEM") + " " + SaveName;
    }

    private void _on_pressed() {
        GetTree().ChangeSceneToFile("user://saves/" + SaveName);
    }
}