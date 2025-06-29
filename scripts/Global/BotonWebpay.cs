using Godot;
using System;

public class BotonWebpay : StaticBody
{
	public void AbrirWebpay()
	{
		OS.ShellOpen("https://delivered.cl");
	}
}
