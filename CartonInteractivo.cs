using Godot;
using System;

public class CartonInteractivo : StaticBody
{
	[Export] public string Mensaje = "Objeto Recogido!";
	[Export] public string NombreProducto = "Caja Leche";
	[Export] public float Precio = 2500f;
	
	private AudioStreamPlayer3D sfxPlayer;
	private Timer deleteTimer;

	public override void _Ready()
	{
		// Conectar detector
		if (HasNode("Detector"))
		{
			var areaDetector = GetNode<Area>("Detector");
			areaDetector.Connect("body_entered", this, nameof(OnBodyEntered));
			GD.Print("✅ Detector conectado correctamente");
		}
		else
		{
			GD.PrintErr("❌ No se encontró el nodo hijo 'Detector'");
		}

		// Obtener AudioStreamPlayer3D
		if (HasNode("SFXPlayer"))
			sfxPlayer = GetNode<AudioStreamPlayer3D>("SFXPlayer");
		else
			GD.PrintErr("❌ No se encontró el nodo hijo 'SFXPlayer'");

		// Obtener Timer para eliminar
		if (HasNode("DeleteTimer"))
		{
			deleteTimer = GetNode<Timer>("DeleteTimer");
			deleteTimer.Connect("timeout", this, nameof(OnDeleteTimerTimeout));
		}
		else
		{
			GD.PrintErr("❌ No se encontró el nodo hijo 'DeleteTimer'");
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Jugador"))
		{
			GD.Print($"🎒 {Mensaje}");

			var huds = GetTree().GetNodesInGroup("HUD");

			if (huds.Count > 0 && huds[0] is HUD hud)
			{
				GD.Print("✅ HUD encontrado y casteado correctamente");
				hud.SumarBotella();
				hud.ImprimirProductosPanel(Mensaje);
				hud.AgregarProductoAlCarrito(NombreProducto, Precio);
			}
			else
			{
				GD.PrintErr("❌ No se encontró el HUD o el casteo falló");
			}

 			var collider = GetNode<CollisionShape>("CollisionShape");
			if (collider != null)
				collider.Disabled = true;
			// Sonido
			if (sfxPlayer != null)
			{
				GD.Print("Intentando reproducir sonido...");
				sfxPlayer.Play();
				GD.Print("Se llamó Play en AudioStreamPlayer3D");
			}
			else
			{
				GD.PrintErr("sfxPlayer es null");
			}

			// Hacer invisible el mesh o todo el nodo para "desaparecer" visualmente
			Visible = false;

			// Iniciar timer para eliminar después de que suene el audio
			if (deleteTimer != null)
				deleteTimer.Start();
			else
				QueueFree();
		}
	}

	private void OnDeleteTimerTimeout()
	{
		QueueFree();
	}

	public void MostrarInfo()
	{
		string mensaje = $"{NombreProducto} - Precio: ${Precio}";
		GD.Print($"👁️ Apuntando a: {mensaje}");

		var huds = GetTree().GetNodesInGroup("HUD");
		if (huds.Count > 0 && huds[0] is HUD hud)
		{
			hud.ImprimirProductosPanel(mensaje);
		}
	}
}
