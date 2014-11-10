using System.Web.Mvc;

namespace RentABook.Web.Areas.Users
{
    public class UsersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Users";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Users",
                url: "Users/{username}",
                defaults: new { controller = "Users", action = "Index", username = UrlParameter.Optional }
            );

            context.MapRoute(
                "Users_Profile",
                "Users/{controller}/{action}/{id}",
                new { controller = "Profile", action = "Settings", id = UrlParameter.Optional }
            );
        }
    }
}