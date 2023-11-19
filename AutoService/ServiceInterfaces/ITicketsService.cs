using AutoService.Models;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface ITicketsService
    {
        Task<List<TicketInfoDTO>> GetTickets();
        Task<TicketInfoDTO> GetTicket(int id);
        Task<ServiceResponce> BookTicket(TicketDTO model);
        Task<ServiceResponce> BuyTicket(TicketDTO model);
        Task<ServiceResponce> CancelBooking(int ticketId);
        Task<ServiceResponce> IssueTicket(TicketDTO model);
        Task<ServiceResponce> PayForTicket(int id);
    }
}
