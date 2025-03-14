using AutoMapper;
using PayphoneWallet.Domain.DTO;
using PayphoneWallet.Domain.Entities;

namespace PayphoneWallet.Application.Profiles;

class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Wallet, WalletDto>().ReverseMap();
        CreateMap<Transaction, TransactionDto>().ReverseMap();
    }
}
