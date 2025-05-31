using Godot;

namespace SketchyGame.scenes.gui;

public partial class TitleView : Control {
    private string _lang = "pl";

    public override void _EnterTree() {
        GetTree().Paused = false;

        var currentLocale = TranslationServer.GetLocale().ToUpper();
        currentLocale = currentLocale == "PL" ? "EN" : "PL";
        GetNode<Button>("%LocaleButton").Text = currentLocale;

        base._EnterTree();
    }

    private void _on_about() {
        var aboutPanel = GetNode<Control>("%About");
        aboutPanel.Visible = true;
    }

    private void _on_game_exit() {
        GetTree().Quit();
    }

    private void _on_about_outside_click(InputEvent inputEvent) {
        if (inputEvent is not InputEventMouseButton || !inputEvent.IsPressed()) return;
        
        using var aboutPanel = GetNode<Control>("%About");
        aboutPanel.Visible = false;
    }

    private void _onLocaleButtonPressed() {
        var localeButton = GetNode<Button>("%LocaleButton");
        localeButton.Text = _lang.ToUpper();
        
        _lang = _lang == "pl" ? "en" : "pl";
        TranslationServer.SetLocale(_lang);
    }
}