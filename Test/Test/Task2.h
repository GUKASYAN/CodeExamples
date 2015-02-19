void GenerateRandomArray(
       int *        _result,  // возвращаемый массив
       unsigned int _maxValue // максимальное число в последовательности
  )
{
  srand(time(NULL));
  int buf;
  bool was;
  for (int i = 0; i <_maxValue; i++)
  {
    was = true;
     while (was)
     {
       buf = rand() % _maxValue + 1;
       if (i == 0) was = false;
       for (int j = 0; j < i; j++)
       {
         if (*(_result + j) == buf)
         {
           was = true;
           break;
         }
         was = false;
       }
     }
    *(_result + i) = buf;
  }
}