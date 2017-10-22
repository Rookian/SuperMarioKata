using System;
using Shouldly;
using Xunit;
using System.Reactive.Linq;
using System.Threading;

namespace SuperMarioKata
{
    public class MarioTests
    {
        [Fact]
        public void Wenn_Super_Mario_einen_Pilz_findet_wächst_er()
        {
            new Mario().FindetGegenstand(new Pilz()).Gegenstand.ShouldBeOfType<Pilz>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Pilz_vom_Gegner_getroffen_wird_wird_er_zum_kleinen_Mario()
        {
            var mario = new Mario(1.Leben(), new Pilz()).WirdGetroffen();
            mario.Gegenstand.ShouldBeOfType<KeinGegenstand>();
            mario.Leben.ShouldBe(1.Leben());
        }

        [Fact]
        public void Wenn_kleiner_Super_Mario_vom_Gegner_getroffen_wird_stirbt_er_und_verliert_ein_Leben()
        {
            var mario = new Mario(1.Leben(), new KeinGegenstand()).WirdGetroffen();
            mario.Gegenstand.ShouldBeOfType<KeinGegenstand>();
            mario.Leben.ShouldBe(0.Leben());
        }

        [Fact]
        public void Wenn_Super_Mario_startet_soll_er_3_Leben_haben()
        {
            var mario = new Mario();
            mario.Leben.ShouldBe(3.Leben());
        }

        [Fact]
        public void Wenn_Super_Mario_stirbt_verliert_er_ein_Leben()
        {
            var mario = new Mario(2.Leben(), new KeinGegenstand());
            mario.WirdGetroffen().Leben.ShouldBe(1.Leben());
        }

        [Fact]
        public void Wenn_alle_Leben_aufgebraucht_sind_ist_Super_Mario_tot()
        {
            var mario = new Mario(new KeinGegenstand());
            mario.WirdGetroffen().Leben.ShouldBeOfType<Tod>();
        }

        [Fact]
        public void Wenn_Super_Mario_ein_Leben_findet_erhöhen_sich_seine_verfügbaren_Leben()
        {
            var mario = new Mario(new Pilz())
                .FindetGegenstand(new LebensPunkt());

            mario.Leben.ShouldBe(1.Leben());
            mario.Gegenstand.ShouldBeOfType<Pilz>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Feuerblume_vom_Gegner_getroffen_wird_wird_er_zum_Mario_mit_Pilz()
        {
            var mario = (IFigur)new Mario(new Feuerblume());
            mario = mario.WirdGetroffen();
            mario.Leben.ShouldBe(0.Leben());
            mario.Gegenstand.ShouldBeOfType<Pilz>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Feuerblume_einen_Pilz_findet_behält_er_seine_Feuerblume()
        {
            var mario = new Mario(new Feuerblume()).FindetGegenstand(new Pilz());
            mario.Gegenstand.ShouldBeOfType<Feuerblume>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Eisblume_vom_Gegner_getroffen_wird_wird_er_zum_Mario_mit_Pilz()
        {
            var mario = new Mario(new Eisblume())
                .WirdGetroffen();

            mario.Leben.ShouldBe(0.Leben());
            mario.Gegenstand.ShouldBeOfType<Pilz>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Eisblume_einen_Pilz_findet_behält_er_seine_Eisblume()
        {
            var mario = new Mario(new Eisblume());
            mario.FindetGegenstand(new Pilz())
                .Gegenstand.ShouldBeOfType<Eisblume>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Eisblume_eine_Feuerblume_findet_wechselt_er_zur_Feuerblume()
        {
            new Mario(new Eisblume())
                .FindetGegenstand(new Feuerblume())
                .Gegenstand.ShouldBeOfType<Feuerblume>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Eisblume_den_Befehl_zum_Schießen_erhält_schießt_er_Eis()
        {
            var mario = new Mario(new Eisblume());
            mario.Schießen().ShouldBeOfType<Eis>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Feuerblume_den_Befehl_zum_Schießen_erhält_schießt_er_Feuer()
        {
            var mario = new Mario(new Feuerblume());
            mario.Schießen().ShouldBeOfType<Feuer>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Pilz_den_Befehl_zum_Schießen_erhält_passiert_nichts()
        {
            var mario = new Mario(new Pilz());
            mario.Schießen().ShouldBeOfType<KeinSchuss>();
        }

        [Fact]
        public void Wenn_kleiner_Super_Mario__den_Befehl_zum_Schießen_erhält_passiert_nichts()
        {
            var mario = new Mario(new KeinGegenstand());
            mario.Schießen().ShouldBeOfType<KeinSchuss>();
        }

        [Fact]
        public void Wenn_Mario_einen_Stern_findet_dann_verliert_er_diesen_automatisch_nach_2_Sekunden_wieder()
        {
            var mario = new Mario();
            var marioMitStern = mario.FindetGegenstand(new Stern(mario.Gegenstand));
            marioMitStern.Gegenstand.ShouldBeOfType<Stern>();
            Thread.Sleep(1000 * 3);
            marioMitStern.Gegenstand.ShouldBeOfType<KeinGegenstand>();
        }

        [Fact]
        public void Wenn_Mario_aktuell_einen_Stern_besitzt_und_vom_Gegner_getroffen_wird_dann_passiert_ihm_nichts()
        {

        }

        [Fact]
        public void Wenn_ein_kleiner_Mario_mit_Stern_Pilz_oder_Blume_findet_wächst_er_behält_aber_seinen_Stern_bei()
        {

        }
    }

    public class Stern : IGegenstand
    {
        public Stern(IGegenstand gegenstand)
        {
            Gegenstand = gegenstand;
        }

        public IGegenstand Gegenstand { get; }

        public IGegenstand Getroffen()
        {
            return this;
        }

        public ISchuss Schuss()
        {
            return Gegenstand.Schuss();
        }
    }

    public class Feuer : ISchuss
    {
    }

    public interface IBlume
    {

    }

    public abstract class Blume<TSchuss> : IGegenstand, IBlume where TSchuss : ISchuss, new()
    {
        public IGegenstand Getroffen()
        {
            return new Pilz();
        }

        public ISchuss Schuss()
        {
            return new TSchuss();
        }
    }

    public class Eisblume : Blume<Eis>
    {
    }

    public class Feuerblume : Blume<Feuer>
    {
    }


    public class Leben : ILeben
    {
        private readonly int _anzahlLeben;

        protected bool Equals(Leben other)
        {
            return _anzahlLeben == other._anzahlLeben;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Leben)obj);
        }

        public override int GetHashCode()
        {
            return _anzahlLeben.GetHashCode();
        }

        public static bool operator ==(Leben left, Leben right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Leben left, Leben right)
        {
            return !Equals(left, right);
        }

        public Leben(int anzahlLeben)
        {
            _anzahlLeben = anzahlLeben;
        }

        public ILeben LebenErhöhen()
        {
            return new Leben(_anzahlLeben + 1);
        }

        public ILeben LebenVerlieren()
        {
            if (_anzahlLeben == 0)
                return new Tod();
            return new Leben(_anzahlLeben - 1);
        }
    }


    public class Tod : ILeben
    {
        public ILeben LebenErhöhen()
        {
            return this;
        }

        public ILeben LebenVerlieren()
        {
            return this;
        }
    }

    public interface IFigur
    {
        ILeben Leben { get; }
        IGegenstand Gegenstand { get; }
        IFigur FindetGegenstand(IGegenstand gegenstand);
        IFigur WirdGetroffen();
    }

    public interface IGegenstand
    {
        IGegenstand Getroffen();
        ISchuss Schuss();
    }

    public class KeinGegenstand : IGegenstand
    {
        public IGegenstand Getroffen()
        {
            return this;
        }

        public ISchuss Schuss()
        {
            return new KeinSchuss();
        }
    }

    public class LebensPunkt : IGegenstand
    {
        public IGegenstand Getroffen()
        {
            return this;
        }

        public ISchuss Schuss()
        {
            return new KeinSchuss();
        }
    }

    public class Pilz : IGegenstand
    {
        public IGegenstand Getroffen()
        {
            return new KeinGegenstand();
        }

        public ISchuss Schuss()
        {
            return new KeinSchuss();
        }
    }

    public class Mario : IFigur
    {
        /// <summary>
        /// Mario mit 0 Leben
        /// </summary>
        /// <param name="gegenstand"></param>
        public Mario(IGegenstand gegenstand) : this(0.Leben(), gegenstand) { }

        /// <summary>
        /// Kleiner Mario mit 3 Leben
        /// </summary>
        public Mario()
        {
            Leben = new Leben(3);
            Gegenstand = new KeinGegenstand();
        }

        public Mario(ILeben leben, IGegenstand gegenstand)
        {
            Leben = leben;
            Gegenstand = gegenstand;

            if (gegenstand is Stern stern)
            {
                Observable
                    .Timer(TimeSpan.FromSeconds(2))
                    .Subscribe(x =>
                    {
                        Gegenstand = stern.Gegenstand;
                    });
            }
        }

        public ILeben Leben { get; }
        public IGegenstand Gegenstand { get; private set; }

        public IFigur WirdGetroffen()
        {
            if (Gegenstand is KeinGegenstand)
            {
                var leben = Leben.LebenVerlieren();
                return new Mario(leben, Gegenstand);
            }

            return new Mario(Leben, Gegenstand.Getroffen());
        }

        public IFigur FindetGegenstand(IGegenstand gegenstand)
        {
            if (gegenstand is LebensPunkt)
            {
                return new Mario(Leben.LebenErhöhen(), Gegenstand);
            }

            if (Gegenstand is IBlume && gegenstand is Pilz)
                return new Mario(Leben, Gegenstand);

            return new Mario(Leben, gegenstand);
        }

        public ISchuss Schießen()
        {
            return Gegenstand.Schuss();
        }
    }

    public interface ISchuss
    {
    }

    public class KeinSchuss : ISchuss
    {

    }
    public class Eis : ISchuss
    {

    }

    public interface ILeben
    {
        ILeben LebenErhöhen();
        ILeben LebenVerlieren();
    }

    public static class LebenExtension
    {
        public static ILeben Leben(this int leben)
        {
            return new Leben(leben);
        }
    }
}
