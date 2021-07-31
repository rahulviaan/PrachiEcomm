using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Models.Interfaces
{
     public interface ISubscriptionOrderPaymentRepository
    {
        UserSubscription Create(UserSubscription entit, string username);
        IQueryable<SubscriptionPayment> SearchFor(Expression<Func<SubscriptionPayment, bool>> predicate);
        SubscriptionPayment Edit(SubscriptionPayment entity);
        IQueryable<SubscriptionMaster> GetAll();
        SubscriptionMaster FindSubscriptionTypeById(int id);
        string GenerateSubTxnId(string SubOrderIdPrefix);
        SubscriptionPayment CreateSubscriptionPayment(SubscriptionPayment entity);
        AspNetUsersModel GetUserByEmail(string username);
        AspNetUsers AddUsers(AspNetUsers aspNetUsers,string transactionid, int SubscriptionId);
        ReadEdgeSubscriptionPayment GetSubscriptionPaymentByTransId(string TransID);
    }
}
