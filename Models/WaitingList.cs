namespace Railway.Models
{
    public class WaitingList
    {
        int WaitingListID {get; set;}
        int TicketID {get; set;}
        int TrainID {get; set;}
        int ClassTypeId {get; set;}
        DateTime RequestDate {get; set;}
        int Position {get; set;}
    }
}