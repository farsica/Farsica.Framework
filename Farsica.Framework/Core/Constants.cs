namespace Farsica.Framework.Core
{
    using System;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public static class Constants
    {
        public const string Gotcha = "Gotcha";

        public const int NullInteger = -1;
        public const string Delimiter = "___";
        public const string DelimiterAlternate = "|";
        public const char JoinDelimiter = ',';
        public const int ForeignKeySqlException = 547;
        public const int DuplicateSqlException = 2627;
        public const string ValidImageExtensions = "png,jpg,jpeg,gif";
        public const string ValidDocumentExtensions = "pdf,doc,docx";
        public const string ValidVideoExtensions = "mpeg,avi";
        public const string ValidAndroidExtensions = "apk";
        public const string ValidWebExtensions = "zip";
        public const string ValidIosExtensions = "ipa";
        public const string ValidWindowsExtensions = "xap";
        public const string ValidImageMimeTypes = "image/png,image/jpeg,image/gif";
        public const string ValidDocumentMimeTypes = "application/pdf,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string ValidVideoMimeTypes = "audio/mpeg,video/x-msvideo";
        public const string ValidZipMimeTypes = "application/zip,application/x-zip-compressed,application/x-7z-compressed,application/x-bzip,application/x-bzip2,application/gzip,application/x-rar-compressed";
        public const string ValidAndroidMimeTypes = "application/vnd.android.package-archive,application/zip,application/x-zip-compressed";
        public const string ValidIosMimeTypes = "application/octet-stream,application/zip,application/x-zip-compressed";
        public const string ValidWindowsMimeTypes = "application/x-silverlight-app,application/zip,application/x-zip-compressed";
        public const string DefaultLanguageCode = "fa";
        public const string LanguageIdentifier = "culture";
        public const string AreaIdentifier = "area";
        public const int TotalRecords = 500000;
        public const string ReportSearchFieldPrefix = "Report_";
        public const string XForwardedFor = "X-Forwarded-For";
        public const string DefaultTenantIdentifier = "Farsica";
        public const string ArchiveIdentifier = "Archive";
        public const string ActionsIdentifier = "Actions";
        public const string MigrateIdentifier = "Migrate";
        public const string LineBreak = "[BR]";
        public const string HierarchyValueDelimiter = "/";
        public const string PasswordIdentifier = "******";
        public const string PasswordIdentifierWithData = "************";
        public const string SchemaIdentifier = "[SCHEMA]";
        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        public const string Cookie = "__f";
        public const string AspNetApplicationCookie = "__fapp";
        public const string RequestVerificationTokenCookie = "__frvt";
        public const int DisplayOrder = 10000;
        public const string ControllerPostfix = "Controller";
        public const string PagePostfix = "Model";
        internal const string HttpClientIgnoreSslAndAutoRedirect = "HttpClientIgnoreSslAndAutoRedirect";

        public enum DegreeType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
            Diploma = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
            Associate = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
            Bachelor = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
            Master = 3,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
            Phd = 4,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
            PostDoc = 5,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
            Other = 6,
        }

        public enum GenderType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(GenderType))]
            Male = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(GenderType))]
            Female = 1,
        }

        public enum OperationResult : byte
        {
            NotFound = 0,
            Succeeded = 1,
            Failed = 2,
            Duplicate = 3,
            NotValid = 4,
        }

        public enum SmtpAuthenticationType : byte
        {
            Anonymous = 0,
            Basic = 1,
            Ntlm = 2,
        }

        public enum DisplayTemplates : byte
        {
            Boolean = 0,
            Date = 1,
            DateTime = 2,
            Decimal = 3,
            Double = 4,
            Guid = 5,
            Integer = 6,
            String = 7,
            TimeSpan = 8,
            Uri = 9,
            LongString = 10,
            Rate = 11,
            LineChart = 12,
            Image = 13,
            PostInfo = 14,
            Color = 15,
            Short = 16,
            StringMultipleSelect = 17,
            PieChart = 18,
            BarChart = 19,
            Enum = 20,
        }

        public enum EditorTemplates : byte
        {
            Boolean = 0,
            Date = 1,
            DateTime = 2,
            Decimal = 3,
            Double = 4,
            Guid = 5,
            Integer = 6,
            LongString = 7,
            Password = 8,
            String = 9,
            TimeSpan = 10,
            Uri = 11,
            Captcha = 12,
            Select = 13,
            MultipleSelect = 14,
            EnumSelect = 15,
            Rate = 16,
            HtmlEditor = 17,
            Version = 18,
            Image = 19,
            Percent = 20,
            Short = 21,
            LocalizableString = 22,
            LocalizableLongString = 23,
            LocalizableHtmlEditor = 24,
            RegisterName = 25,
            Color = 26,
            StringMultipleSelect = 27,
            Month = 28,
            EnumMultipleSelect = 29,
            TimeSpanRange = 30,
            File = 31,
            Treeview = 32,
        }

        public enum UserState : byte
        {
            Disable = 0,

            Enable = 1,

            Deleted = 2,
        }

        public enum UserType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(UserType))]
            Real = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(UserType))]
            Legal = 1,
        }

        public enum DenyLevel
        {
            Read,
            Create,
            Delete,
            Modify,
        }

        public enum Status : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(Status))]
            Failed = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(Status))]
            Succeed = 1,
        }

        public enum OAuthGrantType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OAuthGrantType))]
            AuthorizationCode = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OAuthGrantType))]
            RefreshToken = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OAuthGrantType))]
            Implicit = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OAuthGrantType))]
            Password = 3,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OAuthGrantType))]
            Client = 4,
        }

        public enum ExportType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ExportType))]
            Excel = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ExportType))]
            Pdf = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ExportType))]
            Csv = 2,
        }

        public enum OperandType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            Equals = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            GreaterThan = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            LessThan = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            GreaterThanOrEqual = 3,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            LessThanOrEqual = 4,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            Contains = 5,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            StartsWith = 6,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            EndsWith = 7,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(OperandType))]
            NotEquals = 8,
        }

        public enum ReportType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ReportType))]
            Grid = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ReportType))]
            PieChart = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ReportType))]
            BarChart = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ReportType))]
            LineChart = 3,
        }

        public enum DataType : byte
        {
            Byte = 0,

            NullableByte = 1,

            Int16 = 2,

            NullableInt16 = 3,

            Int32 = 4,

            NullableInt32 = 5,

            Int64 = 6,

            NullableInt64 = 7,

            Boolean = 8,

            NullableBoolean = 9,

            Decimal = 10,

            NullableDecimal = 11,

            String = 12,

            DateTime = 13,

            NullableDateTime = 14,
        }

        public enum EntityState : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(EntityState))]
            Detached = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(EntityState))]
            Unchanged = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(EntityState))]
            Added = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(EntityState))]
            Deleted = 3,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(EntityState))]
            Modified = 4,
        }

        [Flags]
        public enum DayOfWeek : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
            Sunday = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
            Monday = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
            Tuesday = 4,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
            Wednesday = 8,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
            Thursday = 16,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
            Friday = 32,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
            Saturday = 64,
        }

        public enum DayType : byte
        {
            Normal = 0,

            Weekend = 1,

            Holiday = 2,
        }

        public enum TreeViewOperation : byte
        {
            DeleteNode = 0,

            CreateNode = 1,

            RenameNode = 2,

            MoveNode = 3,

            CopyNode = 4,
        }

        [Flags]
        public enum HttpMethod : short
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Post = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Get = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Head = 4,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Put = 8,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Delete = 16,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Connect = 32,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Options = 64,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Trace = 128,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(HttpMethod))]
            Patch = 256,
        }

        public enum Routes
        {
            Default,
            DefaultLocalized,
            DefaultApi,
            DefaultApiLocalized,
        }

        public enum ParameterType : byte
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ParameterType))]
            BodyParameter = 0,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ParameterType))]
            QueryParameter = 1,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ParameterType))]
            PathParameter = 2,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ParameterType))]
            HeaderParameter = 3,
        }

        public enum BankType
        {
            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "CIYBIR")]
            Shahr,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "SABCIR")]
            Saman,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BEGNIR")]
            EghtesadNovin,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "HEKMIR")]
            Hekmat,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "ANSBIR")]
            Ansar,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "DAYBIR")]
            Day,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "SINAIR")]
            Sina,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "SRMBIR")]
            Sarmayeh,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "IVBBIR")]
            IranVenezoela,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "IRZAIR")]
            IranZamin,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "MEHRIR")]
            MehrIran,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "MEDBIR")]
            MehrEghtesad,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "PBIRIR")]
            PostBank,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "REFAIR")]
            Refah,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BKMTIR")]
            Mellat,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BTEJIR")]
            Tejarat,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "SEPBIR")]
            Sepah,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BKBPIR")]
            Pasargad,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "AYBKIR")]
            Ayandeh,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BTOSIR")]
            Tosee,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BOIMIR")]
            SanatMadan,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "MELIIR")]
            Melli,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "KESHIR")]
            Keshavarzi,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BKMNIR")]
            Maskan,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "EDBIIR")]
            ToseeSaderat,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "TTBIIR")]
            ToseeTaavon,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BKPAIR")]
            Parsian,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "KBIDIR")]
            KarAfarin,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "KHMIIR")]
            KhavarMianeh,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "BSIRIR")]
            Saderat,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "TOSMIR")]
            Gardeshgari,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "GHBIIR")]
            Ghavamin,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "RQBAIR")]
            Resalat,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "KOSRIR")]
            Kosar,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "NOORIR")]
            Noor,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "MELBIR")]
            Melal,

            [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(BankType), ShortName = "CASPIR")]
            Caspian,
        }

        public enum SignInStatus
        {
            Success,
            LockedOut,
            RequiresVerification,
            RequiresVerificationEmail,
            Failure,
            NotEnable,
            Limitation,
        }

        internal enum ResourceKey
        {
            Name,
            ShortName,
            Description,
            Prompt,
            GroupName,
        }

        public static Tenant DefaultTenant => new() { Name = DefaultTenantIdentifier, Code = "BOOMIR", Schema = "Farsica", ArchiveSchema = "Farsica_ARCHIVE" };
    }
}
