#include <iostream>
#include <cstdlib>

using namespace std;

const int N = 15; //a gráf mérete

//gráf
int graph[N][N] = { 0 };
void InitGraph();
void SetWeigth(int, int, int);
int GetWeigth(int, int);
int check[N];
void SetCheck(int, int);
int GetCheck(int);
int osszsuly = 0;

//fõ algoritmus
void Solve(int, int);
void Solve(int);

//hashtábla
int Hash[N];
int h(int);
void InsertInHash(int);
void DelInHash(int);
int SearchInHash(int);

//verem
int stack[N];
int stackCount = 0;
void StackPush(int);
int StackPop();
void StackPrint();

int main() 
{
	InitGraph();

	//Solve(99, 0);
	Solve(99);
	cout << "Legrövidebb ut hossza: " << osszsuly << endl;
	system("pause");
	return 0;
}

int h(int x)
{
	return x%N;
}

void InsertInHash(int x) 
{
	int k = h(x);
	int i = 0;

	while (Hash[k] > 0 && Hash[k] != x && i < N) 
	{
		++i;
		k = (k + 1) % N;
	}

	if (i != N)
		Hash[k] = x;
	else
		cout << "A tábla megtelt!" << endl;
}

int SearchInHash(int x) 
{
	int k = h(x);
	int i = 0;

	while (Hash[k] > 0 && Hash[k] != x && i < N)
	{
		++i;
		k = (k + 1) % N;
	}

	if (Hash[k] == x)
		return k;
	else
		return -1;
}

void StackPush(int number) 
{
	if (stackCount > N)
		cout << "A verem megtelt!" << endl;
	else
	{
		stack[stackCount] = number;
		++stackCount;
	}
}

int StackPop() 
{
	if (stackCount == 0)
		cout << "Üres a verem!" << endl;
	else
		return stack[--stackCount];
}

void StackPrint() 
{
	if (stackCount == 0)
		cout << "A verem üres!" << endl;
	else 
	{
		cout << "Az utvonal: ";
		for (int i = 0; i < stackCount; i++)
			cout << stack[i] << " ";
		cout << endl;
	}
}

void SetWeigth(int i, int j, int suly) 
{
	int x = SearchInHash(i),
		y = SearchInHash(j);
	graph[x][y] = suly;
	graph[y][x] = suly;
}

int GetWeigth(int i, int j) 
{
	int x = SearchInHash(i),
		y = SearchInHash(j);
	return graph[x][y];
}

void SetCheck(int i, int j) 
{
	int x = SearchInHash(i);
	check[x] = j;
}

int GetCheck(int i) 
{
	int x = SearchInHash(i);
	return check[x];
}

void InitGraph() 
{
	InsertInHash(1);
	InsertInHash(2);
	InsertInHash(3);
	InsertInHash(6);
	InsertInHash(10);
	InsertInHash(19);
	InsertInHash(23);
	InsertInHash(27);
	InsertInHash(30);
	InsertInHash(33);
	InsertInHash(40);
	InsertInHash(55);
	InsertInHash(60);
	InsertInHash(72);
	InsertInHash(99);

	SetWeigth(1, 99, 1);
	SetWeigth(1, 27, 2);
	SetWeigth(19, 99, 2);
	SetWeigth(19, 2, 3);
	SetWeigth(2, 27, 1);
	SetWeigth(27, 40, 1);
	SetWeigth(27, 23, 2);
	SetWeigth(40, 30, 3);
	SetWeigth(30, 23, 1);
	SetWeigth(30, 72, 1);
	SetWeigth(30, 6, 1);
	SetWeigth(30, 60, 3);
	SetWeigth(60, 6, 1);
	SetWeigth(23, 55, 2);
	SetWeigth(55, 10, 1);
	SetWeigth(10, 72, 3);
	SetWeigth(72, 33, 2);
	SetWeigth(33, 3, 3);
	SetWeigth(3, 6, 2);

	for (int i = 0; i < N; i++)
		check[i] = 999;
}

void Solve(int csucs, int suly) 
{
	int index = SearchInHash(csucs);
	StackPush(csucs);
	SetCheck(csucs, suly);

	int tmp;

	if (csucs == 33) 
	{
		if (osszsuly > suly)
			osszsuly = suly;
		StackPrint();
		tmp = StackPop();
		return;
	}

	int sulytmp;
	for (int i = 0; i < N; i++) 
	{
		if (GetWeigth(csucs, i) != 0 && (suly + GetWeigth(csucs, i)) < GetCheck(i)) 
		{
			sulytmp = suly + graph[index][i];
			Solve(Hash[i], sulytmp);
		}
	}

	tmp = StackPop();
}

void Solve(int csucs) 
{
	StackPush(csucs);
	SetCheck(csucs, osszsuly);

	int i = 0, tmp;

	if (csucs == 33) 
	{
		StackPrint();
		tmp = StackPop();
		return;
	}
	while (i < N - 1) 
	{
		if (GetWeigth(csucs, Hash[i]) != 0 && (osszsuly + GetWeigth(csucs, Hash[i])) < check[i]) 
		{
			osszsuly += GetWeigth(csucs, Hash[i]);
			Solve(Hash[i]);
		}
		i++;
	}
	tmp = StackPop();
}
