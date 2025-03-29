using Godot;
using System;

public partial class LibraryItem : Button
{
	[Export] private Texture2D _thumbnail;
	[Export] private string _name;

	public Texture2D Thumbnail {get => _thumbnail; set => _thumbnail = value; } 
	public string Name {get => _name; set => _name = value; } 
	
	public override void _Ready()
	{
		GetNode<TextureRect>("%Thumbnail").Texture = _thumbnail;
		GetNode<Label>("%Name").Text = _name;
	}
}
