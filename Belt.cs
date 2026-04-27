using Godot;

namespace Robots;

public partial class Belt : Node2D
{
    private const float Speed = 1;
    [Export] private Belt? _input;
    [Export] private Belt? _output;

    [Export] private Item? _item;
    private float _itemProcess;

    public override void _Ready()
    {
        base._Ready();
        SetProcess(_item != null);
    }

    public bool TryInsertItem(Item item)
    {
        if (_item != null)
        {
            return false;
        }

        _item = item;
        SetProcess(true);
        return true;
    }

    public bool TryOutputItem()
    {
        if (_output == null || _item == null)
        {
            return false;
        }

        if (!_output.TryInsertItem(_item))
        {
            return false;
        }

        _item = null;
        SetProcess(false);
        return true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        var dt = (float)delta;
        if (!IsInstanceValid(_item))
        {
            return;
        }

        _itemProcess += dt * Speed;
        if (_itemProcess >= 1)
        {
            _itemProcess = 1;
            if (TryOutputItem())
            {
                return;
            }
        }

        // lerp from input to middle
        if (IsInstanceValid(_output))
        {
            _item.GlobalPosition = GlobalPosition.Lerp(_output.GlobalPosition, _itemProcess);
        }
    }
}