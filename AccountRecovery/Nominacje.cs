using System.Text.RegularExpressions;

namespace AccountRecovery
{
    class Admin : Akcja
    {
        protected string Nick;
        protected int Level;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            Level = int.Parse(grupy[8].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal mianowany na {Level} P@/@/ZG$.");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Admin`='{Level}' WHERE `UID`='{g.Uid}';";
        }
    }

    class PolAdmin : Admin
    {
        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `PAdmin`='{Level}' WHERE `UID`='{g.Uid}';";
        }
    }

    class Zaufany : Admin
    {
        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `ZaufanyGracz`='{Level}' WHERE `UID`='{g.Uid}';";
        }
    }
}
