using Godot;
using System;
using SketchyGame.scenes.Autoloads;
using System.Collections.Generic;
using System.Text.Json;

namespace SketchyGame.scenes.gui_components;

public partial class AddObject : Button
{

    private Dictionary<string, string> objects = [];
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
                objects[objectName] = filePath;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to read filenames {file}: {ex.Message}");
            }
        }

        return;
    }

    public void AddObjectToGame()
    {

        // Get image from sketchpad
        // ? Jak dostać stan sketchpada ? 




        // Run CnnClient
        // string responce = CnnClient.GetCnnOpinion(jsonContent); // jsonContent powinenbyć {"image": [0,0,0,0....]}

        // string className = "";
        // try
        // {
        //     using var doc = JsonDocument.Parse(responce);
        //     if (doc.RootElement.TryGetProperty("class_name", out var classNameElement))
        //     {
        //         className = classNameElement.GetString();
        //     }
        //     else
        //     {
        //         GD.PrintErr("class_name field not found in JSON response.");
        //     }
        // }
        // catch (Exception ex)
        // {
        //     GD.PrintErr($"Failed to parse JSON response: {ex.Message}");
        // }

        // var result = className;

        var result = "guitar";

        if (objects.Keys.Contains(result))
        {
            GD.Print(objects[result]);
            ObjectRenderQueue.Instance.PushSceneToRenderQueue(objects[result]);
            GetTree().ChangeSceneToFile("res://scenes/gui/main_view_new.tscn");
        }
        else
        {
            GD.Print("not found");
            GD.PushError("object not found");
        }
        return;
    }
}
