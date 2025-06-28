using Godot;
using System;

public class Botellainteractiva : Area
{
	[Export] public string Mensaje = "Objeto Recogido!";

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	private async void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Jugador")) // Asegúrate que tu jugador tenga ese grupo
		{
			GD.Print(Mensaje);

			var huds = GetTree().GetNodesInGroup("HUD"); // minúscula, debe coincidir con el grupo que asignaste en el editor

			if (huds.Count > 0)
			{
				var hud = huds[0] as HUD;
				if (hud != null)
				{
					GD.Print("✅ HUD encontrado por grupo (cast correcto)!");
					hud.SumarBotella();
				}
				else
				{
					GD.PrintErr("❌ El nodo en grupo 'hud' no es del tipo HUD");
				}
			}
			else
			{
				GD.PrintErr("❌ HUD no encontrado en grupo 'hud'");
			}

			//this.Visible = false;
			QueueFree(); // si preferís eliminarla
		}
	}
}
