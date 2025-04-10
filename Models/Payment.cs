namespace Railway.Models
{
    public class Payment
    {
        int PaymentID {get; set;}
        int TicketID {get; set;}
        decimal Amount {get; set;}
        DateTime PaymentDate {get; set;}
        string PaymentMode {get; set;}
        string Status {get; set;}
        bool IncludeInsurance {get; set;}
        decimal InsuranceAmount {get; set;}
    }
}