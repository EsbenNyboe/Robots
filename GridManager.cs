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

        if (Input.IsActionJustPressed("ui_down"))
        {
            SetGridCell(_beltScene, 0, 0);
        }
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

    public void SetGridCell(PackedScene gridCellScene, int x, int y)
    {
        _grid[y][x].QueueFree();
        var gridCell = InstantiateGridCell(gridCellScene, x, y);
        _grid[y][x] = gridCell;
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