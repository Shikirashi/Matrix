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
	[SerializeField]
	ErrorPopup popup;
	string errorMsg;
	bool emptyCell;
	void Start() {
		cellContainer = transform.GetChild(1).GetComponent<GridLayoutGroup>();
		rowInput = transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
		colInput = transform.GetChild(0).GetChild(1).GetComponent<TMP_InputField>();
		emptyCell = true;
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
			errorMsg = "Can't parse row";
			Debug.LogError(errorMsg);
			popup.DisplayError(errorMsg);
			return;
		}
		if (int.TryParse(colInput.text, out col)) {
			col = int.Parse(colInput.text);
			cellContainer.constraintCount = col;
			//Debug.Log("Column is " + col);
		}
		else {
			errorMsg = "Can't parse column";
			Debug.LogError(errorMsg);
			popup.DisplayError(errorMsg);
			return;
		}
		PurgeMatrix();
		CheckEmpty();
	}

	void CheckEmpty() {
		for (int i = 0; i < cellContainer.transform.childCount; i++) {
			if (!int.TryParse(cellContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_InputField>().text, out input)) {
				errorMsg = "Empty cell at " + transform.name + " " + cellContainer.transform.GetChild(i).name;
				Debug.LogError(errorMsg);
				emptyCell = true;
				return;
			}
		}
		emptyCell = false;
		matrix = new int[row, col];
		int matrixIndex = 0;
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				matrix[i, j] = int.Parse(cellContainer.transform.GetChild(matrixIndex).transform.GetChild(0).GetComponent<TMP_InputField>().text);
				matrixIndex++;
			}
		}
	}

	public bool isEmptyCell() {
		for (int i = 0; i < cellContainer.transform.childCount; i++) {
			if (!int.TryParse(cellContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_InputField>().text, out input)) {
				errorMsg = "Empty cell at " + transform.name + " " + cellContainer.transform.GetChild(i).name;
				Debug.LogError(errorMsg);
				emptyCell = true;
			}
		}
		return emptyCell;
	}

	void PurgeMatrix() {
		if (cellContainer.transform.childCount != (row * col)) {
			Debug.Log(transform.name + " purging existing cells");
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
	}

	public void InsertMatrix(int row, int col, int[,] matrix) {
		this.matrix = matrix;
		this.row = row;
		this.col = col;
		rowInput.text = row.ToString();
		colInput.text = col.ToString();
		cellContainer.constraintCount = col;
		PurgeMatrix();
		int matrixIndex = 0;
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				cellContainer.transform.GetChild(matrixIndex).transform.GetChild(0).GetComponent<TMP_InputField>().text = matrix[i, j].ToString();
				matrixIndex++;
			}
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
