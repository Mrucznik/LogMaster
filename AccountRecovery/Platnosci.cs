using System.Text.RegularExpressions;

namespace AccountRecovery
{
    public class Kontrakt : Akcja
    {
        protected string Nick;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            Hajs = int.Parse(grupy[8].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"Hitman {Nick} wypelnil kontrakt za {Hajs}$.");
        }

        public void WyswietlBase()
        {
            base.Wyswietl();
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Hajs}' WHERE `UID`='{g.Uid}';";
        }
    }

    public class QPodczasAkcji : Akcja
    {
        protected string Nick;
        protected int Hajs;
        protected int Mats;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            Hajs = int.Parse(grupy[8].Value);
            Mats = int.Parse(grupy[9].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} dal /q podczas akcji i zabrano mu {Hajs}$ i {Mats} materiałów.");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Hajs}', `Materials`=`Materials`-'{Mats}' WHERE `UID`='{g.Uid}';";
        }
    }

    public class Totoloto : Kontrakt
    {
        public override void Wyswietl()
        {
            WyswietlBase();
            Global.LogActionLine($"{Nick} wygral w totoloto {Hajs}$.");
        }
    }

    public class Niwelacja : Akcja
    {
        protected string Nick;
        protected int Lvl;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            Lvl = int.Parse(grupy[8].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"zniwelowano pieniadze do 0 graczowi {Nick} lvl {Lvl}");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`='0', `Bank`='0', `Level`='{Lvl}' WHERE `UID`='{g.Uid}';";
        }
    }

    public class Kradziez : Akcja
    {
        protected string Zlodziej;
        protected string Ofiara;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Zlodziej = grupy[7].Value;
            Ofiara = grupy[8].Value;
            Hajs = int.Parse(grupy[9].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Zlodziej} zabral portfel {Ofiara} z {Hajs}$.");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Ofiara);
            Gracz g2 = Gracze.GetPlayerOrGenerateIfNotExists(Zlodziej);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Hajs}' WHERE `UID`='{g.Uid}';\n" +
                $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Hajs}' WHERE `UID`='{g2.Uid}';";
        }
    }

    public class DajHajs : Akcja
    {
        protected string Dajacy;
        protected int Hajs;
        protected string Odbierajacy;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Dajacy = grupy[7].Value;
            Hajs = int.Parse(grupy[8].Value);
            Odbierajacy = grupy[9].Value;
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Dajacy} dal {Hajs}$ {Odbierajacy}.");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Dajacy);
            Gracz g2 = Gracze.GetPlayerOrGenerateIfNotExists(Odbierajacy);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Hajs}' WHERE `UID`='{g.Uid}';\n" +
                $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Hajs}' WHERE `UID`='{g2.Uid}';";
        }
    }

    public class Przelej : Akcja
    {
        protected string Dajacy;
        protected int Hajs;
        protected string Odbierajacy;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Dajacy = grupy[7].Value;
            Hajs = int.Parse(grupy[8].Value);
            Odbierajacy = grupy[9].Value;
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Dajacy} przelal {Hajs}$ {Odbierajacy}.");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Dajacy);
            Gracz g2 = Gracze.GetPlayerOrGenerateIfNotExists(Odbierajacy);
            return $"UPDATE `{Global.AccountTable}` SET `Bank`=`Bank`-'{Hajs}' WHERE `UID`='{g.Uid}';\n" +
                $"UPDATE `{Global.AccountTable}` SET `Bank`=`Bank`+'{Hajs}' WHERE `UID`='{g2.Uid}';";
        }
    }

    public class Datek : Akcja
    {
        protected string Nick;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            Hajs = int.Parse(grupy[8].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} złożył datek o wartości {Hajs}$.");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Hajs}' WHERE `UID`='{g.Uid}';";
        }
    }

    public class Kostka : Akcja
    {
        protected string Wygrany;
        protected string Przegrany;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Wygrany = grupy[7].Value;
            Przegrany = grupy[8].Value;
            Hajs = int.Parse(grupy[9].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"Kostka: {Wygrany} wygral z {Przegrany} o {Hajs}$.");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Przegrany);
            Gracz g2 = Gracze.GetPlayerOrGenerateIfNotExists(Wygrany);
            return $"UPDATE `{Global.AccountTable}` SET `Bank`=`Bank`-'{Hajs}' WHERE `UID`='{g.Uid}';\n" +
                $"UPDATE `{Global.AccountTable}` SET `Bank`=`Bank`+'{Hajs}' WHERE `UID`='{g2.Uid}';";
        }
    }
}
