using Godot;
using System;

public partial class LibraryItem : Button
{
	[Export] private Texture2D _thumbnail;
	[Export] private string _itemName;

	public Texture2D Thumbnail {get => _thumbnail; set => _thumbnail = value; } 
	public string ItemName {get => _itemName; set => _itemName = value; } 
	
	public override void _Ready()
	{
		GetNode<TextureRect>("%Thumbnail").Texture = _thumbnail;
		GetNode<Label>("%Name").Text = _itemName;
	}
}
