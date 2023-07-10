using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Interface
{
    public interface IEmailService
    {
        public Task SendEmailAsync(EmailMessage mail);
    }
}
