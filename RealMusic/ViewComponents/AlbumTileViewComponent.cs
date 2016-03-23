using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

namespace RealMusic.ViewComponents
{
    public class AlbumTileViewComponent : BaseViewComponent
    {
        public AlbumTileViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke(AlbumInfoDto albumInfoDto)
        {
            return View(albumInfoDto);
        }

        public async Task<IViewComponentResult> InvokeAsync(AlbumInfoDto albumInfoDto)
        {
            return View(albumInfoDto);
        }
    }
}
