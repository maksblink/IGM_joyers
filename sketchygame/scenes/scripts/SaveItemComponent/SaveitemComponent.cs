using Godot;
using System;

public partial class SaveitemComponent : Button
{
	public SaveItem Model { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextureRect thumbnailRect = GetNode<TextureRect>("%ThumbnailRect");
		Label description = GetNode<Label>("%Description");
		
		description.Text = $"Save {Model.SaveIdFormatted} {Model.LastUsageDate.ToString("dd.MM.YYYY")}";
		thumbnailRect.Texture = Model.Thumbnail;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
