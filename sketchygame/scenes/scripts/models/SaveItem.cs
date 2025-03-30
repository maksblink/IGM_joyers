using Godot;
using System;

public class SaveItem
{
	public int SaveId { get; set; }
	public Texture2D Thumbnail { get; set; }
	public DateTime LastUsageDate { get; set; }
	
	public string SaveIdFormatted
	{
		get
		{
			string id = SaveId.ToString();
			return $"{new string('0', 2 - id.Length)}{id}";
		}
	}
}
