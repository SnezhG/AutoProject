using AutoService.Models;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface ITicketsService
    {
        Task<IEnumerable<Ticket>> GetTickets();
        Task<Ticket> GetTicket(int id);
        Task<ServiceResponce> BookTicket(TicketDTO model);
        Task<int> BuyTicket(TicketDTO model);
        Task<ServiceResponce> CancelBooking(int ticketId);
        Task<int> IssueTicket(TicketDTO model);
        Task<ServiceResponce> PayForTicket(int id);
    }
}
