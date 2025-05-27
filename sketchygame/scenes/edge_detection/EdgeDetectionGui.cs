using Godot;
using System;
using System.Collections.Generic;

public partial class EdgeDetectionGui : Control
{
	private OptionButton ImageSelector;
	private TextureRect PreFilter;
	private TextureRect PostFilter;
	private List<string> ImagePaths = new();

    public override void _Ready()
    {
		ImageSelector = GetNode<OptionButton>("%ImageSelector");
		PreFilter = GetNode<TextureRect>("%PreFilter");
		PostFilter = GetNode<TextureRect>("%PostFilter");

		LoadImages();

		ImageSelector.Selected = 0;
		OnImageSelected(0);
    }

    private void LoadImages()
	{
		string path = "res://assets/object base";

        foreach (string fileName in ResourceLoader.ListDirectory(path))
		{
			ImagePaths.Add(path + "/" + fileName);
			ImageSelector.AddItem(fileName);
		}
	}

	public void OnImageSelected(int index)
	{
		string path = ImagePaths[index];
		Texture2D image = ResourceLoader.Load<Texture2D>(path);
		Texture2D filtered = EdgeDetector.ApplyEdgeDetection(image);

		PreFilter.Texture = image;
		PostFilter.Texture = filtered;
	}

	public void OnSavePressed()
	{
		string path = "res://assets/object base/filtered/";

		Texture2D filtered = PostFilter.Texture;
		Image image = filtered.GetImage();
		string imageName = ImageSelector.GetItemText(ImageSelector.Selected);

		image.SavePng(path + "edges_" + imageName);
    }
}
