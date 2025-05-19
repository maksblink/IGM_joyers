using Godot;

namespace SketchyGame.scenes.gui;

public partial class TitleView : Control {
	public override void _EnterTree() {
		GetTree().Paused = false;
		
		base._EnterTree();
	}
	
	private void _on_about() {
		Control aboutPanel = GetNode<Control>("%About");
		aboutPanel.Visible = !aboutPanel.Visible;
	}

	private void _on_game_exit() {
		GetTree().Quit();
	}
}
