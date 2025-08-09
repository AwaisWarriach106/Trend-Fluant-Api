namespace TrendFlaunt.Data.Queries;

internal class AuthenticationQueries
{
    public const string ManualUserRegistration = @"INSERT INTO public.tbl_user_profiles
                                                   (user_id, full_name, phone_number, gender) 
                                                VALUES (@UserId, @FullName, @PhoneNumber, @Gender) RETURNING id;";
    public const string GetProfileByEmail = @"SELECT 
                                            u.""Email"" AS Email,
                                            p.full_name AS Name,
                                            p.id as ProfileId,
                                            p.gender as Gender
                                        FROM 
                                            ""AspNetUsers"" u
                                        JOIN 
                                            tbl_user_profiles p ON u.""Id"" = p.user_id
                                        WHERE 
                                            u.""Email"" = @Email;";

}
