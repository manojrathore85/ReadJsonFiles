namespace ReadJsonFiles.Model
{
    public class ReadExceptionJsonFile
    {
       public List<ReadException> exceptions { get; set; }
    }
    public class ReadException
    {
        public string LEI { get; set; }
        public string ExceptionCategory { get; set; }
        public List<ExceptionReason> ExceptionReason { get; set; }
    }
    public class ExceptionReason
    {
        public string ExceptionReasons { get; set; }
    }
}
