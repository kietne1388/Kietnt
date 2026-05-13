using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Auth;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                    if (result != null && result.ContainsKey("userId"))
                    {
                        HttpContext.Session.SetInt32("UserId", Convert.ToInt32(result["userId"].ToString()));
                        HttpContext.Session.SetString("UserRole", result["role"]?.ToString() ?? "Customer");
                        HttpContext.Session.SetString("UserName", result["userName"]?.ToString() ?? "");
                        HttpContext.Session.SetString("UserEmail", result.ContainsKey("email") ? result["email"]?.ToString() ?? "" : "");

                        var role = result["role"]?.ToString();
                        var emailStr = result.ContainsKey("email") ? result["email"]?.ToString() ?? "" : "";

                        // Test Hook for Demonstration Redirects
                        if (model.Username.ToLower() == "staff" || emailStr == "an@cafelux.vn")
                        {
                            role = "Staff";
                            HttpContext.Session.SetString("UserRole", "Staff");
                        }

                        if (role == "Admin")
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        else if (role == "Staff")
                            return RedirectToAction("Index", "POS", new { area = "Staff" });
                        else
                            return RedirectToAction("Index", "Home", new { area = "Guest" });
                    }
                }
                else
                {
                    // Read error from API
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try 
                    {
                        // Try to parse JSON error message
                        var errorJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                        if(errorJson != null && errorJson.ContainsKey("message"))
                        {
                            ModelState.AddModelError("", "Lỗi từ hệ thống: " + errorJson["message"]);
                        }
                        else 
                        {
                             ModelState.AddModelError("", "Đăng nhập thất bại: " + response.StatusCode);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Lỗi hệ thống: " + response.StatusCode + " " + errorContent);
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi kết nối: " + ex.Message);
            }

            ModelState.AddModelError("", "Login failed. Please check your credentials.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                    
                    // Auto-login after successful registration
                    var loginData = new { Username = model.Username, Password = model.Password };
                    var loginJson = JsonSerializer.Serialize(loginData);
                    var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
                    var loginResponse = await _httpClient.PostAsync("api/Auth/login", loginContent);

                    if (loginResponse.IsSuccessStatusCode)
                    {
                        var loginResult = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                        if (loginResult != null && loginResult.ContainsKey("userId"))
                        {
                            HttpContext.Session.SetInt32("UserId", Convert.ToInt32(loginResult["userId"].ToString()));
                            HttpContext.Session.SetString("UserRole", loginResult["role"]?.ToString() ?? "Customer");
                            HttpContext.Session.SetString("UserName", loginResult["userName"]?.ToString() ?? model.FullName);
                            HttpContext.Session.SetString("UserEmail", loginResult.ContainsKey("email") ? loginResult["email"]?.ToString() ?? model.Email : model.Email);

                            TempData["Success"] = "Đăng ký thành công! Chào mừng bạn đến với FastFood!";
                            TempData["ShowWelcomeGift"] = true;
                            return RedirectToAction("Index", "Home", new { area = "Guest" });
                        }
                    }
                    
                    // Fallback to login page if auto-login fails
                    TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try 
                    {
                        var errorJson = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                        if(errorJson != null && errorJson.ContainsKey("message"))
                        {
                            ModelState.AddModelError("", "Lỗi từ hệ thống: " + errorJson["message"]);
                        }
                        else 
                        {
                            ModelState.AddModelError("", "Đăng ký thất bại: " + response.StatusCode);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Lỗi hệ thống: " + response.StatusCode + " " + errorContent);
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Lỗi kết nối: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $" | Inner: {ex.InnerException.Message}";
                }
                ModelState.AddModelError("", errorMsg);
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "Guest" });
        }
    }
}
