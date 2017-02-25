using System.Text.RegularExpressions;

namespace AccountRecovery
{
    abstract class Kara : Akcja
    {
        protected string Nick;
        protected string Powod;
        protected string Admin;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            Admin = grupy[8].Value;
            Powod = grupy[9].Value;
        }
    }

    class Ban : Akcja
    {
        protected string Nick;
        protected string Powod;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            Powod = grupy[8].Value;
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal zbanowany, powód: {Powod}");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"INSERT INTO `{Global.BanTable}` (`dostal`, `dostal_uid`, `powod`, `typ`, `nadal`) VALUES ('{Nick}', '{g.Uid}', '{Powod}', '2', 'SYSTEM');";
        }
    }

    class BanZAdminem1 : Kara
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal zbanowany przez admina {Admin}, powód: {Powod}");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"INSERT INTO `{Global.BanTable}` (`dostal`, `dostal_uid`, `powod`, `typ`, `nadal`) VALUES ('{Nick}', '{g.Uid}', '{Powod}', '2', '{Admin}');";
        }
    }

    class BanZAdminem2 : Akcja
    {
        protected string Nick;
        protected string Powod;
        protected string Admin;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Admin = grupy[7].Value;
            Nick = grupy[8].Value;
            Powod = grupy[9].Value;
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal zbanowany przez admina {Admin}, powód: {Powod}");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"INSERT INTO `{Global.BanTable}` (`dostal`, `dostal_uid`, `powod`, `typ`, `nadal`) VALUES ('{Nick}', '{g.Uid}', '{Powod}', '2', '{Admin}');";
        }
    }

    class Block : Kara
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal zablokowany przez {Admin}, powód: {Powod}");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"INSERT INTO `{Global.BanTable}` (`dostal`, `dostal_uid`, `powod`, `typ`, `nadal`) VALUES ('{Nick}', '{g.Uid}', '{Powod}', '1', '{Admin}');\n" +
                $"UPDATE `{Global.AccountTable}` SET `Block`='1' WHERE `UID`='{g.Uid}';";
        }
    }

    class Warn : Kara
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zostal zwarnowany przez {Admin}, powód: {Powod}");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Warnings`=`Warnings`+'1' WHERE `UID`='{g.Uid}';";
        }
    }
}
