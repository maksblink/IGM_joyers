using System;
using Godot;

namespace SketchyGame.scenes.gui_components;

[GodotClassName(nameof(ButtonNoFocus))]
public partial class ButtonNoFocus : Button {
	[Export] private string _changeToSceneOnClick = string.Empty;

	private void _on_pressed() {
		if (_changeToSceneOnClick == string.Empty) {
			GD.PushWarning("Cannot change scene, path is empty.");
			return;
		}
		
		try
		{
			if (_changeToSceneOnClick.Contains("library_view.tscn"))
			{
				var scene = GetTree().CurrentScene;
				if (scene != null)
				{
					var worldView = scene.GetNode("GameView/GamePanel/GameWorldContainer/WorldView");
					if (worldView != null)
					{
						GameState.Instance.SaveWorld(worldView);
						GD.Print("\n>> WorldView zapisany\n");
					}
				}
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr("Błąd podczas zapisu WorldView: ", ex.Message);
		}

		Callable.From(() =>
		{
			var err = GetTree().ChangeSceneToFile(_changeToSceneOnClick);
			GD.Print(">> Zmiana sceny: ", err);
		}).CallDeferred();
	}
}
