using Godot;
using System;

using SketchyGame.scenes.gui_components;

public partial class SaveView : Control
{
	public override void _Ready()
	{
		string savePath = "user://saves";
		var scrollContainer = GetNode<ScrollContainer>("ScrollContainer");

		var dir = DirAccess.Open(savePath);

		if(dir == null)
		{
			scrollContainer.Visible = false;
			return;
		}

		dir.ListDirBegin();
		string fileName = dir.GetNext();
		
		bool hasAnySave = false;
		
		while(!string.IsNullOrEmpty(fileName))
		{
			if(!dir.CurrentIsDir() && fileName.EndsWith(".tscn"))
			{
				hasAnySave = true;
				GD.Print("Znaleziono zapis: " + fileName);
				
				var saveItem = new SaveitemComponent();
				GetNode<Container>("ScrollContainer/HFlowContainer").AddChild(saveItem);
			}
			fileName = dir.GetNext();
		}

		dir.ListDirEnd();
		
		// jak nie było żadnych zapisów to zostawał scroll i dodatkowo
		// przemieszczał się na górę ekranu, więc dałem flagę by robił
		// !visible lub visible w zależności czy coś w nim jest czy nie
		scrollContainer.Visible = hasAnySave;
	}
}
