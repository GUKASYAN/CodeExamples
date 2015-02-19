void NumberAsBinary(
       char *       _result, // строка с результатом
	   unsigned int _number  // произвольное число
  )
{
  int j = 0;
  int buffer;
  bool cut = false;
  for (int i = sizeof(_number)* 2 + 1; i >= 0; --i)
  {
    *(_result + j) = '0' + (int)((_number >> i) & 1);
    if (*(_result + j) == '1' && cut == false) cut = true;
    else if (cut == false) continue;
    j++;
  }
}
