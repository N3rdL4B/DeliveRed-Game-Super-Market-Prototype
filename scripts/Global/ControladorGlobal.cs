using Godot;
using System;

public class ControladorGlobal : Spatial
{
	private HTTPRequest _httpRequest;

	public override void _Ready()
	{
		// Crear nodo de solicitud HTTP
		_httpRequest = new HTTPRequest();
		AddChild(_httpRequest);

		// Primero conectar la señal
		_httpRequest.Connect("request_completed", this, nameof(OnRequestCompleted));

		// Luego hacer la petición
		string url = "http://localhost:5069/DeliveRedApi/Productos/GetProductosByIdAmbienteNegocio?idAmbienteNegocio=6";
		GD.Print("🌐 Solicitando productos desde API...");

		var error = _httpRequest.Request(url);
		if (error != Error.Ok)
			GD.PrintErr($"❌ Error en request: {error}");
	}

	private void OnRequestCompleted(int result, int responseCode, string[] headers, byte[] body)
	{
		string json = System.Text.Encoding.UTF8.GetString(body);
		GD.Print("✅ JSON recibido:");
		GD.Print(json);

		// Aquí luego podrás deserializar y mostrar en UI, por ahora solo lo imprime
	}
}
