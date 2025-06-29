using Godot;
using System;

public class Jugador : KinematicBody
{
	[Export] public float Speed = 1.0f;
	[Export] public float MouseSensitivity = 0.005f;
	[Export] public float DistanciaRaycast = 10f;
	private Vector3 _velocity = Vector3.Zero;
	private Vector2 _mouseDelta = Vector2.Zero;

	private Spatial _pivot;
	private Camera _camera;

	public override void _Ready()
	{
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		_pivot = GetNode<Spatial>("Pivot");
		_camera = GetNode<Camera>("Pivot/Position3D/Camera");
	}

	public override void _Input(InputEvent @event)
{
	if (@event is InputEventMouseMotion mouseEvent)
	{
		_mouseDelta = mouseEvent.Relative;
		RotateCamera();
	}

	if (@event is InputEventMouseButton mb && mb.Pressed && mb.ButtonIndex == (int)ButtonList.Left)
	{
		GD.Print($"Click detectado: button {mb.ButtonIndex}");

		var spaceState = GetWorld().DirectSpaceState;

		var screenCenter = new Vector2(
			_camera.GetViewport().Size.x / 2,
			_camera.GetViewport().Size.y / 2
		);

		var from = _camera.ProjectRayOrigin(screenCenter);
		var to = from + _camera.ProjectRayNormal(screenCenter) * DistanciaRaycast;

		var result = spaceState.IntersectRay(from, to);

		if (result.Count > 0 && result.Contains("collider"))
		{
			var collider = result["collider"] as Node;
			GD.Print($"🎯 Click sobre collider: {collider.Name} ({collider.GetType()})");

			if (collider is BotonWebpay boton)
			{
				GD.Print("🖱️ Botón Webpay clickeado!");
				boton.AbrirWebpay();
			}
			else
			{
				GD.Print("❌ El objeto clickeado no es BotonWebpay");
			}
		}

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
		
		DetectarObjeto(); // 👈 Ahora sí, se detecta lo que estás mirando
	}
	
	private void DetectarObjeto()
	{
		var spaceState = GetWorld().DirectSpaceState;

		var screenCenter = new Vector2(
			_camera.GetViewport().Size.x / 2,
			_camera.GetViewport().Size.y / 2
		);

		var from = _camera.ProjectRayOrigin(screenCenter);
		var to = from + _camera.ProjectRayNormal(screenCenter) * DistanciaRaycast;

		// Consulta simple para cuerpos físicos
		var result = spaceState.IntersectRay(from, to);

		if (result.Count > 0 && result.Contains("collider"))
		{
			var nodoCollider = result["collider"] as Node;
			GD.Print("🎯 Collider detectado: " + nodoCollider.Name);

			// Buscar método MostrarInfo en collider o padres
			Node nodoConMetodo = null;
			Node currentNode = nodoCollider;

			while (currentNode != null)
			{
				if (currentNode.HasMethod("MostrarInfo"))
				{
					nodoConMetodo = currentNode;
					break;
				}
				currentNode = currentNode.GetParent();
			}

			if (nodoConMetodo != null)
			{
				GD.Print("✅ Llamando MostrarInfo en: " + nodoConMetodo.Name);
				nodoConMetodo.Call("MostrarInfo");
			}
			else
			{
				GD.Print("❌ No se encontró método MostrarInfo en el collider ni en sus padres");
			}
		}
		else
		{
			GD.Print("❌ No se detectó nada con el raycast");
		}
	}


	private void MostrarJerarquiaAscendente(Node nodo)
	{
		GD.Print($"🔍 Jerarquía ascendente desde nodo: {nodo.Name}");
		Node current = nodo;
		int nivel = 0;

		while (current != null && nivel < 10) // limita para evitar loops
		{
			GD.Print($"Nivel {nivel}: {current.Name} (Tipo: {current.GetType()})");
			current = current.GetParent();
			nivel++;
		}
	}
}
