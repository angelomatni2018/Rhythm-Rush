using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameController
{
	public static string MainMenuScene = "MainMenuLevel";

	static List<string> level_names = new List<string>() {
		"Level1",
		"Maze-Level",
		"Final-level"
	};

	public static string GetNextSceneName(string currentName) {
		int ind = level_names.IndexOf (currentName);
		if (ind == level_names.Count - 1) {
			return MainMenuScene;
		}
		return level_names [ind + 1];
	}

	public static string GetSceneName(int index) {
		return level_names [index];
	}
}