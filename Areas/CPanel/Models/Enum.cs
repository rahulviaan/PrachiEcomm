using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace PrachiIndia.Web.Areas.Model
{
    public enum contentType
    {
        none = 0,
        html = 1,
        mathml = 2,
        png = 3,
        jpg = 4,
        jpeg = 5,
        mp4 = 6,
        pdf = 7,
        ImageHtml = 8
    }
    public enum ChapterType
    {
        [Description("None")]
        None = 0,
        [Description("Lesson Plan")]
        Lesson_Plan = 1,
        [Description("Worksheet")]
        Workheet = 2,
        [Description("Audio Video")]
        Audio_Video = 3,
        [Description("Solution")]
        Solutions = 4
    }
    public enum TextType
    {

        Text = 0,
        Image = 1,

    }
    public enum IsValid
    {

        Inactive = 0,
        Active = 1,
        Authenticated = 2,
        All = 3,
        Deleted = 4,
    }
    public enum Status
    {
        [Description("Inactive")]
        Inactive = 0,
        [Description("Active")]
        Active = 1,
        [Description("Deleted")]
        Deleted = 2,
        [Description("All")]
        All = -1,

    }

    public enum IsPublished
    {

        Yes = 1,
        No = 2,
        All = 0
    }


    public enum MasterSynch
    {


        Class = 0,
        Board = 1,
        Subject = 3,
        Series = 4,
        Book = 5,
        All = 6
    }
    public enum DeviceType
    {
        Window = 0,
        Android = 1,
        Apple = 2,
        Linux = 3,
        Other = 4
    }

    public enum _enBinary
    {
        No,
        Yes
    }


    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> ToSelectLists<TEnum>()
        {
            var myEnumDescriptions = from TEnum n in System.Enum.GetValues(typeof(TEnum))
                                     select new SelectListItem
                                     {
                                         Text = GetEnumDescription(n),
                                         Value = n.GetHashCode().ToString()
                                     };
            return myEnumDescriptions;
        }

        private static string GetEnumDescription<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }

    public enum RepositoryType {
        aspnet_Applications,
        aspnet_Membership,
        aspnet_Paths,
        aspnet_PersonalizationAllUsers,
        aspnet_PersonalizationPerUser,
        aspnet_Profile,
        aspnet_Roles,
        aspnet_SchemaVersions,
        aspnet_Users,
        aspnet_WebEvent_Events,
        AspNetRoleRepository,
        AspNetUserClaim,
        AspNetUserLogin,
        AspNetUserRepository,
        BookSpecification,
        Cart,
        ChapterContent,
        Chapter,
        DeveloperMaster,
        EbookOrder,
        EbookOrderSubject,
        ErrorLog,
        ExceptionLog,
        MasterBoard,
        MasterClass,
        MasterSubject,
        mst_Question_Category,
        NewArrival,
        OrderRepository,
        OrderTrack,
        OTPDETAIL,
        SubjectClass,
        Subscription,
        tbl_Questions,
        tblCataLogBoard,
        tblCataLogClass,
        tblCompanyAddress,
        TblShipingAddress,
        UserAddress,
        UserAddressBook,
        UserLibrary,
        UserReader,
        UserReaderLibrary,
        CityRepository,
        Country,
        tbl_Question_Options,
        mst_subscription,
        GSTTaxList,
        tblCataLog,
        MasterSery,
        DVDMaster,
        OrderProduct,
        State,
        City,
        CountryRepository,
        StateRepositories,
        SubjectRepositories,
        SubjectModel,
        MasterSeriesRepositories,
        MasterSubjectrepository,
        DVDMasterReposiroty,
        MasterClassRepository,
        ClassModel,
        MasterBoardRepository,
        BoardModel,
        CatalogRepository,
        dbPrachiIndia_PortalEntities,
        Books,
        GSTTaxListRepository,
        SeriesModel,
        TopicRepository,
        ChapterRepository
    }
}
