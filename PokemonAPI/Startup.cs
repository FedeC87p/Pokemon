using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PokemonAPI.ApplicationCore;

namespace PokemonAPI
{
    public class Startup
    {
        public const string PokemonBaseApiUrl = "https://pokeapi.co/api/v2/";
        public const string ShakespeareBaseApiUrl = "https://api.funtranslations.com/translate/shakespeare";
        public const string YodaBaseApiUrl = "https://api.funtranslations.com/translate/yoda";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddTransient<IPokemonManager, PokemonManager>();
            services.AddTransient<IPokemonFactory, PokemonFactory>();
            services.AddTransient<ITransationService, TransationService>();
            services.AddTransient<IPokemonService, PokemonService>();
            services.AddTransient<IDescriptionFactory, DescriptionFactory>();

            services.AddHttpClient<ITransationService, TransationService>(c =>
            {

            });
            services.AddHttpClient<IPokemonService, PokemonService>(c =>
            {

            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokemonAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokemonAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
