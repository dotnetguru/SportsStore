﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Concrete;


namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory() 
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {Name = "Football", Category="Soccer", Price = 25},
                new Product {Name = "Surf board", Category="Watersports",  Price = 179},
                new Product {Name = "Running shoes", Category="Soccer", Price = 95},
                new Product {Name = "Chess Set", Category="Chess", Price = 20},
            }.AsQueryable());

            //original mock object binding
            ninjectKernel.Bind<IProductRepository>().ToConstant(mock.Object);

            // Entity Framework binding
            //ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
        }

    }
}