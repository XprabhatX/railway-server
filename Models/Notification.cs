namespace Railway.Models
{
    public class Notification
    {
        int NotificationId {get; set;}
        string Username {get; set;}
        int TrainID {get; set;}
        DateTime NotifiedOn {get; set;}
        string Type {get; set;}
        string Message {get; set;}
    }
}