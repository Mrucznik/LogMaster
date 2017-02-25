using System.Text.RegularExpressions;

namespace AccountRecovery
{
    public class ZmianaNicku : Akcja
    {
        protected string PoprzedniNick;
        protected int Uid;
        protected string Nick;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            PoprzedniNick = grupy[7].Value;
            Uid = int.Parse(grupy[8].Value);
            Nick = grupy[9].Value;

            AddNicknameToPlayers();
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"Zmiana nicku z {PoprzedniNick} na {Nick} (UID: {Uid})");
        }

        public override string GenerujZapytanie()
        {
            return $"UPDATE `{Global.AccountTable}` SET `Nick`='{Nick}' WHERE `UID`='{Uid}';";
        }

        private void AddNicknameToPlayers()
        {
            Gracze.AddPlayer(new Gracz(Uid, Nick));
        }
    }
}
