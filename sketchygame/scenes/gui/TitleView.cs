using Godot;

namespace SketchyGame.scenes.gui;

public partial class TitleView : Control {
   private string _lang = "pl";
  
   public override void _EnterTree() {
		GetTree().Paused = false;
		GetNode<Button>("%LocaleButton").Text = TranslationServer.GetLocale().ToUpper();

	 base._EnterTree();
   }
  
  private void _on_about() {
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
  
	private void _onLocaleButtonPressed() {
		var localeButton = GetNode<Button>("%LocaleButton");
		_lang = _lang == "pl" ? "en" : "pl";
		
		TranslationServer.SetLocale(_lang);
		localeButton.Text = _lang.ToUpper();
	}
}
