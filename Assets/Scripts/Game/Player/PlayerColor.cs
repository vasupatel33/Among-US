using System.Collections.Generic;
using UnityEngine;

public class PlayerColor
{
	private static List<Color> colors = new List<Color>
	{
		new Color(1f, 0f, 0f),
		new Color(0f, 0.1f, 1f),
		new Color(0f, 0.6f, 0f),
		new Color(1f, 0.3f, 0.9f),
		new Color(1f, 0.4f, 0f),
		new Color(1f, 0.9f, 0.1f),
		new Color(0.2f, 0.2f, 0.2f),
		new Color(0.9f, 1f, 1f),
		new Color(0.6f, 0f, 0.6f),
		new Color(0.7f, 0.2f, 0f),
		new Color(0f, 1f, 1f),
		new Color(0.1f, 1f, 0.1f)
	};

	public static Color red => colors[0];

	public static Color blue => colors[1];

	public static Color green => colors[2];

	public static Color pink => colors[3];

	public static Color orange => colors[4];

	public static Color yellow => colors[5];

	public static Color black => colors[6];

	public static Color white => colors[7];

	public static Color purple => colors[8];

	public static Color brown => colors[9];

	public static Color cyan => colors[10];

	public static Color lime => colors[11];

	public static Color GetColor(EPlayerColor playerColor)
	{
		return colors[(int)playerColor];
	}
}
