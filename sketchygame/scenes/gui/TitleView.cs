using Godot;

namespace SketchyGame.scenes.gui;

public partial class TitleView : Control {
	public override void _EnterTree()
	{
		GetTree().Paused = false;
		
		base._EnterTree();
	}
	
	private void _on_about()
	{
		Control aboutPanel = GetNode<Control>("%About");
		aboutPanel.Visible = true;
	}

	private void _on_game_exit() 
	{
		GetTree().Quit();
	}

	private void _on_about_outside_click(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton && inputEvent.IsPressed())
		{
			Control aboutPanel = GetNode<Control>("%About");
			aboutPanel.Visible = false;
		}
	}
}
