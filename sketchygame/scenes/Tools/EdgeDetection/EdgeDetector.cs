using System.Runtime.InteropServices;
using Godot;
using OpenCvSharp;

namespace SketchyGame.scenes.Tools.EdgeDetection;

public static class EdgeDetector {
	private const double CannyThreshold1 = 1; // Lower for more sensitivity
	private const double CannyThreshold2 = 2; // Keep ratio (e.g., 2x-3x threshold1)
	private const int GaussianBlurSize = 3; // Kernel size for Gaussian blur (odd number, e.g., 3 or 5)

	public static Texture2D ApplyEdgeDetection(Texture2D inputTexture) {
		if (inputTexture == null) {
			GD.PrintErr("EdgeDetector: Input texture is null.");
			return null;
		}

		using var gdImage = inputTexture.GetImage();

		if (gdImage == null || gdImage.IsEmpty()) {
			GD.PrintErr("EdgeDetector: Failed to get image data from texture or image is empty.");
			return null;
		}

		if (gdImage.GetFormat() != Image.Format.Rgba8 && gdImage.GetFormat() != Image.Format.Rgb8) {
			gdImage.Convert(Image.Format.Rgba8);
			if (gdImage.GetFormat() != Image.Format.Rgba8) {
				GD.PrintErr($"EdgeDetector: Could not convert image to RGBA8. Current format: {gdImage.GetFormat()}");
				return null;
			}
		}

		byte[] godotImageData = gdImage.GetData();
		int width = gdImage.GetWidth();
		int height = gdImage.GetHeight();
		MatType inputMatType = (gdImage.GetFormat() == Image.Format.Rgba8) ? MatType.CV_8UC4 : MatType.CV_8UC3;
		ColorConversionCodes colorConversionCode = (gdImage.GetFormat() == Image.Format.Rgba8)
			? ColorConversionCodes.RGBA2GRAY
			: ColorConversionCodes.RGB2GRAY;

		using Mat srcMat = Mat.FromPixelData(height, width, inputMatType, godotImageData);
		using Mat grayMat = new Mat();
		using Mat blurredMat = new Mat();
		using Mat edgesMat = new Mat();

		Cv2.CvtColor(srcMat, grayMat, colorConversionCode);
		Cv2.GaussianBlur(grayMat, blurredMat, new OpenCvSharp.Size(GaussianBlurSize, GaussianBlurSize), 0, 0,
			BorderTypes.Constant);
		Cv2.Canny(blurredMat, edgesMat, CannyThreshold1, CannyThreshold2);

		byte[] finalPixelData = new byte[width * height * 2];

		byte[] cannyData = new byte[width * height];
		if (edgesMat.IsContinuous()) {
			Marshal.Copy(edgesMat.Data, cannyData, 0, cannyData.Length);
		}
		else {
			for (int r = 0; r < edgesMat.Rows; ++r) {
				Marshal.Copy(edgesMat.Ptr(r), cannyData, r * edgesMat.Cols, edgesMat.Cols);
			}
		}


		for (int i = 0; i < cannyData.Length; i++) {
			byte edgeValue = cannyData[i];

			finalPixelData[i * 2] = 0;

			finalPixelData[i * 2 + 1] = (edgeValue == 255) ? (byte)255 : (byte)0;
		}

		Image resultImage = Image.CreateFromData(width, height, false, Image.Format.La8, finalPixelData);

		if (resultImage == null || resultImage.IsEmpty()) {
			GD.PrintErr("EdgeDetector: Failed to create result image (LA8) from edge data.");
			return null;
		}

		Texture2D resultTexture = ImageTexture.CreateFromImage(resultImage);
		return resultTexture;
	}
}
