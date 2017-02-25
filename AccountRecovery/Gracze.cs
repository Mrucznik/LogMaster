using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AccountRecovery
{
    public static class Gracze
    {
        public static int LastUid = 100946;
        private static readonly List<Gracz> StarzyGracze = new List<Gracz>();
        private static readonly List<Gracz> NowiGracze = new List<Gracz>();
        static Gracze()
        {
            using (var kontaFile = new StreamReader(Global.Patch + "\\konta.csv"))
            {
                var regex = new Regex(@"(\d*),(\w*_\w*)");
                string line;
                while ((line = kontaFile.ReadLine()) != null)
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        StarzyGracze.Add(new Gracz(int.Parse(match.Groups[1].Value), match.Groups[2].Value));
                    }
                }
            }
        }

        public static bool IsPlayerExists(string nick)
        {
            if (StarzyGracze.BinarySearch(new Gracz(nick)) >= 0)
                return true;
            return NowiGracze.Find(g => g.Nick == nick) != null;
        }

        public static bool IsPlayerExists(int uid)
        {
            foreach (var gracz in StarzyGracze)
            {
                if (gracz.Uid == uid)
                    return true;
            }
            return NowiGracze.Find(g => g.Uid == uid) != null;
        }

        public static Gracz GetPlayer(string nick)
        {
            int index = StarzyGracze.BinarySearch(new Gracz(nick));
            if (index >= 0)
                return StarzyGracze[index];
            return NowiGracze.Find(g => g.Nick == nick);
        }

        public static Gracz GetPlayer(int uid)
        {
            foreach (var gracz in StarzyGracze)
            {
                if (gracz.Uid == uid)
                    return gracz;
            }
            return NowiGracze.Find(g => g.Uid == uid);
        }

        public static void AddPlayer(Gracz gracz)
        {
            NowiGracze.Add(gracz);
            Global.LogActionLine($"Dodano gracza {gracz.Nick}");
        }

        public static bool GeneratePlayerIfNotExists(string nick)
        {
            if (!IsPlayerExists(nick))
            {
                AddNewPlayer(nick);
                return true;
            }
            return false;
        }

        public static bool GeneratePlayerIfNotExists(string nick, int uid)
        {
            if (!IsPlayerExists(uid))
            {
                if (!IsPlayerExists(nick))
                {
                    GeneratePlayer(nick);
                    return true;
                }
            }
            return false;
        }

        public static Gracz GetPlayerOrGenerateIfNotExists(string nick)
        {
            Gracz g = GetPlayer(nick);
            if (g == null)
            {
                return GeneratePlayer(nick);
            }
            return g;
        }

        public static void GenerujZapytaniaTworzace()
        {
            using (var noweKontaPlik = new StreamWriter(@"output\nowe_konta.sql"))
            {
                foreach (var gracz in NowiGracze)
                {
                    if(gracz.CzyNowy)
                        noweKontaPlik.WriteLine(gracz.GenerujZapytanieTworzace());
                }
            }
        }

        private static Gracz GeneratePlayer(string nick)
        {
            Gracz g = AddNewPlayer(nick);
            return g;
        }

        private static Gracz AddNewPlayer(string nick)
        {
            LastUid += 1;
            var gracz = new Gracz(LastUid, nick, true);
            NowiGracze.Add(gracz);
            return gracz;
        }
    }
}
