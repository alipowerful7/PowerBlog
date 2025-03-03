using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PowerBlog.Site.Attributes
{
    public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // دریافت Session از HttpContext
            var session = context.HttpContext.Session;
            // دریافت نقش کاربر از Session (فرض کنید نقش به عنوان رشته ذخیره شده است)
            var userRole = session.GetString("UserRole");

            // اگر نقش کاربر موجود نباشد یا برابر با "Admin" نباشد، دسترسی رد می‌شود.
            if (string.IsNullOrEmpty(userRole) || !userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // هدایت به صفحه اصلی یا صفحه دیگری به عنوان دسترسی غیرمجاز
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
            }
        }
    }
}
