using System.ComponentModel.DataAnnotations;
namespace RentABook.Models.Poco
{
    public enum BookState
    {
        [Display(Name = "Waiting for approval")]
        WaitingForApproval = 0,

        [Display(Name = "Book is available")]
        Available = 1,

        [Display(Name = "Book is rented")]
        InRent = 2,

        [Display(Name = "Book is not returned")]
        NotReturned = 3,

        [Display(Name = "Book is archived")]
        Archived = 4
    }
}
