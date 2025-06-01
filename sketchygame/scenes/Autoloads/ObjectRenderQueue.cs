using System.Collections.Generic;
using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.Autoloads;

public partial class ObjectRenderQueue : Node {
	[Signal]
	public delegate void RenderQueueChangedEventHandler();

	[Signal]
	public delegate void EnteredRenderSceneEventHandler();

	public static ObjectRenderQueue Instance { get; private set; } = null!;

	private readonly Queue<WorldObjectBase> _renderQueue = [];

	public override void _Ready() {
		Instance ??= this;

		base._Ready();
	}

	public WorldObjectBase GetNextRenderItem() {
		if (_renderQueue.Count == 0) {
			return null;
		}

		var worldObject = _renderQueue.Dequeue();

		return worldObject;
	}

	public void PushNodeToRenderQueue(WorldObjectBase node) {
		_renderQueue.Enqueue(node);
	}

	public void PushSceneToRenderQueue(string scenePath /* res://scenes/WorldObjects/<>.tscn */) {
		var scene = GD.Load<PackedScene>(scenePath).Instantiate<WorldObjectBase>(); 
		PushNodeToRenderQueue(scene);
	}
}
