using Microsoft.AspNet.Mvc;
using RealMusic.Db;

namespace RealMusic.ViewComponents
{
    public class BaseViewComponent : ViewComponent
    {
        protected readonly IMapStore Data;
        public BaseViewComponent()
        {
            
        }

        public BaseViewComponent(IMapStore data)
        {
            Data = data;
        }
    }
}
