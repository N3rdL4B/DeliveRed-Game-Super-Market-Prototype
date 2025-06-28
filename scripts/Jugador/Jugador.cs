using Godot;
using System;

public class Jugador : KinematicBody
{
	[Export] public float Speed = 1.0f;
	[Export] public float MouseSensitivity = 0.005f;

	private Vector3 _velocity = Vector3.Zero;
	private Vector2 _mouseDelta = Vector2.Zero;

	private Spatial _pivot;
	private Camera _camera;

	public override void _Ready()
	{
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		_pivot = GetNode<Spatial>("Pivot");
		_camera = GetNode<Camera>("Pivot/Camera");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			_mouseDelta = mouseEvent.Relative;
			RotateCamera();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector3 inputDir = Vector3.Zero;
		if (Input.IsActionPressed("ui_up")) inputDir.z -= 1;
		if (Input.IsActionPressed("ui_down")) inputDir.z += 1;
		if (Input.IsActionPressed("ui_left")) inputDir.x -= 1;
		if (Input.IsActionPressed("ui_right")) inputDir.x += 1;

		inputDir = inputDir.Normalized();

		Vector3 direction = Transform.basis.z * inputDir.z + Transform.basis.x * inputDir.x;
		direction = direction.Normalized() * Speed;

		_velocity.x = direction.x;
		_velocity.z = direction.z;

		MoveAndSlide(_velocity, Vector3.Up);
	}


	private void RotateCamera()
	{
		// Rotación horizontal (Jugador)
		RotateY(-_mouseDelta.x * MouseSensitivity);

		// Rotación vertical (Pivot)
		float verticalRotation = _pivot.RotationDegrees.x - _mouseDelta.y * MouseSensitivity * 100f;
		verticalRotation = Mathf.Clamp(verticalRotation, -89, 89);
		_pivot.RotationDegrees = new Vector3(verticalRotation, 0, 0);

		_mouseDelta = Vector2.Zero;
	}
	
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
		}
	}

}
