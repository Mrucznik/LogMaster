using System;
using System.Text.RegularExpressions;

namespace AccountRecovery
{
    abstract class AutoAkcja : Akcja
    {
        public int KonwertujMarkeNaModel(string marka)
        {
            string[] nazwy = {
               "Landstalker",  "Bravura",  "Buffalo", "TIR", "Perennial", "Sentinel",
               "Wywrotka",  "Straz" ,  "Smieciarka" ,  "Limuzyna",  "Manana",  "Infernus",
               "Voodoo", "Pony",  "Mule", "Cheetah", "Karetka",  "Leviathan",  "Moonbeam",
               "Esperanto", "Taxi",  "Washington",  "Bobcat",  "Lodziarnia", "BF Injection",
               "Hunter", "Premier",  "Enforcer",  "Securicar", "Banshee", "Predator", "Bus",
               "Czolg",  "Barracks",  "Hotknife",  "Przyczepa",  "Previon", "Autobus", "Taxi",
               "Stallion", "Rumpo", "RC Bandit",  "Karawan", "Packer", "Monster",  "Admiral",
               "Squalo", "Seasparrow", "Pizzaboy", "Tramwaj", "Przyczepa",  "Turismo", "Speeder",
               "Kuter",/* Reefer */ "Tropic", "Flatbed","Yankee", "Caddy", "Solair","Berkley's RC Van",
               "Skimmer", "PCJ-600", "Faggio", "Freeway", "RC Baron","RC Raider","Glendale",
               "Oceanic", "Sanchez", "Sparrow",  "Hummer", "Quad",  "Coastguard", "Ponton",
               "Hermes", "Sabre", "Rustler", "ZR-350", "Walton",  "Regina",  "Comet", "BMX",
               "Burrito", "Camper", "Jacht", "Baggage", "Dozer","Maverick","Newsokopter",
               "Rancher", "Rancher FBI", "Virgo", "Greenwood","Jetmax","Hotring","Sandking",
               "Blista Compact", "Policyjny Maverick", "Boxville", "Benson","Mesa","RC Goblin",
               "Hotring Racer", "Hotring Racer", "Bloodring Banger", "Rancher",  "Super GT",
               "Elegant", "Kamping", "Rower", "Rower Gorski", "Beagle", "Cropdust", "Stunt",
               "Tanker", "Pociag", "Nebula", "Majestic", "Buccaneer", "Shamal",  "Hydra",
               "FCR-900","NRG-500","HPV1000","Cement Truck","Tow Truck","Fortune","Cadrona",
               "Armatka Wodna", "Willard", "Forklift","Traktor","Combine","Feltzer","Remington",
               "Slamvan", "Blade", "Freight", "Streak","Vortex","Vincent","Bullet","Clover",
               "Sadler",  "Straz", "Hustler", "Intruder", "Primo", "Cargobob",  "Tampa",
               "Sunrise", "Merit",  "Utility Truck",  "Nevada", "Yosemite", "Windsor",  "Monster",
               "Monster","Uranus","Jester","Sultan","Stratum","Elegy","Raindance","RCTiger",
               "Flash","Tahoma","Savanna", "Bandito", "Freight", "Trailer", "Kart", "Turbowozek",
               "Dune", "Sweeper", "Broadway", "Tornado", "AT-400",  "DFT-30", "Huntley",
               "Stafford", "BF-400", "SANvan","Tug","Trailer","Emperor","Wayfarer","Euros",
               "Hotdog", "Club", "Trailer", "Trailer","Andromada","Dodo","RC Cam", "Launch",
               "Radiowoz (LSPD)", "Radiowoz (SFPD)","Radiowoz (LVPD)","Policyjny Jeep",
               "Picador",   "Pancernik FBI",  "Alpha",   "Phoenix",   "Glendale",   "Sadler",
               "Luggage Trailer","Luggage Trailer","Stair Trailer", "Boxville", "Kombajn",
               "Utility Trailer", "Brak pojazdu", "Brak łodzi", "Brak samolotu"
            };

            for (int i = 0; i < nazwy.Length; i++)
            {
                if (nazwy[i] == marka)
                    return 400+i;
            }
            throw new Exception("Nieznana marka pojazdu");
        }

        public Vector3F LosujPozycje()
        {
            Random random = new Random();
            double[,] pos =
            {
                {2161.2605,-1197.3385,23.5517,89.7108},//1
                {2161.0071,-1192.6439,23.4812,90.5042},//2
                {2160.9656,-1187.9816,23.4800,90.5042},//3
                {2160.9233,-1183.1466,23.4788,90.5042},//4
                {2160.8806,-1178.2858,23.4776,90.5042},//5
                {2160.8352,-1173.1339,23.4763,90.5042},//6
                {2160.7915,-1168.2013,23.4751,90.5042},//7
                {2160.7498,-1163.4933,23.4739,90.5042},//8
                {2160.7043,-1158.3149,23.4726,90.5042},//9
                {2160.6587,-1153.1633,23.4713,90.5042},//10
                {2160.6128,-1148.3796,23.9337,90.5042},//11
                {2160.5649,-1143.8113,24.8596,90.5042},//12
                {2148.7363,-1203.4053,23.5150,270.6497},//13
                {2148.6887,-1199.0850,23.6128,270.6497},//14
                {2148.6409,-1194.7642,23.7106,270.6497},//15
                {2148.5842,-1189.6375,23.8267,270.6497},//16
                {2148.5342,-1185.1234,23.9289,270.6497},//17
                {2148.4814,-1180.3663,24.0366,270.6497},//18
                {2148.3794,-1171.1194,24.2460,270.6497},//19
                {2148.3274,-1166.4340,24.3521,270.6497},//20
                {2148.2791,-1162.0425,24.4516,270.6497},//21
                {2148.2290,-1157.5012,24.5544,270.6497},//22
                {2148.1829,-1153.3276,24.6489,270.6497},//23
                {2148.1255,-1148.1309,24.7666,270.6497},//24
                {2148.0740,-1143.4689,24.8721,270.6497},//25
                {2148.0215,-1138.7086,24.9799,270.6497},//26
                {2148.6426,-1133.7229,25.2246,268.2947}//27
            };
            int idx = random.Next(0, 27);
            return new Vector3F((float)pos[idx, 0], (float)pos[idx, 1], (float)pos[idx, 2]);
        }
    }

    class KupnoAuta : AutoAkcja
    {
        protected string Kupujacy;
        protected string Marka;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Kupujacy = grupy[7].Value;
            Marka = grupy[8].Value;
            Hajs = int.Parse(grupy[9].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Kupujacy} kupil {Marka} za {Hajs}$");
        }

        public override string GenerujZapytanie()
        {
            int model = KonwertujMarkeNaModel(Marka);
            Vector3F pos = LosujPozycje();
            return $"INSERT INTO `{Global.CarTable}` (`model`, `x`, `y`, `z`, `angle`, `color1`, `color2`) " + 
                $"VALUES ('{model}', '{pos.X:F}', '{pos.Y:F}', '{pos.Z:F}', '0.0', '-1', '-1');";
        }
    }

    class SprzedarzAuta : AutoAkcja
    {
        protected string Kupujacy;
        protected string Sprzedajacy;
        protected string Marka;
        protected int CarUID;
        protected int Hajs;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Kupujacy = grupy[7].Value;
            Sprzedajacy = grupy[8].Value;
            Marka = grupy[9].Value;
            CarUID = int.Parse(grupy[10].Value);
            Hajs = int.Parse(grupy[11].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Kupujacy} kupil {Marka}[{CarUID}] od {Sprzedajacy} za {Hajs}$");
        }

        public override string GenerujZapytanie()
        {
            Gracz gKupujacy = Gracze.GetPlayerOrGenerateIfNotExists(Kupujacy);
            Gracz gSprzedajacy = Gracze.GetPlayerOrGenerateIfNotExists(Sprzedajacy);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Hajs}' WHERE `UID`='{gKupujacy.Uid}';\n" +
                   $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Hajs}' WHERE `UID`='{gSprzedajacy.Uid}';\n" +
                   $"UPDATE `{Global.CarTable}` SET `owner`='{gKupujacy.Uid}' WHERE `UID`='{CarUID}';";
        }
    }

    class WymianaAuta : AutoAkcja
    {
        protected string Kupujacy;
        protected string Sprzedajacy;
        protected string MarkaKupujacy;
        protected string MarkaSprzedajacy;
        protected int CarUIDKupujacy;
        protected int CarUIDSprzedajacy;
        protected int Doplata;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            Kupujacy = grupy[7].Value;
            Sprzedajacy = grupy[8].Value;
            MarkaKupujacy = grupy[9].Value;
            CarUIDKupujacy = int.Parse(grupy[10].Value);
            MarkaSprzedajacy = grupy[11].Value;
            CarUIDSprzedajacy = int.Parse(grupy[12].Value);
            Doplata = int.Parse(grupy[13].Value);
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Kupujacy} wymienil {MarkaKupujacy}[{CarUIDKupujacy}] z {Sprzedajacy} za {MarkaSprzedajacy}[{CarUIDSprzedajacy}] i dopłacił {Doplata}$");
        }

        public override string GenerujZapytanie()
        {
            Gracz gKupujacy = Gracze.GetPlayerOrGenerateIfNotExists(Kupujacy);
            Gracz gSprzedajacy = Gracze.GetPlayerOrGenerateIfNotExists(Sprzedajacy);
            return $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`-'{Doplata}' WHERE `UID`='{gKupujacy.Uid}';\n" +
                   $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'{Doplata}' WHERE `UID`='{gSprzedajacy.Uid}';\n" +
                   $"UPDATE `{Global.CarTable}` SET `owner`='{gKupujacy.Uid}' WHERE `UID`='{CarUIDSprzedajacy}';\n" +
                   $"UPDATE `{Global.CarTable}` SET `owner`='{gSprzedajacy.Uid}' WHERE `UID`='{CarUIDKupujacy}';";
        }
    }

    class ZlomowanieAuta : Akcja
    {
        protected string Nick;
        protected int CarUID;

        public override void Parsuj(GroupCollection grupy)
        {
            base.Parsuj(grupy);
            CarUID = int.Parse(grupy[7].Value);
            Nick = grupy[8].Value;
        }

        public override void Wyswietl()
        {
            base.Wyswietl();
            Global.LogActionLine($"{Nick} zezlomowal swoje auto o UID {CarUID}");
        }

        public override string GenerujZapytanie()
        {
            Gracz g = Gracze.GetPlayerOrGenerateIfNotExists(Nick);
            return $"DELETE FROM `{Global.CarTable}` WHERE `UID`='{CarUID}';\n" +
                $"UPDATE `{Global.AccountTable}` SET `Money`=`Money`+'5000' WHERE `UID`='{g.Uid}';";
        }
    }
}
