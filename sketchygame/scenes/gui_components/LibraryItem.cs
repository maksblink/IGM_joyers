#nullable enable
using Godot;

namespace SketchyGame.scenes.gui_components;

public partial class LibraryItem : Button {
    [Signal]
    public delegate void LibraryItemPressedEventHandler(string item);
    
    [Signal]
    public delegate void LibraryItemPressedTscnEventHandler(PackedScene item);
    
    [Export]
    private PackedScene? _worldObjectScene = null;

    [Export]
    private string _worldObjectPath = string.Empty;

    [field: Export]
    private Texture2D _thumbnail = null!;

    public override void _Ready() {
        GetNode<TextureRect>("%Thumbnail").Texture = _thumbnail;
    }

    private void _on_pressed() {
        if (_worldObjectScene is null) {
            EmitSignal(SignalName.LibraryItemPressed, _worldObjectPath);
            return;
        }
        else {
            EmitSignal(SignalName.LibraryItemPressedTscn, _worldObjectScene);
        }
    }
}