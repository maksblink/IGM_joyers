using Godot;
using System;
using SketchyGame.scenes.WorldObjects;

public partial class GameState : Node
{
	public static GameState Instance;

	public override void _Ready()
	{
		Instance = this;
	}

	public void SaveWorld(Node worldView, Node tempStorage)
	{
		GD.Print("\n=== Zapisywanie stanu MainView ===");

		var children = worldView.GetChildren();

		foreach(Node child in children)
		{
			child.Reparent(tempStorage);
		}

		GD.Print("=== Zapisywanie zakończone ===\n");
	}

	public void RestoreWorld(Node tempStorage, Node worldView)
	{
		GD.Print("\n=== Przywracanie stanu gry ===");

		var children = tempStorage.GetChildren();
		GD.Print("Obiektów w tempStorage: ", children.Count);

		foreach (Node child in children)
		{
			child.Reparent(worldView);
		}

		GD.Print("=== Zakończono przywracanie ===\n");
	}
}
