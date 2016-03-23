using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

namespace RealMusic.ViewComponents
{
    public class MyCartViewComponent : BaseViewComponent
    {
        public MyCartViewComponent(IMapStore data) : base(data)
        {
        }

        public IViewComponentResult Invoke(OrderDto orderDto = null)
        {
            return View(orderDto);
        }

        public async Task<IViewComponentResult> InvokeAsync(OrderDto orderDto = null)
        {
            return View(orderDto);
        }
    }
}
