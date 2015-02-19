void Sort(
       int *        _values, // сортируемый массив
       unsigned int _count   // количество элементов в массиве
  )
{
  int buf;
  for (int j = 0; j < _count - 1; j++)
    for (int i = 0; i < _count - 1; i++)
    {
      if (*(_values + i)>*(_values + i + 1))
      {
        buf = *(_values + i);
        *(_values + i) = *(_values + i + 1);
        *(_values + i + 1) = buf;
      }
    }
}