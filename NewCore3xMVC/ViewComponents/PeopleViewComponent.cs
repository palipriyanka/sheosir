using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewCore3xMVC.Data;
using System.Threading.Tasks;

namespace NewCore3xMVC.ViewComponents
{
    public class PeopleViewComponent : ViewComponent
    {
        private readonly NewCore3xMVCContext _context;

        public PeopleViewComponent(NewCore3xMVCContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _context.Person.ToListAsync();
            return View(data);
        }

    }
}
