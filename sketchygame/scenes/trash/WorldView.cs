using Godot;

namespace SketchyGame.scenes.trash;

public partial class WorldView : SubViewport {
    [Export]
    private Timer _timer;
    
    [Export]
    private Godot.Collections.Array<Sprite2D> _sprites;
    
    private int _spriteIndex = 0;
    
    private void _on_timer_timeout() {
        _timer.WaitTime = System.Random.Shared.NextInt64(1, 5);
        _timer.Start();

        _sprites[_spriteIndex].Visible = !_sprites[_spriteIndex].Visible;
        _spriteIndex = (int)System.Random.Shared.NextInt64(0, 3);
    }
}