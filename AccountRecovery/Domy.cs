using System.Text.RegularExpressions;

namespace AccountRecovery
{
    abstract class Dom : Akcja
    {
        public abstract override string GeneratePawnCode();
    }

    class KupnoDomu : Dom
    {
        protected string Nick;
        protected int DomID;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            DomID = int.Parse(grupy[8].Value);
            Hajs = int.Parse(grupy[9].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} kupił dom[{DomID}] za {Hajs}$");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Hajs}', `Dom`='{DomID}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"dini_Set(\"Domy/Dom{DomID}.int\", \"Wlasciciel\", \"{Nick}\");\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"Kupiony\", 1);\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"UID_Wlascicela\", {g.Uid});";
        }
    }

    class ZlomowanieDomu : Dom
    {
        protected string Nick;
        protected int DomID;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            DomID = int.Parse(grupy[8].Value);
            Hajs = int.Parse(grupy[9].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zezłomował dom[{DomID}] i dostał {Hajs}$");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Hajs}', `Dom`='0' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            return $"dini_Set(\"Domy/Dom{DomID}.int\", \"Wlasciciel\", \"Brak\");\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"Kupiony\", 0);\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"UID_Wlascicela\", 0);";
        }
    }

    class Eksmisja : Dom
    {
        protected int DomID;
        protected string Nick;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            DomID = int.Parse(grupy[7].Value);
            Nick = grupy[7].Value;
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"Dom[{DomID}] został zezłomowany z powodu nieaktywności");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            return null;
        }

        public override string GeneratePawnCode()
        {
            return $"dini_Set(\"Domy/Dom{DomID}.int\", \"Wlasciciel\", \"Gracz Nieaktywny\");\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"Kupiony\", 0);\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"UID_Wlascicela\", 0);";
        }
    }

    class PlatnaEksmisja : Dom
    {
        protected string Nick;
        protected int DomID;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Nick = grupy[7].Value;
            DomID = int.Parse(grupy[8].Value);
            Hajs = int.Parse(grupy[9].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"Dom[{DomID}] gracza {Nick} został zezłomowany, zwrot {Hajs}$");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Hajs}', `Dom`='0' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            return $"dini_Set(\"Domy/Dom{DomID}.int\", \"Wlasciciel\", \"Gracz Nieaktywny\");\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"Kupiony\", 0);\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"UID_Wlascicela\", 0);";
        }
    }

    class SprzedazDomu : Dom
    {
        protected string Kupujacy;
        protected string Sprzedajacy;
        protected int DomID;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Kupujacy = grupy[7].Value;
            DomID = int.Parse(grupy[8].Value);
            Sprzedajacy = grupy[9].Value;
            Hajs = int.Parse(grupy[10].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Kupujacy} kupił dom[{DomID}] od {Sprzedajacy} za {Hajs}$");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz gKupujacy = Gracze.GetPlayerOrGenerateIfNotExists(Kupujacy);
            Gracz gSprzedajacy = Gracze.GetPlayerOrGenerateIfNotExists(Sprzedajacy);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Hajs}', `Dom`='0' WHERE `UID`='{gSprzedajacy.Uid}';\n" +
                $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Hajs}', `Dom`='{DomID}' WHERE `UID`='{gKupujacy.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            Gracz gKupujacy = Gracze.GetPlayerOrGenerateIfNotExists(Kupujacy);
            return $"dini_Set(\"Domy/Dom{DomID}.int\", \"Wlasciciel\", \"{Kupujacy}\");\n" +
                   $"dini_IntSet(\"Domy/Dom{DomID}.int\", \"UID_Wlascicela\", {gKupujacy.Uid});";
        }
    }
}
