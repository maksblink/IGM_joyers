using System;
using Godot;

[GodotClassName(nameof(ButtonNoFocus))]
public partial class ButtonNoFocus : Button {
    [Export] private string _changeToSceneOnClick;

    private void _on_pressed() {
        if (_changeToSceneOnClick == string.Empty) return;
        
        GetTree().ChangeSceneToFile(_changeToSceneOnClick);
    }
}