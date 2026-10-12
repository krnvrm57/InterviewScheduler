namespace InterviewScheduler.Filters
{
    internal class JsonResponse
    {
        public string status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public JsonResponse()
        {
        }

        public JsonResponse(string _status)
            : this(_status, null)
        {
        }

        public JsonResponse(string _status, string message)
            : this(_status, message, null)
        {
        }

        public JsonResponse(string _status, string message, object data)
        {
            this.status = _status;
            this.Message = message;
            this.Data = data;
        }
    }
}