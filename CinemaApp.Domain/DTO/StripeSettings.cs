using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DTO
{
    public class StripeSettings
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
    }
}
