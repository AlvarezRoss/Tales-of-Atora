using Godot;
using System;

public struct Stats
{
	// Called when the node enters the scene tree for the first time.
	public int str;
	public int constitution;
	public int dex;
	public int wis;
	public int intel;
	public int charisma;

	public Stats(int str,int wis, int intel, int con, int dex, int charisma)
	{
		this.str = str;
		this.constitution = con;
		this.dex = dex;
		this.wis = wis;
		this.intel = intel;
		this.charisma = charisma;
	}
}
