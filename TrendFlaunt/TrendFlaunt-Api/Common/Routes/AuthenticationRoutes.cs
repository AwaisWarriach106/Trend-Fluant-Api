namespace TrendFlaunt_Api.Common.Routes;

public class AuthenticationRoutes
{
    public static string MapGroupAuthentication = $"{ApiPattern.Base}/authentication/";

    public static string Login = "login";
    public static string LoginWithGoogle = "login-with-google";
    public static string RegisterUser = "register-user";
}
