using Godot;
using System;

using SketchyGame.scenes.gui_components;

public partial class SaveitemComponent : Button
{
	public SaveItem Model { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("SaveItemComponent został utworzony!");
		// TextureRect thumbnailRect = GetNode<TextureRect>("%ThumbnailRect");
		Label description = GetNode<Label>("%Description");
	
		// thumbnailRect.Texture = Model.Thumbnail;
		description.Text = "Save  " + Model.formattedDate;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_pressed() 
	{
		GetTree().ChangeSceneToFile("user://saves/" + Model.fileName);
	}
}
