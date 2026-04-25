using Microsoft.AspNetCore.Mvc;

namespace практы_курсак.Controllers
{
    public class ThemeController : Controller
    {
        public IActionResult Switch(string theme, string returnUrl = "/")
        {
            if (theme == "dark" || theme == "light")
            {
                Response.Cookies.Append("Theme", theme, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1),
                    IsEssential = true
                });
            }
            return LocalRedirect(returnUrl);
        }
    }
}