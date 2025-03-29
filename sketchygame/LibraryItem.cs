using Godot;
using System;

public partial class LibraryItem : Button
{
	private TextureRect _thumbnail;
	private Label _name;

	public override void _Ready()
	{
		_thumbnail = GetNode<TextureRect>("%Thumbnail");
		_name = GetNode<Label>("%Name");
	}

	public override void _Process(double delta)
	{
		_thumbnail.Texture = texture;
		_name.Text = displayName;
	}
}
