using System;
using System.Text.RegularExpressions;

namespace AccountRecovery
{
    public class Detektor
    {
        private readonly Regex _regex;
        private readonly Type _typ;

        public Detektor(string pattern, Type type)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
            _typ = type;
        }

        public Akcja Dopasuj(string text)
        {
            var match = _regex.Match(text);
            if (match.Success)
            {
                return GenerujObiekt(match.Groups);
            }
            return null;
        }

        private Akcja GenerujObiekt(GroupCollection grupy)
        {
            var o = (Akcja)Activator.CreateInstance(_typ);
            o.Parsuj(grupy);
            return o;
        }
    }
}
