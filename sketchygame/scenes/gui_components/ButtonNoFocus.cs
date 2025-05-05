using System;
using Godot;

namespace SketchyGame.scenes.gui_components;

[GodotClassName(nameof(ButtonNoFocus))]
public partial class ButtonNoFocus : Button {
    [Export] private string _changeToSceneOnClick = string.Empty;

    private void _on_pressed() {
        if (_changeToSceneOnClick == string.Empty) return;
        
        GetTree().ChangeSceneToFile(_changeToSceneOnClick);
    }
}