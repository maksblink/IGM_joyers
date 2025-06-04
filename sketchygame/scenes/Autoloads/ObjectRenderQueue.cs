using System.Collections.Generic;
using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.Autoloads;

/// <summary>
/// Klasa typu Singleton — używana do dodawania i renderowania obiektów.
/// </summary>
public partial class ObjectRenderQueue : Node {
    /// <summary>
    /// Sygnał wysyłany, gdy kolejka obiektów (_renderQueue) się zmieni, np. dodanie obiektu do kolejki lub usunięcie go.
    /// </summary>
    [Signal]
    public delegate void RenderQueueChangedEventHandler();

    /// <summary>
    /// Sygnał wysyłany, gdy wejdziemy do sceny, w której mają być wyrenderowane obiekty.
    /// </summary>
    [Signal]
    public delegate void EnteredRenderSceneEventHandler();

    /// <summary>
    /// Instancja tej klasy jako globalny Singleton.
    /// </summary>
    public static ObjectRenderQueue Instance { get; private set; } = null!;

    /// <summary>
    /// Kolejka obiektów gotowych do renderingu.
    /// </summary>
    private readonly Queue<WorldObjectBase> _renderQueue = [];

    /// <summary>
    /// Funkcja wywoływana po zainicjowaniu klasy w drzewie obiektów. Tworzy globalny Singleton tej klasy.
    /// </summary>
    public override void _Ready() {
        Instance ??= this;

        base._Ready();
    }

    /// <summary>
    /// Pobiera kolejny element z _renderQueue i usuwa go z kolejki.
    /// </summary>
    /// <returns>Następny element w kolejce lub NULL, jeżeli _renderQueue jest puste.</returns>
    public WorldObjectBase GetNextRenderItem() {
        if (_renderQueue.Count == 0) {
            return null;
        }

        var worldObject = _renderQueue.Dequeue();

        return worldObject;
    }

    /// <summary>
    /// Dodaje węzeł do _renderQueue.
    /// </summary>
    /// <param name="node">Węzeł do umieszczenia w kolejce.</param>
    public void PushNodeToRenderQueue(WorldObjectBase node) {
        _renderQueue.Enqueue(node);
    }

    /// <summary>
    /// Dodaje i tworzy instancję węzła do _renderQueue.
    /// </summary>
    /// <param name="scenePath">Ścieżka do sceny z obiektem.</param>
    public void PushSceneToRenderQueue(string scenePath /* res://scenes/WorldObjects/<>.tscn */) {
        var scene = GD.Load<PackedScene>(scenePath).Instantiate<WorldObjectBase>();
        PushNodeToRenderQueue(scene);
    }
}