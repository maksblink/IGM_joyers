using System.Collections.Generic;
using Godot;
using SketchyGame.scenes.tools.Formatters;

namespace SketchyGame.scenes.gui;

public partial class SaveView : Control {
    [Export]
    private PackedScene _saveItemScene = null!;
    
    private const string SavePath = "user://saves";
    
    public override void _Ready() {

        var dir = DirAccess.Open(SavePath);

        var files = new List<string>();

        dir.ListDirBegin();
        var fileName = dir.GetNext();

        while (!string.IsNullOrEmpty(fileName)) {
            if (!dir.CurrentIsDir() && fileName.EndsWith(".tscn"))
                files.Add(fileName);

            fileName = dir.GetNext();
        }

        dir.ListDirEnd();

        files.Reverse(); // najnowsze będą na początku

        foreach (var file in files) {
            var model = FormatSaveFileName.FormattedDate(file);

            var saveItem = _saveItemScene.Instantiate<gui_components.SaveItemComponent>();
            saveItem.SaveName = model;
            GetNode<Container>("%SaveItemContainer").AddChild(saveItem);
        }
    }
}