using Godot;

namespace SketchyGame.scenes.gui;

public partial class TitleView : Control {
	public override void _EnterTree() {
		GetTree().Paused = false;
		
		if (GameState.Instance != null)
		{
			GameState.Instance.ClearWorldState();
		}
	
		base._EnterTree();
	}

	private void _on_game_exit() {
		GetTree().Quit();
	}
}
