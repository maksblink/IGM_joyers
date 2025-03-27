using Godot;
using System;

public partial class SaveitemComponent : Button
{
	public SaveItem Model { get; set; }

	private string SaveIdFormatted
	{
		get
		{
			string id = Model.SaveId.ToString();
			return $"{new string('0', 2 - id.Length)}{id}";
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextureRect thumbnailRenderer = GetChild<TextureRect>(0);
		Text = $"Save {SaveIdFormatted} {Model.LastUsageDate.ToString("dd.MM.YYYY")}";
		thumbnailRenderer.Texture = Model.Thumbnail;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
