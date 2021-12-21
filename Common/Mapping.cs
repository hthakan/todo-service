using AutoMapper;

namespace Common
{
    public static class Mapping
    {
        public static TTarget Map<TSource, TTarget>(TSource value)
            where TSource : class
            where TTarget : class
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<TSource, TTarget>());

            var mapper = new Mapper(config);
            var result = mapper.Map<TSource, TTarget>(value);


            return result;
        }
    }
}
