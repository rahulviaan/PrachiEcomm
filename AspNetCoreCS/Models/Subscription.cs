using DAL.Models.Entities;
using DAL.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GleamTech.DocumentUltimateExamples.AspNetCoreCS.Models
{
    public class Subscription
    {
        private ISubscriptionOrderPaymentRepository _ISubscriptionOrderPaymentRepository { get; set; }
        private readonly IConfiguration _iConfig;
        public Subscription(ISubscriptionOrderPaymentRepository subscriptionOrderPaymentRepository, IConfiguration iConfig)
        {
            _ISubscriptionOrderPaymentRepository = subscriptionOrderPaymentRepository;
            _iConfig = iConfig;
        }

        public UserSubscription Create(UserSubscription userSubscription,string username)
        {
            return _ISubscriptionOrderPaymentRepository.Create(userSubscription, username);
        }
        public IQueryable<SubscriptionPayment> Search(Expression<Func<SubscriptionPayment, bool>> predicate)
        {
            return _ISubscriptionOrderPaymentRepository.SearchFor(predicate);
        }

        public SubscriptionPayment Edit(SubscriptionPayment entity)
        {
            _ISubscriptionOrderPaymentRepository.Edit(entity);
            return entity;
        }

        public IQueryable<SubscriptionMaster> GetAll()
        {
            return _ISubscriptionOrderPaymentRepository.GetAll();
        }
        public SubscriptionMaster FindSubscriptionTypeById(int id)
        {
            return _ISubscriptionOrderPaymentRepository.FindSubscriptionTypeById(id);
        }

        public string GenerateSubTxnId()
        {
            var SubOrderIdPrefix = Convert.ToString(_iConfig.GetValue<string>("AppSettings:SubOrderIdPrefix"));
            return _ISubscriptionOrderPaymentRepository.GenerateSubTxnId(SubOrderIdPrefix);
        }

        public SubscriptionPayment CreateSubscriptionPayment(SubscriptionPayment entity)
        {
            return _ISubscriptionOrderPaymentRepository.CreateSubscriptionPayment(entity);

        }
         public AspNetUsersModel GetUserByEmail(string Email)
        {
            return _ISubscriptionOrderPaymentRepository.GetUserByEmail(Email);
        }
        public AspNetUsers AddUser(AspNetUsers aspNetUsers,string transactionid,int subscriptionid)
        {
            return _ISubscriptionOrderPaymentRepository.AddUsers(aspNetUsers, transactionid, subscriptionid);
        }

        public ReadEdgeSubscriptionPayment GetSubscriptionPaymentByTransId(string TransID)
        {
            return _ISubscriptionOrderPaymentRepository.GetSubscriptionPaymentByTransId(TransID);
        }
    }
}