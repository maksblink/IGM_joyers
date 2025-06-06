using Godot;

namespace SketchyGame.scenes.gui_components;

public partial class AddItemPopup : Panel {
    public string ObjectName { get; set; } = "";

    public void SetLabelName() {
        var label = GetNode<Label>("%Label");

        var baseTextTr = Tr("ADD_OBJECT_POPUP");
        var objectTextTr = Tr(ObjectName);
        
        label.Text = baseTextTr + " " + objectTextTr;
    }
    
    private void _onAutoOffTimeOut() {
        QueueFree();
    }
}