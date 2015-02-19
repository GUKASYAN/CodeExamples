int GetMissingValue(
      const int *  _values, // неупорядоченный массив целых чисел от 1 до _count
      unsigned int _count   // количество элементов в массиве
  )
{
  int sum = 0;
  for (int i = 1; i <= _count; i++)
  {
    sum += i;
  }
  for (int i = 0; i < _count - 1; i++)
  {
    sum -= *(_values + i);
  }
  return sum;
}