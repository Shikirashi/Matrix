using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class MatrixCalculator : MonoBehaviour {
	int[,] matrixA, matrixB, matrixC;
	int rowA, colA, rowB, colB, rowC, colC, scalarValue, determinantResult;
	[SerializeField]
	MatrixScript matA, matB, matC;
	[SerializeField]
	TMP_Dropdown dropdownA, dropdownB, dropdownOperation;
	[SerializeField]
	GameObject scalarCell, determinantCell;
	[SerializeField]
	ErrorPopup popup;
	string errorMsg;
	void Start() {
		if (SceneManager.GetActiveScene().buildIndex == 0) {
			matA = transform.GetChild(0).GetComponent<MatrixScript>();
			matB = transform.GetChild(1).GetComponent<MatrixScript>();
			matC = transform.GetChild(2).GetComponent<MatrixScript>();
		}
		else {
			if (determinantCell == null) {
				errorMsg = "Determinant cell missing!";
				Debug.LogError(errorMsg);
				popup.DisplayError(errorMsg);
			}
		}
		if (scalarCell != null) {
			scalarCell.SetActive(false);
		}
		if (dropdownA != null) {
			if (dropdownA.value == 0) {
				dropdownB.value = 1;
			}
			else {
				dropdownB.value = 0;
			}
		}
	}

	public void UpdateDropdowns() {
		if (dropdownA != null) {
			if (dropdownOperation.value == 2) {
				//scalar multiplication
				scalarCell.SetActive(true);
				dropdownA.gameObject.SetActive(false);
				dropdownB.gameObject.SetActive(false);
				rowA = matA.GetRow();
				colA = matA.GetCol();
				rowB = matB.GetRow();
				colB = matB.GetCol();
				matA.RecalculateGrid();
				matB.RecalculateGrid();
				matrixA = matA.matrix;
				matrixB = matB.matrix;
				matB.gameObject.SetActive(false);
				Debug.Log("Matrix B isEmpty?: " + matB.isEmptyCell());
			}
			else if (dropdownA.value == 0) {
				Debug.Log("Calculating A to B");
				scalarCell.SetActive(false);
				dropdownA.gameObject.SetActive(true);
				dropdownB.gameObject.SetActive(true);
				matB.gameObject.SetActive(true);
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
			else if (dropdownA.value == 1) {
				Debug.Log("Calculating B to A");
				scalarCell.SetActive(false);
				dropdownA.gameObject.SetActive(true);
				dropdownB.gameObject.SetActive(true);
				matB.gameObject.SetActive(true);
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
		else {
			rowA = matA.GetRow();
			colA = matA.GetCol();
			matA.RecalculateGrid();
			matrixA = matA.matrix;
		}
	}

	public void CheckLegal() {
		if (dropdownOperation != null) {
			if (dropdownOperation.value == 0) {
				//addition
				if ((rowA != rowB) && (colA != colB)) {
					errorMsg = "The matrix sizes are not the same!";
					Debug.LogError(errorMsg);
					popup.DisplayError(errorMsg);
					return;
				}
			}
			else if (dropdownOperation.value == 2) {
				if (!int.TryParse(scalarCell.transform.GetChild(0).GetComponent<TMP_InputField>().text, out scalarValue)) {
					errorMsg = "No scalar value input!";
					Debug.LogError(errorMsg);
					popup.DisplayError(errorMsg);
					return;
				}
				scalarValue = int.Parse(scalarCell.transform.GetChild(0).GetComponent<TMP_InputField>().text);
				if (!matA.isEmptyCell()) {
					ScalarMultiplication(matrixA, scalarValue);
				}
				Debug.Log("Scalar value is: " + scalarValue);
				return;
			}
			else if (dropdownOperation.value == 1) {
				//multiply
				if (colA != rowB) {
					if (dropdownA.value == 0) {
						errorMsg = "Column A and row B are not the same length!";
					}
					else {
						errorMsg = "Column B and row A are not the same length!";
					}
					Debug.LogError(errorMsg);
					popup.DisplayError(errorMsg);
					return;
				}
			}
			else if (dropdownOperation.value == 3) {
				//subtraction
				if ((rowA != rowB) && (colA != colB)) {
					errorMsg = "The matrix sizes are not the same!";
					Debug.LogError(errorMsg);
					popup.DisplayError(errorMsg);
					return;
				}
			}
			if (!matA.isEmptyCell() && !matB.isEmptyCell()) {
				Calculate(matrixA, matrixB, dropdownOperation.value);
			}
			else {
				errorMsg = "Check empty cell(s)";
				popup.DisplayError(errorMsg);
			}
		}
		else {
			CalculateDeterminant();
		}
	}

	public void SwitchDeterminant() {
		SceneManager.LoadScene(1);
	}

	public void SwitchOperations() {
		SceneManager.LoadScene(0);
	}

	public void CalculateDeterminant() {
		int result = 0;
		if (!matA.isEmptyCell()) {
			if (rowA == 1 && colA == 1) {
				//1x1 matrix
				result = matrixA[0, 0];
			}
			else if (rowA == 2 && colA == 2) {
				//2x2 matrix
				result = (matrixA[0, 0] * matrixA[1, 1]) - (matrixA[1, 0] * matrixA[0, 1]);
			}
			else if (rowA == 3 && colA == 3) {
				//3x3 matrix
				result = (matrixA[0, 0] * matrixA[1, 1] * matrixA[2, 2]) + (matrixA[0, 1] * matrixA[1, 2] * matrixA[2, 0]) + (matrixA[0, 2] * (matrixA[1, 0] * matrixA[2, 1]) - (matrixA[0, 2] * matrixA[1, 1] * matrixA[2, 0]) - (matrixA[0, 0] * matrixA[1, 2] * matrixA[2, 1]) - (matrixA[0, 1] * matrixA[1, 0] * matrixA[2, 2]));
			}
			else {
				errorMsg = "Matrix too big!";
				Debug.LogError(errorMsg);
				popup.DisplayError(errorMsg);
				return;
			}
		}
		else {
			errorMsg = "Check empty cell(s)";
			Debug.LogError(errorMsg);
			popup.DisplayError(errorMsg);
			return;
		}
		determinantCell.transform.GetChild(0).GetComponent<TMP_InputField>().text = result.ToString();
	}

	void ScalarMultiplication(int[,] matrix, int value) {
		rowC = matrix.GetLength(0);
		colC = matrix.GetLength(1);
		matrixC = new int[rowC, colC];
		for (int i = 0; i < rowC; i++) {
			for (int j = 0; j < colC; j++) {
				matrixC[i, j] = matrix[i, j] * value;
			}
		}
		matC.InsertMatrix(rowC, colC, matrixC);
	}

	void Calculate(int[,] A, int[,] B, int operation) {
		if (A == null) {
			errorMsg = "Matrix A is null!";
			Debug.LogError(errorMsg);
			popup.DisplayError(errorMsg);
			return;
		}
		if (B == null) {
			errorMsg = "Matrix B is null!";
			Debug.LogError(errorMsg);
			popup.DisplayError(errorMsg);
			return;
		}
		if (operation == 0) {
			//A + B
			if ((A.GetLength(0) == B.GetLength(0)) && (A.GetLength(1) == B.GetLength(1))) {
				rowC = A.GetLength(0);
				colC = A.GetLength(1);
				matrixC = new int[rowC, colC];
				for (int i = 0; i < matrixC.GetLength(0); i++) {
					for (int j = 0; j < matrixC.GetLength(1); j++) {
						matrixC[i, j] = A[i, j] + B[i, j];
					}
				}
			}
			else {
				errorMsg = "The matrix sizes are not the same";
				Debug.LogError(errorMsg);
				popup.DisplayError(errorMsg);
			}
		}
		else if (operation == 1) {
			//A x B
			rowC = A.GetLength(0);
			colC = B.GetLength(1);
			matrixC = new int[rowC, colC];
			for (int i = 0; i < rowC; i++) {
				for (int j = 0; j < colC; j++) {
					matrixC[i, j] = 0;
					for (int k = 0; k < A.GetLength(1); k++) {
						matrixC[i, j] += A[i, k] * B[k, j];
					}
				}
			}
		}
		else if (operation == 3) {
			//A - B
			if ((A.GetLength(0) == B.GetLength(0)) && (A.GetLength(1) == B.GetLength(1))) {
				rowC = A.GetLength(0);
				colC = A.GetLength(1);
				matrixC = new int[rowC, colC];
				for (int i = 0; i < matrixC.GetLength(0); i++) {
					for (int j = 0; j < matrixC.GetLength(1); j++) {
						matrixC[i, j] = A[i, j] + B[i, j];
					}
				}
			}
			else {
				errorMsg = "The matrix sizes are not the same";
				Debug.LogError(errorMsg);
				popup.DisplayError(errorMsg);
			}
		}
		//calculation finished, insert matrix
		matC.InsertMatrix(rowC, colC, matrixC);
	}
}
