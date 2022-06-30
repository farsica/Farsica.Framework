namespace Farsica.Framework.Powershell.Pages
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class LoginModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ILogger logger;
        private readonly IStringLocalizer<LoginModel> localizer;

        public LoginModel(ILogger<LoginModel> logger, IStringLocalizer<LoginModel> localizer)
        {
            this.logger = logger;
            this.localizer = localizer;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //    if (!string.IsNullOrEmpty(returnUrl))
            //        return LocalRedirect(returnUrl);

            // return RedirectToAreaPage("Index", nameof(Areas.Admin));
            // }
            // returnUrl = returnUrl ?? Url.Content("~/");

            //// Clear the existing external cookie to ensure a clean login process
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // returnUrl = returnUrl ?? Url.Content("~/");
            // if (ModelState.IsValid)
            // {
            //    var IsUserExist = await userService.IsUsernameExistAsync(Input.UserName);
            //    if (!IsUserExist)
            //    {
            //        if (!Input.UserName.IsMobileNumber())
            //        {
            //            ModelState.AddModelError("ErrorMessage", Login.UserNotExist);
            //            return Page();
            //        }
            //        // normal login for users
            //        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, rememberMe = Input.RememberMe });
            //    }
            //    else
            //    {
            //        return RedirectToPage("./PasswordSignIn", new { userName = Input.UserName, ReturnUrl = returnUrl });
            //    }
            // }

            // ModelState.AddModelError("ErrorMessage", Login.UserLoginError);
            return Page();
        }
    }
}
