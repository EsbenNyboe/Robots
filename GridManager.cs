using System;
using System.Collections.Generic;
using Godot;
using Robots;

public partial class GridManager : Node2D
{
    [Export] private PackedScene _gridCellScene;
    [Export] private PackedScene _beltScene;
    [Export] private int _width = 10;
    [Export] private int _height = 10;
    [Export] private int _cellSize = 1;

    private List<List<GridCell>> _grid;

    private SpawnType _currentSpawnType;

    public override void _EnterTree()
    {
        base._EnterTree();
        RebuildGrid();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_accept"))
        {
            RebuildGrid();
        }
    }
    
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton mouseButton)
        {
            var mousePosition = GetViewport().GetCamera2D().GetGlobalMousePosition();
            if (mouseButton.ButtonIndex == MouseButton.Left && !mouseButton.Pressed)
            {
                switch (_currentSpawnType)
                {
                    case SpawnType.None:
                        break;
                    case SpawnType.Tile:
                        SetCellAtPosition(mousePosition, _beltScene);
                        break;
                    case SpawnType.Robot:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (mouseButton.ButtonIndex == MouseButton.Right && !mouseButton.Pressed)
            {
                switch (_currentSpawnType)
                {
                    case SpawnType.None:
                        break;
                    case SpawnType.Tile:
                        SetCellAtPosition(mousePosition, _gridCellScene);
                        break;
                    case SpawnType.Robot:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public void SetSpawnType(SpawnType spawnType)
    {
        _currentSpawnType = spawnType;
    }
    
    private void RebuildGrid()
    {
        if (_grid != null)
        {
            foreach (var gridColumn in _grid)
            {
                foreach (var gridCell in gridColumn)
                {
                    gridCell.QueueFree();
                }
            }
        }

        _grid = new List<List<GridCell>>();
        for (var x = 0; x < _width; x++)
        {
            _grid.Add(new List<GridCell>());
            for (var y = 0; y < _height; y++)
            {
                var gridCell = InstantiateGridCell(_gridCellScene, x, y);
                _grid[x].Add(gridCell);
            }
        }
    }

    private void SetCellAtPosition(Vector2 mouseButtonPosition, PackedScene cellScene)
    {
        var (x, y) = GetCellCoordinatesFromPosition(mouseButtonPosition);
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return;
        }
        SetGridCell(cellScene, x, y);
    }

    private (int x, int y) GetCellCoordinatesFromPosition(Vector2 mouseButtonPosition)
    {
        var correctedX = mouseButtonPosition.X + (_cellSize * 0.5f);
        var correctedY = mouseButtonPosition.Y + (_cellSize * 0.5f);
        return ((int)correctedX / _cellSize, (int)correctedY / _cellSize);
    }

    private void SetGridCell(PackedScene gridCellScene, int x, int y)
    {
        _grid[x][y].QueueFree();
        var gridCell = InstantiateGridCell(gridCellScene, x, y);
        _grid[x][y] = gridCell;
    }
    
    private GridCell InstantiateGridCell(PackedScene gridCellScene, int x, int y)
    {
        var gridCell = gridCellScene.Instantiate<GridCell>();
        gridCell.X = x;
        gridCell.Y = y;
        gridCell.Scale = Vector2.One * _cellSize;
        gridCell.Position = new Vector2(x, y) * _cellSize;
        AddChild(gridCell);
        return gridCell;
    }
}

public enum SpawnType
{
    None,
    Tile,
    Robot
}