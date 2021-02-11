using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrachiIndia.Web.Areas.Model;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Sql.Repositories;
using PrachiIndia.Sql;

namespace PrachiIndia.Portal.Factory
{
    /// <summary>
    /// Author : Rahul Srivastava
    /// Purose : Implemeting Singleton And Factory pattern 
    /// </summary>
    public class Factory
    {
        #region Properties
        public static object aspnet_Applications { get; set; }
        public static object aspnet_Membership { get; set; }
        public static object aspnet_Paths { get; set; }
        public static object aspnet_PersonalizationAllUsers { get; set; }
        public static object aspnet_PersonalizationPerUser { get; set; }
        public static object aspnet_Profile { get; set; }
        public static object aspnet_Roles { get; set; }
        public static object aspnet_SchemaVersions { get; set; }
        public static object aspnet_Users { get; set; }
        public static object aspnet_WebEvent_Events { get; set; }
        public static object aspNetUserClaim { get; set; }
        public static object aspNetUserLogin { get; set; }
        public static object aspNetUserRepository { get; set; }
        public static object bookSpecification { get; set; }
        public static object cart { get; set; }
        public static object chapterContent { get; set; }
        public static object chapter { get; set; }
        public static object developerMaster { get; set; }
        public static object ebookOrder { get; set; }
        public static object ebookOrderSubject { get; set; }
        public static object errorLog { get; set; }
        public static object exceptionLog { get; set; }
        public static object masterBoard { get; set; }
        public static object masterClass { get; set; }
        public static object masterSubject { get; set; }
        public static object mst_Question_Category { get; set; }
        public static object newArrival { get; set; }
        public static object orderRepository { get; set; }
        public static object orderTrack { get; set; }
        public static object oTPDETAIL { get; set; }
        public static object subjectClass { get; set; }
        public static object subscription { get; set; }
        public static object tbl_Questions { get; set; }
        public static object tblCataLogBoard { get; set; }
        public static object tblCataLogClass { get; set; }
        public static object tblCompanyAddress { get; set; }
        public static object tblShipingAddress { get; set; }
        public static object userAddress { get; set; }
        public static object userAddressBook { get; set; }
        public static object userLibrary { get; set; }
        public static object userReader { get; set; }
        public static object userReaderLibrary { get; set; }
        public static object country { get; set; }
        public static object tbl_Question_Options { get; set; }
        public static object mst_subscription { get; set; }
        public static object gSTTaxList { get; set; }
        public static object tblCataLog { get; set; }
        public static object masterSery { get; set; }
        public static object dVDMaster { get; set; }
        public static object orderProduct { get; set; }
        public static object state { get; set; }
        public static object city { get; set; }
        public static object serires { get; set; }
        public static object subject { get; set; }        
        public static object classModel { get; set; }
        public static object boardModel { get; set; }
        public static object book { get; set; }
        public static object seriesModel { get; set; }
        static object objIfactory = null;

        
        #endregion

        public static object GetInstance(RepositoryType repositoryType)
        {

            switch (repositoryType)
            {
                #region Country

                case RepositoryType.Country:
                    if (!(country is Country))
                    {
                        country = new Country();
                    }
                    return country;
                #endregion

                #region City

                case RepositoryType.City:
                    if (!(city is City))
                    {
                       city = new City();
                    }
                    return city;
                #endregion

                #region State

                case RepositoryType.State:
                    if (!(state is State))
                    {
                        state = new State();
                    }
                    return state;

                #endregion

                #region Subject
                case RepositoryType.SubjectModel:
                    if (!(subject is SubjectModel))
                    {
                        subject = new SubjectModel();
                     }
                    return subject;
                #endregion

                #region Series
                case RepositoryType.MasterSery:
                    if (!(serires is SeriesModel))
                    {
                        serires= new SeriesModel();
                    }
                    return serires;
                #endregion

                #region ClassModel
                case RepositoryType.ClassModel:
                    if (!(classModel is ClassModel))
                    {
                        classModel = new ClassModel();
                    }
                    return classModel;
                #endregion

                #region BoardModel
                case RepositoryType.BoardModel:
                    if (!(boardModel is BoardModel))
                    {
                        boardModel = new BoardModel();
                    }
                    return boardModel;
                #endregion

                #region MasterClass
                case RepositoryType.MasterClass:
                    if (!(masterClass is MasterClass))
                    {
                        masterClass = new MasterClass();
                    }
                    return masterClass;
                #endregion

                #region Books
                case RepositoryType.Books:
                    if (!(book is Books))
                    {
                        book = new Books();
                    }
                    return book;
                #endregion

                #region SeriesModel
                case RepositoryType.SeriesModel:
                    if (!(seriesModel is SeriesModel))
                    {
                        seriesModel = new SeriesModel();
                    }
                    return seriesModel;
                #endregion

                default:
                    return objIfactory;
            }
        }
    }
}