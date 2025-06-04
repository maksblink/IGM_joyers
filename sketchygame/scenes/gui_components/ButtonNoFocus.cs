using Godot;

namespace SketchyGame.scenes.gui_components;

/// <summary>
/// Skrypt podstawowego przycisku używanego w grze. Służy do zmiany sceny.
/// </summary>
[GodotClassName(nameof(ButtonNoFocus))]
public partial class ButtonNoFocus : Button {
	/// <summary>
	/// Ścieżka do pliku ze sceną lub widokiem, na który ma się przełączyć po wciśnięciu przycisku.
	/// </summary>
	[Export] private string _changeToSceneOnClick = string.Empty;

	/// <summary>
	/// Zmienia scenę gry.
	/// </summary>
	protected virtual void _on_pressed() {
		if (_changeToSceneOnClick == string.Empty) {
			GD.PushWarning("Cannot change scene, path is empty.");
			return;
		}

		GetTree().ChangeSceneToFile(_changeToSceneOnClick);
	}
}
