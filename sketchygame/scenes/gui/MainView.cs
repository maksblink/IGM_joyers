using Godot;

namespace SketchyGame.scenes.gui;

public partial class MainView : Control {
    [Export]
    private Control _gameView = null!;
    
    [Export]
    private Control _menuView = null!;
    
    private bool _isMenuOpen = false;

    private void _onOpenMenuButtonPressed() {
        _menuView.Visible = !_menuView.Visible;
        GetTree().Paused = _menuView.Visible;
    }
}