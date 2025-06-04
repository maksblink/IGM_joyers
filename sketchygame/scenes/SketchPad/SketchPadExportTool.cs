using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;
using OpenCvSharp;
using SketchyGame.scenes.Autoloads;
using SketchyGame.scenes.Tools.Cnn;

namespace SketchyGame.scenes.SketchPad;

/// <summary>
/// Klasa generująca obraz.
/// </summary>
[GodotClassName("SketchPadExportTool")]
public partial class SketchPadExportTool : Node {
    private MeshDataTool _mdt = new();

    [Export]
    private string _savePath = string.Empty;

    private readonly Dictionary<string, string> _objectsPaths = [];

    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów.
    /// </summary>
    public override void _Ready()
    {
        string objectsPath = "/scenes/WorldObjects";
        string absPath = System.IO.Path.Join(System.IO.Directory.GetCurrentDirectory(), objectsPath);

        var files = System.IO.Directory.GetFiles(absPath);
        foreach (var file in files)
        {
            try
            {
                string fileName = System.IO.Path.GetFileName(file);
                string filePath = System.IO.Path.GetFullPath(file);
                var objectName = fileName.Split('_')[0];
                _objectsPaths[objectName] = filePath;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to read filenames {file}: {ex.Message}");
            }
        }

        return;
    }

    /// <summary>
    /// Funkcja eksportująca listę wierzchołków do obrazu w formacie BMP.
    /// </summary>
    /// <param name="meshContainer">Węzeł przechowujący wierzchołki obrazu</param>
    /// <param name="canvasSize">Wielkość obrazu, jaki ma być wygenerowany</param>
    public void ExportAsBitMap(Node meshContainer, Vector2I canvasSize)
    {
        var meshInstance2Ds = new List<MeshInstance2D>();

        foreach (var mesh in meshContainer.GetChildren())
        {
            if (mesh is not MeshInstance2D mesh2D) continue;
            meshInstance2Ds.Add(mesh2D);
        }

        var image = CreateImage(canvasSize.X, canvasSize.Y);

        foreach (var mesh in meshInstance2Ds.Select(meshInstance => meshInstance.Mesh))
        {
            if (mesh is not ArrayMesh arrayMesh) continue;

            var surface = mesh.SurfaceGetArrays(0);
            var vertices = (Godot.Collections.Array)surface[(int)Mesh.ArrayType.Vertex];

            for (var i = 0; i < vertices.Count - 1; ++i)
            {
                var thisVert = (Vector3I)vertices[i];
                var nextVert = (Vector3I)vertices[i + 1];

                image = LineGenerate.GenerateLine(image, thisVert, nextVert);
            }
        }

        AddObject(image);

        // In release mode (standalone exe) image will be saved in same folder as exe,
        // otherwise res://assets/exported
        var path = (OS.IsDebugBuild() ? CastPathToAbsolute(_savePath) : GetExecutablePath()) + CreateFileName();
        GD.Print(path);
        Cv2.ImWrite(path, image);
    }

    /// <summary>
    /// Funkcja dodająca obiekt do świata gry na podstawie wygenerowanego obrazu.
    /// </summary>
    /// <param name="image">Obraz w postaci macierzowej, jednokanałowy o typie uint_8</param>
    private async void AddObject(Mat image)
    {
        var array = new int[image.Cols][];

        // convert image to int_32 2D array
        for (var i = 0; i < image.Rows; i++)
        {
            array[i] = new int[image.Rows];
            for (var j = 0; j < image.Cols; j++)
            {
                array[i][j] = image.At<int>(i, j);
            }
        }

        // Serialize array - {"image" : [[0, 0, ...], [...], ...]}
        var jsonContent = JsonSerializer.Serialize(new { image = array });

        // Get Cnn Opinion
        var response = await CnnClient.GetCnnOpinion(jsonContent);
        var responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
        using var doc = JsonDocument.Parse(new ReadOnlyMemory<byte>(responseBytes));
        string className = doc.RootElement.GetProperty("class_name").GetString();

        GD.Print($"Predicted class: {className}");

        // Add Object
        if (_objectsPaths.Keys.Contains(className))
        {
            ObjectRenderQueue.Instance.PushSceneToRenderQueue(_objectsPaths[className]);
            GetTree().ChangeSceneToFile("res://scenes/gui/main_view_new.tscn");
        }
        else
        {
            GD.Print("not found");
            GD.PushError("object not found");
        }

    }

    /// <summary>
    /// Tworzy nazwę pliku dla obrazu na podstawie aktualnej daty.
    /// </summary>
    /// <returns></returns>
    private static string CreateFileName() {
        var date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return $"{date}_sketchy_draw.bmp";
    }

    /// <summary>
    /// Tworzy globalną ścieżkę do pliku na podstawie ścieżki względnej.
    /// </summary>
    /// <param name="path">Względna ścieżka do danego folderu.</param>
    /// <returns>Globalną ścieżkę do danego folderu</returns>
    private static string CastPathToAbsolute(string path) {
        return ProjectSettings.GlobalizePath(path);
    }

    /// <summary>
    /// Generuje ścieżkę, w której znajduje się plik wykonywalny gry.
    /// </summary>
    /// <returns>Ścieżka do pliku wykonywalnego z grą.</returns>
    private static string GetExecutablePath() {
        return OS.GetExecutablePath().GetBaseDir() + '\\';
    }

    private static Mat CreateImage(int width, int height) {
        return new Mat(height, width, MatType.CV_8UC1, Scalar.Black); // uint8, 3 channels
    }
}

/// <summary>
/// Generator prostej linii w obrazie na podstawie dwóch punktów.
/// </summary>
public static class LineGenerate {
    /// <summary>
    /// Renderuje prostę linie w obrazie pomiędzy dwoma punktami 3D.
    /// </summary>
    /// <param name="image">Obraz, w którym ma być wyrenderowana linia.</param>
    /// <param name="start">Punkt początkowy linii.</param>
    /// <param name="end">Punkt końcowy linii.</param>
    /// <returns></returns>
    public static Mat GenerateLine(Mat image, Vector3I start, Vector3I end) {
        Cv2.Line(image, start.X, start.Y, end.X, end.Y, Scalar.White);
        return image;
    }
}