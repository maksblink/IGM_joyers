using System;
using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

public partial class AddVelocityOnClick : ClickActionResource {
    [Export]
    private float _velocityAdded = 100f;
    
    public override void ClickAction(params object[] args) {
        GD.Print("Derived action: ", args.Length);
        
        if (args?[0] is not WorldObjectBase worldObject) {
            GD.Print("Wrong arguments");
            return;
        }

        var randX = Random.Shared.NextSingle() * 2f - 1f;

        var vec = new Vector2(randX, -1f).Normalized();
        
        worldObject.ApplyImpulse(vec * _velocityAdded);
    }
}