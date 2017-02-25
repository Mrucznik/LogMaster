using System.Collections.Generic;
using System.IO;

namespace AccountRecovery
{
    public class Logs
    {
        public string Filename;
        public string Name { get; }
        private readonly StreamWriter _output;
        private readonly List<Detektor> _detektory = new List<Detektor>();

        public Logs(string filename, string name)
        {
            Filename = filename;
            Name = name;
            _output = new StreamWriter($"output\\{name}.sql");
        }

        public void Close()
        {
            _output.Flush();
            _output.Close();
            _output.Dispose();
        }

        public void AddDetector(Detektor detektor)
        {
            _detektory.Add(detektor);
        }

        public bool MatchDetector(string text)
        {
            foreach (var akcja in _detektory)
            {
                Akcja o = akcja.Dopasuj(text);
                if (o != null)
                {
                    string pawnCode = o.GeneratePawnCode();
                    string query = o.GenerujZapytanie();
                    if (pawnCode != null)
                        Global.WitePawnCode(pawnCode);
                    if(query != null)
                        _output.WriteLine(query);
                    o.Wyswietl();
                    Global.GeneratedQueries++;
                }
            }
            return false;
        }
    }
}
