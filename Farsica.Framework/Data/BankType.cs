namespace Farsica.Framework.Data
{
    using Farsica.Framework.Core;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public sealed class BankType : Enumeration.Enumeration<byte>
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Shahr = new(nameof(Shahr), 0, "CIYBIR", "061");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Saman = new(nameof(Saman), 1, "SABCIR", "056");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType EghtesadNovin = new(nameof(EghtesadNovin), 2, "BEGNIR", "055");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Day = new(nameof(Day), 3, "DAYBIR", "066");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Sina = new(nameof(Sina), 4, "SINAIR", "059");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Sarmayeh = new(nameof(Sarmayeh), 5, "SRMBIR", "058");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType IranVenezoela = new(nameof(IranVenezoela), 6, "IVBBIR", "095");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType IranZamin = new(nameof(IranZamin), 7, "IRZAIR", "069");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType MehrIran = new(nameof(MehrIran), 8, "MEHRIR", "060");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType PostBank = new(nameof(PostBank), 9, "PBIRIR", "021");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Refah = new(nameof(Refah), 10, "REFAIR", "013");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Mellat = new(nameof(Mellat), 11, "BKMTIR", "012");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Tejarat = new(nameof(Tejarat), 12, "BTEJIR", "018");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Sepah = new(nameof(Sepah), 13, "SEPBIR", "015");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Pasargad = new(nameof(Pasargad), 14, "BKBPIR", "057");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Ayandeh = new(nameof(Ayandeh), 15, "AYBKIR", "062");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Tosee = new(nameof(Tosee), 16, "BTOSIR", "051");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType SanatMadan = new(nameof(SanatMadan), 17, "BOIMIR", "011");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Melli = new(nameof(Melli), 18, "MELIIR", "017");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Keshavarzi = new(nameof(Keshavarzi), 19, "KESHIR", "016");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Maskan = new(nameof(Maskan), 20, "BKMNIR", "014");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType ToseeSaderat = new(nameof(ToseeSaderat), 21, "EDBIIR", "020");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType ToseeTaavon = new(nameof(ToseeTaavon), 22, "TTBIIR", "022");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Parsian = new(nameof(Parsian), 23, "BKPAIR", "054");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType KarAfarin = new(nameof(KarAfarin), 24, "KBIDIR", "053");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType KhavarMianeh = new(nameof(KhavarMianeh), 25, "KHMIIR", "078");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Saderat = new(nameof(Saderat), 26, "BSIRIR", "019");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Gardeshgari = new(nameof(Gardeshgari), 27, "TOSMIR", "064");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Resalat = new(nameof(Resalat), 28, "RQBAIR", "070");

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType))]
        public static readonly BankType Melal = new(nameof(Melal), 29, "MELBIR", "075");

        public BankType(string name, byte value, string swiftCode, string ban)
            : base(name, value)
        {
            SwiftCode = swiftCode;
            Ban = ban;
        }

        public string SwiftCode { get; set; }

        public string Ban { get; set; }

        public string? LocalizedName => Globals.DisplayNameFor<BankType>(t => t.Value == Value) ?? Name;
    }
}
