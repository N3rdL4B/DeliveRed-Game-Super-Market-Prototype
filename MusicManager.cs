using Godot;
using System;

public class MusicManager : Node2D
{
	private AudioStreamPlayer2D musicPlayer;
	private AudioStream[] pistas;
	private int indice;

	public override void _Ready()
	{
		musicPlayer = GetNode<AudioStreamPlayer2D>("MusicPlayer");

		pistas = new AudioStream[]
		{
			GD.Load<AudioStream>("res://audio/musica_fondo/tema1.mp3"),
			GD.Load<AudioStream>("res://audio/musica_fondo/tema2.mp3"),
			GD.Load<AudioStream>("res://audio/musica_fondo/tema3.mp3")
		};

		// Escoger índice aleatorio al iniciar
		indice = new Random().Next(0, pistas.Length);

		musicPlayer.VolumeDb = -15f;
		musicPlayer.Stream = pistas[indice];
		musicPlayer.Play();

		musicPlayer.Connect("finished", this, nameof(OnMusicFinished));
	}

	private void OnMusicFinished()
	{
		indice = (indice + 1) % pistas.Length;
		musicPlayer.Stream = pistas[indice];
		musicPlayer.Play();
	}
}
