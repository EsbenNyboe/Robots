using Godot;
using System;

public partial class Robot : AnimatedSprite2D
{
    private RobotState _state;

    [Export]
    private RobotState State
    {
        get => _state;
        set
        {
            _state = value;
            switch (_state)
            {
                case RobotState.Idle:
                    Animation = "idle";
                    Play();
                    break;
                case RobotState.Moving:
                    Animation = "moving";
                    Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Export]
    private float _speed = 1;

    private Vector2 _target;
    
    public override void _Ready()
    {
        base._Ready();
        GridManager.OnOrderUnitsToPosition += OnOrderUnitsToPosition;
        State = RobotState.Idle;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GridManager.OnOrderUnitsToPosition -= OnOrderUnitsToPosition;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        switch (_state)
        {
            case RobotState.Idle:
                break;
            case RobotState.Moving:
                var diff = _target - Position;
                var direction = diff.Normalized();
                var velocity = direction * _speed * (float)delta;
                Position += velocity;
                
                if (Position.DistanceTo(_target) < 1)
                {
                    State = RobotState.Idle;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnOrderUnitsToPosition(Vector2 mousePosition)
    {
        State = RobotState.Moving;
        _target = mousePosition;
    }
}

public enum RobotState
{
    Idle,
    Moving
}
