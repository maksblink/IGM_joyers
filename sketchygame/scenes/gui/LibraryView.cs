using Godot;
using SketchyGame.scenes.Autoloads;
using SketchyGame.scenes.gui_components;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.gui;

/// <summary>
/// Skrypt widoku `Library View`
/// </summary>
public partial class LibraryView : Control {
	private ObjectRenderQueue _renderQueueAutoload = null!;
	
	[Export]
	private HFlowContainer _libraryContainer = null!;
	
	[Export]
	private PackedScene _addObjectPopupScene = null!;

	/// <summary>
	/// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów.
	/// </summary>
	public override void _Ready() {
		_renderQueueAutoload = ObjectRenderQueue.Instance;
		ConnectLibraryItems();
		
		base._Ready();
	}

	/// <summary>
	/// Wywołuje funkcję <code>ObjectRenderQueue.PushSceneToRenderQueue()</code> aby dodać wybrany obiekt do kolejki.
	/// </summary>
	/// <param name="scenePath">Ścieżka do sceny obiektu.</param>
	private void _OnLibraryItemPressed(string scenePath) {
		_renderQueueAutoload.PushSceneToRenderQueue(scenePath);
	}

	/// <summary>
	/// Wywołuje funkcję <code>ObjectRenderQueue.PushSceneToRenderQueue()</code> aby dodać wybrany obiekt do kolejki.
	/// </summary>
	/// <param name="scene">Scena obiektu do dodania do kolejki.</param>
	private void _OnLibraryItemTscnPressed(PackedScene scene) {
		var instance = scene.Instantiate<WorldObjectBase>();
		
		_renderQueueAutoload.PushNodeToRenderQueue(instance);
	}

	/// <summary>
	/// Łączy wszystkie dostępne przyciski w aktualnym widoku z ich odpowiednimi funkcjami obsługującymi sygnał Pressed.
	/// </summary>
	private void ConnectLibraryItems() {
		if (_libraryContainer is null) return;

		foreach (var child in _libraryContainer.GetChildren()) {
			if (child is not LibraryItem libraryItem) continue;
			
			libraryItem.LibraryItemPressed += _OnLibraryItemPressed;
			libraryItem.LibraryItemPressedTscn += _OnLibraryItemTscnPressed;
			libraryItem.NotifyLibraryItemPressed += AddObjectAddedPopup;
		}
	}

	/// <summary>
	/// Rozłącza wszystkie dostępne przyciski od ich funkcji obsługujących sygnał Pressed.
	/// </summary>
	private void DisconnectLibraryItems() {
		if (_libraryContainer is null) return;

		foreach (var child in _libraryContainer.GetChildren()) {
			if (child is not LibraryItem libraryItem) continue;

			libraryItem.LibraryItemPressed -= _OnLibraryItemPressed;
			libraryItem.LibraryItemPressedTscn -= _OnLibraryItemTscnPressed;
			libraryItem.NotifyLibraryItemPressed -= AddObjectAddedPopup;
		}
	}

	private void AddObjectAddedPopup(string objectDictName) {
		if (_addObjectPopupScene is null) return;

		var addObjectPopup = _addObjectPopupScene.Instantiate<AddItemPopup>();
		addObjectPopup.ObjectName = objectDictName;
		addObjectPopup.SetLabelName();

		var notifierContainer = GetNode<VBoxContainer>("%Notificator");
		notifierContainer.AddChild(addObjectPopup);
	}
}
