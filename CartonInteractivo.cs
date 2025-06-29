using Godot;
using System;

public class CartonInteractivo : StaticBody
{
	[Export] public string Mensaje = "Objeto Recogido!";
	[Export] public string NombreProducto = "Caja Leche";
	[Export] public float Precio = 2500f;
	
	public override void _Ready()
	{
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
			GD.Print("🧹 Eliminando objeto");
			// Eliminamos el objeto recogido
			QueueFree();
		}
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
