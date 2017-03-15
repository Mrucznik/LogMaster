using System;
using System.Text.RegularExpressions;

namespace AccountRecovery
{
    abstract class Sejf : Akcja
    {
        protected string Nick;
        protected int Ilosc;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);

            Nick = grupy[7].Value;
            Ilosc = int.Parse(grupy[8].Value);
        }

        public abstract string GeneratePawnCode();
    }

    abstract class SejfFrakcji : Sejf
    {
        protected int FrakcjaID;
        protected int StanObecny;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);

            FrakcjaID = int.Parse(grupy[9].Value);
            StanObecny = int.Parse(grupy[10].Value);
        }
    }

    class SejfHajsDo : Sejf
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} wpłacił do sejfu {Ilosc}$");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }

    class SejfHajsZ : Sejf
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} wypłacił z sejfu {Ilosc}$");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }

    class SejfMatsDo : Sejf
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} włożył do sejfu {Ilosc} materiałów");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Materials`=`Materials`-'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }

    class SejfMatsZ : Sejf
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} wyjął z sejfu {Ilosc} materiałów");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Materials`=`Materials`+'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }

    class SejfDragiDo : Sejf
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} włożył do sejfu {Ilosc} dragów");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Drugs`=`Drugs`-'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }

    class SejfDragiZ : Sejf
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} wyjął z sejfu {Ilosc} dragów");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Drugs`=`Drugs`+'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }

    class SejfFrakcjiZ : SejfFrakcji
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} wypłacił z sejfu frakcji {FrakcjaID} {Ilosc}$ (Obecna ilość {StanObecny}$)");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }

    class SejfFrakcjiDo : SejfFrakcji
    {
        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} wpłacił do sejfu frakcji {FrakcjaID} {Ilosc}$ (Obecna ilość {StanObecny}$)");
        }

        public override string GenerujZapytanie()
        {
            GeneratePawnCode();
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Ilosc}' WHERE `UID`='{g.Uid}';";
        }

        public override string GeneratePawnCode()
        {
            throw new NotImplementedException();
        }
    }
}
