using Godot;
using System;

public class HUD : CanvasLayer
{
	private Label _contadorLabel;
	private int _botellas = 0;

	private Label _labelMensaje;
	private Control _panel;
	private Timer _ocultarPanelTimer;

	DynamicFont fontHUDPrincipal = new DynamicFont();
	DynamicFont fontMensajes = new DynamicFont();

	public override void _Ready()
	{
		_contadorLabel = GetNode<Label>("BotellaCounterLabel");
		_panel = GetNode<Control>("Panel");
		_labelMensaje = _panel.GetNode<Label>("Mensaje");
		_ocultarPanelTimer = GetNode<Timer>("OcultarPanelTimer");

		// Configurar fuentes
		fontHUDPrincipal.FontData = GD.Load<DynamicFontData>("res://fonts/Comfortaa-VariableFont_wght.ttf");
		fontHUDPrincipal.Size = 25;
		_contadorLabel.AddFontOverride("font", fontHUDPrincipal);

		fontMensajes.FontData = GD.Load<DynamicFontData>("res://fonts/Comfortaa-VariableFont_wght.ttf");
		fontMensajes.Size = 12;
		_labelMensaje.AddFontOverride("font", fontMensajes);

		// Inicial
		_contadorLabel.Text = "Objetos en el carrito: 0";
		_contadorLabel.Visible = true;
		_panel.Visible = false;

		_ocultarPanelTimer.Connect("timeout", this, nameof(OnOcultarPanelTimeout));
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

	public void ImprimirProductosPanel(string mensaje)
	{
		GD.Print("🔔 Mostrando mensaje: " + mensaje);

		_labelMensaje.Text = mensaje;
		_panel.Visible = true;

		_ocultarPanelTimer.Stop(); // Reiniciar el timer
		_ocultarPanelTimer.Start(); // Comienza de nuevo
	}

	private void OnOcultarPanelTimeout()
	{
		_panel.Visible = false;
	}
}
