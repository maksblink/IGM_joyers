using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

public partial class LightOnClick : ClickActionResource {
    public override void ClickAction(Godot.Collections.Array<Node> callArgs) {
        GD.Print(callArgs);

        if (callArgs[0] is PointLight2D light2D)
            GD.Print(light2D.Position, " Lampa");
    }
}