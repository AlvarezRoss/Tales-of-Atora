using Godot;
using System;

public struct Stats
{
	// Called when the node enters the scene tree for the first time.
	int str;
	int constitution;
	int dex;
	int wis;
	int intel;

	Stats(int str,int con, int dex, int wis, int intel)
	{
		this.str = str;
		this.constitution = con;
		this.dex = dex;
		this.wis = wis;
		this.intel = intel;
	}
}
