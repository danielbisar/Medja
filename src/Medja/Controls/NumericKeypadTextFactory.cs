using System;
using System.Collections.Generic;
using Medja.Utils.Text;

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

            var navigator = new TextReaderNavigator(layout);

            while (navigator.HasMore)
            {
                var c = navigator.ReadChar();
                var translatedC = Translate(c);

                subList.Add(translatedC);

                // if (navigator.HasMore)
                // {
                //     if (navigator.IsAtNewLine())
                //     {
                //         navigator.SkipLine();

                //         if (navigator.HasMore)
                //         {
                //             subList = new List<string>();
                //             resultList.Add(subList);
                //         }
                //     }
                //     else
                //     {
                //         navigator.SkipExpected(" ");
                //     }
                // }
            }

            return resultList;
        }
    }
}