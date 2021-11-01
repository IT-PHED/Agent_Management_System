using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fedco.PHED.Agent.management.DAL;
using Fedco.PHED.Agent.management.Models;
using AutoMapper;

namespace FedcoWalletMVCApp.Repository
{
    /// <summary>
    /// Mapper class to convert objects database to mvc model and vice-versa
    /// </summary>
    /// <typeparam name="Source"></typeparam>
    /// <typeparam name="Destination"></typeparam>
    public class FedcoWalletMapper<Source, Destination>
        where Source: class
        where Destination: class
    {
        //The mapper class
        public FedcoWalletMapper()
        {
            Mapper.CreateMap<UserType, Fedco.PHED.Agent.management.Web.Models.UserType>();
            Mapper.CreateMap<Fedco.PHED.Agent.management.Web.Models.UserType, UserType>();

            Mapper.CreateMap<AgentWallet, Fedco.PHED.Agent.management.Web.Models.AgentWallet>();
            Mapper.CreateMap<Fedco.PHED.Agent.management.Web.Models.AgentWallet, AgentWallet>();

            //User -> Models.User
            Mapper.CreateMap<User, Fedco.PHED.Agent.management.Web.Models.User>();
            //Models.User-> User
            Mapper.CreateMap<Fedco.PHED.Agent.management.Web.Models.User, User>();

            Mapper.CreateMap<ChangePassword, Fedco.PHED.Agent.management.Web.Models.ChangePasswordModel>();
            Mapper.CreateMap<Fedco.PHED.Agent.management.Web.Models.ChangePasswordModel, ChangePassword>();
            
            Mapper.CreateMap<Fedco.PHED.Agent.management.Web.Models.AgentModel, Agent>();
            Mapper.CreateMap<Agent, Fedco.PHED.Agent.management.Web.Models.AgentModel>();
            Mapper.CreateMap<Fedco.PHED.Agent.management.Web.Models.UserTransaction, UserTransaction>();
            Mapper.CreateMap<UserTransaction, Fedco.PHED.Agent.management.Web.Models.UserTransaction>();
        }
        /// <summary>
        /// Used to translate between entity and model, and vice-versa
        /// </summary>
        /// <param name="obj">Object to be translated</param>
        /// <returns></returns>
        public Destination Translate(Source obj)
        {
            return Mapper.Map<Source, Destination>(obj);
        }
    }
}