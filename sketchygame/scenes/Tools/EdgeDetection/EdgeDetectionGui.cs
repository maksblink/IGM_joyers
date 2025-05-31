using System.Collections.Generic;
using System.Linq;
using Godot;

namespace SketchyGame.scenes.Tools.EdgeDetection;

public partial class EdgeDetectionGui : Control {
	private OptionButton _imageSelector;
	private TextureRect _preFilter;
	private TextureRect _postFilter;
	private readonly List<string> _imagePaths = [];

	public override void _Ready() {
		_imageSelector = GetNode<OptionButton>("%ImageSelector");
		_preFilter = GetNode<TextureRect>("%PreFilter");
		_postFilter = GetNode<TextureRect>("%PostFilter");

		LoadImages();

		_imageSelector.Selected = 0;
		OnImageSelected(0);
	}

	private void LoadImages() {
		const string path = "res://assets/object_base";

		foreach (var fileName in ResourceLoader.ListDirectory(path).Where(file => file.EndsWith(".png"))) {
			_imagePaths.Add(path + "/" + fileName);
			_imageSelector.AddItem(fileName);
		}
	}

	private void OnImageSelected(int index) {
		var path = _imagePaths[index];
		var image = ResourceLoader.Load<Texture2D>(path);
		var filtered = EdgeDetector.ApplyEdgeDetection(image);

		_preFilter.Texture = image;
		_postFilter.Texture = filtered;
	}

	private void OnSavePressed() {
		const string path = "res://assets/object_base/filtered/";

		using var filtered = _postFilter.Texture;
		using var image = filtered.GetImage();
		var imageName = _imageSelector.GetItemText(_imageSelector.Selected);

		image.SavePng(path + "edges_" + imageName);
	}
}
