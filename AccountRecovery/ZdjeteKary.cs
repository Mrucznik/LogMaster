using System.Text.RegularExpressions;

namespace AccountRecovery
{
    abstract class ZdjetaKara : Akcja
    {
        protected string Nick;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
        }
    }

    class UnBan : ZdjetaKara
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal odbanowany");
        }

        public override string GenerujZapytanie()
        {
            return $"DELETE FROM `{Global.BanTable}` WHERE `dostal`='{Nick}';";
        }
    }

    class UnBanIP : Akcja
    {
        protected string Ip;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Ip = grupy[7].Value;
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"IP {Ip} zostalo odbanowane");
        }

        public override string GenerujZapytanie()
        {
            return $"DELETE FROM `{Global.BanTable}` WHERE `IP`='{Ip}';";
        }
    }

    class UnBlock : ZdjetaKara
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal odblokowany");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"DELETE FROM `{Global.BanTable}` WHERE `dostal`='{Nick}' AND `typ`='1';\n" +
                $"UPDATE `{Global.AccountTable}` SET `Block`='0' WHERE `UID`='{g.Uid}';";
        }
    }

    class UnWarn : ZdjetaKara
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal unwarnowany");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Warnings`=`Warnings`-'1' WHERE `UID`='{g.Uid}';";
        }
    }
}
