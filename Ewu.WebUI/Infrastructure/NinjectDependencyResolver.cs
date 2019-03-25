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
using System.Configuration;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Infrastructure.Concrete;

namespace Ewu.WebUI.Infrastructure
{
    /// <summary>
    /// 一个自定义的依赖项解析器
    /// 保证MVC框架在任何时候都能使用Ninject创建对象
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        //创建一个Ninject内核(Kernel)实例
        private IKernel kernel;


        /// <summary>
        /// 构造函数传递Kernel
        /// </summary>
        /// <param name="kernelParam"></param>
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        /// <summary>
        /// MVC框架需要类实例
        /// 以便对传入的请求进行服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            //返回绑定，没有这返回NULL
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            //对单一类型的多个绑定
            return kernel.GetAll(serviceType);
        }

        //这里存放绑定
        private void AddBindings()
        {
            //绑定Treasure存储库的接口与上下文类
            kernel.Bind<ITreasuresRepository>().To<EFTreasureRepository>();

            //绑定订单接口和邮箱订单
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("setting", new EmailSettings { });

            //绑定验证接口，注册FormsAuthProvider
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}