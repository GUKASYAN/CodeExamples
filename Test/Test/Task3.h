void Sort(
       int *        _values, // ����������� ������
       unsigned int _count   // ���������� ��������� � �������
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