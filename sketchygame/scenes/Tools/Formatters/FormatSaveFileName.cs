using System;

namespace SketchyGame.scenes.tools.Formatters;

public static class FormatSaveFileName {
    public static string FormattedDate(string saveFileName) {
        // usuwamy ".tscn" z końca
        var name = saveFileName.Replace(".tscn", "");

        // parsujemy datę z formatu "yyyyMMdd-HHmmss"
        var date = DateTime.ParseExact(name, "yyyyMMdd-HHmmss", null);

        // zwracamy jako "dd-MM-yyyy HH:mm"
        return date.ToString("dd-MM-yyyy HH:mm");
    }
}
