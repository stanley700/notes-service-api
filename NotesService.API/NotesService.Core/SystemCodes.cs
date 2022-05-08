using System;

namespace NotesService.Core
{
    public class SystemCodes
    {
        public const string Successful = "00";
        public const string InvalidRequest = "101";
        public const string DataNotFound = "404";
        public const string UsernameAlreadyExist = "02";
        public const string UsernameAndPasswordAreRequired = "03";
        public const string DataSecurityViolation = "04";
        public const string NoteAlreadyExist = "05";

    }
}
