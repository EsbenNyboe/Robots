using Godot;
using System;
using System.Collections.Generic;
using Robots;

public partial class GridManagerTest : Node2D
{
    [Export]
    private int _width = 10;
    
    [Export]
    private int _height = 10;

    [Export] private int _cellSize = 1;
    
    private List<List<GridCell>> _grid;
    
    [Export]
    private PackedScene _gridScene;

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
            GD.Print("Test");
            RebuildGrid();
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
        for (int x = 0; x < _width; x++)
        {
            _grid.Add(new List<GridCell>());
            for (int y = 0; y < _height; y++)
            {
                var gridCell = _gridScene.Instantiate<GridCell>();
                gridCell.X = x;
                gridCell.Y = y;
                gridCell.Scale = Vector2.One * _cellSize;
                gridCell.Position = new Vector2(x, y) * _cellSize;
                AddChild(gridCell);
                _grid[x].Add(gridCell);
            }
        }
    }
}