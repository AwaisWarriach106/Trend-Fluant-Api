namespace TrendFlaunt.Data.Queries;

internal class AuthenticationQueries
{
    public const string ManualUserRegistration = @"INSERT INTO public.tbl_user_profiles
                                                   (user_id, full_name, phone_number, gender, user_type) 
                                                VALUES (@UserId, @FullName, @PhoneNumber, @Gender, @UserType::user_type_enum) RETURNING id;";

}
