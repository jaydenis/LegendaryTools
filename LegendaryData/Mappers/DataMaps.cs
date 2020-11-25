using AutoMapper;
using LegendaryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryData.Mappers
{
    public class DataMap
    {
        public static IMapper CreateMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();

            });

            return config.CreateMapper();
        }

        public static AuthorViewModel GetMapping(Stat_Author source, AuthorViewModel destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                opt.BeforeMap((src, dest) => src.AuthorName = dest.Name);
            });

            return dto;
        }

      
        public static Stat_Author AddMapping(AuthorViewModel source, Stat_Author destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
                opt.BeforeMap((src, dest) => src.Name = dest.AuthorName);

            });

            return dto;
        }

        public static Stat_Author UpdateMapping(AuthorViewModel source, Stat_Author destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                opt.BeforeMap((src, dest) => src.Name = dest.AuthorName);
            });

            return dto;
        }

        public static BaseCardViewModel GetMapping(Heroes source, BaseCardViewModel destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                opt.BeforeMap((src, dest) => src.HeroName = dest.Name);
            });

            return dto;
        }


        public static Heroes AddMapping(BaseCardViewModel source, Heroes destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
                opt.BeforeMap((src, dest) => src.Name = dest.HeroName);
            });

            return dto;
        }

        public static Heroes UpdateMapping(BaseCardViewModel source, Heroes destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                opt.BeforeMap((src, dest) => src.Name = dest.HeroName);
            });

            return dto;
        }

        public static Stat_Affiliation AddMapping(Stat_Affiliation source, Stat_Affiliation destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
            });

            return dto;
        }

        public static Stat_Affiliation UpdateMapping(Stat_Affiliation source, Stat_Affiliation destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                //opt.BeforeMap((src, dest) => src.DATE_UPDATED = DateTime.Now);
            });

            return dto;
        }

        public static Masterminds AddMapping(Masterminds source, Masterminds destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
            });

            return dto;
        }

        public static Masterminds UpdateMapping(Masterminds source, Masterminds destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                //opt.BeforeMap((src, dest) => src.DATE_UPDATED = DateTime.Now);
            });

            return dto;
        }

        public static VillainGroups AddMapping(VillainGroups source, VillainGroups destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
            });

            return dto;
        }

        public static VillainGroups UpdateMapping(VillainGroups source, VillainGroups destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                //opt.BeforeMap((src, dest) => src.DATE_UPDATED = DateTime.Now);
            });

            return dto;
        }

        public static Henchmen AddMapping(Henchmen source, Henchmen destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
            });

            return dto;
        }

        public static Henchmen UpdateMapping(Henchmen source, Henchmen destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                //opt.BeforeMap((src, dest) => src.DATE_UPDATED = DateTime.Now);
            });

            return dto;
        }



    }
}
