using DAL.Models.Entities;
using DAL.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Models.Repository
{
    public class SubscriptionOrderPaymentRepository : ISubscriptionOrderPaymentRepository
    {

        private readonly UserContext context;
        public SubscriptionOrderPaymentRepository(UserContext userContext)
        {
            context = userContext;
        }
        UserSubscription ISubscriptionOrderPaymentRepository.Create(UserSubscription entity,string username)
        {
            //context.Add(entity);
            //context.SaveChanges();
            context.Database.ExecuteSqlCommand("ReadEdge_UserSubscription_Create @p0,@p1,@p2,@p3,@p4,@p5", entity.UserId, entity.TypeId, entity.StartDate, entity.EndDate, entity.TransactionId,username);
            return entity;
        }



        IQueryable<SubscriptionPayment> ISubscriptionOrderPaymentRepository.SearchFor(Expression<Func<SubscriptionPayment, bool>> predicate)
        {
            return context.SubscriptionPayment.Where(predicate);
        }
        ReadEdgeSubscriptionPayment ISubscriptionOrderPaymentRepository.GetSubscriptionPaymentByTransId(string TransID)
        {
            return context.ReadEdgeSubscriptionPayment.FromSql("ReadEdge_UserSubscription @p0", TransID).ToList().FirstOrDefault();
        }

        SubscriptionPayment ISubscriptionOrderPaymentRepository.Edit(SubscriptionPayment entity)
        {

            //context.Entry(entity).State = EntityState.Modified;
            //context.SaveChanges();

            context.Database.ExecuteSqlCommand("ReadEdge_Subscription_Edit @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", entity.AddtionalCharge, entity.Error, entity.PayUMoneyId, entity.PGType, entity.ResponseHas, entity.ResponseLog, entity.TransactionId, entity.Status, entity.UpdatedDate, entity.UserId);

            return entity;
        }

        public IQueryable<SubscriptionMaster> GetAll()
        {
            return context.SubscriptionMaster;
        }
        public SubscriptionMaster FindSubscriptionTypeById(int id)
        {
            return context.SubscriptionMaster.Where(x=> x.Id==id).FirstOrDefault();
        }
        public string GenerateSubTxnId(string SubOrderIdPrefix)
        {
            //string SubOrderIdPrefix = _iConfig.GetValue<string>("AppSettings:SubOrderIdPrefix");

            //lock (context.Orders)
            //{
                string orderid = Convert.ToString(context.SubscriptionPayment.Max(x => x.Id) + 1);
                string TransactionID = SubOrderIdPrefix + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + orderid;
                return TransactionID;
            //}
        }

         SubscriptionPayment ISubscriptionOrderPaymentRepository.CreateSubscriptionPayment(SubscriptionPayment entity)
        {
            //context.Add(entity);
            //context.SaveChanges();
            //context.Database.ExecuteSqlCommand("ReadEdge_Subscription @Amount,@DiscountPer,@TotalAmount,@Email,@Name,@Phone,@SubscriptionId,@TransactionId,@RequestLog,@RequestHash,@Status,@CreatedDate,@UpdatedDate,@UserId", entity.Amount, entity.DiscountPer, entity.TotalAmount, entity.Email, entity.Name, entity.Phone, entity.SubscriptionId, entity.TransactionId,entity.RequestLog, entity.ResponseHas, entity.Status, entity.CreatedDate, entity.UpdatedDate, entity.UserId);
            context.Database.ExecuteSqlCommand("ReadEdge_Subscription @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13", entity.Amount, entity.DiscountPer, entity.TotalAmount, entity.Email, entity.Name, entity.Phone, entity.SubscriptionId, entity.TransactionId,entity.RequestLog, entity.RequestHash, entity.Status, entity.CreatedDate, entity.UpdatedDate, entity.UserId);
            return entity;
        }

        public AspNetUsersModel GetUserByEmail(string username)
        {
            return context.AspNetUsersModel.FromSql("ReadEdge_AspNetUsers @p0", username).ToList().FirstOrDefault();
        }
        public AspNetUsers AddUsers(AspNetUsers entity,string transactionid,int SubscriptionId)
        {
            context.Database.ExecuteSqlCommand("ReadEdge_AspNetUsers_Create @p0,@p1,@p2,@p3,@p4,@p5,@p6", entity.FirstName, entity.Email, entity.PasswordHash, entity.ProfileImage, entity.PhoneNumber,transactionid,SubscriptionId);
            return entity;
        }

          

    }
}
