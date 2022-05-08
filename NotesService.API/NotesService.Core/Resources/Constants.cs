using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.Core.Resources
{
    public class Constants
    {
        public const string Role_Admin = "administrator";
        public const string Role_User = "user";

        public const string TokenProviderBearerKeyPlaceHolder = "TokenProvider:BearerKey";
        public const string TokenProviderIssuerPlaceHolder = "TokenProvider:Issuer";
        public const string TokenProviderAudiencePlaceHolder = "TokenProvider:Audience";
        public const string TokenProviderPathPlaceHolder = "TokenProvider:Path";
    }
}
