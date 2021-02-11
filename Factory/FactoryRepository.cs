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
    public class FactoryRepository
    {
        public static object GetInstance(RepositoryType repositoryType)
        {

            switch (repositoryType)
            {
                #region AspNetUserRepository
                case RepositoryType.AspNetUserRepository:
                    return new AspNetUserRepository();
                #endregion

                #region OrderRepository
                case RepositoryType.OrderRepository:
                    return new OrderRepository();
                #endregion

                #region AspNetRolesRepository

                case RepositoryType.AspNetRoleRepository:
                    return new AspNetRolesRepository();

                #endregion

                #region CityRepository

                case RepositoryType.CityRepository:
                    return new CityRepositories();

                #endregion

                #region CountryRepository

                case RepositoryType.CountryRepository:
                    return new CountryRepositories();
                #endregion

                #region StateRepository

                case RepositoryType.StateRepositories:
                    return new StateRepositories();

                #endregion

                #region SubjectRepository

                case RepositoryType.SubjectRepositories:
                    return new MasterSubjectRepository();

                #endregion

                #region SeriesRepository

                case RepositoryType.MasterSeriesRepositories:
                    return new MasterSeriesRepositories();

                #endregion
                #region SubjectRepository

                case RepositoryType.MasterSubjectrepository:
                    return new MasterSubjectRepository();

                #endregion
                #region DVDRepository

                case RepositoryType.DVDMasterReposiroty:
                    return new DVDRepository();

                #endregion

                #region ClassRepository

                case RepositoryType.MasterClassRepository:
                    return new MasterClassRepository();

                #endregion

                #region BoardRepository

                case RepositoryType.MasterBoardRepository:
                    return new MasterBoardRepository();

                #endregion

                #region CatalogRepository
                case RepositoryType.CatalogRepository:
                    return new CatalogRepository();
                #endregion

                #region dbPrachiIndia_PortalEntities
                case RepositoryType.dbPrachiIndia_PortalEntities:
                    return new dbPrachiIndia_PortalEntities();
                #endregion

                #region GSTTaxListRepository
                case RepositoryType.GSTTaxListRepository:
                    return new GSTTaxListRepository();
                #endregion

                #region TopicRepository
                case RepositoryType.TopicRepository:
                    return new TopicRepository();
                #endregion

                #region ChapterRepository
                case RepositoryType.ChapterRepository:
                    return new ChapterRepository();
                    #endregion

                    
                default:
                    return null;
            }
        }
    }
}