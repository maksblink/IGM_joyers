using Godot;

namespace SketchyGame.scenes.gui_components;

/// <summary>
/// Skrypt komponentu przechowującego dane o zapisie gry.
/// </summary>
public partial class SaveItemComponent : Button {
    /// <summary>
    /// Nazwa wyświetlana zapisu gry.
    /// </summary>
    public string SaveName { get; set; }
    
    /// <summary>
    /// Ścieżka do pliku z zapisem gry.
    /// </summary>
    public string SaveFile { get; set; }
    
    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów.
    /// </summary>
    public override void _Ready() {
        var description = GetNode<Label>("%Description");
        description.Text = Tr("SAVE_ITEM") + " " + SaveName;
    }

    /// <summary>
    /// Reaguje na sygnał wciśnięcia i wczytuje plik z zapisem gry, po czym zmienia scenę na widok gry.
    /// </summary>
    private void _on_pressed() {
        if (SaveFile.StartsWith("res")) {
            GetTree().ChangeSceneToFile(SaveFile);
            return;
        }
        
        GetTree().ChangeSceneToFile("user://saves/" + SaveFile);
    }
}