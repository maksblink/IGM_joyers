using System;
using Godot;
using SketchyGame.scenes.Autoloads;

namespace SketchyGame.scenes.gui;

public partial class MainViewNew : Node2D {
    [Export]
    private Control _menuView = null!;
    
    [Export]
    private Node2D _worldObjectContainer = null!;
    
    
    private bool _isMenuOpen = false;
    
    private ObjectRenderQueue _renderQueue = null!;

    public override void _Ready() {
        _renderQueue = ObjectRenderQueue.Instance;
        
        RenderNewObjects();
        LoadGameState();
        
        base._Ready();
    }

    private void RenderNewObjects() {
        var item = _renderQueue.GetNextRenderItem();
        
        while (item is not null) {
            item.Position = new Vector2(Random.Shared.Next(500), Random.Shared.Next(500));
            _worldObjectContainer.AddChild(item);
            
            item = _renderQueue.GetNextRenderItem();
        }
    }

    private void LoadGameState() {
        GameState.Instance.RestoreWorld(_worldObjectContainer);
    }

    private void SaveGameState() {
        GameState.Instance.SaveWorld(_worldObjectContainer);
    }

    private void _onOpenMenuButtonPressed() {
        _menuView.Visible = !_menuView.Visible;
        GetTree().Paused = _menuView.Visible;
    }
}