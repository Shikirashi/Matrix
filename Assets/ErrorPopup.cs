using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ErrorPopup : MonoBehaviour {
	[SerializeField]
	TMP_Text errorText;
	[SerializeField]
	GameObject errorPanel;
	[SerializeField]
	float errorDisplayTime;

	private void Start() {
		errorPanel.SetActive(false);
	}

	public void DisplayError(string message) {
		errorText.text = message;
		StartCoroutine(ErrorDisplay(message));
	}

	IEnumerator ErrorDisplay(string message) {
		errorPanel.SetActive(true);
		yield return new WaitForSeconds(errorDisplayTime);
		errorPanel.SetActive(false);
	}
}
