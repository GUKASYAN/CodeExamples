int GetMissingValue(
      const int *  _values, // ��������������� ������ ����� ����� �� 1 �� _count
      unsigned int _count   // ���������� ��������� � �������
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