using System;
using Godot;
using SketchyGame.scenes.Autoloads;
using SketchyGame.scenes.gui_components;

namespace SketchyGame.scenes.gui;

public partial class LibraryView : Control {
    private ObjectRenderQueue _renderQueueAutoload = null!;
    
    [Export]
    private HFlowContainer _libraryContainer = null!;

    public override void _Ready() {
        _renderQueueAutoload = ObjectRenderQueue.Instance;
        ConnectLibraryItems();
        
        base._Ready();
    }

    private void _onLibraryItemPressed(string scenePath) {
        GD.Print("Add to queue");
        
        _renderQueueAutoload.PushSceneToRenderQueue(scenePath);
    }

    private void ConnectLibraryItems() {
        if (_libraryContainer is null) return;

        foreach (var child in _libraryContainer.GetChildren()) {
            if (child is not LibraryItem libraryItem) continue;
            
            libraryItem.LibraryItemPressed += _onLibraryItemPressed;
        }
    }

    private void DisconnectLibraryItems() {
        if (_libraryContainer is null) return;

        foreach (var child in _libraryContainer.GetChildren()) {
            if (child is not LibraryItem libraryItem) continue;

            libraryItem.LibraryItemPressed -= _onLibraryItemPressed;
        }
    }
}