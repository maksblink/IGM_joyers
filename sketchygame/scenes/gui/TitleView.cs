using Godot;

namespace SketchyGame.scenes.gui;

public partial class TitleView : Control {

    private void _on_game_exit() {
        GetTree().Quit();
    }
}