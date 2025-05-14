using Godot;
using System;
using System.Collections.Generic;

public partial class GameState : Node
{
	private List<(string name, Vector2 position, bool visible)> _worldObjectStates = new();
	
	public static GameState Instance;

	public override void _Ready()
	{
		Instance = this;
	}
	
	public void ClearWorldState()
	{
		_worldObjectStates.Clear();
	}

	public void SaveWorld(Node worldView)
	{
		GD.Print("\n=== [GameState] Start saving the world ===\n");

		_worldObjectStates.Clear();

		if(worldView == null)
		{
			GD.PrintErr("worldView == null");
			return;
		}

		GD.Print("worldView: ", worldView.Name, " (", worldView.GetType(), ")");

		var children = worldView.GetChildren();
		int spriteCount = 0;

		foreach (var child in children)
		{
			if (child is Sprite2D)
				spriteCount++;
		}

		GD.Print("\nLiczba Sprite2D w worldView: ", spriteCount);

		foreach (Node child in children)
		{
			if (child == null)
			{
				GD.PrintErr("child == null");
				continue;
			}

			GD.Print("- child: ", child.Name, " (", child.GetType(), ")");

			if(child is Sprite2D sprite)
			{
				try
				{
					GD.Print("Sprite2D: ", sprite.Name, ", Position: ", sprite.Position, ", Visible: ", sprite.Visible);
					_worldObjectStates.Add((sprite.Name, sprite.Position, sprite.Visible));
				}
				catch (Exception ex)
				{
					GD.PrintErr($"Nie udało się zapisać {sprite.Name}: ", ex.Message);
				}
			}
			else
			{
				GD.Print("Pominięto dziecko bo to nie sprite - ", child.Name);
			}
		}

		GD.Print("\n=== [GameState] SaveWorld zakończony ===");
	}

	public void RestoreWorld(Node worldView)
	{
		foreach (Node child in worldView.GetChildren())
		{
			if (child is Sprite2D sprite)
			{
				var saved = _worldObjectStates.Find(x => x.name == sprite.Name);
				if (!string.IsNullOrEmpty(saved.name))
				{
					sprite.Position = saved.position;
					sprite.Visible = saved.visible;
				}
			}
		}
	}
	
	public bool HasSavedWorld => _worldObjectStates.Count > 0;
}
