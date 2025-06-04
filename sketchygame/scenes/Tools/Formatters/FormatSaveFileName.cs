using System;

namespace SketchyGame.scenes.tools.Formatters;

/// <summary>
/// Klasa formatująca nazwę pliku zapisu na obiekt typu string.
/// </summary>
public static class FormatSaveFileName {
    /// <summary>
    /// Funkcja formatująca nazwę pliku zapisu na obiekt typu string.
    /// </summary>
    /// <param name="saveFileName">Nazwa pliku z zapisem gry.</param>
    /// <returns>Sformatowana nazwa pliku z zapisem gry.</returns>
    public static string FormatDate(string saveFileName) {
        // usuwamy ".tscn" z końca
        var name = saveFileName.Replace(".tscn", "");

        // parsujemy datę z formatu "yyyyMMdd-HHmmss"
        var date = DateTime.ParseExact(name, "yyyyMMdd-HHmmss", null);

        // zwracamy jako "dd-MM-yyyy HH:mm"
        return date.ToString("dd-MM-yyyy HH:mm");
    }
}
