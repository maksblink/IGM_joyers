using Godot;

namespace SketchyGame.scenes.gui;

/// <summary>
/// Skrypt widoku tytułowego gry.
/// </summary>
public partial class TitleView : Control {
    private string _lang = "pl";

    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów. Ustalana początkowy język na podstawie języka systemowego.
    public override void _EnterTree() {
        GetTree().Paused = false;

        var currentLocale = TranslationServer.GetLocale().ToUpper();
        currentLocale = currentLocale == "PL" ? "EN" : "PL";
        GetNode<Button>("%LocaleButton").Text = currentLocale;

        base._EnterTree();
    }

    /// <summary>
    /// Otwiera i popup `o nas`.
    /// </summary>
    private void _on_about() {
        var aboutPanel = GetNode<Control>("%About");

        aboutPanel.Visible = true;
    }

    /// <summary>
    /// Wyłącza program.
    /// </summary>
    private void _on_game_exit() {
        GetTree().Quit();
    }

    /// <summary>
    /// Wyłącza popup `o nas`.
    /// </summary>
    /// <param name="inputEvent">Akcja myszy — przekazywane automatycznie przez silnik</param>
    private void _on_about_outside_click(InputEvent inputEvent) {
        if (inputEvent is not InputEventMouseButton || !inputEvent.IsPressed()) return;
        
        using var aboutPanel = GetNode<Control>("%About");
        aboutPanel.Visible = false;
    }

    /// <summary>
    /// Zmienia język gry z Angielskiego na Polski i odwrotnie.
    /// </summary>
    private void _onLocaleButtonPressed() {
        var localeButton = GetNode<Button>("%LocaleButton");
        localeButton.Text = _lang.ToUpper();
        
        _lang = _lang == "pl" ? "en" : "pl";
        TranslationServer.SetLocale(_lang);
    }
}