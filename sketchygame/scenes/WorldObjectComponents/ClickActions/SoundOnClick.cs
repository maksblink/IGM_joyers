using System;
using System.Linq;
using Godot;
using SketchyGame.scenes.WorldObjects;

namespace SketchyGame.scenes.WorldObjectComponents.ClickActions;

/// <summary>
/// Klasa odtwarzająca dźwięk po wciśnięciu przycisku myszy.
/// </summary>
public partial class SoundOnClick : ClickActionResource {
    [Export]
    private string _audioStreamPlayer2DPath = null!;

    [Export]
    private Godot.Collections.Array<AudioStreamWav> _audioStreamWavs = [];

    /// <summary>
    /// Definicja zachowania do odtworzenia dźwięku.
    /// </summary>
    /// <param name="args">Dodatkowe argumenty wywołania funkcji</param>
    public override void ClickAction(params object[] args) {
        AudioStreamPlayer2D audioStreamPlayer = null!;
        WorldObjectBase worldObject = null!;

        foreach (var arg in args) {
            switch (arg) {
                case WorldObjectBase worldObjectBase:
                    worldObject = worldObjectBase;
                    break;
            }
        }

        if (worldObject is null) return;

        audioStreamPlayer = worldObject.GetNode<AudioStreamPlayer2D>(_audioStreamPlayer2DPath);

        if (audioStreamPlayer == null) return;
        if (audioStreamPlayer.IsPlaying()) return;
        if (_audioStreamWavs.Count == 0) return;

        var trackId = Random.Shared.Next(_audioStreamWavs.Count);
        audioStreamPlayer.Stream = _audioStreamWavs[trackId];

        audioStreamPlayer.Play();
    }
}