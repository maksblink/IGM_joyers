using System.Collections.Generic;
using System.Linq;
using Godot;
using OpenCvSharp;

namespace SketchyGame.scenes.SketchPad;

public partial class SketchPadExportTool : Node
{
    private MeshDataTool _mdt = new();
    
    public void ExportAsBitMap(Node meshContainer, Vector2I canvasSize)
    {
        var meshes = new List<MeshInstance2D>();

        foreach (var mesh in meshContainer.GetChildren())
        {
            if (mesh is not MeshInstance2D mesh2D) continue;
            meshes.Add(mesh2D);
        }
        
        var image = CreateImage(canvasSize.X, canvasSize.Y);

        foreach (var mesh in meshes.Select(meshInstance => meshInstance.Mesh))
        {
            if (mesh is not ArrayMesh arrayMesh) continue;

            _mdt.CreateFromSurface(arrayMesh, 0);

            for (var i = 0; i < _mdt.GetVertexCount() - 1; ++i)
            {
                var thisVert = (Vector3I)_mdt.GetVertex(i);
                var nextVert = (Vector3I)_mdt.GetVertex(i + 1);
                
                image = LineGenerate.GenerateLine(image, thisVert, nextVert);
            }
        }
    }

    private Mat CreateImage(int width, int height)
    {
        return new Mat(height, width, MatType.CV_8UC3); // uint8, 3 channels
    }
}

public static class LineGenerate
{
    public static Mat GenerateLine(Mat image, Vector3I start, Vector3I end)
    {
        Cv2.Line(image, start.X, start.Y, end.X, end.Y, Scalar.Black);
        return image;
    }
}