using Godot;
using OpenCvSharp;
using System.Collections.Generic;
using System.Linq;

public static class PolygonGenerator
{
	public static Vector2[] GeneratePolygon(Texture2D edgeTexture, float delta)
	{
		Image image = edgeTexture.GetImage();

		int width = image.GetWidth();
		int height = image.GetHeight();
		byte[] data = image.GetData();

		Mat edgeMat = new Mat(height, width, MatType.CV_8UC1);
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				int i = (y * width + x) * 2;
				byte alpha = data[i + 1];
				edgeMat.Set(y, x, alpha > 0 ? 255 : 0);
			}
		}

		Cv2.FindContours(edgeMat, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

		if (contours.Length == 0)
			return new Vector2[0];

		Point[] largest = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();
		
		List<Vector2> sampled = new();
		double deltaSqr = delta * delta;

		Point prev = largest[0];
		sampled.Add(new Vector2(prev.X, prev.Y));

		foreach (Point p in largest)
		{
			if ((p.X - prev.X) * (p.X - prev.X) + (p.Y - prev.Y) * (p.Y - prev.Y) >= deltaSqr)
			{
				sampled.Add(new Vector2(p.X, p.Y));
				prev = p;
			}
		}

		return sampled.ToArray();
	}
}
