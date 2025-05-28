using Godot;
using System;

public class SaveItem
{
	// public int SaveId { get; set; }
	// public Texture2D Thumbnail { get; set; }
	public string fileName { get; set; }
	
	public string formattedDate
	{
		get
		{
				// usuwamy ".tscn" z końca
				var name = fileName.Replace(".tscn", "");

				// parsujemy datę z formatu "yyyyMMdd-HHmmss"
				DateTime date = DateTime.ParseExact(name, "yyyyMMdd-HHmmss", null);

				// zwracamy jako "dd-MM-yyyy HH:mm"
				return date.ToString("dd-MM-yyyy HH:mm");
		}
	}
}
