using Microsoft.AspNetCore.Mvc;
using ConsultaAcoes.Services;
using ConsultaAcoes.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultaAcoes.Controllers
{
    public class AcaoController : Controller
    {
        private readonly AcaoService _acaoService;

        public AcaoController(AcaoService acaoService)
        {
            _acaoService = acaoService;
        }

        public async Task<IActionResult> Index(string search = "", string sortBy = "", string sortOrder = "asc", int limit = 10, int page = 1, string type = "", string sector = "")
        {
            try
            {
                var acoes = await _acaoService.ObterAcoesAsync(search, sortBy, sortOrder, limit, page, type, sector);
                var topValorizadas = acoes.OrderByDescending(a => a.Change).Take(10);
                var topDesvalorizadas = acoes.OrderBy(a => a.Change).Take(10);

                var viewModel = new AcaoViewModel
                {
                    Acoes = acoes,
                    TopValorizadas = topValorizadas,
                    TopDesvalorizadas = topDesvalorizadas,
                    Search = search
                };

                return View(viewModel);
            }
            catch (HttpRequestException ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> Details(string stock)
        {
            try
            {
                var resultado = await _acaoService.ObterAcoesAsync(stock);
                return View(resultado);
            }
            catch (HttpRequestException ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = ex.Message });
            }
        }
    }
}
