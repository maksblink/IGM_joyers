using System;
using Godot;
using SketchyGame.scenes.Autoloads;

namespace SketchyGame.scenes.gui;

/// <summary>
/// 
/// </summary>
public partial class MainViewNew : Node2D {
	/// <summary>
	/// Węzeł głównego widoku.
	/// </summary>
	[Export]
	private Control _menuView = null!;
	
	/// <summary>
	/// Kontener przechowujący wszystkie obiekty występujące w świecie gry.
	/// </summary>
	[Export]
	private Node2D _worldObjectContainer = null!;
	
	private bool _isMenuOpen;
	
	private ObjectRenderQueue _renderQueue = null!;

	/// <summary>
	/// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów.
	/// </summary>
	public override void _Ready() {
		_renderQueue = ObjectRenderQueue.Instance;
		
		RenderNewObjects();
		LoadGameState();
		
		base._Ready();
	}

	/// <summary>
	/// Dodaje obiekty z klasy ObjectRenderQueue, które zostały dodane w widoku LibraryView.
	/// </summary>
	private void RenderNewObjects() {
		var item = _renderQueue.GetNextRenderItem();
		
		while (item is not null) {
			item.Position = new Vector2(Random.Shared.Next(500), Random.Shared.Next(500));
			_worldObjectContainer.AddChild(item);
			item.Owner = _worldObjectContainer.Owner;
			
			item = _renderQueue.GetNextRenderItem();
		}
	}

	/// <summary>
	/// Wczytuje stan gry podczas przechodzenia między widokami.
	/// </summary>
	private void LoadGameState() {
		GameState.Instance.RestoreWorld(_worldObjectContainer);
	}

	/// <summary>
	/// Zapisuje stan gry podczas przechodzenia między widokami.
	/// </summary>
	private void SaveGameState() {
		GameState.Instance.SaveWorld(_worldObjectContainer);
	}

	/// <summary>
	/// Funkcja reagująca na wciśnięcie przycisku `Otwórz Menu` - otwiera menu.
	/// </summary>
	private void _onOpenMenuButtonPressed() {
		_menuView.Visible = !_menuView.Visible;
		GetTree().Paused = _menuView.Visible;
	}
	
	/// <summary>
	/// Funkcja tworząca nowy zapis gry w folderze lokalnym gry.
	/// </summary>
	private void _onSaveGameButtonPressed()
	{
		// Sprawdzenie, czy katalog saves istnieje
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
