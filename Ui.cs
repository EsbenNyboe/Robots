using Godot;
using System;

public partial class Ui : Control
{
    [Export] private Button _spawnTileButton;
    [Export] private Button _spawnRobotButton;
    [Export] private GridManager _gridManager;

    public override void _Ready()
    {
        base._Ready();
        _spawnTileButton.Pressed += OnSpawnTileButtonPressed;
        _spawnRobotButton.Pressed += OnSpawnRobotButtonPressed;
    }

    private void OnSpawnTileButtonPressed()
    {
        _gridManager.SetSpawnType(SpawnType.Tile);
    }

    private void OnSpawnRobotButtonPressed()
    {
        _gridManager.SetSpawnType(SpawnType.Robot);
    }
}
