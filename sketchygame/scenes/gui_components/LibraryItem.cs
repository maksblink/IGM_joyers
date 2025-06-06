#nullable enable
using Godot;

namespace SketchyGame.scenes.gui_components;

/// <summary>
/// Skrypt komponentu wyświetlającego jeden z dostępnych obiektów w grze w bibliotece obiektów.
/// </summary>
public partial class LibraryItem : Button {
    /// <summary>
    /// Sygnał wysyłany po wybraniu i wciśnięciu instancji tego komponentu.
    /// </summary>
    /// <param name="item">Ścieżka do sceny obiektu z plików gry.</param>
    [Signal]
    public delegate void LibraryItemPressedEventHandler(string item);

    /// <summary>
    /// Sygnał wysyłany po wybraniu i wciśnięciu instancji tego komponentu.
    /// </summary>
    /// <param name="item">Scena z wybranym obiektem.</param>
    [Signal]
    public delegate void LibraryItemPressedTscnEventHandler(PackedScene item);
    
    [Signal]
    public delegate void NotifyLibraryItemPressedEventHandler(string objectName);

    [Export]
    private PackedScene? _worldObjectScene = null;

    [Export]
    private string _worldObjectPath = string.Empty;

    [field: Export]
    private Texture2D _thumbnail = null!;

    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów. Ustawia miniaturę komponentu na bazie tekstury obsługiwanego obiektu.
    /// </summary>
    public override void _Ready() {
        GetNode<TextureRect>("%Thumbnail").Texture = _thumbnail;
    }

    /// <summary>
    /// Wysyła sygnał po wciśnięciu tego komponentu.
    /// </summary>
    private void _on_pressed() {
        if (_worldObjectScene is null) {
            EmitSignal(SignalName.LibraryItemPressed, _worldObjectPath);
            EmitSignalNotifyLibraryItemPressed(GetNode<Label>("%Name").Text);
            return;
        }
        else {
            EmitSignal(SignalName.LibraryItemPressedTscn, _worldObjectScene);
            EmitSignalNotifyLibraryItemPressed(GetNode<Label>("%Name").Text);
            GD.Print(GetNode<Label>("%Name").Text);
        }
    }
}