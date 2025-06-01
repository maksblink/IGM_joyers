using Godot;

namespace SketchyGame.scenes.gui_components;

public partial class SaveItemComponent : Button {
    public string SaveName { get; set; }
    public string SaveFile { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        var description = GetNode<Label>("%Description");
        description.Text = Tr("SAVE_ITEM") + " " + SaveName;
    }

    private void _on_pressed() {
        if (SaveFile.StartsWith("res")) {
            GetTree().ChangeSceneToFile(SaveFile);
            return;
        }
        
        GetTree().ChangeSceneToFile("user://saves/" + SaveFile);
    }
}