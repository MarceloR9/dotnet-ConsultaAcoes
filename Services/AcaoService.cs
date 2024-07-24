using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ConsultaAcoes.Models;

namespace ConsultaAcoes.Services
{
    public class AcaoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AcaoService> _logger;
        private readonly string _token = "g9R3r58G3zMT7QEhEPZs5D";

        public AcaoService(HttpClient httpClient, ILogger<AcaoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Acao>> ObterAcoesAsync(string search = "", string sortBy = "", string sortOrder = "asc", int limit = 10, int page = 1, string type = "", string sector = "")
        {
            var url = $"https://brapi.dev/api/quote/list?token={_token}&search={search}&sortBy={sortBy}&sortOrder={sortOrder}&limit={limit}&page={page}&type={type}&sector={sector}";
            _logger.LogInformation("Fetching stock data from URL: {Url}", url);

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return data?.Stocks ?? new List<Acao>();
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch stock data. Status code: {StatusCode}, Content: {Content}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Failed to fetch stock data. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching stock data from URL: {Url}", url);
                throw;
            }
        }

        public class ApiResponse
        {
            public IEnumerable<Acao> Stocks { get; set; }
        }

        public async Task<IEnumerable<Acao>> ObterTop10MaisValorizadasAsync()
        {
            var acoes = await ObterAcoesAsync();
            return acoes.OrderByDescending(a => a.Change).Take(10);
        }

        public async Task<IEnumerable<Acao>> ObterTop10MaisDesvalorizadasAsync()
        {
            var acoes = await ObterAcoesAsync();
            return acoes.OrderBy(a => a.Change).Take(10);
        }
    }
}
