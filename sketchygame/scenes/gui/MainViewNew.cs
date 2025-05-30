using System;
using Godot;
using SketchyGame.scenes.Autoloads;

namespace SketchyGame.scenes.gui;

public partial class MainViewNew : Node2D {
	[Export]
	private Control _menuView = null!;
	
	[Export]
	private Node2D _worldObjectContainer = null!;
	
	private bool _isMenuOpen;
	
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
			item.Owner = _worldObjectContainer.Owner;
			
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
	
	private void _onSaveGameButtonPressed()
	{
		// sprawdzenie czy katalog saves istnieje
		var dir = DirAccess.Open("user://");
		if(!dir.DirExists("saves"))
		{
			var err = dir.MakeDir("saves");
			if (err != Error.Ok)
			{
				GD.PrintErr("Nie udało się utworzyć folderu saves: " + err);
				return;
			}
		}

		// data
		DateTime now = DateTime.Now;
		string timestamp = now.ToString("yyyyMMdd-HHmmss");

		// pełna ścieżka
		string fullPath = $"user://saves/{timestamp}.tscn";
	
		// spakowanie sceny i zapisanie pliku
		var node = GetNode<Node2D>("./");
		var scene = new PackedScene();
		scene.Pack(node);

		var result = ResourceSaver.Save(scene, fullPath);
		if(result == Error.Ok)
		{
			GD.Print("Gra zapisana: " + timestamp);
			
			var popup = GetNode<AcceptDialog>("SaveSuccessPopup");
			popup.Visible = true;
			GetTree().CreateTimer(1.0).Timeout += () => popup.Hide();
		}
		else
		{
			GD.PrintErr("Błąd przy zapisie gry: " + result);
		}
	}
}
