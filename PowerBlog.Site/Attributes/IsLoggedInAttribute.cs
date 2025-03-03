using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PowerBlog.Site.Attributes
{
    public class IsLoggedInAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // دریافت Session از HttpContext
            var session = context.HttpContext.Session;
            // دریافت نقش کاربر از Session (فرض کنید نقش به عنوان رشته ذخیره شده است)
            var isLoggedIn = session.GetString("IsLoggedIn");

            // اگر نقش کاربر موجود نباشد یا برابر با "Admin" نباشد، دسترسی رد می‌شود.
            if (string.IsNullOrEmpty(isLoggedIn) || !isLoggedIn.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                // هدایت به صفحه اصلی یا صفحه دیگری به عنوان دسترسی غیرمجاز
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
            }
        }
    }
}
