using System.Collections.Generic;
using System.Linq;
using Godot;
using OpenCvSharp;

namespace SketchyGame.scenes.Tools.PolygonGenerator;

/// <summary>
/// Klasa odpowiedzialna za generowanie wielokątów na podstawie mapy krawędzi
/// Umożliwia konwersję obrazu krawędzi na uproszczony kontur w postaci tablicy punktów Vector2,
/// </summary>
public static class PolygonGenerator {
	/// <summary>
	/// Funckja generuje wielokąt reprezentujący zewnętrzny kontur obiektu na obrazie krawędziowym <paramref name="edgeTexture"/>
	/// W pierwszym kroku tworzy binarną maskę krawędzi na podstawie kanału alfa tekstury.
	/// Następnie stosuje operację morfologiczną zamykania (closing), aby domknąć drobne w konturze szczeliny.
	/// Na tak przetworzonym obrazie wykrywane są kontury zewnętrzne, z których wybierany jest największy.
	/// Ten kontur jest próbkowany w równych odstępach określonych przez parametr <paramref name="delta"/>.
	/// </summary>
	/// <param name="edgeTexture">Obraz krawędziowy, z którego generowany będzie kontur.</param>
	/// <param name="delta">Minimalna odległość (w pikselach) pomiędzy kolejnymi punktami konturu. Im większa wartość, tym mniej punktów w wielokącie.</param>
	/// <returns>Tablica punktów Vector2 reprezentujących uproszczony zewnętrzny kontur (wielokąt).</returns>
	public static Vector2[] GeneratePolygon(Texture2D edgeTexture, float delta) {
		Image image = edgeTexture.GetImage();

		int width = image.GetWidth();
		int height = image.GetHeight();
		byte[] data = image.GetData();

		// utworzenie binarnej maski krawędzi
		Mat edgeMat = new Mat(height, width, MatType.CV_8UC1);

		// Dla każdego piksela:
		// - odczytujemy wartość z data[i + 1]
		// - jeśli > 0, to znaczy, że jest tam krawędź -> ustawiamy piksel na 255 (biały)
		// - w przeciwnym wypadku → 0 (czarny)
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				int i = (y * width + x) * 2;
				byte alpha = data[i + 1];
				edgeMat.Set(y, x, alpha > 0 ? 255 : 0);
			}
		}
		
		// Zamykanie drobnych przerw w konturze
		Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
		Cv2.MorphologyEx(edgeMat, edgeMat, MorphTypes.Close, kernel);
		// FindContours skanuje binarny obraz (edgeMat) i znajduje wszystkie zamknięte kształty (kontury)
		Cv2.FindContours(edgeMat, out Point[][] contours, out _, RetrievalModes.External,
			ContourApproximationModes.ApproxSimple);
		// RetrievalModes.External – szuka tylko konturów zewnętrznych
		// ApproxSimple – upraszcza kontury (usuwa zbędne punkty w linii prostej)

		// jeśli nie znaleziono konturów -> zwracamy pustą tablicę
		if (contours.Length == 0)
			return new Vector2[0];

		// w przeciwnym wypadku -> wybieramy kontur o największym polu powierzchni
		Point[] largest = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();

		// próbkowanie punktów co delta

		// tworzymy listę punktów sampled (będzie naszym polygonem)
		List<Vector2> sampled = new();
		// deltaSqr – używamy kwadratu odległości
		double deltaSqr = delta * delta;

		// dodajemy pierwszy punkt konturu
		Point prev = largest[0];
		sampled.Add(new Vector2(prev.X, prev.Y));

		// iterujemy przez kolejne punkty
		foreach (Point p in largest) {
			// jeżeli odległość od poprzedniego punktu >= delta, dodajemy go do listy
			if ((p.X - prev.X) * (p.X - prev.X) + (p.Y - prev.Y) * (p.Y - prev.Y) >= deltaSqr) {
				sampled.Add(new Vector2(p.X, p.Y));
				prev = p;
			}
		}

		return sampled.ToArray();
	}
}
