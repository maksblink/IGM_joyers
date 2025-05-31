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
        var title = GetNode<Label>("%Title");
        var description = GetNode<Label>("%Description");
        var leader = GetNode<Label>("%Leader");
        var authors = GetNode<Label>("%AuthorsTitle");
        var releaseDate = GetNode<Label>("%ReleaseDate");

        title.Text = TranslationServer.Translate("ABOUT_TITLE");
        description.Text = TranslationServer.Translate("ABOUT_DESCRIPTION");
        leader.Text = TranslationServer.Translate("ABOUT_LEADER");
        authors.Text = TranslationServer.Translate("ABOUT_AUTHORS");
        releaseDate.Text = TranslationServer.Translate("ABOUT_RELEASE_DATE");

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