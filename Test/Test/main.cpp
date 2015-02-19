#include <stdio.h>
#include <iostream>
#include <time.h>
#include "Task1.h"
#include "Task2.h"
#include "Task3.h"
#include "Task4.h"
using namespace std;

void TestTask1()
{
  int number;
  srand(time(NULL));
  for (int i = 0; i < 10; i++)
  {
    char *result = new char[11];
    number = rand() % 512;
    NumberAsBinary(result, number);
    printf("\n%d number (%d) :\n", i + 1, number);
    for (int j = 0; j < 10; j++)
    {
      if (*(result + j) == '0' || *(result + j) == '1')  cout << *(result + j);
    }
    delete[] result;
  }	
}
void TestTask2_3_4 (
	int _mode
  )	
{
  int *randArray = new int [15];
  int count;
  srand(time(NULL)); 
  for (int i = 0; i < 4; i++)
  {
    count = rand() % 10 + 5;
    GenerateRandomArray(randArray, count);
    printf("\n%d number (%d):\n", i + 1, count);
    for (int j = 0; j < count; j++)
    {
      cout << *(randArray + j);
      cout << " ";
    }
    if (_mode==3)
    {
      Sort(randArray, count);
      cout << "\nSorted:\n";
      for (int j = 0; j < count; j++)
      {
        cout << *(randArray + j);
        cout << " ";
      }
    }
    if (_mode == 4) 
    {
      printf("\nMissing value:\n%d",GetMissingValue(randArray, count));
    }
  }	 
  delete[] randArray;
}
void main()
{
  cout << "\n\nTask1 START:\n";
  TestTask1();
  cout << "\n\nTask1 END\n";
  cout << "\n\nTask2 START:\n"; 
  TestTask2_3_4(2);
  cout << "\n\nTask2 END\n";
  cout << "\n\nTask3 START:\n";
  TestTask2_3_4(3);
  cout << "\n\nTask3 END\n";
  cout << "\n\nTask4 START:\n";
  TestTask2_3_4(4);
  cout << "\n\nTask4 END\n";
  getchar();
}
 