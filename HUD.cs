using Godot;
using System;

public class HUD : CanvasLayer
{
	private Label _contadorLabel;
	private int _botellas = 0;

	public override void _Ready()
	{
		// Buscar el label que debe llamarse exactamente así en el árbol
		_contadorLabel = GetNode<Label>("BotellaCounterLabel");

		// Asegurar que tenga una fuente visible
		var font = new DynamicFont();
		font.FontData = GD.Load<DynamicFontData>("res://fonts/Comfortaa-VariableFont_wght.ttf");
		font.Size = 25;  // Ajustá el tamaño para que se vea bien grande
		_contadorLabel.AddFontOverride("font", font);

		_contadorLabel.Text = "Objetos en el carrito: 0";

		_contadorLabel.Visible = true;

		GD.Print("✅ HUD _Ready ejecutado y label inicializado");
	}

	public void SumarBotella()
	{
		_botellas++;
		GD.Print("Sumando botella: " + _botellas);
		ActualizarContador();
	}

	private void ActualizarContador()
	{
		_contadorLabel.Text = $"Objetos en el carrito: {_botellas}";
	}
}
