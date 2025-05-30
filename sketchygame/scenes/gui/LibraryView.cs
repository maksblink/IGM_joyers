using Godot;
using SketchyGame.scenes.Autoloads;
using SketchyGame.scenes.gui_components;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.gui;

public partial class LibraryView : Control {
	private ObjectRenderQueue _renderQueueAutoload = null!;
	
	[Export]
	private HFlowContainer _libraryContainer = null!;

	public override void _Ready() {
		_renderQueueAutoload = ObjectRenderQueue.Instance;
		ConnectLibraryItems();
		
		base._Ready();
	}

	private void _OnLibraryItemPressed(string scenePath) {
		_renderQueueAutoload.PushSceneToRenderQueue(scenePath);
	}

	private void _OnLibraryItemTscnPressed(PackedScene scene) {
		var instance = scene.Instantiate<WorldObjectBase>();
		
		_renderQueueAutoload.PushNodeToRenderQueue(instance);
	}

	private void ConnectLibraryItems() {
		if (_libraryContainer is null) return;

		foreach (var child in _libraryContainer.GetChildren()) {
			if (child is not LibraryItem libraryItem) continue;
			
			libraryItem.LibraryItemPressed += _OnLibraryItemPressed;
			libraryItem.LibraryItemPressedTscn += _OnLibraryItemTscnPressed;
		}
	}

	private void DisconnectLibraryItems() {
		if (_libraryContainer is null) return;

		foreach (var child in _libraryContainer.GetChildren()) {
			if (child is not LibraryItem libraryItem) continue;

			libraryItem.LibraryItemPressed -= _OnLibraryItemPressed;
			libraryItem.LibraryItemPressedTscn -= _OnLibraryItemTscnPressed;
		}
	}
}
