using System.Collections.Generic;
using Godot;
using SketchyGame.scenes.tools.Formatters;

namespace SketchyGame.scenes.gui;

/// <summary>
/// Skrypt widoku z zapisanymi stanami gry.
/// </summary>
public partial class SaveView : Control {
    [Export]
    private PackedScene _saveItemScene = null!;

    private const string SavePath = "user://saves";

    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów. Odczytuje wszystkie dostępne zapisane gry z folderu lokalnego.
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
            var model = FormatSaveFileName.FormatDate(file);

            var saveItem = _saveItemScene.Instantiate<gui_components.SaveItemComponent>();
            saveItem.SaveName = model;
            saveItem.SaveFile = file;
            GetNode<Container>("%SaveItemContainer").AddChild(saveItem);
        }

        var modelNik = FormatSaveFileName.FormatDate("20250531-232454.tscn");
        var saveFileNik = _saveItemScene.Instantiate<gui_components.SaveItemComponent>();
        saveFileNik.SaveName = modelNik;
        saveFileNik.SaveFile = "res://assets/20250531-232454.tscn";
        GetNode<Container>("%SaveItemContainer").AddChild(saveFileNik);
    }
}