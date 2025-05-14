using System;
using Godot;
using SketchyGame.scenes.Autoloads;

namespace SketchyGame.scenes.gui;

public partial class MainView : Control {
	[Export]
	private Control _gameView = null!;
	
	[Export]
	private Control _menuView = null!;
	
	[Export]
	private SubViewport _worldView = null!;
	
	private bool _isMenuOpen = false;
	
	private ObjectRenderQueue _renderQueue = null!;

	public override void _Ready() {
		_renderQueue = ObjectRenderQueue.Instance;
		
		RenderObjects();
		
		base._Ready();
	}

	private void RenderObjects() {
		var item = _renderQueue.GetNextRenderItem();
		
		while (item is not null) {
			item.Position = new Vector2(Random.Shared.Next(500), Random.Shared.Next(500));
			_worldView.AddChild(item);
			
			item = _renderQueue.GetNextRenderItem();
		}
	}

	private void _onOpenMenuButtonPressed() {
		_menuView.Visible = !_menuView.Visible;
		GetTree().Paused = _menuView.Visible;
	}
}
