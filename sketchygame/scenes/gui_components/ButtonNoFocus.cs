using System;
using Godot;

namespace SketchyGame.scenes.gui_components;

[GodotClassName(nameof(ButtonNoFocus))]
public partial class ButtonNoFocus : Button {
	[Export] private string _changeToSceneOnClick = string.Empty;

	public virtual void _on_pressed() {
		if (_changeToSceneOnClick == string.Empty) {
			GD.PushWarning("Cannot change scene, path is empty.");
			return;
		}

		GetTree().ChangeSceneToFile(_changeToSceneOnClick);
	}
}
