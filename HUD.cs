using Godot;
using System;
using System.Collections.Generic;

public class HUD : CanvasLayer
{
	private Label _contadorLabel;
	private int _botellas = 0;

	private Label _labelMensaje;
	private Control _panel;
	private Timer _ocultarPanelTimer;

	DynamicFont fontHUDPrincipal = new DynamicFont();
	DynamicFont fontMensajes = new DynamicFont();

	/**************************************************************/
	private VBoxContainer _itemsContainer;
	private Label _totalLabel;
	private float _total = 0f;
	/**************************************************************/

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
		
		_itemsContainer = GetNode<VBoxContainer>("PanelCarrito/ItemsContainer");
		_totalLabel = GetNode<Label>("PanelCarrito/TotalLabel");
		ActualizarTotal();
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
	
	private void ActualizarListaCarrito()
	{
		foreach (Node child in _itemsContainer.GetChildren())
		{
			child.QueueFree();
		}

		_total = 0;

		foreach (var item in _productos.Values)
		{
			var label = new Label
			{
				Text = $"{item.Nombre} x{item.Cantidad} - ${item.Precio * item.Cantidad}"
			};
			_itemsContainer.AddChild(label);

			_total += item.Precio * item.Cantidad;
		}

		ActualizarTotal();
	}

	private void ActualizarTotal()
	{
		_totalLabel.Text = $"🧾 Total: ${_total}";
	}
	
	public void AgregarProductoAlCarrito(string nombre, float precio)
	{
		if (_productos.ContainsKey(nombre))
		{
			_productos[nombre].Cantidad++;
		}
		else
		{
			_productos[nombre] = new ItemCarrito(nombre, precio);
		}

		ActualizarListaCarrito();
	}
	
	// Clase interna para guardar items
	private class ItemCarrito
	{
		public string Nombre;
		public float Precio;
		public int Cantidad;

		public ItemCarrito(string nombre, float precio)
		{
			Nombre = nombre;
			Precio = precio;
			Cantidad = 1;
		}
	}

	private Dictionary<string, ItemCarrito> _productos = new Dictionary<string, ItemCarrito>();
}
