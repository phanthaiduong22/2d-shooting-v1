using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
	public GameObject DifficultyToggles;
	public void PlayGame()
	{
		SceneManager.LoadScene(3);

	}
	public void ReturnMainMenu()
	{
		SceneManager.LoadScene(0);

	}
	public void QuitGame()
	{
		Debug.Log("Quit!");
		Application.Quit();
	}

	#region Difficulty
	public void SetEasyDifficulty(bool isOn)
	{
		if (isOn)
		{
			GameValues.Difficulty = GameValues.Difficulties.Easy;
		}
	}
	public void SetMediumDifficulty(bool isOn)
	{
		if (isOn)
		{
			GameValues.Difficulty = GameValues.Difficulties.Medium;
		}
	}
	public void SetHardDifficulty(bool isOn)
	{
		if (isOn)
		{
			GameValues.Difficulty = GameValues.Difficulties.Hard;
		}
	}
	#endregion
}
