using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;
using Ewu.Domain.Abstract;
using Ewu.Domain.Concrete;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        //这里存放绑定
        private void AddBindings()
        {
            kernel.Bind<ITreasuresRepository>().To<EFTreasureRepository>();
            #region 模仿库
            //Mock<ITreasuresRepository> mock = new Mock<ITreasuresRepository>();
            //mock.Setup(m => m.Treasures).Returns(new List<Treasure>
            //{
            //    new Treasure{TreasureName="蓝牙鼠标",DamageDegree="九成新",TradeRange="省内" },
            //    new Treasure{TreasureName="考研教材",DamageDegree="八成新",TradeRange="市内" },
            //    new Treasure{TreasureName="Xbox游戏机",DamageDegree="七成新",TradeRange="不限" },
            //});
            //kernel.Bind<ITreasuresRepository>().ToConstant(mock.Object);
            #endregion
        }
    }
}