using System;

namespace AccountRecovery
{
    public class Gracz : IComparable
    {
        public int Uid;
        public string Nick;
        public bool CzyNowy;

        public Gracz(string nick, bool czyNowy=false)
        {
            Nick = nick;
            CzyNowy = czyNowy;
        }

        public Gracz(int uid, string nick, bool czyNowy = false)
        {
            Uid = uid;
            Nick = nick;
            CzyNowy = czyNowy;
        }

        public int CompareTo(object obj)
        {
            return string.Compare(Nick, ((Gracz) obj).Nick, StringComparison.CurrentCulture);
        }

        public string GenerujZapytanieTworzace()
        {
            return $"INSERT INTO `{Global.AccountTable}` (`Nick`, `Key`, `Level`, `ConnectedTime`, `Bank`, `CarLic`) VALUES ('{Nick}', MD5('{Nick}{Uid}'), '5', '250', '2000000', '1');";
        }
    }
}
