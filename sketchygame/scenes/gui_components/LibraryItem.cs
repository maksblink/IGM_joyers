using Godot;

namespace SketchyGame.scenes.gui_components;

public partial class LibraryItem : Button {
	[Signal]
	public delegate void LibraryItemPressedEventHandler(string item);

	[Export]
	private PackedScene _worldObjectScene = null!;

	[Export]
	private string _worldObjectPath = string.Empty;

	[Export]
	private Texture2D _thumbnail;

	[Export]
	private string _itemName;

	public Texture2D Thumbnail {
		get => _thumbnail;
		set => _thumbnail = value;
	}

	public string ItemName {
		get => _itemName;
		set => _itemName = value;
	}

	public override void _Ready() {
		GetNode<TextureRect>("%Thumbnail").Texture = _thumbnail;
		GetNode<Label>("%Name").Text = _itemName;

		RetrieveSceneInformation();
	}

	private void RetrieveSceneInformation() {
		if (_worldObjectScene is not null) {
			// TODO: Get sprite and name from scene
		}
		else {
			// TODO: Get sprite and name from scene path
		}
	}

	private void _on_pressed() {
		EmitSignal(SignalName.LibraryItemPressed, _worldObjectPath);
	}
}
