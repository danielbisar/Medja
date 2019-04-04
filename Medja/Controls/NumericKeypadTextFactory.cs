using System;
using System.Collections.Generic;

namespace Medja.Controls
{
    public class NumericKeypadTextFactory
    {
        private readonly Dictionary<char, string> _keys;

        public NumericKeypadTextFactory()
        {
            _keys = new Dictionary<char, string>
            {
                {'c', Globalization.Clear},
                {'b', Globalization.Back},
                {'-', ""}
            };

            // 48 = ascii digit 0, 49 = 1, ... 
            // unicode has the same values
            for (int i = 0; i <= 9; i++)
                _keys.Add((char)(i + 48), i.ToString());
        }

        public string Translate(char c)
        {
            return _keys[c];
        }

        public List<List<string>> Translate(string layout)
        {
            var resultList = new List<List<string>>();
            var subList = new List<string>();
            resultList.Add(subList);

            for (int i = 0; i < layout.Length; i++)
            {
                var c = layout[i];
                var translatedC = Translate(c);

                subList.Add(translatedC);

                if (i + 1 < layout.Length)
                {
                    i++;
                    c = layout[i];

                    if (c != ' ' && c != '\n' && c != '\r')
                        throw new Exception("Expected space, EOL or new line.");

                    // newline \n, \r, \r\n
                    if (c == '\r')
                    {
                        if (i + 1 < layout.Length)
                        {
                            if (layout[i + 1] == '\n')
                                i++;
                        }
                    }

                    if (c == '\n' || c == '\r' && i + 1 < layout.Length)
                    {
                        subList = new List<string>();
                        resultList.Add(subList);
                    }
                }
            }

            return resultList;
        }
    }
}