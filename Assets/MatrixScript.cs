using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MatrixScript : MonoBehaviour {
	[SerializeField]
	TMP_InputField rowInput, colInput;
	[SerializeField]
	GridLayoutGroup cellContainer;
	[SerializeField]
	int row, col, input;
	[SerializeField]
	GameObject cellPrefab;
	[SerializeField]
	public int[,] matrix;
	void Start() {
		cellContainer = transform.GetChild(1).GetComponent<GridLayoutGroup>();
		rowInput = transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
		colInput = transform.GetChild(0).GetChild(1).GetComponent<TMP_InputField>();
		RecalculateGrid();
	}

	void Update() {

	}

	public void RecalculateGrid() {
		cellContainer.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		if (int.TryParse(rowInput.text, out row)) {
			row = int.Parse(rowInput.text);
			//Debug.Log("Row is " + row);
		}
		else {
			Debug.LogError("Can't parse row");
			return;
		}
		if (int.TryParse(colInput.text, out col)) {
			col = int.Parse(colInput.text);
			cellContainer.constraintCount = col;
			//Debug.Log("Column is " + col);
		}
		else {
			Debug.LogError("Can't parse column");
			return;
		}
		//StartCoroutine(RefreshGrid());
		if (cellContainer.transform.childCount != (row * col)) {
			Debug.Log(transform.name + " purging existing cells");
			/*for (int i = cellContainer.transform.childCount - 1; i >= 0; i--) {
				Destroy(cellContainer.transform.GetChild(0).gameObject);
			}*/
			Destroy(cellContainer.gameObject);
			cellContainer = new GameObject("cellContainer").AddComponent<GridLayoutGroup>();
			cellContainer.cellSize = new Vector2(128, 128);
			cellContainer.spacing = new Vector2(12, 12);
			cellContainer.startCorner = GridLayoutGroup.Corner.UpperLeft;
			cellContainer.startAxis = GridLayoutGroup.Axis.Horizontal;
			cellContainer.childAlignment = TextAnchor.UpperCenter;
			cellContainer.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			cellContainer.constraintCount = col;
			cellContainer.transform.SetParent(transform, false);
			while (cellContainer.transform.childCount < (row * col)) {
				GameObject cell = Instantiate(cellPrefab, cellContainer.transform);
				cell.name = transform.name + "_" + (cellContainer.transform.childCount);
			}
		}
		for (int i = 0; i < cellContainer.transform.childCount; i++) {
			if (!int.TryParse(cellContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_InputField>().text, out input)) {
				//Debug.LogError("There is an empty cell");
				return;
			}
		}
		matrix = new int[row, col];
		int matrixIndex = 0;
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				matrix[i, j] = int.Parse(cellContainer.transform.GetChild(matrixIndex).transform.GetChild(0).GetComponent<TMP_InputField>().text);
				matrixIndex++;
				//Debug.Log(matrix[i, j]);
			}
		}
	}

	public void InsertMatrix(int row, int col, int[,] matrix) {
		this.matrix = matrix;
		cellContainer.constraintCount = col;
		if (cellContainer.transform.childCount < row * col) {
			while (cellContainer.transform.childCount < (row * col)) {
				GameObject cell = Instantiate(cellPrefab, cellContainer.transform);
			}
		}
		int matrixIndex = 0;
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				matrix[i, j] = int.Parse(cellContainer.transform.GetChild(matrixIndex).transform.GetChild(0).GetComponent<TMP_InputField>().text);
				matrixIndex++;
				//Debug.Log(matrix[i, j]);
			}
		}
	}

	IEnumerator RefreshGrid() {
		if (cellContainer.transform.childCount != (row * col)) {
			Debug.Log(transform.name + " purging existing cells");
			for (int i = 0; i < cellContainer.transform.childCount; i++) {
				//Debug.Log(cellContainer.transform.GetChild(0).name);
				Destroy(cellContainer.transform.GetChild(0).gameObject);
				yield return new WaitForSeconds(2);
			}
			while (cellContainer.transform.childCount - 1 < (row * col)) {
				GameObject cell = Instantiate(cellPrefab, cellContainer.transform);
				cell.name = transform.name + "_" + (cellContainer.transform.childCount - 1);
				yield return new WaitForSeconds(2);
			}
			/*if (cellContainer.transform.childCount - 1 == (row * col)) {
				Debug.Log(transform.name + " count is: " + (cellContainer.transform.childCount - 1));
				Debug.Log(transform.name + " rowXcol is: " + (row * col));
			}
			else {
				Debug.Log(transform.name + " count is: " + (cellContainer.transform.childCount - 1));
				Debug.Log(transform.name + " rowXcol is: " + (row * col));
			}*/
		}
	}

	public int GetRow() {
		return row;
	}
	public int GetCol() {
		return col;
	}
	public int[,] GetMatrix() {
		return matrix;
	}

}
