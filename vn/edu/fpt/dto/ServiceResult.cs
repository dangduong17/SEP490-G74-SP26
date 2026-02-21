namespace vn.edu.fpt.dto
{
    public class ServiceResult
    {
        public bool Succeeded { get; set; }
        public bool NotFound { get; set; }
        public List<ServiceError> Errors { get; set; } = new();

        public static ServiceResult Success()
        {
            return new ServiceResult { Succeeded = true };
        }

        public static ServiceResult Failed(params ServiceError[] errors)
        {
            return new ServiceResult
            {
                Succeeded = false,
                Errors = errors.ToList()
            };
        }

        public static ServiceResult NotFoundResult(string message = "Không tìm thấy dữ liệu.")
        {
            return new ServiceResult
            {
                Succeeded = false,
                NotFound = true,
                Errors = new List<ServiceError> { new ServiceError { Message = message } }
            };
        }
    }

    public class ServiceError
    {
        public string? Key { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
