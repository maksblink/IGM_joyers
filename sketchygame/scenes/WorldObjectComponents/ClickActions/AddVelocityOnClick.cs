using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

public partial class AddVelocityOnClick : ClickActionResource {
    [Export]
    private float _velocityAdded = 50f;
    
    public override void ClickAction(params object[] args) {
        GD.Print("Derived action: ", args.Length);
        
        if (args?[0] is not WorldObjectBase worldObject) {
            GD.Print("Wrong arguments");
            return;
        }
        
        worldObject.ApplyImpulse(Vector2.Left * _velocityAdded);
    }
}