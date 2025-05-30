using System;
using Godot;

namespace SketchyGame.scenes.gui_components;

[GodotClassName(nameof(ButtonNoFocus))]
public partial class ButtonNoFocus : Button {
	[Export] private string _changeToSceneOnClick = string.Empty;

	public virtual void _on_pressed() {
		GD.Print(_changeToSceneOnClick);
		if (_changeToSceneOnClick == string.Empty) { return; }
		else
		{
			GetTree().ChangeSceneToFile(_changeToSceneOnClick);
		}
	}
}
