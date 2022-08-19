namespace LeaveManagement.Web.Data
{
    //Partial means it will not be able to be instantiated on its own. This class will have things that models can inherit
    public partial class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
