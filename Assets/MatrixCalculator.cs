using TMPro;
using UnityEngine;
[System.Serializable]
public class MatrixCalculator : MonoBehaviour {
	int[,] matrixA, matrixB, matrixC;
	int rowA, colA, rowB, colB;
	[SerializeField]
	MatrixScript matA, matB, matC;
	[SerializeField]
	TMP_Dropdown dropdownA, dropdownB, dropdownOperation;
	void Start() {
		matA = transform.GetChild(0).GetComponent<MatrixScript>();
		matB = transform.GetChild(1).GetComponent<MatrixScript>();
		matC = transform.GetChild(2).GetComponent<MatrixScript>();
		//Debug.Log("Dropdown A value is " + dropdownA.value);
		//Debug.Log("Dropdown B value is " + dropdownB.value);
		if (dropdownA.value == 0) {
			dropdownB.value = 1;
		}
		else {
			dropdownB.value = 0;
		}
	}

	void Update() {

	}

	public void GetMatrices() {
		rowA = matA.GetRow();
		colA = matA.GetCol();
		rowB = matB.GetRow();
		colB = matB.GetCol();
		matA.RecalculateGrid();
		matB.RecalculateGrid();
		matrixA = matA.matrix;
		matrixB = matB.matrix;
	}

	public void UpdateDropdowns() {
		if (dropdownA.value == 0) {
			dropdownB.value = 1;
			rowA = matA.GetRow();
			colA = matA.GetCol();
			rowB = matB.GetRow();
			colB = matB.GetCol();
			matA.RecalculateGrid();
			matB.RecalculateGrid();
			matrixA = matA.matrix;
			matrixB = matB.matrix;
		}
		else {
			dropdownB.value = 0;
			rowA = matB.GetRow();
			colA = matB.GetCol();
			rowB = matA.GetRow();
			colB = matA.GetCol();
			matA.RecalculateGrid();
			matB.RecalculateGrid();
			matrixA = matB.matrix;
			matrixB = matA.matrix;
		}
	}

	public void CheckLegal() {
		if (dropdownOperation.value == 0) {
			//addition
			if ((rowA != rowB) && (colA != colB)) {
				Debug.LogError("The matrice sizes are not the same!");
				return;
			}
		}
		else {
			//multiply
			if (dropdownA.value == 0) {
				//A to B
				if (colA != rowB) {
					Debug.LogError("Column and row are not the same length!");
					return;
				}
			}
			else {
				//B to A
				if (colB != rowA) {
					Debug.LogError("Column and row are not the same length!");
					return;
				}
			}
		}
	}

	public void Calculate(int[,] A, int[,] B, int operation) {
		if (operation == 0) {
			//A x B
			matrixC = new int[A.GetLength(0), B.GetLength(1)];
		}
		else {
			//A + B
			matrixC = new int[A.GetLength(0), A.GetLength(1)];
		}
	}
}
