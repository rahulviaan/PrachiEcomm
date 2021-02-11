using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PrachiIndia.Sql.CustomRepositories;
namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ReadedgeController : Controller
    {

        public ContentResult GetAllSubject()
        {

             var lst = new List<Sql.MasterSubject>();
            if (Request.IsAjaxRequest())
            {
                //var MasterSubjectRepository = new MasterSubjectRepository();
                //var lstSubject = MasterSubjectRepository.GetAll();
                var lstMasterSubject = new Readedge_BusLogic.blSubject().GetAll();
                if (lstMasterSubject != null)
                {
                    try
                    {
                        var result = from c in lstMasterSubject
                                     orderby c.OredrNo ascending
                                     select new
                                     {                                         
                                         Title = c.Title,
                                         idServer = c.IdServer
                                     };
                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                        return new ContentResult()
                        {
                            Content = serializer.Serialize(result),
                            ContentType = "application/json",
                        };
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return null;
        }

        public ContentResult GetAllSeries()
        {

            var lst = new List<Sql.MasterSubject>();
            if (Request.IsAjaxRequest())
            {
                //var MasterSubjectRepository = new MasterSubjectRepository();
                //var lstSubject = MasterSubjectRepository.GetAll();
                var lstMasterSeries = new Readedge_BusLogic.blSeries().GetAll();
                if (lstMasterSeries != null)
                {
                    try
                    {
                        var result = from c in lstMasterSeries
                                     orderby c.OredrNo ascending
                                     select new
                                     {
                                         Title = c.Title,
                                        // idServer = c.IdServer
                                     };
                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                        return new ContentResult()
                        {
                            Content = serializer.Serialize(result),
                            ContentType = "application/json",
                        };
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return null;
        }


        
         public ContentResult GetAllClass()
        {

            var lst = new List<Sql.MasterClass>();
            if (Request.IsAjaxRequest())
            {
                //var MasterSubjectRepository = new MasterSubjectRepository();
                //var lstSubject = MasterSubjectRepository.GetAll();
                var lstMasterClass = new Readedge_BusLogic.blClass().GetAll();
                if (lstMasterClass != null)
                {
                    try
                    {
                        var result = from c in lstMasterClass
                                     orderby c.OredrNo ascending
                                     select new
                                     {
                                         Title = c.Title,
                                         idServer = c.IdServer
                                     };
                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                        return new ContentResult()
                        {
                            Content = serializer.Serialize(result),
                            ContentType = "application/json",
                        };
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return null;
        }

        public ContentResult GetAllBoard()
        {

            var lst = new List<Sql.MasterBoard>();
            if (Request.IsAjaxRequest())
            {
                //var MasterSubjectRepository = new MasterSubjectRepository();
                //var lstSubject = MasterSubjectRepository.GetAll();
                var lstMasterClass = new Readedge_BusLogic.blBoard().GetAll();
                if (lstMasterClass != null)
                {
                    try
                    {
                        var result = from c in lstMasterClass
                                     orderby c.OredrNo ascending
                                     select new
                                     {
                                         Title = c.Title,
                                         idServer = c.IdServer
                                     };
                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                        return new ContentResult()
                        {
                            Content = serializer.Serialize(result),
                            ContentType = "application/json",
                        };
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return null;
        }
    }
}
