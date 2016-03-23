using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

namespace RealMusic.ViewComponents
{
    public class AlbumSearchListViewComponent : BaseViewComponent
    {
        public AlbumSearchListViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke(SearchResultDto searchResultDto)
        {
            return View(searchResultDto);
        }

        public async Task<IViewComponentResult> InvokeAsync(SearchResultDto searchResultDto)
        {
            return View(searchResultDto);
        }
    }
}
