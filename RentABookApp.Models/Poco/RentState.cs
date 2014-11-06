namespace RentABookApp.Models.Poco
{
    public enum RentState
    {
        Started = 0,
        BookReceived = 1,
        ReceivedNotActive = 2,
        OwnerNotActive = 3,
        BookNotReturned = 4,
        Completed = 5
    }
}
