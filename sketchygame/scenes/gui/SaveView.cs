using Godot;
using System;
using System.Collections.Generic;

using SketchyGame.scenes.gui_components;

public partial class SaveView : Control
{
	public override void _Ready()
	{
		string savePath = "user://saves";
		var scrollContainer = GetNode<ScrollContainer>("ScrollContainer");

		var dir = DirAccess.Open(savePath);

		List<string> files = new List<string>();

		dir.ListDirBegin();
		string fileName = dir.GetNext();

		while(!string.IsNullOrEmpty(fileName))
		{
			if(!dir.CurrentIsDir() && fileName.EndsWith(".tscn"))
				files.Add(fileName);

			fileName = dir.GetNext();
		}
		dir.ListDirEnd();

		files.Reverse(); // najnowsze będą na początku

		foreach(var file in files)
		{
			var model = new SaveItem
			{
				fileName = file
			};

			var saveItem = GD.Load<PackedScene>("res://scenes/gui_components/save_item_component.tscn").Instantiate<SaveitemComponent>();
			saveItem.Model = model;
			GetNode<Container>("ScrollContainer/HFlowContainer").AddChild(saveItem);
		}
	}
}
