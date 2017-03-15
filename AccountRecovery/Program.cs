using System;
using System.IO;

namespace AccountRecovery
{
    public static class Global
    {
        private static readonly StreamWriter PawnOutput;
        private static readonly StreamWriter LogOutput;

#warning "Enter valid data"
        public static string AccountTable => "mru_konta";
        public static string BanTable => "mru_bany";
        public static string CarTable => "mru_cars";
        public static string Patch = @"input";
        public static int ProcessedLogLines;
        public static int GeneratedQueries;

        static Global()
        {
            PawnOutput = new StreamWriter(@"output\pawnCode.pwn");
            LogOutput = new StreamWriter(@"output\PROGRAM.log");
        }

        public static void WitePawnCode(string text)
        {
            PawnOutput.WriteLine(text);
        }

        public static void LogActionLine(string text)
        {
            LogOutput.WriteLine(text);
        }

        public static void LogAction(string text)
        {
            LogOutput.Write(text);
        }

        public static void Close()
        {
            PawnOutput.Flush();
            PawnOutput.Close();
            PawnOutput.Dispose();
            LogOutput.Flush();
            LogOutput.Close();
            LogOutput.Dispose();
        }
    }

    public struct Vector3F
    {
        public float X, Y, Z;

        public Vector3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public enum Logi
    {
        Nick,
        Pay,
        Kasyno,
        Ck,
        Ban,
        Warn,
        Setstat
    }

    class Program
    {
        static void Main()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            Console.WriteLine("Pracuję...");
            var start = Environment.TickCount & Int32.MaxValue;

            Console.WriteLine("Dodaję logi...");
            Logs[] logi = new Logs[7];
            logi[0] = new Logs(Global.Patch + @"\nick.log", "NickLog");
            logi[1] = new Logs(Global.Patch + @"\pay.log", "PayLog");
            logi[2] = new Logs(Global.Patch + @"\kasyno.log", "KasynoLog");
            logi[3] = new Logs(Global.Patch + @"\ck.log", "CKLog");
            logi[4] = new Logs(Global.Patch + @"\ban.log", "BanLog");
            logi[5] = new Logs(Global.Patch + @"\warn.log", "WarnLog");
            logi[6] = new Logs(Global.Patch + @"\setstats.log", "SetstatsLog");

            Console.WriteLine("Dodaję detektory...");
            //---- [ NickLog ] ----
            logi[(int)Logi.Nick].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Administrator (?:\w*_\w*) zmieni. nick (\w*_\w*)\[(\d*)\] - Nowy nick: (\w*_\w*)", typeof(ZmianaNicku)));
            logi[(int)Logi.Nick].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*)\[(\d*)\] zmieni. sobie nick - Nowy nick: (\w*_\w*)", typeof(ZmianaNicku)));

            //---- [ Płatności ] ----
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] << Hitman (\w*_\w*) wype.ni. kontrakt na: (?:\w*_\w*) i zarobi. \$(\d*) >>", typeof(Kontrakt)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) dal \/q podczas akcji i zabrano mu (\d*)\$ i (\d*) mats oraz bronie", typeof(QPodczasAkcji)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Totolotek: (\w*_\w*) Wygra. nagrod. w wysoko.ci: \$(\d*).", typeof(Totoloto)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) ZNIWELOWANO TWOJE (?:\d*)\$ DO 0\$. JE.ELI UWA.ASZ, .E NIES.USZNIE - ZG.O. STRAT. NA FORUM. AKUTALNY LVL: (\d*)", typeof(Niwelacja)));
            //logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] \* (\w*_\w*) zabiera portfel (\w*_\w*) razem z (\d*)\$", typeof(Kradziez)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) da. \$(\d*) graczowi (\w*_\w*)", typeof(DajHajs)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) dal teczke z \$(\d*) graczowi (\w*_\w*)", typeof(DajHajs)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) przela. \$(\d*) do (\w*_\w*)", typeof(Przelej)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) bardzo dzi.kujemy za przekazan. sum. \$(\d*).", typeof(Datek)));
            logi[(int)Logi.Kasyno].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] \[Kostka\] (\w*_\w*) wygral z (\w*_\w*) o (\d*)\$", typeof(Kostka)));

            //---- [ Auta ] ----
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) kupil pojazd (\w*\s?-?\w*) za (\d*)\$. UID (\d*)", typeof(KupnoAuta)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) kupi. od (\w*_\w*) auto marki (\w*\s?-?\w*) \(ID pliku auta:(\d*)\) za (\d*)\$", typeof(SprzedarzAuta)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) wymieni. z (\w*_\w*) auto marki (\w*\s?-?\w*) \(ID pliku auta:(\d*)\) za (\w*\s?-?\w*) \(ID pliku auta:(\d*)\) z dop.at. (\d*)\$", typeof(WymianaAuta)));
            //logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) kupi. od (\w*_\w*) neony do auta (\w*\s?-?\w*) \(UID auta:(\d*)\)", typeof(PustaAkcja)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Auto o ID (\d*) zosta.o zez.omowane przez (\w*_\w*)", typeof(ZlomowanieAuta)));
            
            //---- [ Domy ] ----
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) kupil dom \(id (\d*)\) za (\d*)\$", typeof(KupnoDomu)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) zezlomowal dom nr (\d*) i dostal (\d*)\$", typeof(ZlomowanieDomu)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Dom (\d*) wlasciciel: (\w*_\w*) zostal zezlomowany z powodu nieaktywnosci dluzszej niz 30 dni", typeof(Eksmisja)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) stracil dom z powodu nieaktywnosci \(id (\d*)\) i dostal (\d*)\$", typeof(PlatnaEksmisja)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) stracil dom z powodu dwoch wlascicieli \(id (\d*)\) i dostal (\d*)\$", typeof(PlatnaEksmisja)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) kupil dom \(id (\d*)\) od (\w*_\w*) za (\d*)\$.", typeof(SprzedazDomu)));
            
            //---- [ Sejfy ] ----
            /*logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Gracz (\w*_\w*) wlozyl (\d*)\$ do sejfu. W sejfie przed: (?:\d*), po: (?:\d*)", typeof(SejfHajsDo)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Gracz (\w*_\w*) wlozyl (\d*) matsow do sejfu", typeof(SejfMatsDo)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Gracz (\w*_\w*) wlozyl (\d*) dragow do sejfu", typeof(SejfDragiDo)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Gracz (\w*_\w*) wyjal (\d*)\$ z sejfu", typeof(SejfHajsZ)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Gracz (\w*_\w*) wyjal (\d*) mats z sejfu, poprzedni stan (?:\d*), nowy stan:", typeof(SejfMatsDo)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Gracz (\w*_\w*) wyjal (\d*) dragow z sejfu", typeof(SejfDragiDo)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] .* (\w*_\w*) wyplacil (\d*)\$ z sejfu frakcji nr (\d*). Jest w nim teraz (\d*)\$", typeof(SejfFrakcjiZ)));
            logi[(int)Logi.Pay].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] .* (\w*_\w*) wplacil (\d*)\$ do sejfu frakcji nr (\d*). Jest w nim teraz (\d*)\$", typeof(SejfFrakcjiDo)));
            */
            //---- [ Nominacje ] ----
            logi[(int)Logi.Setstat].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: (?:\w*_\w*) mianowa. (\w*_\w*) na (\d*) level admina.", typeof(Admin)));
            logi[(int)Logi.Ck].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: (?:\w*_\w*) mianowal (\w*_\w*) na (\d*) level poladmina.", typeof(PolAdmin)));
            logi[(int)Logi.Ck].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: (?:\w*_\w*) mianowa. (\w*_\w*) na (\d*) level zaufanego.", typeof(Zaufany)));

            //---- [ Kary ] ----
            //Bany
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) zostal zbanowany za (10mln i 1 lvl)", typeof(Ban)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Cziter (\w*_\w*) zostal zbanowany za (ucieczke z AJ)", typeof(Ban)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] (\w*_\w*) zostal zbanowany za (zbanowanie admina \/sban)", typeof(Ban)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: (\w*_\w*) zosta. zbanowany przez Admina (\w*_\w*) \(3 warny\), pow.d: (.*)", typeof(BanZAdminem1)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: P..Admin (\w*_\w*) zbanowa. (\w*_\w*), pow.d: (.*)", typeof(BanZAdminem2)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: Admin (\w*_\w*) zbanowal (\w*_\w*), pow.d: (.*)", typeof(BanZAdminem2)));
            //Blocki
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: Konto gracza (\w*_\w*) zostalo zablokowane przez (\w*_\w*), Powod: (.*)", typeof(Block)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: Konto gracza OFFLINE (\w*_\w*) zostalo zablokowane przez (\w*_\w*), Powod: (.*)", typeof(Block)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: Konto gracza (\w*_\w*) zostalo sblock przez (\w*_\w*), Powod: (.*)", typeof(Block)));
            //Warny
            logi[(int)Logi.Warn].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: Konto gracza OFFLINE (\w*_\w*) zostalo zwarnowane przez (\w*_\w*), Powod: (.*)", typeof(Warn)));
            logi[(int)Logi.Warn].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: (\w*_\w*) zostal zwarnowany przez Admina (\w*_\w*), pow.d: (.*)", typeof(Warn)));

            //---- [ Zdjęte Kary ] ----
            //UnBany
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] ADM: (?:\w*_\w*) - odblokowano nick: (\w*_\w*)", typeof(UnBan)));
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] ADM: (?:\w*_\w*) - odblokowano IP: (([0-9]+\.[0-9]+\.[0-9]+\.[0-9]+))", typeof(UnBanIP)));
            //UnBlocki
            logi[(int)Logi.Ban].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] Administrator (?:\w*_\w*) ublokowa. (\w*_\w*)", typeof(UnBlock)));
            //UnWarny
            logi[(int)Logi.Warn].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: Konto gracza (\w*_\w*) zosta.o unwarnowane przez (?:\w*_\w*).", typeof(UnWarn)));
            logi[(int)Logi.Warn].AddDetector(new Detektor(@"\[(\d{4})\/(\d{2})\/(\d{2}) - (\d{2}):(\d{2}):(\d{2})\] AdmCmd: (\w*_\w*) zosta. UN-warnowany przez Admina (?:\w*_\w*), pow.d: (?:.*)", typeof(UnWarn)));

            Console.WriteLine("Zaczynam przetwarzać logi");
            foreach (var log in logi)
            {
                using (var logFile = new StreamReader(log.Filename))
                {
                    Console.WriteLine($"Przetawrzam logi {log.Name}...");
                    string line;
                    while ((line = logFile.ReadLine()) != null)
                    {
                        log.MatchDetector(line);
                        Global.ProcessedLogLines++;
                    }
                }
                log.Close();
            }

            Console.WriteLine("Generuje zapytania tworzące nowych graczy...");
            Gracze.GenerujZapytaniaTworzace();
            Global.Close();

            var stop = Environment.TickCount & Int32.MaxValue;
            Console.WriteLine($"Koniec\nCzas wykonania: {(stop-start)/1000}s\nLiczba linii przetworzonych logów: {Global.ProcessedLogLines}\nLiczba wygenerowanych zapytań: {Global.GeneratedQueries}\nLiczba stworzonych nowych kont od 30.06.2016: {Gracze.LastUid - 100946}");
            Console.ReadKey();
        }
    }
}
