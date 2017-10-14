using Shouldly;
using Xunit;

namespace SuperMarioKata
{
    public class UnitTest1
    {
        [Fact]
        public void Wenn_alle_Leben_aufgebraucht_sind_ist_Super_Mario_tot()
        {
            var mario = new Mario(new Leben(0), new KeinGegenstand());
            mario.WirdGetroffen().Leben.ShouldBeOfType<Tod>();
        }

        [Fact]
        public void Wenn_Super_Mario_einen_Pilz_findet_wächst_er()
        {
            new Mario().FindetPilz().Gegenstand.ShouldBeOfType<Pilz>();
        }

        [Fact]
        public void Wenn_Super_Mario_mit_Pilz_vom_Gegner_getroffen_wird_wird_er_zum_kleinen_Mario()
        {
            var mario = new Mario(new Leben(1), new Pilz()).WirdGetroffen();
            mario.Gegenstand.ShouldBeOfType<KeinGegenstand>();
            mario.Leben.ShouldBe(new Leben(1));
        }

        [Fact]
        public void Wenn_Super_Mario_startet_soll_er_3_Leben_haben()
        {
            var mario = new Mario();
            mario.Leben.ShouldBe(new Leben(3));
        }

        [Fact]
        public void Wenn_Super_Mario_stirbt_verliert_er_ein_Leben()
        {
            var mario = new Mario(new Leben(2), new KeinGegenstand());
            mario.WirdGetroffen().Leben.ShouldBe(new Leben(1));
        }
    }


    public class Leben : ILebensZustand
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Leben)obj);
        }

        public override int GetHashCode()
        {
            return _anzahlLeben;
        }

        public static bool operator ==(Leben left, Leben right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Leben left, Leben right)
        {
            return !Equals(left, right);
        }

        public static Leben operator -(Leben leben1, Leben leben2)
        {
            return new Leben(leben1._anzahlLeben - leben2._anzahlLeben);
        }

        public Leben(int anzahlLeben)
        {
            _anzahlLeben = anzahlLeben;
        }
    }


    public class Tod : ILebensZustand
    {
    }

    public interface IFigur
    {
        ILebensZustand Leben { get; }
        IGegenstand Gegenstand { get; }
        IFigur FindetPilz();
    }

    public interface IGegenstand
    {

    }

    public class KeinGegenstand : IGegenstand
    {

    }

    public class Pilz : IGegenstand
    {

    }

    public class Mario : IFigur
    {
        public Mario()
        {
            Leben = new Leben(3);
            Gegenstand = new KeinGegenstand();
        }

        public Mario(ILebensZustand lebenszustand, IGegenstand gegenstand)
        {
            Leben = lebenszustand;
            Gegenstand = gegenstand;
        }

        public ILebensZustand Leben { get; }
        public IGegenstand Gegenstand { get; }

        public IFigur WirdGetroffen()
        {
            if (Gegenstand is Pilz)
            {
                return new Mario(Leben, new KeinGegenstand());
            }

            var leben = (Leben)Leben;
            if (leben == new Leben(0))
            {
                return new Mario(new Tod(), new KeinGegenstand());
            }
            return new Mario(leben - new Leben(1), new KeinGegenstand());
        }

        public IFigur FindetPilz()
        {
            return new Mario(Leben, new Pilz());
        }
    }

    public interface ILebensZustand
    {
    }
}
