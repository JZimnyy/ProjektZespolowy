using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjektZespolowy.Models.Passengers;

namespace ProjektZespolowy.Models
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            Passengers();
        }

        protected void Passengers()
        {
            CreateMap<Passenger, PassengerViewModel>();

            CreateMap<PassengerViewModel, Passenger>();

            CreateMap<Passenger, PassengerFormModel>();

        }


    }
}