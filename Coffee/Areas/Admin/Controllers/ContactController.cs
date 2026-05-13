using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Contact;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FastFood.Filters;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class ContactController : Controller
    {
        private readonly HttpClient _httpClient;

        public ContactController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/contact");
                if (response.IsSuccessStatusCode)
                {
                    var contacts = await response.Content.ReadFromJsonAsync<List<ContactModel>>();
                    return View(contacts ?? new List<ContactModel>());
                }
            }
            catch { }

            return View(new List<ContactModel>());
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/contact/{id}/read", null);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
            }
            catch { }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/contact/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch { }

            return BadRequest();
        }
    }
}
