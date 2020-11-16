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
        public static Stat_Author AddMapping(Stat_Author source, Stat_Author destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
            });

            return dto;
        }

        public static Stat_Author UpdateMapping(Stat_Author source, Stat_Author destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                //opt.BeforeMap((src, dest) => src.DATE_UPDATED = DateTime.Now);
            });

            return dto;
        }

        public static Heroes AddMapping(Heroes source, Heroes destination)
        {
            var iMapper = CreateMap(source, destination);

            var dto = iMapper.Map(source, destination, opt =>
            {
                 //opt.BeforeMap((src, dest) => src.DATE_UPDATED = DateTime.Now);
            });

            return dto;
        }

        public static Heroes UpdateMapping(Heroes source, Heroes destination)
        {
            var iMapper = CreateMap(source, destination);
            var dto = iMapper.Map(source, destination, opt =>
            {
                //opt.BeforeMap((src, dest) => src.DATE_UPDATED = DateTime.Now);
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
