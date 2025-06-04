using Godot;
using System;

public partial class PolygonDrawer : Node2D
{
	public Vector2[] Points = new Vector2[0];
	
	public override void _Draw()
	{
		foreach (var point in Points)
		{
			DrawCircle(point, 4f, Colors.Blue);
		}
	}
}
