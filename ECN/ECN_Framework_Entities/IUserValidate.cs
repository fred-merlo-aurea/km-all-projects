namespace ECN_Framework_Entities
{
    public interface IUserValidate
    {
        bool HasValidID { get; }
        int? CreatedUserID { get; }
        int? UpdatedUserID { get; }
    }
}
