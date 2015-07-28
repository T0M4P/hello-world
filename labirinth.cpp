#include <iostream>
#include <cstdlib>
#include <ctime>

using namespace std;
/*
*
*	Kiút a labirintusból - Programozás módszertan 2. beadandó
*	2015. tavaszi félév
*	Copyright (C) NIER TAMÁS - NITWAAT.PTE
*	2015. 03. 22.
*
*
*	A program egy nullákból és egyesekbõl véletlen generált labirintusban keresi
*	meg a kiutat Backtrack algoritmust használva. A nullák jelölik a falakat 
*	és az egyesek mentén haladhatunk.
*	A program az összes lehetséges útvonalat megkeresi, majd kiírja õket és
*	a végén megmutatja, hogy melyik volt a legrövidebb ezek közül.
*	
*/

const int N = 20;
int matrix[N][N] = {
{ 0, 1, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 1 },
{ 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0 },
{ 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0 },
{ 0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0 },
{ 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1 },
{ 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0 },
{ 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 1, 1, 1, 0 },
{ 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0 },
{ 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 },
{ 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1 },
{ 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1 },
{ 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1 },
{ 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1 },
{ 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 },
{ 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1 },
{ 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0 },
{ 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0 },
{ 1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1 },
{ 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1 },
{ 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0 } };		//N x N-es mátrix, ez a labirintus

int tmp[N][N] = { 0 };													//segéd mátrix. ebben fogjuk eltárolni a megoldást
int solutions[N] = { 0 };												//megoldások tárolására szolgáló tömb
bool isSolvable(int A[N][N]);											//függvény, amely ellenõrzi, hogy a labirintusban van-e belépési és kilépési pont
bool isValidLab(int matrix[N][N], int, int);							//függvény, amely ellenõrzi, hogy a kapott koordináták a labirintuson belül vannak-e és a mezõ 1-es
bool isValidMove(int matrix[N][N], int, int, int tmp[N][N]);			//függvény, amely ellenõrzi, hogy az aktuális helyrõl merre tudunk tovább haladni
bool solveLab(int matrix[N][N], int);									//függvény, amely megkeresi a kivezetõ utat a labirintusból
void print(int A[N][N]);												//kirajzolja a labirintust
void initLab(int A[N][N]);												//inicializálja a labirintust (0-1 közötti számokkal tölti fel véletlenszerûen
void clearLab(int A[N][N]);												//0-ra állítja az adott mátrixot
int getMin(int A[N]);													//visszaadja egy tömb legkisebb elemét
int counter = 0, i = 0, k = 0;											//segédváltozók

int main()
{
	initLab(matrix);
	print(matrix);

	//ha a labirintusba be és ki tudunk lépni
	if (isSolvable(matrix))
	{
		//ha a mátrixot sikeresen megoldottuk, akkor kirajzoljuk a megoldást és kiírjuk az út hosszát, majd keresünk egy másik kiutat
		while (i >= 0 && i < N)
		{
			if (solveLab(matrix, i++))
			{
				cout << "Megoldas" << endl;
				print(tmp);
				cout << "Ut hossza: " << counter << endl;
				solutions[k++] = counter;
				clearLab(tmp);
				counter = 0;
			}
		}
		
		if (getMin(solutions) != 0)
			cout << "Legrovidebb ut: " << getMin(solutions) << endl;
		else
			cout << "Nincs kiut a labirintusbol" << endl;
	}
	else
		cout << "Rossz labirintust generaltunk..." << endl;
	
	system("pause");
	return 0;
}

bool isSolvable(int A[N][N])
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < N; j++)
		{
			//ellenõrzi, hogy az elsõ és utolsó sorban van-e 1-es mezõ
			if (A[0][i] == 1 && A[N - 1][j] == 1)
				return true;
		}
	}
	return false;
}

bool isValidLab(int matrix[N][N], int x, int y) 
{
	// ha az x és y a labirintuson belül van és a mátrix adott helyén 1-es van és a megoldásunkban még nem szerepel az adott hely
	if ((x >= 0 && x < N) && (y >= 0 && y < N) && matrix[x][y] == 1 && tmp[x][y] != 1)
		return true;
	return false;
}

bool isValidMove(int matrix[N][N], int x, int y, int tmp[N][N]) 
{
	//Base Case.
	//amennyiben az utolsó sor valamelyikében vagyunk és a mátrixban ez 1-es mezõ, akkor kijutottunk :)
	if ((x == N - 1) && (y <= N - 1) && matrix[x][y] == 1)
	{
		tmp[x][y] = 1;
		counter++;
		return true;
	}
	//megvizsgáljuk, hogy a labirintuson belül vagyunk-e
	if (isValidLab(matrix, x, y)) 
	{
		//jelöljük ezt a pozíciót
		tmp[x][y] = 1;
		counter++;
		//megvizsgáljuk, hogy a környezõ mezõkre tudunk-e lépni
		if (isValidMove(matrix, x + 1, y, tmp))							//jobbra
			return true;
		if (isValidMove(matrix, x, y + 1, tmp))							//lent
			return true;
		if (isValidMove(matrix, x, y - 1, tmp))							//balra
			return true;
		if (isValidMove(matrix, x - 1, y, tmp))							//fent
			return true;
		//BACKTRACK
		//nem jó ez az útvonal, vonjuk vissza a jelölést
		tmp[x][y] = 0;
		counter--;
		return false;
	}
	return false;
}

bool solveLab(int matrix[N][N], int x) 
{
	if (isValidMove(matrix, 0, x, tmp))
		return true;
	return false;
}

void print(int A[N][N]) 
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < N; j++)
		{
			cout << A[i][j] << " ";
		}
		cout << endl;
	}
	cout << endl;
}
void initLab(int A[N][N]) 
{
	srand(time(NULL));
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < N; j++)
			A[i][j] = rand() % 2;
	}
}

void clearLab(int A[N][N]) 
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < N; j++)
		{
			A[i][j] = 0;
		}
	}
}

int getMin(int A[N]) 
{
	int min = A[0];
	i = 0;
	while (A[++i] != 0) 
		if (min > A[i])
			min = A[i];
	return min;
}
