namespace TaskTracker.Middlewares
{
    public class BadResponseObject
    {
        public string Message { get; set; }
        public object ResponseObject { get; set; }

        public BadResponseObject(string message, object responseObject = null)
        {
            Message = message;
            ResponseObject = responseObject;
        }

        public BadResponseObject()
        {

        }
    }
}
