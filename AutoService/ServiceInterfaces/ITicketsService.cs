using AutoService.Models;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface ITicketsService
    {
        Task<IEnumerable<Ticket>> GetTickets(int clientId);
        Task<Ticket> GetTicket(int id);
        Task<ServiceResponce> BookTicket(TicketViewModel model);
        Task<ServiceResponce> BuyTicket(TicketViewModel model);
        Task<ServiceResponce> CancelBooking(int ticketId);
        Task<Ticket> IssueTicket(TicketViewModel model);
        Task<ServiceResponce> PayForTicket(int id);
    }
}
