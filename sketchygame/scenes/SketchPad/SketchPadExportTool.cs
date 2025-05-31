using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using OpenCvSharp;

namespace SketchyGame.scenes.SketchPad;

[GodotClassName("SketchPadExportTool")]
public partial class SketchPadExportTool : Node {
    private MeshDataTool _mdt = new();

    [Export]
    private string _savePath = string.Empty;

    private Dictionary<string, string> _paths = [];

    public string ExportAsBitMap(Node meshContainer, Vector2I canvasSize) {
        var meshInstance2Ds = new List<MeshInstance2D>();

        foreach (var mesh in meshContainer.GetChildren()) {
            if (mesh is not MeshInstance2D mesh2D) continue;
            meshInstance2Ds.Add(mesh2D);
        }

        var image = CreateImage(canvasSize.X, canvasSize.Y);

        foreach (var mesh in meshInstance2Ds.Select(meshInstance => meshInstance.Mesh)) {
            if (mesh is not ArrayMesh arrayMesh) continue;

            var surface = mesh.SurfaceGetArrays(0);
            var vertices = (Godot.Collections.Array)surface[(int)Mesh.ArrayType.Vertex];

            for (var i = 0; i < vertices.Count - 1; ++i) {
                var thisVert = (Vector3I)vertices[i];
                var nextVert = (Vector3I)vertices[i + 1];

                image = LineGenerate.GenerateLine(image, thisVert, nextVert);
            }
        }

        // In release mode (standalone exe) image will be saved in same folder as exe,
        // otherwise res://assets/exported
        var path = (OS.IsDebugBuild() ? CastPathToAbsolute(_savePath) : GetExecutablePath()) + CreateFileName();
        GD.Print(path);
        Cv2.ImWrite(path, image);

        return path;
    }

    private static string CreateFileName() {
        var date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return $"{date}_sketchy_draw.bmp";
    }

    private static string CastPathToAbsolute(string path) {
        return ProjectSettings.GlobalizePath(path);
    }

    private static string GetExecutablePath() {
        return OS.GetExecutablePath().GetBaseDir() + '\\';
    }

    private static Mat CreateImage(int width, int height) {
        return new Mat(height, width, MatType.CV_8UC3); // uint8, 3 channels
    }
}

public static class LineGenerate {
    public static Mat GenerateLine(Mat image, Vector3I start, Vector3I end) {
        Cv2.Line(image, start.X, start.Y, end.X, end.Y, Scalar.White);
        return image;
    }
}